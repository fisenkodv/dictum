using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;

namespace Dictum.ML
{
    internal static class Program
    {
        private static MLContext _mlContext;

        private static void Main(string[] args)
        {
            // Create MLContext
            _mlContext = new MLContext(seed: 0);

            // Load Data
            var data = _mlContext.Data.LoadFromTextFile<ModelInput>("Data/quotes_train.tsv", hasHeader: true);

            // Split data into training and test sets
            DataOperationsCatalog.TrainTestData dataSplit = _mlContext.Data.TrainTestSplit(data);
            IDataView trainingData = dataSplit.TrainSet;
            IDataView testData = dataSplit.TestSet;

            // Define training pipeline
            IEstimator<ITransformer> trainingPipeline = GetTrainingPipeline();

            // Train model using training pipeline
            ITransformer model = TrainModel(trainingData, trainingPipeline);

            var preview = model.Transform(testData).Preview();

            // Evaluate the model
            Evaluate(testData, model);

            // Save the model
            _mlContext.Model.Save(model, trainingData.Schema, "MLModel.zip");

            Predict();

            Console.ReadKey();
        }

        static IEstimator<ITransformer> GetTrainingPipeline()
        {
            IEstimator<ITransformer> dataPrepPipeline = _mlContext
                    .Transforms.Conversion
                    .MapValueToKey(inputColumnName: "Topic", outputColumnName: "Label")
                    .Append(_mlContext.Transforms.Text.FeaturizeText(
                            "TextFeaturized",
                            new TextFeaturizingEstimator.Options
                            {
                                KeepDiacritics = false,
                                KeepPunctuations = false,
                                CaseMode = TextNormalizingEstimator.CaseMode.Lower,
                                Norm = TextFeaturizingEstimator.NormFunction.L2,
                                StopWordsRemoverOptions =
                                    new StopWordsRemovingEstimator.Options()
                                    {
                                        Language = TextFeaturizingEstimator.Language.English
                                    },
                                CharFeatureExtractor =
                                    new WordBagEstimator.Options {NgramLength = 3, UseAllLengths = false},
                                WordFeatureExtractor =
                                    new WordBagEstimator.Options {NgramLength = 3, UseAllLengths = true}
                            }
                            , "Text", "Topic"
                        )
                    )
                    .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Topic",
                        outputColumnName: "TopicFeaturized"))
                    .Append(_mlContext.Transforms.Concatenate("Features", "TextFeaturized", "TopicFeaturized"))
                ;

            IEstimator<ITransformer> trainingPipeline =
                dataPrepPipeline
                    .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                    .Append(_mlContext.MulticlassClassification.Trainers.SdcaNonCalibrated())
                    .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            return trainingPipeline;
        }

        static ITransformer TrainModel(IDataView trainingData, IEstimator<ITransformer> trainingPipeline)
        {
            return trainingPipeline.Fit(trainingData);
        }

        static void Evaluate(IDataView testData, ITransformer trainedModel)
        {
            IDataView scoredData = trainedModel.Transform(testData);
            MulticlassClassificationMetrics
                evaluationMetrics = _mlContext.MulticlassClassification.Evaluate(scoredData);

            Console.WriteLine($"Model MicroAccuracy: {evaluationMetrics.MicroAccuracy:P2}");
            Console.WriteLine($"Model MacroAccuracy: {evaluationMetrics.MacroAccuracy:P2}");
            Console.WriteLine($"Model LogLoss: {evaluationMetrics.LogLoss:P2}");
            Console.WriteLine($"Model LogLossReduction: {evaluationMetrics.LogLossReduction:#.###}");
        }

        static void Predict()
        {
            var quotes = new[]
            {
                "Wherever a man turns he can find someone who needs him.",
                "All men who have achieved great things have been great dreamers.",
                "The most formidable weapon against errors of every kind is reason.",
                "If you love someone, set them free. If they come back they're yours; if they don't they never were.",
                "Let us never know what old age is. Let us know the happiness time brings, not count the years."
            };

            ITransformer transformer = _mlContext.Model.Load("MLModel.zip", out DataViewSchema inputSchema);

            PredictionEngine<ModelInput, ModelOutput> predictionEngine =
                _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(transformer);

            foreach (var quote in quotes)
            {
                ModelOutput prediction = predictionEngine.Predict(new ModelInput {Text = quote});

                Console.WriteLine($"Prediction Result: {prediction.Topic}");
            }
        }
    }
}
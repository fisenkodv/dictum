using System;
using System.Globalization;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Transforms.Text;

namespace Dictum.ML
{
    internal static class Program
    {
        private static string _appPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        private static string _trainDataPath => Path.Combine(_appPath, "..", "..", "..", "Data", "quotes_train.tsv");
        private static string _testDataPath => Path.Combine(_appPath, "..", "..", "..", "Data", "quotes_test.tsv");
        private static string _modelPath => Path.Combine(_appPath, "..", "..", "..", "Models", "model.zip");

        private static MLContext _mlContext;
        private static PredictionEngine<Quote, QuotePrediction> _predictionEngine;
        private static ITransformer _trainedModel;
        private static IDataView _trainingDataView;

        private static void Main(string[] args)
        {
            _mlContext = new MLContext(seed: 0);


            // STEP 1: Common data loading configuration 
            Console.WriteLine($"=============== Loading Dataset  ===============");
            _trainingDataView = _mlContext.Data.LoadFromTextFile<Quote>(_trainDataPath, hasHeader: true);
            Console.WriteLine($"=============== Finished Loading Dataset  ===============");

            // STEP 2: Common data process configuration with pipeline data transformations
            Console.WriteLine($"=============== Processing Data ===============");
            var pipeline = ProcessData();
            Console.WriteLine($"=============== Finished Processing Data ===============");


            var trainingPipeline = BuildAndTrainModel(_trainingDataView, pipeline);
            Evaluate(_trainingDataView.Schema);
            PredictIssue();
        }

        private static IEstimator<ITransformer> ProcessData()
        {
            var pipeline = _mlContext
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
                .AppendCacheCheckpoint(_mlContext);

            return pipeline;
        }

        private static IEstimator<ITransformer> BuildAndTrainModel(IDataView trainingDataView,
            IEstimator<ITransformer> pipeline)
        {
            // STEP 3: Create the training algorithm/trainer
            var trainingPipeline =
                pipeline.Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                    .Append(_mlContext.MulticlassClassification.Trainers.SdcaNonCalibrated())
                    .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // STEP 4: Train the model fitting to the DataSet
            Console.WriteLine($"=============== Training the model  ===============");

            _trainedModel = trainingPipeline.Fit(trainingDataView);
            Console.WriteLine(
                $"=============== Finished Training the model Ending time: {DateTime.Now.ToString()} ===============");

            // (OPTIONAL) Try/test a single prediction with the "just-trained model" (Before saving the model)
            Console.WriteLine($"=============== Single Prediction just-trained-model ===============");

            // Create prediction engine related to the loaded trained model
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<Quote, QuotePrediction>(_trainedModel);

            var quote = new Quote()
            {
                Text =
                    "All our knowledge begins with the senses, proceeds then to the understanding, and ends with reason. There is nothing higher than reason."
            };

            var prediction = _predictionEngine.Predict(quote);

            Console.WriteLine(
                $"=============== Single Prediction just-trained-model - Result: {prediction.Topic} ===============");

            return trainingPipeline;
        }

        private static void Evaluate(DataViewSchema trainingDataViewSchema)
        {
            // STEP 5:  Evaluate the model in order to get the model's accuracy metrics
            Console.WriteLine(
                $"=============== Evaluating to get model's accuracy metrics - Starting time: {DateTime.Now.ToString(CultureInfo.InvariantCulture)} ===============");

            //Load the test dataset into the IDataView
            var testDataView = _mlContext.Data.LoadFromTextFile<Quote>(_testDataPath, hasHeader: true);

            //Evaluate the model on a test dataset and calculate metrics of the model on the test data.
            var testMetrics =
                _mlContext.MulticlassClassification.Evaluate(_trainedModel.Transform(testDataView));

            Console.WriteLine(
                $"=============== Evaluating to get model's accuracy metrics - Ending time: {DateTime.Now.ToString(CultureInfo.InvariantCulture)} ===============");

            Console.WriteLine(
                $"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for Multi-class Classification model - Test Data     ");
            Console.WriteLine(
                $"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"*       MicroAccuracy:    {testMetrics.MicroAccuracy:P2}");
            Console.WriteLine($"*       MacroAccuracy:    {testMetrics.MacroAccuracy:P2}");
            Console.WriteLine($"*       LogLoss:          {testMetrics.LogLoss:P2}");
            Console.WriteLine($"*       LogLossReduction: {testMetrics.LogLossReduction:#.###}");
            Console.WriteLine(
                $"*************************************************************************************************************");

            // Save the new model to .ZIP file

            SaveModelAsFile(_mlContext, trainingDataViewSchema, _trainedModel);
        }

        private static void PredictIssue()
        {
            var loadedModel = _mlContext.Model.Load(_modelPath, out DataViewSchema modelInputSchema);

            var quotes = new[]
            {
                "Wherever a man turns he can find someone who needs him.",
                "All men who have achieved great things have been great dreamers.",
                "The most formidable weapon against errors of every kind is reason.",
                "If you love someone, set them free. If they come back they're yours; if they don't they never were.",
                "Let us never know what old age is. Let us know the happiness time brings, not count the years."
            };

            _predictionEngine = _mlContext.Model.CreatePredictionEngine<Quote, QuotePrediction>(loadedModel);

            foreach (var quote in quotes)
            {
                QuotePrediction prediction = _predictionEngine.Predict(new Quote() {Text = quote});

                Console.WriteLine($"=============== Single Prediction - Result: {prediction.Topic} ===============");
            }
        }

        private static void SaveModelAsFile(MLContext mlContext, DataViewSchema trainingDataViewSchema,
            ITransformer model)
        {
            mlContext.Model.Save(model, trainingDataViewSchema, _modelPath);

            Console.WriteLine("The model is saved to {0}", _modelPath);
        }
    }
}
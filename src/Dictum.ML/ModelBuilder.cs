using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;

namespace Dictum.ML
{
    public class ModelBuilder
    {
        private readonly MLContext _mlContext;

        public ModelBuilder()
        {
            _mlContext = new MLContext(seed: 1);
        }

        public void CreateModel(string trainDataFilePath, string modelFilePath)
        {
            // Load Data
            IDataView trainingDataView = _mlContext.Data.LoadFromTextFile<ModelInput>(
                path: trainDataFilePath,
                hasHeader: true,
                separatorChar: '\t',
                allowQuoting: true,
                allowSparse: false);

            // Build training pipeline
            IEstimator<ITransformer> trainingPipeline = BuildTrainingPipeline(_mlContext);

            // Split data into training and test sets
            DataOperationsCatalog.TrainTestData dataSplit = _mlContext.Data.TrainTestSplit(trainingDataView);
            IDataView trainingData = dataSplit.TrainSet;
            IDataView testData = dataSplit.TestSet;

            // Evaluate quality of Model
            Evaluate(_mlContext, trainingData, trainingPipeline);

            // Train Model
            ITransformer mlModel = TrainModel(trainingDataView, trainingPipeline);

            // Evaluate quality of Model
            EvaluateTestData(_mlContext, testData, mlModel);

            // Save model
            SaveModel(_mlContext, mlModel, modelFilePath, trainingDataView.Schema);
        }

        private static IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext)
        {
            IEstimator<ITransformer> dataProcessPipeline = mlContext
                .Transforms.Conversion
                .MapValueToKey(inputColumnName: "Topic", outputColumnName: "Topic")
                .Append(mlContext.Transforms.Text.FeaturizeText(
                        "Quote_tf",
                        new TextFeaturizingEstimator.Options
                        {
                            KeepDiacritics = false,
                            KeepPunctuations = false,
                            CaseMode = TextNormalizingEstimator.CaseMode.Lower,
                            Norm = TextFeaturizingEstimator.NormFunction.L2,
                            StopWordsRemoverOptions =
                                new StopWordsRemovingEstimator.Options
                                {
                                    Language = TextFeaturizingEstimator.Language.English
                                },
                            CharFeatureExtractor =
                                new WordBagEstimator.Options {NgramLength = 3, UseAllLengths = false},
                            WordFeatureExtractor =
                                new WordBagEstimator.Options {NgramLength = 3, UseAllLengths = true}
                        }
                        , "Quote"
                    )
                )
                .Append(mlContext.Transforms.CopyColumns("Features", "Quote_tf"))
                .Append(mlContext.Transforms.NormalizeMinMax("Features", "Features"))
                .AppendCacheCheckpoint(mlContext);

            // Set the training algorithm 
            var trainer = mlContext.MulticlassClassification.Trainers
                .SdcaMaximumEntropy(labelColumnName: "Topic", featureColumnName: "Features")
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            return trainingPipeline;
        }

        private static ITransformer TrainModel(IDataView trainingData, IEstimator<ITransformer> trainingPipeline)
        {
            ITransformer model = trainingPipeline.Fit(trainingData);
            return model;
        }

        private static void Evaluate(MLContext mlContext, IDataView trainingDataView,
            IEstimator<ITransformer> trainingPipeline)
        {
            var crossValidationResults = mlContext.MulticlassClassification.CrossValidate(trainingDataView,
                trainingPipeline, numberOfFolds: 5, labelColumnName: "Topic");
            PrintMulticlassClassificationFoldsAverageMetrics(crossValidationResults);
        }

        private static void EvaluateTestData(MLContext mlContext, IDataView testDataView,
            ITransformer trainedModel)
        {
            IDataView scoredData = trainedModel.Transform(testDataView);
            MulticlassClassificationMetrics
                evaluationMetrics = mlContext.MulticlassClassification.Evaluate(scoredData);
            PrintMulticlassClassificationMetrics(evaluationMetrics);
        }

        private static void SaveModel(MLContext mlContext, ITransformer mlModel, string modelRelativePath,
            DataViewSchema modelInputSchema)
        {
            mlContext.Model.Save(mlModel, modelInputSchema, "MLModel.zip");
        }

        private static void PrintMulticlassClassificationMetrics(MulticlassClassificationMetrics metrics)
        {
            Console.WriteLine($"************************************************************");
            Console.WriteLine($"*    Metrics for multi-class classification model   ");
            Console.WriteLine($"*-----------------------------------------------------------");
            Console.WriteLine(
                $"    MacroAccuracy = {metrics.MacroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            Console.WriteLine(
                $"    MicroAccuracy = {metrics.MicroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
            Console.WriteLine($"    LogLoss = {metrics.LogLoss:0.####}, the closer to 0, the better");
            for (int i = 0; i < metrics.PerClassLogLoss.Count; i++)
            {
                Console.WriteLine(
                    $"    LogLoss for class {i + 1} = {metrics.PerClassLogLoss[i]:0.####}, the closer to 0, the better");
            }

            Console.WriteLine($"************************************************************");
        }

        private static void PrintMulticlassClassificationFoldsAverageMetrics(
            IEnumerable<TrainCatalogBase.CrossValidationResult<MulticlassClassificationMetrics>> crossValResults)
        {
            var metricsInMultipleFolds = crossValResults.Select(r => r.Metrics).ToList();

            var microAccuracyValues = metricsInMultipleFolds.Select(m => m.MicroAccuracy).ToList();
            var microAccuracyAverage = microAccuracyValues.Average();
            var microAccuraciesStdDeviation = CalculateStandardDeviation(microAccuracyValues);
            var microAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(microAccuracyValues);

            var macroAccuracyValues = metricsInMultipleFolds.Select(m => m.MacroAccuracy).ToList();
            var macroAccuracyAverage = macroAccuracyValues.Average();
            var macroAccuraciesStdDeviation = CalculateStandardDeviation(macroAccuracyValues);
            var macroAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(macroAccuracyValues);

            var logLossValues = metricsInMultipleFolds.Select(m => m.LogLoss).ToList();
            var logLossAverage = logLossValues.Average();
            var logLossStdDeviation = CalculateStandardDeviation(logLossValues);
            var logLossConfidenceInterval95 = CalculateConfidenceInterval95(logLossValues);

            var logLossReductionValues = metricsInMultipleFolds.Select(m => m.LogLossReduction).ToList();
            var logLossReductionAverage = logLossReductionValues.Average();
            var logLossReductionStdDeviation = CalculateStandardDeviation(logLossReductionValues);
            var logLossReductionConfidenceInterval95 = CalculateConfidenceInterval95(logLossReductionValues);

            Console.WriteLine(
                $"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for Multi-class Classification model      ");
            Console.WriteLine(
                $"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(
                $"*       Average MicroAccuracy:    {microAccuracyAverage:0.###}  - Standard deviation: ({microAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({microAccuraciesConfidenceInterval95:#.###})");
            Console.WriteLine(
                $"*       Average MacroAccuracy:    {macroAccuracyAverage:0.###}  - Standard deviation: ({macroAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({macroAccuraciesConfidenceInterval95:#.###})");
            Console.WriteLine(
                $"*       Average LogLoss:          {logLossAverage:#.###}  - Standard deviation: ({logLossStdDeviation:#.###})  - Confidence Interval 95%: ({logLossConfidenceInterval95:#.###})");
            Console.WriteLine(
                $"*       Average LogLossReduction: {logLossReductionAverage:#.###}  - Standard deviation: ({logLossReductionStdDeviation:#.###})  - Confidence Interval 95%: ({logLossReductionConfidenceInterval95:#.###})");
            Console.WriteLine(
                $"*************************************************************************************************************");
        }

        private static double CalculateStandardDeviation(IEnumerable<double> values)
        {
            IEnumerable<double> list = values.ToList();
            double average = list.Average();
            double sumOfSquaresOfDifferences = list.Select(val => (val - average) * (val - average)).Sum();
            double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / (list.Count() - 1));
            return standardDeviation;
        }

        private static double CalculateConfidenceInterval95(IEnumerable<double> values)
        {
            IEnumerable<double> list = values.ToList();
            double confidenceInterval95 = 1.96 * CalculateStandardDeviation(list) / Math.Sqrt(list.Count() - 1);
            return confidenceInterval95;
        }
    }
}
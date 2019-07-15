using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;

namespace Quote.ML
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            ModelBuilder builder = new ModelBuilder();
            builder.CreateModel("Data/quotes_train.tsv", "MLModel.zip");

            var quotes = new[]
            {
                "Wherever a man turns he can find someone who needs him.",
                "All men who have achieved great things have been great dreamers.",
                "The most formidable weapon against errors of every kind is reason.",
                "If you love someone, set them free. If they come back they're yours; if they don't they never were.",
                "Let us never know what old age is. Let us know the happiness time brings, not count the years."
            };
            Predict(quotes);

            Console.ReadKey();
        }

        private static void Predict(IEnumerable<string> quotes)
        {
            MLContext mlContext = new MLContext(seed: 0);

            ITransformer transformer = mlContext.Model.Load("MLModel.zip", out DataViewSchema inputSchema);

            PredictionEngine<ModelInput, ModelOutput> predictionEngine =
                mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(transformer);

            foreach (var quote in quotes)
            {
                ModelOutput prediction = predictionEngine.Predict(new ModelInput {Quote = quote});

                Console.WriteLine(
                    $"Prediction Result: {prediction.Prediction}. Confidence: {prediction.Score.First():P2}");
            }
        }
    }
}
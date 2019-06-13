using Microsoft.ML.Data;

namespace Dictum.ML
{
    public class Quote
    {
        [LoadColumn(0)] public string Topic { get; set; }
        [LoadColumn(1)] public string Text { get; set; }
    }

    public class QuotePrediction
    {
        [ColumnName("PredictedLabel")] public string Topic;
    }
}
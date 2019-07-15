using Microsoft.ML.Data;

namespace Quote.ML
{
    public class ModelOutput
    {
        [ColumnName("PredictedLabel")] public string Prediction { get; set; }
        public float[] Score { get; set; }
    }
}
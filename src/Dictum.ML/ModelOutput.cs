using Microsoft.ML.Data;

namespace Dictum.ML
{
    public class ModelOutput
    {
        [ColumnName("PredictedLabel")] public string Prediction { get; set; }
        public float[] Score { get; set; }
    }
}
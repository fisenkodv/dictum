using Microsoft.ML.Data;

namespace Dictum.ML
{
    public class ModelInput
    {
        [ColumnName("Topic"), LoadColumn(0)] public string Topic { get; set; }
        [ColumnName("Quote"), LoadColumn(1)] public string Quote { get; set; }
    }
}
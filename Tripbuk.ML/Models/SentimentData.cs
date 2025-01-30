using Microsoft.ML.Data;

namespace Tripbuk.ML.Models;

class SentimentData
{
    [LoadColumn(0)] public string? Text;
    [LoadColumn(1), ColumnName("Label")] public bool Sentiment;
}

class SentimentPrediction : SentimentData
{
    [ColumnName("PredictedLabel")] public bool Prediction { get; set; }
    public float Probability { get; set; }
    public float Score { get; set; }
}
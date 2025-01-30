using Microsoft.ML;
using Spectre.Console;
using Tripbuk.ML.Models;
using Console = Spectre.Console.AnsiConsole;

var ctx = new MLContext();

// load data
var dataView = ctx.Data
    .LoadFromTextFile<SentimentData>("yelp_labelled.txt");

// split data into testing set
var splitDataView = ctx.Data
    .TrainTestSplit(dataView, testFraction: 0.2);

// Build model
var estimator = ctx.Transforms.Text
    .FeaturizeText(
        outputColumnName: "Features",
        inputColumnName: nameof(SentimentData.Text)
    ).Append(ctx.BinaryClassification.Trainers.SdcaLogisticRegression(featureColumnName: "Features"));

// Train model
ITransformer model = default!;

var rule = new Rule("Create and Train Model");
Console
    .Live(rule)
    .Start(console =>
    {
        // training happens here
        model = estimator.Fit(splitDataView.TrainSet);
        var predictions = model.Transform(splitDataView.TestSet);

        rule.Title = "🏁 Training Complete, Evaluating Accuracy.";
        console.Refresh();

        // evaluate the accuracy of our model
        var metrics = ctx.BinaryClassification.Evaluate(predictions);

        var table = new Table()
            .MinimalBorder()
            .Title("💯 Model Accuracy");
        table.AddColumns("Accuracy", "Auc", "F1Score");
        table.AddRow($"{metrics.Accuracy:P2}", $"{metrics.AreaUnderRocCurve:P2}", $"{metrics.F1Score:P2}");

        console.UpdateTarget(table);
        console.Refresh();
    });

// while (true)
// {
//     var text = AnsiConsole.Ask<string>("What's your [green]review text[/]?");
//     var engine = ctx.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
//
//     var input = new SentimentData { Text = text };
//     var result = engine.Predict(input);
//     var style = result.Prediction
//         ? (color: "green", emoji: "👍")
//         : (color: "red", emoji: "👎");
//
//     Console.MarkupLine($"{style.emoji} [{style.color}]\"{text}\" ({result.Probability:P00})[/] ");
// }
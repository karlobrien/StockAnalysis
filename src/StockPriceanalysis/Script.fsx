// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#r "..//..//packages//FSharp.Data//lib//net40//FSharp.Data.dll"
#I "../../packages/XPlot.GoogleCharts/lib/net45"
#I "../../packages/Deedle/lib/net40"

#r "Deedle.dll"
#r "XPlot.GoogleCharts.dll"

#load "../../packages/FsLab/FsLab.fsx"

open FsLab
open Deedle
open FSharp.Data
open System
open System.Xml
open XPlot.GoogleCharts
open Deedle

type Stocks = CsvProvider<"../data/msft.csv">

let msft = Stocks.Load("../data//msft_raw.csv")
let google = Stocks.Load("../data/google_all.csv")
let apple = Stocks.Load("../data/apple_all.csv")

let firstRow = msft.Rows |> Seq.head
let yymmdd1 (date:DateTime) = date.ToString("yy.MM.dd")

let msft_data = msft.Rows |> Seq.toList |> List.map (fun p -> ((yymmdd1 p.Date), p.``Adj Close``))
let google_data = google.Rows |> Seq.toList |> List.map (fun p -> ((yymmdd1 p.Date), p.``Adj Close``))
let apple_data = apple.Rows |> Seq.toList |> List.map (fun p -> ((yymmdd1 p.Date), p.``Adj Close``))

//data |> List.iter (fun c -> printfn "%A" c)

let options =
  Options
    ( title = "Adjusted Price Stock Performance", curveType = "function",
      legend = Legend(position = "bottom") )

[msft_data; google_data; apple_data]
|> Chart.Line
|> Chart.WithOptions options
|> Chart.WithLabels ["MSFT"; "Google"; "Apple"]


//let candle = msft.Rows |> Seq.toArray |> Array.map (fun p -> (p.Date, p.Close, p.Low, p.High, p.Close))

//candle
//|> Chart.Candlestick
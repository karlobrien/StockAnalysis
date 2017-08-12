#r "..//..//packages//FSharp.Data//lib//net40//FSharp.Data.dll"
#I "../../packages/XPlot.GoogleCharts/lib/net45"
#I "../../packages/Deedle/lib/net40"

#r "Deedle.dll"
#r "XPlot.GoogleCharts.dll"

#load "../../packages/FsLab/FsLab.fsx"

open FSharp.Data
open System
open System.Xml
open XPlot.GoogleCharts
open Deedle
open FsLab

type Price = { Day : DateTime; Open : float; Close: float; High: float; Low: float; ``Adj Close``: float }

let msftCsv = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/../data//msft_raw.csv", hasHeaders=true);
let googleCsv = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/../data//google_raw.csv", hasHeaders=true,inferTypes=true);
let appleCsv = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/../data//apple_raw.csv", hasHeaders=true,inferTypes=true);

let msftOrd =
  msftCsv
  |> Frame.indexRowsDate "Date"
  |> Frame.sortRowsByKey
let msft = msftOrd.Columns.[ ["Close";] ]

let google =
  googleCsv
  |> Frame.indexRowsDate "Date"
  |> Frame.sortRowsByKey
  |> Frame.sliceCols ["Close"]

let apple =
  appleCsv
  |> Frame.indexRowsDate "Date"
  |> Frame.sortRowsByKey
  |> Frame.sliceCols ["Close"]

let apple20d = apple?Close |> Stats.movingMean 20
let apple50d = apple?Close |> Stats.movingMean 50
let apple200d = apple?Close |> Stats.movingMean 200

apple?twentyDay <- apple20d
apple?fiftyDay <- apple50d
apple?twoHundred <- apple200d

let options =
  Options
    ( title = "20 Day, 50 Day, 200 Day Close Performance", curveType = "function",
      legend = Legend(position = "bottom") )

let d = apple.Rows.[DateTime(2016, 1, 4) .. DateTime(2016, 8, 7)]
d
|> Chart.Line
|> Chart.WithOptions options
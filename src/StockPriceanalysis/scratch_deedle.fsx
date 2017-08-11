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

let msftCsv = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/../data//msft_raw.csv", hasHeaders=true,inferTypes=true);
let googleCsv = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/../data//google_raw.csv", hasHeaders=true,inferTypes=true);
let appleCsv = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/../data//apple_raw.csv", hasHeaders=true,inferTypes=true);

let msftOrd =
  msftCsv
  |> Frame.indexRowsDate "Date"
  |> Frame.sortRowsByKey

// Create data frame with just Open and Close prices
let msft = msftOrd.Columns.[ ["Open"; "Close"] ]

// Add new column with the difference between Open & Close
msft?Difference <- msft?Open - msft?Close

// Do the same thing for Facebook
let google =
  googleCsv
  |> Frame.indexRowsDate "Date"
  |> Frame.sortRowsByKey
  |> Frame.sliceCols ["Open"; "Close"]
google?Difference <- google?Open - google?Close

// Now we can easily plot the differences
let options =
  Options
    ( title = "Open Verses Stock Performance", curveType = "function",
      legend = Legend(position = "bottom") )

let d = [google?Difference |> Series.observations; msft?Difference |> Series.observations]
d
|> Chart.Line
|> Chart.WithOptions options
|> Chart.WithLabels ["Google"; "Msft"]
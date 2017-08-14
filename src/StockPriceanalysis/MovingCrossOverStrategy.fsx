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

let appleCsv = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/../data//apple_raw.csv", hasHeaders=true,inferTypes=true);

let apple =
  appleCsv
  |> Frame.indexRowsDate "Date"
  |> Frame.sortRowsByKey
  |> Frame.sliceCols ["Close"]

apple?twentyDay <- apple?Close |> Stats.movingMean 20
apple?fiftyDay <- apple?Close |> Stats.movingMean 50
apple?twoHundred <- apple?Close |> Stats.movingMean 200
apple?difference <- apple?twentyDay - apple?fiftyDay

apple?Regime <- apple |> Frame.mapRowValues (fun row ->
  if row?difference > 0.0
    then 1
  else if row?difference = 0.0
    then 0
  else
    -1)

let janToAug = apple.Rows.[DateTime(2016, 1, 4) .. DateTime(2016, 8, 7)]

janToAug?Regime
|> Chart.Line

apple?Regime |> Series.groupInto
  (fun k v -> v)
  (fun len regime -> Series.countValues regime)
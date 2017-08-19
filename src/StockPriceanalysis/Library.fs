namespace StockPriceanalysis

open System
open FSharp.Data
open System.Xml
open XPlot.GoogleCharts
open Deedle
open System.IO

module PriceLoader =

  type Price = { Open : float; Close: float; High: float; Low: float; AdjClose: float }

  /// Loads Frame from csv file
  let frameReader (name: string) =
    let path = sprintf "/../data//%s_raw.csv" name
    Frame.ReadCsv(__SOURCE_DIRECTORY__ + path, hasHeaders=true,inferTypes=true)


  /// Returns a Series of <Date, Prices>
  ///
  /// ## Parameters
  /// - `symbol` - apple, msft, google
  let prices symbol =
    frameReader symbol
    |> Frame.indexRowsDate "Date"
    |> Frame.sortRowsByKey
    |> Frame.mapRowValues (fun c ->
      {
        Open = c?Open
        Close = c?Close
        High = c?High
        Low = c?Low
        AdjClose = c?``Adj Close``
      })



  /// Returns 42
  ///
  /// ## Parameters
  ///  - `num` - whatever
  let hello num = 42
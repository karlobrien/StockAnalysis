module StockPriceanalysis.Tests

open StockPriceanalysis
open NUnit.Framework

[<Test>]
let ``hello returns 42`` () =
  let result = PriceLoader.hello 42
  printfn "%i" result
  Assert.AreEqual(42,result)

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StockAnalyzer.Core.Domain;
using StockAnalyzer.Windows.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestClass]
    public class MockStockServiceTests
    {
        private Mock<IStockService> _stockService;

        public MockStockServiceTests()
        {
            _stockService = new Mock<IStockService>();
        }


        [TestMethod]
        public async Task Can_Load_MSFT_Stocks()
        {
            var ticker = "MSFT";
            var token = CancellationToken.None;
            _stockService.Setup(x => x.GetStockPricesFor(ticker, CancellationToken.None)).Returns(this.GetStockPricesFor(ticker, CancellationToken.None));

            var service = new MockStockService();
            var stocks = await service.GetStockPricesFor(ticker, CancellationToken.None);

            Assert.AreEqual(stocks.Count(), 2);
            Assert.AreEqual(stocks.Sum(stock => stock.Change), 0.7m);
        }

        public Task<IEnumerable<StockPrice>> GetStockPricesFor(string ticker, CancellationToken cancellationToken)
        {
            var stocks = new List<StockPrice>
            {
                new StockPrice { Ticker = "MSFT", Change = 0.5m, ChangePercent = 0.75m },
                new StockPrice { Ticker = "MSFT", Change = 0.2m, ChangePercent = 0.15m },
                new StockPrice { Ticker = "GOOGL", Change = 0.3m, ChangePercent = 0.25m },
                new StockPrice { Ticker = "GOOGL", Change = 0.5m, ChangePercent = 0.65m },
            };

            return Task.FromResult(stocks.Where(sock => sock.Ticker == ticker));
        }
    }
}

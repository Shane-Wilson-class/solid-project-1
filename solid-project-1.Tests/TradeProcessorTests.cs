using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Moq;
using NUnit.Framework;

namespace solid_project_1.Tests;

/// <summary>
/// Unit tests for TradeProcessor class.
/// Uses mocked dependencies to verify correct method calls and parameter passing without external dependencies.
/// </summary>
[TestFixture]
public class TradeProcessorTests
{
    private Mock<ITradeDataProvider> _mockTradeDataProvider;
    private Mock<ITradeParser> _mockTradeParser;
    private Mock<ITradeStorage> _mockTradeStorage;
    private TradeProcessor _tradeProcessor;

    [SetUp]
    public void Setup()
    {
        _mockTradeDataProvider = new Mock<ITradeDataProvider>();
        _mockTradeParser = new Mock<ITradeParser>();
        _mockTradeStorage = new Mock<ITradeStorage>();
        
        _tradeProcessor = new TradeProcessor(
            _mockTradeParser.Object,
            _mockTradeStorage.Object,
            _mockTradeDataProvider.Object);
    }

    [Test]
    public void ProcessTrades_WithValidStream_CallsGetTradeDataOnce()
    {
        // Arrange
        var stream = new MemoryStream();
        var lines = new List<string> { "GBPUSD,1000,1.51" };
        var trades = CreateSampleTrades();

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.Setup(x => x.Parse(lines)).Returns(trades);
        _mockTradeStorage.Setup(x => x.Persist(trades)).Returns("INFO: 1 trades processed");

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert
        _mockTradeDataProvider.Verify(x => x.GetTradeData(stream), Times.Once);
    }

    [Test]
    public void ProcessTrades_WithValidStream_CallsParseOnce()
    {
        // Arrange
        var stream = new MemoryStream();
        var lines = new List<string> { "GBPUSD,1000,1.51" };
        var trades = CreateSampleTrades();

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.Setup(x => x.Parse(lines)).Returns(trades);
        _mockTradeStorage.Setup(x => x.Persist(trades)).Returns("INFO: 1 trades processed");

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert
        _mockTradeParser.Verify(x => x.Parse(lines), Times.Once);
    }

    [Test]
    public void ProcessTrades_WithValidStream_CallsPersistOnce()
    {
        // Arrange
        var stream = new MemoryStream();
        var lines = new List<string> { "GBPUSD,1000,1.51" };
        var trades = CreateSampleTrades();

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.Setup(x => x.Parse(lines)).Returns(trades);
        _mockTradeStorage.Setup(x => x.Persist(trades)).Returns("INFO: 1 trades processed");

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert
        _mockTradeStorage.Verify(x => x.Persist(trades), Times.Once);
    }

    [Test]
    public void ProcessTrades_WithValidStream_CallsMethodsInCorrectOrder()
    {
        // Arrange
        var stream = new MemoryStream();
        var lines = new List<string> { "GBPUSD,1000,1.51" };
        var trades = CreateSampleTrades();

        var callOrder = new List<string>();
        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream))
            .Returns(lines)
            .Callback(() => callOrder.Add("GetTradeData"));
        _mockTradeParser.Setup(x => x.Parse(lines))
            .Returns(trades)
            .Callback(() => callOrder.Add("Parse"));
        _mockTradeStorage.Setup(x => x.Persist(trades))
            .Returns("INFO: 1 trades processed")
            .Callback(() => callOrder.Add("Persist"));

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert
        Assert.That(callOrder, Has.Count.EqualTo(3));
        Assert.That(callOrder[0], Is.EqualTo("GetTradeData"));
        Assert.That(callOrder[1], Is.EqualTo("Parse"));
        Assert.That(callOrder[2], Is.EqualTo("Persist"));
    }

    [Test]
    public void ProcessTrades_WithEmptyStream_HandlesEmptyDataCorrectly()
    {
        // Arrange
        var stream = new MemoryStream();
        var emptyLines = new List<string>();
        var emptyTrades = new List<TradeRecord>();

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(emptyLines);
        _mockTradeParser.Setup(x => x.Parse(emptyLines)).Returns(emptyTrades);
        _mockTradeStorage.Setup(x => x.Persist(emptyTrades)).Returns("INFO: 0 trades processed");

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert
        _mockTradeDataProvider.Verify(x => x.GetTradeData(stream), Times.Once);
        _mockTradeParser.Verify(x => x.Parse(emptyLines), Times.Once);
        _mockTradeStorage.Verify(x => x.Persist(emptyTrades), Times.Once);
    }

    [Test]
    public void ProcessTrades_WithMultipleTrades_PassesCorrectDataBetweenComponents()
    {
        // Arrange
        var stream = new MemoryStream();
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",
            "EURJPY,1500,178.13",
            "GBPJPY,800,188.71"
        };
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10, Price = 1.51m },
            new TradeRecord { SourceCurrency = "EUR", DestinationCurrency = "JPY", Lots = 15, Price = 178.13m },
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "JPY", Lots = 8, Price = 188.71m }
        };

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.Setup(x => x.Parse(lines)).Returns(trades);
        _mockTradeStorage.Setup(x => x.Persist(trades)).Returns("INFO: 3 trades processed");

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert
        _mockTradeDataProvider.Verify(x => x.GetTradeData(stream), Times.Once);
        _mockTradeParser.Verify(x => x.Parse(lines), Times.Once);
        _mockTradeStorage.Verify(x => x.Persist(trades), Times.Once);
    }

    [Test]
    public void ProcessTrades_WithNullStream_DoesNotThrowImmediately()
    {
        // Arrange
        Stream nullStream = null;

        // Act & Assert - Current implementation doesn't validate null stream parameter
        Assert.DoesNotThrow(() => _tradeProcessor.ProcessTrades(nullStream));
    }

    [Test]
    public void ProcessTrades_WhenGetTradeDataThrowsException_PropagatesException()
    {
        // Arrange
        var stream = new MemoryStream();
        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream))
            .Throws(new InvalidOperationException("Failed to read stream"));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _tradeProcessor.ProcessTrades(stream));
        Assert.That(exception.Message, Is.EqualTo("Failed to read stream"));
    }

    [Test]
    public void ProcessTrades_WhenParseThrowsException_PropagatesException()
    {
        // Arrange
        var stream = new MemoryStream();
        var lines = new List<string> { "GBPUSD,1000,1.51" };

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.Setup(x => x.Parse(lines))
            .Throws(new InvalidOperationException("Failed to parse trades"));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _tradeProcessor.ProcessTrades(stream));
        Assert.That(exception.Message, Is.EqualTo("Failed to parse trades"));
    }

    [Test]
    public void ProcessTrades_WhenPersistThrowsException_PropagatesException()
    {
        // Arrange
        var stream = new MemoryStream();
        var lines = new List<string> { "GBPUSD,1000,1.51" };
        var trades = CreateSampleTrades();

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.Setup(x => x.Parse(lines)).Returns(trades);
        _mockTradeStorage.Setup(x => x.Persist(trades))
            .Throws(new InvalidOperationException("Failed to persist trades"));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _tradeProcessor.ProcessTrades(stream));
        Assert.That(exception.Message, Is.EqualTo("Failed to persist trades"));
    }

    [Test]
    public void ProcessTrades_VerifyNoUnexpectedMethodCalls()
    {
        // Arrange
        var stream = new MemoryStream();
        var lines = new List<string> { "GBPUSD,1000,1.51" };
        var trades = CreateSampleTrades();

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.Setup(x => x.Parse(lines)).Returns(trades);
        _mockTradeStorage.Setup(x => x.Persist(trades)).Returns("INFO: 1 trades processed");

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert - Verify only expected methods were called
        _mockTradeDataProvider.Verify(x => x.GetTradeData(It.IsAny<Stream>()), Times.Once);
        _mockTradeParser.Verify(x => x.Parse(It.IsAny<List<string>>()), Times.Once);
        _mockTradeStorage.Verify(x => x.Persist(It.IsAny<List<TradeRecord>>()), Times.Once);
        
        // Verify no other methods were called
        _mockTradeDataProvider.VerifyNoOtherCalls();
        _mockTradeParser.VerifyNoOtherCalls();
        _mockTradeStorage.VerifyNoOtherCalls();
    }

    [Test]
    public void Constructor_WithNullTradeParser_DoesNotThrowImmediately()
    {
        // Arrange
        ITradeParser nullParser = null;

        // Act & Assert - Constructor doesn't validate null parameters
        Assert.DoesNotThrow(() => new TradeProcessor(
            nullParser,
            _mockTradeStorage.Object,
            _mockTradeDataProvider.Object));
    }

    [Test]
    public void Constructor_WithNullTradeStorage_DoesNotThrowImmediately()
    {
        // Arrange
        ITradeStorage nullStorage = null;

        // Act & Assert - Constructor doesn't validate null parameters
        Assert.DoesNotThrow(() => new TradeProcessor(
            _mockTradeParser.Object,
            nullStorage,
            _mockTradeDataProvider.Object));
    }

    [Test]
    public void Constructor_WithNullTradeDataProvider_DoesNotThrowImmediately()
    {
        // Arrange
        ITradeDataProvider nullProvider = null;

        // Act & Assert - Constructor doesn't validate null parameters
        Assert.DoesNotThrow(() => new TradeProcessor(
            _mockTradeParser.Object,
            _mockTradeStorage.Object,
            nullProvider));
    }

    [Test]
    public void ProcessTrades_WithLargeDataSet_HandlesLargeVolume()
    {
        // Arrange
        var stream = new MemoryStream();
        var lines = new List<string>();
        var trades = new List<TradeRecord>();

        for (int i = 0; i < 1000; i++)
        {
            lines.Add($"GBPUSD,{1000 + i},{1.51 + (i * 0.001):F3}");
            trades.Add(new TradeRecord
            {
                SourceCurrency = "GBP",
                DestinationCurrency = "USD",
                Lots = 10 + i,
                Price = 1.51m + (i * 0.001m)
            });
        }

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.Setup(x => x.Parse(lines)).Returns(trades);
        _mockTradeStorage.Setup(x => x.Persist(trades)).Returns("INFO: 1000 trades processed");

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert
        _mockTradeDataProvider.Verify(x => x.GetTradeData(stream), Times.Once);
        _mockTradeParser.Verify(x => x.Parse(lines), Times.Once);
        _mockTradeStorage.Verify(x => x.Persist(trades), Times.Once);
    }

    private static List<TradeRecord> CreateSampleTrades()
    {
        return new List<TradeRecord>
        {
            new TradeRecord
            {
                SourceCurrency = "GBP",
                DestinationCurrency = "USD",
                Lots = 10,
                Price = 1.51m
            }
        };
    }
}

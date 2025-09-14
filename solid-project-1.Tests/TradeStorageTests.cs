using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace solid_project_1.Tests;

/// <summary>
/// Unit tests for TradeStorage class.
/// Uses Moq to mock IDatabaseRepository dependency and verify database operations without actual database calls.
/// </summary>
[TestFixture]
public class TradeStorageTests
{
    private Mock<IDatabaseRepository> _mockDatabaseRepository;
    private TradeStorage _tradeStorage;

    [SetUp]
    public void Setup()
    {
        _mockDatabaseRepository = new Mock<IDatabaseRepository>();
        _tradeStorage = new TradeStorage(_mockDatabaseRepository.Object);
    }

    [Test]
    public void Persist_WithValidTrades_CallsClearAllTradesOnce()
    {
        // Arrange
        var trades = CreateSampleTrades();

        // Act
        _tradeStorage.Persist(trades);

        // Assert
        _mockDatabaseRepository.Verify(x => x.ClearAllTrades(), Times.Once);
    }

    [Test]
    public void Persist_WithValidTrades_CallsInsertTradesOnce()
    {
        // Arrange
        var trades = CreateSampleTrades();

        // Act
        _tradeStorage.Persist(trades);

        // Assert
        _mockDatabaseRepository.Verify(x => x.InsertTrades(trades), Times.Once);
    }

    [Test]
    public void Persist_WithValidTrades_ReturnsCorrectStatusMessage()
    {
        // Arrange
        var trades = CreateSampleTrades();
        var expectedMessage = $"INFO: {trades.Count} trades processed";

        // Act
        var result = _tradeStorage.Persist(trades);

        // Assert
        Assert.That(result, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void Persist_WithEmptyTradeList_CallsClearAllTradesOnce()
    {
        // Arrange
        var trades = new List<TradeRecord>();

        // Act
        _tradeStorage.Persist(trades);

        // Assert
        _mockDatabaseRepository.Verify(x => x.ClearAllTrades(), Times.Once);
    }

    [Test]
    public void Persist_WithEmptyTradeList_CallsInsertTradesOnce()
    {
        // Arrange
        var trades = new List<TradeRecord>();

        // Act
        _tradeStorage.Persist(trades);

        // Assert
        _mockDatabaseRepository.Verify(x => x.InsertTrades(trades), Times.Once);
    }

    [Test]
    public void Persist_WithEmptyTradeList_ReturnsZeroTradesMessage()
    {
        // Arrange
        var trades = new List<TradeRecord>();
        var expectedMessage = "INFO: 0 trades processed";

        // Act
        var result = _tradeStorage.Persist(trades);

        // Assert
        Assert.That(result, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void Persist_WithSingleTrade_CallsMethodsInCorrectOrder()
    {
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord
            {
                SourceCurrency = "GBP",
                DestinationCurrency = "USD",
                Lots = 10,
                Price = 1.51m
            }
        };

        var callOrder = new List<string>();
        _mockDatabaseRepository.Setup(x => x.ClearAllTrades())
            .Callback(() => callOrder.Add("ClearAllTrades"));
        _mockDatabaseRepository.Setup(x => x.InsertTrades(It.IsAny<IEnumerable<TradeRecord>>()))
            .Callback(() => callOrder.Add("InsertTrades"));

        // Act
        _tradeStorage.Persist(trades);

        // Assert
        Assert.That(callOrder, Has.Count.EqualTo(2));
        Assert.That(callOrder[0], Is.EqualTo("ClearAllTrades"));
        Assert.That(callOrder[1], Is.EqualTo("InsertTrades"));
    }

    [Test]
    public void Persist_WithLargeTradeList_HandlesLargeDataSet()
    {
        // Arrange
        var trades = new List<TradeRecord>();
        for (int i = 0; i < 1000; i++)
        {
            trades.Add(new TradeRecord
            {
                SourceCurrency = "GBP",
                DestinationCurrency = "USD",
                Lots = 10 + i,
                Price = 1.51m + (i * 0.001m)
            });
        }

        // Act
        var result = _tradeStorage.Persist(trades);

        // Assert
        _mockDatabaseRepository.Verify(x => x.ClearAllTrades(), Times.Once);
        _mockDatabaseRepository.Verify(x => x.InsertTrades(trades), Times.Once);
        Assert.That(result, Is.EqualTo("INFO: 1000 trades processed"));
    }

    [Test]
    public void Persist_WithNullTradeList_ThrowsNullReferenceException()
    {
        // Arrange
        List<TradeRecord> trades = null;

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _tradeStorage.Persist(trades));
    }

    [Test]
    public void Persist_WhenClearAllTradesThrowsException_PropagatesException()
    {
        // Arrange
        var trades = CreateSampleTrades();
        _mockDatabaseRepository.Setup(x => x.ClearAllTrades())
            .Throws(new InvalidOperationException("Database connection failed"));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _tradeStorage.Persist(trades));
        Assert.That(exception.Message, Is.EqualTo("Database connection failed"));
    }

    [Test]
    public void Persist_WhenInsertTradesThrowsException_PropagatesException()
    {
        // Arrange
        var trades = CreateSampleTrades();
        _mockDatabaseRepository.Setup(x => x.InsertTrades(It.IsAny<IEnumerable<TradeRecord>>()))
            .Throws(new InvalidOperationException("Insert operation failed"));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _tradeStorage.Persist(trades));
        Assert.That(exception.Message, Is.EqualTo("Insert operation failed"));
    }

    [Test]
    public void Persist_WithSpecificTradeData_PassesCorrectDataToRepository()
    {
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord
            {
                SourceCurrency = "EUR",
                DestinationCurrency = "JPY",
                Lots = 15.5f,
                Price = 178.13m
            },
            new TradeRecord
            {
                SourceCurrency = "GBP",
                DestinationCurrency = "JPY",
                Lots = 8.25f,
                Price = 188.71m
            }
        };

        List<TradeRecord> capturedTrades = null;
        _mockDatabaseRepository.Setup(x => x.InsertTrades(It.IsAny<IEnumerable<TradeRecord>>()))
            .Callback<IEnumerable<TradeRecord>>(t => capturedTrades = new List<TradeRecord>(t));

        // Act
        _tradeStorage.Persist(trades);

        // Assert
        Assert.That(capturedTrades, Is.Not.Null);
        Assert.That(capturedTrades, Has.Count.EqualTo(2));
        
        Assert.That(capturedTrades[0].SourceCurrency, Is.EqualTo("EUR"));
        Assert.That(capturedTrades[0].DestinationCurrency, Is.EqualTo("JPY"));
        Assert.That(capturedTrades[0].Lots, Is.EqualTo(15.5f));
        Assert.That(capturedTrades[0].Price, Is.EqualTo(178.13m));
        
        Assert.That(capturedTrades[1].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(capturedTrades[1].DestinationCurrency, Is.EqualTo("JPY"));
        Assert.That(capturedTrades[1].Lots, Is.EqualTo(8.25f));
        Assert.That(capturedTrades[1].Price, Is.EqualTo(188.71m));
    }

    [Test]
    public void Constructor_WithNullRepository_DoesNotThrowImmediately()
    {
        // Arrange
        IDatabaseRepository nullRepository = null;

        // Act & Assert - Constructor doesn't validate null, but usage will fail
        Assert.DoesNotThrow(() => new TradeStorage(nullRepository));
    }

    [Test]
    public void Persist_VerifyNoOtherMethodsCalled()
    {
        // Arrange
        var trades = CreateSampleTrades();

        // Act
        _tradeStorage.Persist(trades);

        // Assert - Verify only expected methods were called
        _mockDatabaseRepository.Verify(x => x.ClearAllTrades(), Times.Once);
        _mockDatabaseRepository.Verify(x => x.InsertTrades(It.IsAny<IEnumerable<TradeRecord>>()), Times.Once);
        _mockDatabaseRepository.Verify(x => x.GetAllTrades(), Times.Never);
        _mockDatabaseRepository.Verify(x => x.GetTradeCount(), Times.Never);
        _mockDatabaseRepository.Verify(x => x.InsertTrade(It.IsAny<TradeRecord>()), Times.Never);
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
            },
            new TradeRecord
            {
                SourceCurrency = "EUR",
                DestinationCurrency = "JPY",
                Lots = 15,
                Price = 178.13m
            }
        };
    }
}

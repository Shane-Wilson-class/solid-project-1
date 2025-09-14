using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace solid_project_1.Tests;

/// <summary>
/// Unit tests for DatabaseRepository class.
/// Tests all database operations with appropriate setup/teardown using temporary database files.
/// </summary>
[TestFixture]
public class DatabaseRepositoryTests
{
    private DatabaseRepository _databaseRepository;
    private string _testDatabasePath;

    [SetUp]
    public void Setup()
    {
        // Create a unique temporary database file for each test
        _testDatabasePath = Path.Combine(Path.GetTempPath(), $"test_trades_{Guid.NewGuid()}.db");
        _databaseRepository = new DatabaseRepository(_testDatabasePath);
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the temporary database file
        if (File.Exists(_testDatabasePath))
        {
            File.Delete(_testDatabasePath);
        }
    }

    [Test]
    public void Constructor_WithDefaultPath_CreatesRepository()
    {
        // Arrange & Act
        var repository = new DatabaseRepository();

        // Assert
        Assert.That(repository, Is.Not.Null);
    }

    [Test]
    public void Constructor_WithCustomPath_CreatesRepository()
    {
        // Arrange
        var customPath = Path.Combine(Path.GetTempPath(), "custom_test.db");

        // Act
        var repository = new DatabaseRepository(customPath);

        // Assert
        Assert.That(repository, Is.Not.Null);

        // Cleanup
        if (File.Exists(customPath))
        {
            File.Delete(customPath);
        }
    }

    [Test]
    public void InsertTrade_WithValidTrade_InsertsTradeSuccessfully()
    {
        // Arrange
        var trade = new TradeRecord
        {
            SourceCurrency = "GBP",
            DestinationCurrency = "USD",
            Lots = 10,
            Price = 1.51m
        };

        // Act
        _databaseRepository.InsertTrade(trade);

        // Assert
        var allTrades = _databaseRepository.GetAllTrades();
        Assert.That(allTrades, Has.Count.EqualTo(1));
        Assert.That(allTrades[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(allTrades[0].DestinationCurrency, Is.EqualTo("USD"));
        Assert.That(allTrades[0].Lots, Is.EqualTo(10));
        Assert.That(allTrades[0].Price, Is.EqualTo(1.51m));
    }

    [Test]
    public void InsertTrades_WithMultipleTrades_InsertsAllTradesSuccessfully()
    {
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10, Price = 1.51m },
            new TradeRecord { SourceCurrency = "EUR", DestinationCurrency = "JPY", Lots = 15, Price = 178.13m },
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "JPY", Lots = 8, Price = 188.71m }
        };

        // Act
        _databaseRepository.InsertTrades(trades);

        // Assert
        var allTrades = _databaseRepository.GetAllTrades();
        Assert.That(allTrades, Has.Count.EqualTo(3));
        
        Assert.That(allTrades[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(allTrades[0].DestinationCurrency, Is.EqualTo("USD"));
        
        Assert.That(allTrades[1].SourceCurrency, Is.EqualTo("EUR"));
        Assert.That(allTrades[1].DestinationCurrency, Is.EqualTo("JPY"));
        
        Assert.That(allTrades[2].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(allTrades[2].DestinationCurrency, Is.EqualTo("JPY"));
    }

    [Test]
    public void InsertTrades_WithEmptyList_DoesNotInsertAnyTrades()
    {
        // Arrange
        var trades = new List<TradeRecord>();

        // Act
        _databaseRepository.InsertTrades(trades);

        // Assert
        var allTrades = _databaseRepository.GetAllTrades();
        Assert.That(allTrades, Is.Empty);
    }

    [Test]
    public void GetAllTrades_WithNoTrades_ReturnsEmptyList()
    {
        // Arrange & Act
        var allTrades = _databaseRepository.GetAllTrades();

        // Assert
        Assert.That(allTrades, Is.Not.Null);
        Assert.That(allTrades, Is.Empty);
    }

    [Test]
    public void GetTradeCount_WithNoTrades_ReturnsZero()
    {
        // Arrange & Act
        var count = _databaseRepository.GetTradeCount();

        // Assert
        Assert.That(count, Is.EqualTo(0));
    }

    [Test]
    public void GetTradeCount_WithMultipleTrades_ReturnsCorrectCount()
    {
        // Arrange
        var trades = CreateSampleTrades(5);
        _databaseRepository.InsertTrades(trades);

        // Act
        var count = _databaseRepository.GetTradeCount();

        // Assert
        Assert.That(count, Is.EqualTo(5));
    }

    [Test]
    public void ClearAllTrades_WithExistingTrades_RemovesAllTrades()
    {
        // Arrange
        var trades = CreateSampleTrades(3);
        _databaseRepository.InsertTrades(trades);
        
        // Verify trades were inserted
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(3));

        // Act
        _databaseRepository.ClearAllTrades();

        // Assert
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(0));
        Assert.That(_databaseRepository.GetAllTrades(), Is.Empty);
    }

    [Test]
    public void ClearAllTrades_WithNoTrades_DoesNotThrow()
    {
        // Arrange & Act & Assert
        Assert.DoesNotThrow(() => _databaseRepository.ClearAllTrades());
    }

    [Test]
    public void InsertTrade_WithNullTrade_ThrowsArgumentNullException()
    {
        // Arrange
        TradeRecord nullTrade = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _databaseRepository.InsertTrade(nullTrade));
    }

    [Test]
    public void InsertTrades_WithNullCollection_ThrowsNullReferenceException()
    {
        // Arrange
        IEnumerable<TradeRecord> nullTrades = null;

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _databaseRepository.InsertTrades(nullTrades));
    }

    [Test]
    public void DatabaseOperations_WithLargeDataSet_HandlesLargeVolume()
    {
        // Arrange
        var trades = CreateSampleTrades(1000);

        // Act
        _databaseRepository.InsertTrades(trades);

        // Assert
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(1000));
        
        var allTrades = _databaseRepository.GetAllTrades();
        Assert.That(allTrades, Has.Count.EqualTo(1000));
        
        // Verify first and last trades
        Assert.That(allTrades[0].Lots, Is.EqualTo(10));
        Assert.That(allTrades[999].Lots, Is.EqualTo(1009));
    }

    [Test]
    public void DatabaseOperations_WithSpecialCharacters_PreservesData()
    {
        // Arrange
        var trade = new TradeRecord
        {
            SourceCurrency = "GB£",
            DestinationCurrency = "US$",
            Lots = 10.5f,
            Price = 1.51234567890123456789m
        };

        // Act
        _databaseRepository.InsertTrade(trade);

        // Assert
        var retrievedTrades = _databaseRepository.GetAllTrades();
        Assert.That(retrievedTrades, Has.Count.EqualTo(1));
        Assert.That(retrievedTrades[0].SourceCurrency, Is.EqualTo("GB£"));
        Assert.That(retrievedTrades[0].DestinationCurrency, Is.EqualTo("US$"));
        Assert.That(retrievedTrades[0].Lots, Is.EqualTo(10.5f));
        Assert.That(retrievedTrades[0].Price, Is.EqualTo(1.51234567890123456789m));
    }

    [Test]
    public void DatabaseOperations_MultipleOperations_MaintainsDataIntegrity()
    {
        // Arrange
        var initialTrades = CreateSampleTrades(3);
        var additionalTrades = CreateSampleTrades(2, startIndex: 100);

        // Act
        _databaseRepository.InsertTrades(initialTrades);
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(3));

        _databaseRepository.InsertTrades(additionalTrades);
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(5));

        _databaseRepository.ClearAllTrades();
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(0));

        _databaseRepository.InsertTrades(initialTrades);
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(3));

        // Assert
        var finalTrades = _databaseRepository.GetAllTrades();
        Assert.That(finalTrades, Has.Count.EqualTo(3));
        Assert.That(finalTrades[0].Lots, Is.EqualTo(10));
        Assert.That(finalTrades[1].Lots, Is.EqualTo(11));
        Assert.That(finalTrades[2].Lots, Is.EqualTo(12));
    }

    [Test]
    public void DatabaseOperations_ConcurrentAccess_HandlesMultipleRepositoryInstances()
    {
        // Arrange
        var repository1 = new DatabaseRepository(_testDatabasePath);
        var repository2 = new DatabaseRepository(_testDatabasePath);
        
        var trades1 = CreateSampleTrades(2);
        var trades2 = CreateSampleTrades(3, startIndex: 10);

        // Act
        repository1.InsertTrades(trades1);
        repository2.InsertTrades(trades2);

        // Assert
        var count1 = repository1.GetTradeCount();
        var count2 = repository2.GetTradeCount();
        
        Assert.That(count1, Is.EqualTo(5));
        Assert.That(count2, Is.EqualTo(5));
        
        var allTrades1 = repository1.GetAllTrades();
        var allTrades2 = repository2.GetAllTrades();
        
        Assert.That(allTrades1, Has.Count.EqualTo(5));
        Assert.That(allTrades2, Has.Count.EqualTo(5));
    }

    [Test]
    public void DatabaseOperations_WithZeroAndNegativeValues_HandlesEdgeCases()
    {
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 0, Price = 0m },
            new TradeRecord { SourceCurrency = "EUR", DestinationCurrency = "JPY", Lots = -5, Price = -100.50m }
        };

        // Act
        _databaseRepository.InsertTrades(trades);

        // Assert
        var retrievedTrades = _databaseRepository.GetAllTrades();
        Assert.That(retrievedTrades, Has.Count.EqualTo(2));
        Assert.That(retrievedTrades[0].Lots, Is.EqualTo(0));
        Assert.That(retrievedTrades[0].Price, Is.EqualTo(0m));
        Assert.That(retrievedTrades[1].Lots, Is.EqualTo(-5));
        Assert.That(retrievedTrades[1].Price, Is.EqualTo(-100.50m));
    }

    private static List<TradeRecord> CreateSampleTrades(int count, int startIndex = 0)
    {
        var trades = new List<TradeRecord>();
        for (int i = 0; i < count; i++)
        {
            trades.Add(new TradeRecord
            {
                SourceCurrency = "GBP",
                DestinationCurrency = "USD",
                Lots = 10 + startIndex + i,
                Price = 1.51m + (i * 0.01m)
            });
        }
        return trades;
    }
}

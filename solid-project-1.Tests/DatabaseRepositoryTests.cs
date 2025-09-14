using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace solid_project_1.Tests;

/*
 * SOLID PRINCIPLES LEARNING GUIDE - DatabaseRepository Tests
 * 
 * LEARNING OBJECTIVES:
 * - Understand database testing with temporary files for isolation
 * - Learn professional testing patterns for data persistence
 * - See working examples of comprehensive unit tests
 * - Practice reading and understanding test code
 * - Experience proper setup/teardown patterns
 * 
 * IMPLEMENTATION STATUS: ✅ FULLY ENABLED
 * These tests work immediately because DatabaseRepository class already exists.
 * They serve as working examples of professional database testing patterns.
 * 
 * WHAT DatabaseRepository DOES:
 * - Wraps all LiteDB database operations
 * - Provides methods for inserting, retrieving, and clearing trade data
 * - Represents a "volatile dependency" that students will learn to abstract
 * - Demonstrates concrete implementation before interface abstraction
 */

/// <summary>
/// Unit tests for DatabaseRepository class.
/// Tests database operations with temporary database files to ensure proper data persistence.
/// 
/// NOTE: These tests are FULLY ENABLED because DatabaseRepository class already exists.
/// They demonstrate professional database testing patterns with test isolation.
/// Study these tests to understand professional testing practices!
/// </summary>
[TestFixture]
public class DatabaseRepositoryTests
{
    private DatabaseRepository _databaseRepository;
    private string _testDatabasePath;

    [SetUp]
    public void Setup()
    {
        // LEARNING: Test isolation - each test gets its own database file
        // This prevents tests from interfering with each other
        _testDatabasePath = Path.Combine(Path.GetTempPath(), $"test_trades_{Guid.NewGuid()}.db");
        _databaseRepository = new DatabaseRepository(_testDatabasePath);
    }

    [TearDown]
    public void TearDown()
    {
        // LEARNING: Cleanup - remove temporary files after each test
        // This keeps the test environment clean
        if (File.Exists(_testDatabasePath))
        {
            File.Delete(_testDatabasePath);
        }
    }

    [Test]
    public void InsertTrade_WithValidTrade_InsertsTradeSuccessfully()
    {
        // LEARNING: Basic functionality test with AAA pattern
        
        // Arrange
        var trade = new TradeRecord
        {
            SourceCurrency = "GBP",
            DestinationCurrency = "USD",
            Lots = 10.0f,
            Price = 1.51m
        };

        // Act
        _databaseRepository.InsertTrade(trade);

        // Assert
        var allTrades = _databaseRepository.GetAllTrades();
        Assert.That(allTrades, Has.Count.EqualTo(1));
        Assert.That(allTrades[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(allTrades[0].DestinationCurrency, Is.EqualTo("USD"));
        Assert.That(allTrades[0].Lots, Is.EqualTo(10.0f));
        Assert.That(allTrades[0].Price, Is.EqualTo(1.51m));
    }

    [Test]
    public void InsertTrades_WithMultipleTrades_InsertsAllTradesSuccessfully()
    {
        // LEARNING: Batch operation testing
        
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m },
            new TradeRecord { SourceCurrency = "EUR", DestinationCurrency = "JPY", Lots = 15.0f, Price = 178.13m },
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "JPY", Lots = 8.0f, Price = 188.71m }
        };

        // Act
        _databaseRepository.InsertTrades(trades);

        // Assert
        var allTrades = _databaseRepository.GetAllTrades();
        Assert.That(allTrades, Has.Count.EqualTo(3));
        Assert.That(allTrades[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(allTrades[1].SourceCurrency, Is.EqualTo("EUR"));
        Assert.That(allTrades[2].SourceCurrency, Is.EqualTo("GBP"));
    }

    [Test]
    public void GetAllTrades_WithEmptyDatabase_ReturnsEmptyList()
    {
        // LEARNING: Edge case testing - empty state
        
        // Act
        var result = _databaseRepository.GetAllTrades();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetTradeCount_WithEmptyDatabase_ReturnsZero()
    {
        // LEARNING: Edge case testing - count operations
        
        // Act
        var count = _databaseRepository.GetTradeCount();

        // Assert
        Assert.That(count, Is.EqualTo(0));
    }

    [Test]
    public void GetTradeCount_WithTrades_ReturnsCorrectCount()
    {
        // LEARNING: Count verification after data insertion
        
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
        // LEARNING: Data clearing functionality
        
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
    public void ClearAllTrades_WithEmptyDatabase_DoesNotThrow()
    {
        // LEARNING: Robustness testing - operations on empty state
        
        // Act & Assert
        Assert.DoesNotThrow(() => _databaseRepository.ClearAllTrades());
    }

    [Test]
    public void InsertTrades_WithEmptyList_DoesNotThrow()
    {
        // LEARNING: Edge case testing - empty input handling
        
        // Arrange
        var emptyTrades = new List<TradeRecord>();

        // Act & Assert
        Assert.DoesNotThrow(() => _databaseRepository.InsertTrades(emptyTrades));
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(0));
    }

    [Test]
    public void DatabaseOperations_WithLargeDataSet_HandlesLargeDataSetCorrectly()
    {
        // LEARNING: Performance testing with larger datasets
        
        // Arrange
        var trades = CreateSampleTrades(1000);

        // Act
        _databaseRepository.InsertTrades(trades);

        // Assert
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(1000));
        
        var allTrades = _databaseRepository.GetAllTrades();
        Assert.That(allTrades, Has.Count.EqualTo(1000));
        Assert.That(allTrades[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(allTrades[999].SourceCurrency, Is.EqualTo("NZD"));
    }

    [Test]
    public void InsertTrades_ThenClearThenInsertAgain_WorksCorrectly()
    {
        // LEARNING: Workflow testing - multiple operations in sequence
        
        // Arrange
        var firstBatch = CreateSampleTrades(3);
        var secondBatch = CreateSampleTrades(2);

        // Act - Insert first batch
        _databaseRepository.InsertTrades(firstBatch);
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(3));

        // Act - Clear all trades
        _databaseRepository.ClearAllTrades();
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(0));

        // Act - Insert second batch
        _databaseRepository.InsertTrades(secondBatch);

        // Assert
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(2));
        var allTrades = _databaseRepository.GetAllTrades();
        Assert.That(allTrades, Has.Count.EqualTo(2));
    }

    [Test]
    public void DatabaseRepository_WithCustomPath_CreatesFileAtSpecifiedLocation()
    {
        // LEARNING: Configuration testing - custom database paths
        
        // Arrange
        var customPath = Path.Combine(Path.GetTempPath(), $"custom_test_{Guid.NewGuid()}.db");
        var customRepository = new DatabaseRepository(customPath);

        try
        {
            // Act
            var trade = new TradeRecord
            {
                SourceCurrency = "GBP",
                DestinationCurrency = "USD",
                Lots = 10.0f,
                Price = 1.51m
            };
            customRepository.InsertTrade(trade);

            // Assert
            Assert.That(File.Exists(customPath), Is.True);
            Assert.That(customRepository.GetTradeCount(), Is.EqualTo(1));
        }
        finally
        {
            // Cleanup
            if (File.Exists(customPath))
            {
                File.Delete(customPath);
            }
        }
    }

    [Test]
    public void GetAllTrades_ReturnsTradesWithCorrectProperties()
    {
        // LEARNING: Data integrity testing - verify all properties are preserved
        
        // Arrange
        var originalTrade = new TradeRecord
        {
            SourceCurrency = "GBP",
            DestinationCurrency = "USD",
            Lots = 10.5f,
            Price = 1.51234m
        };

        // Act
        _databaseRepository.InsertTrade(originalTrade);
        var retrievedTrades = _databaseRepository.GetAllTrades();

        // Assert
        Assert.That(retrievedTrades, Has.Count.EqualTo(1));
        var retrievedTrade = retrievedTrades[0];
        
        Assert.That(retrievedTrade.SourceCurrency, Is.EqualTo(originalTrade.SourceCurrency));
        Assert.That(retrievedTrade.DestinationCurrency, Is.EqualTo(originalTrade.DestinationCurrency));
        Assert.That(retrievedTrade.Lots, Is.EqualTo(originalTrade.Lots));
        Assert.That(retrievedTrade.Price, Is.EqualTo(originalTrade.Price));
        Assert.That(retrievedTrade.Id, Is.GreaterThan(0)); // LiteDB assigns auto-incrementing IDs
    }

    [Test]
    public void MultipleOperations_InSequence_MaintainDataIntegrity()
    {
        // LEARNING: Integration testing - complex operation sequences
        
        // Arrange & Act - Perform multiple operations
        var batch1 = CreateSampleTrades(2);
        _databaseRepository.InsertTrades(batch1);
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(2));

        var singleTrade = new TradeRecord { SourceCurrency = "USD", DestinationCurrency = "CHF", Lots = 5.0f, Price = 0.92m };
        _databaseRepository.InsertTrade(singleTrade);
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(3));

        _databaseRepository.ClearAllTrades();
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(0));

        var batch2 = CreateSampleTrades(4);
        _databaseRepository.InsertTrades(batch2);

        // Assert
        Assert.That(_databaseRepository.GetTradeCount(), Is.EqualTo(4));
        var finalTrades = _databaseRepository.GetAllTrades();
        Assert.That(finalTrades, Has.Count.EqualTo(4));
    }

    // LEARNING: Helper methods make tests more readable and maintainable
    // This method creates realistic test data for various test scenarios
    private List<TradeRecord> CreateSampleTrades(int count)
    {
        var trades = new List<TradeRecord>();
        var currencies = new[] { "GBP", "EUR", "USD", "JPY", "CHF", "AUD", "CAD", "NZD" };
        
        for (int i = 0; i < count; i++)
        {
            trades.Add(new TradeRecord
            {
                SourceCurrency = currencies[i % currencies.Length],
                DestinationCurrency = currencies[(i + 1) % currencies.Length],
                Lots = (i + 1) * 0.5f,
                Price = 1.0m + (i * 0.01m)
            });
        }
        
        return trades;
    }
}

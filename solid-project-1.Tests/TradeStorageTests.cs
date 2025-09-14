using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace solid_project_1.Tests;

/*
 * SOLID PRINCIPLES LEARNING GUIDE - TradeStorage Tests
 * 
 * LEARNING OBJECTIVES:
 * - Understand Single Responsibility Principle (SRP) - data persistence only
 * - Practice interface-based programming with ITradeStorage
 * - Learn dependency injection and mocking with Moq framework
 * - Master professional testing patterns with mock verification
 * - Experience Dependency Inversion Principle in action
 * 
 * IMPLEMENTATION STEPS:
 * STEP 1: Read this file to understand what TradeStorage should do
 * STEP 2: Create ITradeStorage interface with Persist(List<TradeRecord>) method
 * STEP 3: Uncomment the first set of tests (marked with STEP 3)
 * STEP 4: Create TradeStorage class implementing ITradeStorage
 * STEP 5: Move StoreTrades() method logic from TradeProcessor to TradeStorage.Persist()
 * STEP 6: Update TradeStorage to depend on IDatabaseRepository (dependency injection)
 * STEP 7: Uncomment remaining tests (marked with STEP 4)
 * STEP 8: Run tests - they should all pass if implementation is correct!
 * 
 * WHAT THIS CLASS SHOULD DO:
 * - Persist List<TradeRecord> to storage (database, file, etc.)
 * - Clear existing data before inserting new data (prevent duplicates)
 * - Depend on IDatabaseRepository interface (not concrete DatabaseRepository)
 * - Log information about processing results
 * - Follow Single Responsibility Principle - ONLY handle data persistence
 * - Follow Dependency Inversion Principle - depend on abstractions
 */

/// <summary>
/// Unit tests for TradeStorage class.
/// Tests the Persist method using mocked dependencies to ensure proper data persistence logic.
/// 
/// INSTRUCTIONS: Uncomment tests as you complete each implementation step:
/// 1. After creating ITradeStorage interface -> Uncomment STEP 3 tests
/// 2. After creating TradeStorage class with IDatabaseRepository dependency -> Uncomment STEP 4 tests
/// 
/// NOTE: These tests use Moq framework to mock IDatabaseRepository dependency.
/// This demonstrates professional testing practices with dependency isolation.
/// </summary>
[TestFixture]
public class TradeStorageTests
{
    // TODO: Uncomment after creating TradeStorage class and IDatabaseRepository interface
    // private TradeStorage _tradeStorage;
    // private Mock<IDatabaseRepository> _mockDatabaseRepository;

    [SetUp]
    public void Setup()
    {
        // TODO: Uncomment after creating TradeStorage class and IDatabaseRepository interface
        // _mockDatabaseRepository = new Mock<IDatabaseRepository>();
        // _tradeStorage = new TradeStorage(_mockDatabaseRepository.Object);
    }

    // ========== STEP 3: UNCOMMENT AFTER CREATING ITradeStorage INTERFACE ==========
    /*
    [Test]
    public void Persist_WithValidTrades_CallsClearAndInsert()
    {
        // LEARNING: This test validates the basic contract of ITradeStorage
        // It shows how to use Moq to verify method calls on dependencies
        
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m },
            new TradeRecord { SourceCurrency = "EUR", DestinationCurrency = "JPY", Lots = 15.0f, Price = 178.13m }
        };

        // Act
        _tradeStorage.Persist(trades);

        // Assert - Verify that the storage clears existing data first
        _mockDatabaseRepository.Verify(x => x.ClearAllTrades(), Times.Once);
        
        // Assert - Verify that the storage inserts the new trades
        _mockDatabaseRepository.Verify(x => x.InsertTrades(trades), Times.Once);
    }

    [Test]
    public void Persist_WithSingleTrade_CallsClearAndInsert()
    {
        // LEARNING: Edge case testing - single item processing
        
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m }
        };

        // Act
        _tradeStorage.Persist(trades);

        // Assert
        _mockDatabaseRepository.Verify(x => x.ClearAllTrades(), Times.Once);
        _mockDatabaseRepository.Verify(x => x.InsertTrades(trades), Times.Once);
    }

    [Test]
    public void Persist_WithEmptyList_CallsClearButNotInsert()
    {
        // LEARNING: Edge case testing - empty input handling
        
        // Arrange
        var trades = new List<TradeRecord>();

        // Act
        _tradeStorage.Persist(trades);

        // Assert
        _mockDatabaseRepository.Verify(x => x.ClearAllTrades(), Times.Once);
        _mockDatabaseRepository.Verify(x => x.InsertTrades(trades), Times.Once);
    }
    */

    // ========== STEP 4: UNCOMMENT AFTER CREATING TradeStorage CLASS ==========
    /*
    [Test]
    public void Persist_CallsMethodsInCorrectOrder()
    {
        // LEARNING: Sequence verification - ensure operations happen in correct order
        
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m }
        };

        var sequence = new MockSequence();
        _mockDatabaseRepository.InSequence(sequence).Setup(x => x.ClearAllTrades());
        _mockDatabaseRepository.InSequence(sequence).Setup(x => x.InsertTrades(trades));

        // Act
        _tradeStorage.Persist(trades);

        // Assert - Verify methods were called in the correct sequence
        _mockDatabaseRepository.Verify(x => x.ClearAllTrades(), Times.Once);
        _mockDatabaseRepository.Verify(x => x.InsertTrades(trades), Times.Once);
    }

    [Test]
    public void Persist_WithLargeDataSet_ProcessesAllTrades()
    {
        // LEARNING: Performance testing with larger datasets
        
        // Arrange
        var trades = CreateSampleTrades(1000);

        // Act
        _tradeStorage.Persist(trades);

        // Assert
        _mockDatabaseRepository.Verify(x => x.ClearAllTrades(), Times.Once);
        _mockDatabaseRepository.Verify(x => x.InsertTrades(It.Is<List<TradeRecord>>(t => t.Count == 1000)), Times.Once);
    }

    [Test]
    public void Persist_WhenDatabaseThrowsException_ExceptionPropagates()
    {
        // LEARNING: Error handling - exceptions should propagate to caller
        
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m }
        };

        _mockDatabaseRepository.Setup(x => x.ClearAllTrades()).Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _tradeStorage.Persist(trades));
    }

    [Test]
    public void Persist_WhenInsertThrowsException_ExceptionPropagates()
    {
        // LEARNING: Error handling - exceptions during insert should propagate
        
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m }
        };

        _mockDatabaseRepository.Setup(x => x.InsertTrades(It.IsAny<List<TradeRecord>>())).Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _tradeStorage.Persist(trades));
    }

    [Test]
    public void Persist_WithNullTrades_ThrowsArgumentNullException()
    {
        // LEARNING: Input validation - null parameters should be handled
        
        // Arrange
        List<TradeRecord> nullTrades = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _tradeStorage.Persist(nullTrades));
    }

    [Test]
    public void Persist_WithValidTrades_PassesExactTradeList()
    {
        // LEARNING: Parameter verification - ensure exact data is passed through
        
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m },
            new TradeRecord { SourceCurrency = "EUR", DestinationCurrency = "JPY", Lots = 15.0f, Price = 178.13m }
        };

        // Act
        _tradeStorage.Persist(trades);

        // Assert - Verify the exact list was passed to the repository
        _mockDatabaseRepository.Verify(x => x.InsertTrades(It.Is<List<TradeRecord>>(
            t => t.Count == 2 &&
                 t[0].SourceCurrency == "GBP" &&
                 t[0].DestinationCurrency == "USD" &&
                 t[1].SourceCurrency == "EUR" &&
                 t[1].DestinationCurrency == "JPY"
        )), Times.Once);
    }

    [Test]
    public void Persist_WithDifferentTradeTypes_HandlesAllTradeTypes()
    {
        // LEARNING: Data variety testing - different trade configurations
        
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m },
            new TradeRecord { SourceCurrency = "EUR", DestinationCurrency = "JPY", Lots = -5.0f, Price = 178.13m }, // Negative lots
            new TradeRecord { SourceCurrency = "USD", DestinationCurrency = "CHF", Lots = 0.0f, Price = 0.92m },    // Zero lots
            new TradeRecord { SourceCurrency = "AUD", DestinationCurrency = "NZD", Lots = 100.0f, Price = 1.07m }   // Large lots
        };

        // Act
        _tradeStorage.Persist(trades);

        // Assert
        _mockDatabaseRepository.Verify(x => x.ClearAllTrades(), Times.Once);
        _mockDatabaseRepository.Verify(x => x.InsertTrades(It.Is<List<TradeRecord>>(t => t.Count == 4)), Times.Once);
    }

    [Test]
    public void Persist_VerifyNoOtherMethodsCalled()
    {
        // LEARNING: Strict verification - ensure only expected methods are called
        
        // Arrange
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m }
        };

        // Act
        _tradeStorage.Persist(trades);

        // Assert - Verify only the expected methods were called
        _mockDatabaseRepository.Verify(x => x.ClearAllTrades(), Times.Once);
        _mockDatabaseRepository.Verify(x => x.InsertTrades(It.IsAny<List<TradeRecord>>()), Times.Once);
        _mockDatabaseRepository.VerifyNoOtherCalls();
    }

    [Test]
    public void Persist_WithMultipleCalls_ClearsEachTime()
    {
        // LEARNING: State management - each call should clear existing data
        
        // Arrange
        var trades1 = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m }
        };
        var trades2 = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "EUR", DestinationCurrency = "JPY", Lots = 15.0f, Price = 178.13m }
        };

        // Act
        _tradeStorage.Persist(trades1);
        _tradeStorage.Persist(trades2);

        // Assert - Verify clear was called twice (once for each persist call)
        _mockDatabaseRepository.Verify(x => x.ClearAllTrades(), Times.Exactly(2));
        _mockDatabaseRepository.Verify(x => x.InsertTrades(trades1), Times.Once);
        _mockDatabaseRepository.Verify(x => x.InsertTrades(trades2), Times.Once);
    }

    // Helper method to create sample trades for testing
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
    */
}

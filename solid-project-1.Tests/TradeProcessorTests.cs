using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Moq;
using NUnit.Framework;

namespace solid_project_1.Tests;

/*
 * SOLID PRINCIPLES LEARNING GUIDE - TradeProcessor Tests
 * 
 * LEARNING OBJECTIVES:
 * - Understand Single Responsibility Principle (SRP) - orchestration only
 * - Practice Dependency Inversion Principle (DIP) - depend on interfaces
 * - Learn constructor injection and dependency management
 * - Master integration testing with multiple mocked dependencies
 * - Experience professional orchestration patterns
 * 
 * IMPLEMENTATION STEPS:
 * STEP 1: Read this file to understand what the refactored TradeProcessor should do
 * STEP 2: Create all interfaces (ITradeDataProvider, ITradeParser, ITradeStorage)
 * STEP 3: Create all implementation classes
 * STEP 4: Refactor TradeProcessor to use constructor injection
 * STEP 5: Change ProcessTrades from static to instance method
 * STEP 6: Uncomment all tests (marked with STEP 5)
 * STEP 7: Run tests - they should all pass if refactoring is correct!
 * 
 * WHAT THE REFACTORED TradeProcessor SHOULD DO:
 * - Accept dependencies through constructor injection (ITradeDataProvider, ITradeParser, ITradeStorage)
 * - Orchestrate the trade processing workflow: data → parsing → storage
 * - NOT contain any business logic (that's moved to the individual classes)
 * - Follow Single Responsibility Principle - ONLY handle orchestration
 * - Follow Dependency Inversion Principle - depend on interfaces, not concrete classes
 */

/// <summary>
/// Unit tests for the refactored TradeProcessor class.
/// Tests the ProcessTrades method using mocked dependencies to ensure proper orchestration.
/// 
/// INSTRUCTIONS: Uncomment tests after completing TradeProcessor refactoring:
/// 1. After refactoring TradeProcessor with dependency injection -> Uncomment STEP 5 tests
/// 
/// NOTE: These tests use Moq framework to mock all dependencies.
/// This demonstrates professional integration testing with complete dependency isolation.
/// </summary>
[TestFixture]
public class TradeProcessorTests
{
    // TODO: Uncomment after refactoring TradeProcessor with dependency injection
    // private TradeProcessor _tradeProcessor;
    // private Mock<ITradeDataProvider> _mockTradeDataProvider;
    // private Mock<ITradeParser> _mockTradeParser;
    // private Mock<ITradeStorage> _mockTradeStorage;

    [SetUp]
    public void Setup()
    {
        // TODO: Uncomment after refactoring TradeProcessor with dependency injection
        // _mockTradeDataProvider = new Mock<ITradeDataProvider>();
        // _mockTradeParser = new Mock<ITradeParser>();
        // _mockTradeStorage = new Mock<ITradeStorage>();
        // _tradeProcessor = new TradeProcessor(_mockTradeDataProvider.Object, _mockTradeParser.Object, _mockTradeStorage.Object);
    }

    // ========== STEP 5: UNCOMMENT AFTER REFACTORING TradeProcessor WITH DEPENDENCY INJECTION ==========
    /*
    [Test]
    public void ProcessTrades_WithValidStream_CallsAllDependenciesInCorrectOrder()
    {
        // LEARNING: This test validates the orchestration logic of TradeProcessor
        // It shows how to test workflow coordination using multiple mocks
        
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("GBPUSD,1000,1.51"));
        var lines = new List<string> { "GBPUSD,1000,1.51" };
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m }
        };

        // Setup mocks to return expected data
        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.Setup(x => x.Parse(lines)).Returns(trades);
        _mockTradeStorage.Setup(x => x.Persist(trades));

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert - Verify each dependency was called exactly once with correct parameters
        _mockTradeDataProvider.Verify(x => x.GetTradeData(stream), Times.Once);
        _mockTradeParser.Verify(x => x.Parse(lines), Times.Once);
        _mockTradeStorage.Verify(x => x.Persist(trades), Times.Once);
    }

    [Test]
    public void ProcessTrades_CallsMethodsInCorrectSequence()
    {
        // LEARNING: Sequence verification - ensure workflow steps happen in correct order
        
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("GBPUSD,1000,1.51"));
        var lines = new List<string> { "GBPUSD,1000,1.51" };
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m }
        };

        var sequence = new MockSequence();
        _mockTradeDataProvider.InSequence(sequence).Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.InSequence(sequence).Setup(x => x.Parse(lines)).Returns(trades);
        _mockTradeStorage.InSequence(sequence).Setup(x => x.Persist(trades));

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert - Verify methods were called in the correct sequence
        _mockTradeDataProvider.Verify(x => x.GetTradeData(stream), Times.Once);
        _mockTradeParser.Verify(x => x.Parse(lines), Times.Once);
        _mockTradeStorage.Verify(x => x.Persist(trades), Times.Once);
    }

    [Test]
    public void ProcessTrades_WithEmptyStream_StillCallsAllDependencies()
    {
        // LEARNING: Edge case testing - empty input should still follow complete workflow
        
        // Arrange
        var stream = new MemoryStream();
        var emptyLines = new List<string>();
        var emptyTrades = new List<TradeRecord>();

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(emptyLines);
        _mockTradeParser.Setup(x => x.Parse(emptyLines)).Returns(emptyTrades);
        _mockTradeStorage.Setup(x => x.Persist(emptyTrades));

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert
        _mockTradeDataProvider.Verify(x => x.GetTradeData(stream), Times.Once);
        _mockTradeParser.Verify(x => x.Parse(emptyLines), Times.Once);
        _mockTradeStorage.Verify(x => x.Persist(emptyTrades), Times.Once);
    }

    [Test]
    public void ProcessTrades_WithLargeDataSet_ProcessesAllData()
    {
        // LEARNING: Performance testing - large datasets should be handled correctly
        
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("large data"));
        var largeLinesList = CreateSampleLines(1000);
        var largeTradesList = CreateSampleTrades(1000);

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(largeLinesList);
        _mockTradeParser.Setup(x => x.Parse(largeLinesList)).Returns(largeTradesList);
        _mockTradeStorage.Setup(x => x.Persist(largeTradesList));

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert
        _mockTradeDataProvider.Verify(x => x.GetTradeData(stream), Times.Once);
        _mockTradeParser.Verify(x => x.Parse(It.Is<List<string>>(l => l.Count == 1000)), Times.Once);
        _mockTradeStorage.Verify(x => x.Persist(It.Is<List<TradeRecord>>(t => t.Count == 1000)), Times.Once);
    }

    [Test]
    public void ProcessTrades_WhenDataProviderThrowsException_ExceptionPropagates()
    {
        // LEARNING: Error handling - exceptions from dependencies should propagate
        
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("GBPUSD,1000,1.51"));
        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _tradeProcessor.ProcessTrades(stream));
        
        // Verify that subsequent dependencies were not called
        _mockTradeParser.Verify(x => x.Parse(It.IsAny<List<string>>()), Times.Never);
        _mockTradeStorage.Verify(x => x.Persist(It.IsAny<List<TradeRecord>>()), Times.Never);
    }

    [Test]
    public void ProcessTrades_WhenParserThrowsException_ExceptionPropagates()
    {
        // LEARNING: Error handling - exceptions during parsing should propagate
        
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("GBPUSD,1000,1.51"));
        var lines = new List<string> { "GBPUSD,1000,1.51" };

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.Setup(x => x.Parse(lines)).Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _tradeProcessor.ProcessTrades(stream));
        
        // Verify that storage was not called
        _mockTradeStorage.Verify(x => x.Persist(It.IsAny<List<TradeRecord>>()), Times.Never);
    }

    [Test]
    public void ProcessTrades_WhenStorageThrowsException_ExceptionPropagates()
    {
        // LEARNING: Error handling - exceptions during storage should propagate
        
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("GBPUSD,1000,1.51"));
        var lines = new List<string> { "GBPUSD,1000,1.51" };
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m }
        };

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.Setup(x => x.Parse(lines)).Returns(trades);
        _mockTradeStorage.Setup(x => x.Persist(trades)).Throws<InvalidOperationException>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _tradeProcessor.ProcessTrades(stream));
    }

    [Test]
    public void ProcessTrades_WithNullStream_PassesNullToDataProvider()
    {
        // LEARNING: Parameter passing - null parameters should be passed through
        
        // Arrange
        Stream nullStream = null;
        _mockTradeDataProvider.Setup(x => x.GetTradeData(nullStream)).Throws<ArgumentNullException>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _tradeProcessor.ProcessTrades(nullStream));
    }

    [Test]
    public void ProcessTrades_PassesDataBetweenDependenciesCorrectly()
    {
        // LEARNING: Data flow verification - ensure data flows correctly through the pipeline
        
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("test data"));
        var expectedLines = new List<string> { "line1", "line2", "line3" };
        var expectedTrades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m },
            new TradeRecord { SourceCurrency = "EUR", DestinationCurrency = "JPY", Lots = 15.0f, Price = 178.13m }
        };

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(expectedLines);
        _mockTradeParser.Setup(x => x.Parse(expectedLines)).Returns(expectedTrades);
        _mockTradeStorage.Setup(x => x.Persist(expectedTrades));

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert - Verify exact data was passed between dependencies
        _mockTradeDataProvider.Verify(x => x.GetTradeData(stream), Times.Once);
        _mockTradeParser.Verify(x => x.Parse(expectedLines), Times.Once);
        _mockTradeStorage.Verify(x => x.Persist(expectedTrades), Times.Once);
    }

    [Test]
    public void ProcessTrades_VerifyNoOtherMethodsCalled()
    {
        // LEARNING: Strict verification - ensure only expected methods are called
        
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("GBPUSD,1000,1.51"));
        var lines = new List<string> { "GBPUSD,1000,1.51" };
        var trades = new List<TradeRecord>
        {
            new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m }
        };

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream)).Returns(lines);
        _mockTradeParser.Setup(x => x.Parse(lines)).Returns(trades);
        _mockTradeStorage.Setup(x => x.Persist(trades));

        // Act
        _tradeProcessor.ProcessTrades(stream);

        // Assert - Verify only the expected methods were called
        _mockTradeDataProvider.VerifyNoOtherCalls();
        _mockTradeParser.VerifyNoOtherCalls();
        _mockTradeStorage.VerifyNoOtherCalls();
    }

    [Test]
    public void ProcessTrades_WithMultipleCalls_EachCallIsIndependent()
    {
        // LEARNING: State management - each call should be independent
        
        // Arrange
        var stream1 = new MemoryStream(Encoding.UTF8.GetBytes("data1"));
        var stream2 = new MemoryStream(Encoding.UTF8.GetBytes("data2"));
        var lines1 = new List<string> { "line1" };
        var lines2 = new List<string> { "line2" };
        var trades1 = new List<TradeRecord> { new TradeRecord { SourceCurrency = "GBP", DestinationCurrency = "USD", Lots = 10.0f, Price = 1.51m } };
        var trades2 = new List<TradeRecord> { new TradeRecord { SourceCurrency = "EUR", DestinationCurrency = "JPY", Lots = 15.0f, Price = 178.13m } };

        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream1)).Returns(lines1);
        _mockTradeDataProvider.Setup(x => x.GetTradeData(stream2)).Returns(lines2);
        _mockTradeParser.Setup(x => x.Parse(lines1)).Returns(trades1);
        _mockTradeParser.Setup(x => x.Parse(lines2)).Returns(trades2);
        _mockTradeStorage.Setup(x => x.Persist(trades1));
        _mockTradeStorage.Setup(x => x.Persist(trades2));

        // Act
        _tradeProcessor.ProcessTrades(stream1);
        _tradeProcessor.ProcessTrades(stream2);

        // Assert - Verify each call was handled independently
        _mockTradeDataProvider.Verify(x => x.GetTradeData(stream1), Times.Once);
        _mockTradeDataProvider.Verify(x => x.GetTradeData(stream2), Times.Once);
        _mockTradeParser.Verify(x => x.Parse(lines1), Times.Once);
        _mockTradeParser.Verify(x => x.Parse(lines2), Times.Once);
        _mockTradeStorage.Verify(x => x.Persist(trades1), Times.Once);
        _mockTradeStorage.Verify(x => x.Persist(trades2), Times.Once);
    }

    // Helper methods to create sample data for testing
    private List<string> CreateSampleLines(int count)
    {
        var lines = new List<string>();
        for (int i = 0; i < count; i++)
        {
            lines.Add($"GBPUSD,{1000 + i},{1.51 + (i * 0.001):F3}");
        }
        return lines;
    }

    private List<TradeRecord> CreateSampleTrades(int count)
    {
        var trades = new List<TradeRecord>();
        for (int i = 0; i < count; i++)
        {
            trades.Add(new TradeRecord
            {
                SourceCurrency = "GBP",
                DestinationCurrency = "USD",
                Lots = (1000 + i) / 100.0f,
                Price = 1.51m + (i * 0.001m)
            });
        }
        return trades;
    }
    */
}

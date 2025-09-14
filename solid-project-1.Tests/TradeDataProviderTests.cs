using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace solid_project_1.Tests;

/*
 * SOLID PRINCIPLES LEARNING GUIDE - TradeDataProvider Tests
 * 
 * LEARNING OBJECTIVES:
 * - Understand Single Responsibility Principle (SRP) - data reading only
 * - Practice interface-based programming with ITradeDataProvider
 * - Learn professional unit testing patterns with edge cases
 * - Experience Test-Driven Development workflow
 * - Master stream handling and data extraction techniques
 * 
 * IMPLEMENTATION STEPS:
 * STEP 1: Read this file to understand what TradeDataProvider should do
 * STEP 2: Create ITradeDataProvider interface with GetTradeData(Stream) method
 * STEP 3: Uncomment the first set of tests (marked with STEP 3)
 * STEP 4: Create TradeDataProvider class implementing ITradeDataProvider
 * STEP 5: Move ReadTradData() method from TradeProcessor to TradeDataProvider.GetTradeData()
 * STEP 6: Uncomment remaining tests (marked with STEP 4)
 * STEP 7: Run tests - they should all pass if implementation is correct!
 * 
 * WHAT THIS CLASS SHOULD DO:
 * - Read data from a Stream (file, network, memory, etc.)
 * - Return List<string> where each string is a line from the stream
 * - Handle various stream conditions (empty, large, disposed, etc.)
 * - Follow Single Responsibility Principle - ONLY handle data reading
 */

/// <summary>
/// Unit tests for TradeDataProvider class.
/// Tests the GetTradeData method with various Stream scenarios to ensure proper data extraction.
/// 
/// INSTRUCTIONS: Uncomment tests as you complete each implementation step:
/// 1. After creating ITradeDataProvider interface -> Uncomment STEP 3 tests
/// 2. After creating TradeDataProvider class -> Uncomment STEP 4 tests
/// </summary>
[TestFixture]
public class TradeDataProviderTests
{
    // TODO: Uncomment after creating TradeDataProvider class
    // private TradeDataProvider _tradeDataProvider;

    [SetUp]
    public void Setup()
    {
        // TODO: Uncomment after creating TradeDataProvider class
        // _tradeDataProvider = new TradeDataProvider();
    }

    // ========== STEP 3: UNCOMMENT AFTER CREATING ITradeDataProvider INTERFACE ==========
    /*
    [Test]
    public void GetTradeData_WithValidTradeData_ReturnsCorrectLines()
    {
        // LEARNING: This test validates the basic contract of ITradeDataProvider
        // It shows how to test stream reading with realistic trade data
        
        // Arrange
        var testData = "GBPUSD,1000,1.51\nEURJPY,1500,178.13\nGBPJPY,800,188.71";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));

        // Act
        var result = _tradeDataProvider.GetTradeData(stream);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result[0], Is.EqualTo("GBPUSD,1000,1.51"));
        Assert.That(result[1], Is.EqualTo("EURJPY,1500,178.13"));
        Assert.That(result[2], Is.EqualTo("GBPJPY,800,188.71"));
    }

    [Test]
    public void GetTradeData_WithSingleLine_ReturnsSingleItem()
    {
        // LEARNING: Edge case testing - single line input
        
        // Arrange
        var testData = "GBPUSD,1000,1.51";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));

        // Act
        var result = _tradeDataProvider.GetTradeData(stream);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0], Is.EqualTo("GBPUSD,1000,1.51"));
    }

    [Test]
    public void GetTradeData_WithEmptyStream_ReturnsEmptyList()
    {
        // LEARNING: Edge case testing - empty input handling
        
        // Arrange
        var stream = new MemoryStream();

        // Act
        var result = _tradeDataProvider.GetTradeData(stream);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }
    */

    // ========== STEP 4: UNCOMMENT AFTER CREATING TradeDataProvider CLASS ==========
    /*
    [Test]
    public void GetTradeData_WithEmptyLines_ReturnsAllLines()
    {
        // LEARNING: TradeDataProvider should return ALL lines, including empty ones
        // The TradeParser will handle filtering invalid data later (SRP!)
        
        // Arrange
        var testData = "GBPUSD,1000,1.51\n\n\nEURJPY,1500,178.13\n\n";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));

        // Act
        var result = _tradeDataProvider.GetTradeData(stream);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(5)); // Includes empty lines
        Assert.That(result[0], Is.EqualTo("GBPUSD,1000,1.51"));
        Assert.That(result[1], Is.EqualTo(""));
        Assert.That(result[2], Is.EqualTo(""));
        Assert.That(result[3], Is.EqualTo("EURJPY,1500,178.13"));
        Assert.That(result[4], Is.EqualTo(""));
    }

    [Test]
    public void GetTradeData_WithWhitespaceOnlyLines_ReturnsAllLines()
    {
        // LEARNING: Data provider doesn't filter - that's the parser's job!
        
        // Arrange
        var testData = "GBPUSD,1000,1.51\n   \n\t\nEURJPY,1500,178.13\n  \t  ";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));

        // Act
        var result = _tradeDataProvider.GetTradeData(stream);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(5)); // Includes whitespace lines
        Assert.That(result[0], Is.EqualTo("GBPUSD,1000,1.51"));
        Assert.That(result[1], Is.EqualTo("   "));
        Assert.That(result[2], Is.EqualTo("\t"));
        Assert.That(result[3], Is.EqualTo("EURJPY,1500,178.13"));
        Assert.That(result[4], Is.EqualTo("  \t  "));
    }

    [Test]
    public void GetTradeData_WithMixedLineEndings_HandlesAllLineEndings()
    {
        // LEARNING: Professional applications must handle different line endings
        
        // Arrange - Mix of \n, \r\n, and \r line endings
        var testData = "GBPUSD,1000,1.51\nEURJPY,1500,178.13\r\nGBPJPY,800,188.71\rUSDJPY,2000,110.25";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));

        // Act
        var result = _tradeDataProvider.GetTradeData(stream);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(4));
        Assert.That(result[0], Is.EqualTo("GBPUSD,1000,1.51"));
        Assert.That(result[1], Is.EqualTo("EURJPY,1500,178.13"));
        Assert.That(result[2], Is.EqualTo("GBPJPY,800,188.71"));
        Assert.That(result[3], Is.EqualTo("USDJPY,2000,110.25"));
    }

    [Test]
    public void GetTradeData_WithLargeDataSet_ProcessesAllLines()
    {
        // LEARNING: Performance testing with larger datasets
        
        // Arrange
        var lines = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
            lines.Add($"GBPUSD,{1000 + i},{1.51 + (i * 0.001):F3}");
        }
        var testData = string.Join("\n", lines);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));

        // Act
        var result = _tradeDataProvider.GetTradeData(stream);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1000));
        Assert.That(result[0], Is.EqualTo("GBPUSD,1000,1.510"));
        Assert.That(result[999], Is.EqualTo("GBPUSD,1999,2.509"));
    }

    [Test]
    public void GetTradeData_WithNullStream_ThrowsArgumentNullException()
    {
        // LEARNING: Proper error handling for invalid inputs
        
        // Arrange
        Stream nullStream = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _tradeDataProvider.GetTradeData(nullStream));
    }

    [Test]
    public void GetTradeData_WithDisposedStream_ThrowsArgumentException()
    {
        // LEARNING: Error handling for disposed resources
        
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("GBPUSD,1000,1.51"));
        stream.Dispose();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _tradeDataProvider.GetTradeData(stream));
    }

    [Test]
    public void GetTradeData_WithSpecialCharacters_PreservesSpecialCharacters()
    {
        // LEARNING: Data integrity - preserve all characters exactly as received
        
        // Arrange
        var testData = "GBP/USD,1000,1.51\nEUR-JPY,1500,178.13\nGBP_JPY,800,188.71";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));

        // Act
        var result = _tradeDataProvider.GetTradeData(stream);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result[0], Is.EqualTo("GBP/USD,1000,1.51"));
        Assert.That(result[1], Is.EqualTo("EUR-JPY,1500,178.13"));
        Assert.That(result[2], Is.EqualTo("GBP_JPY,800,188.71"));
    }

    [Test]
    public void GetTradeData_WithUnicodeCharacters_HandlesUnicodeCorrectly()
    {
        // LEARNING: International character support
        
        // Arrange
        var testData = "GBPUSD,1000,1.51\nEURJPY,1500,178.13\n测试,800,188.71";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(testData));

        // Act
        var result = _tradeDataProvider.GetTradeData(stream);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result[0], Is.EqualTo("GBPUSD,1000,1.51"));
        Assert.That(result[1], Is.EqualTo("EURJPY,1500,178.13"));
        Assert.That(result[2], Is.EqualTo("测试,800,188.71"));
    }
    */
}

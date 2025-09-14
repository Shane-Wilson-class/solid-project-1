using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace solid_project_1.Tests;

/*
 * SOLID PRINCIPLES LEARNING GUIDE - TradeParser Tests
 * 
 * LEARNING OBJECTIVES:
 * - Understand Single Responsibility Principle (SRP) - parsing and validation only
 * - Practice interface-based programming with ITradeParser
 * - Learn data validation and transformation techniques
 * - Master error handling and edge case management
 * - Experience professional parsing patterns
 * 
 * IMPLEMENTATION STEPS:
 * STEP 1: Read this file to understand what TradeParser should do
 * STEP 2: Create ITradeParser interface with Parse(List<string>) method
 * STEP 3: Uncomment the first set of tests (marked with STEP 3)
 * STEP 4: Create TradeParser class implementing ITradeParser
 * STEP 5: Move Parse(), MapTradeDataToTradeRecord(), ValidateTradeData() methods from TradeProcessor
 * STEP 6: Move LotSize constant to TradeParser class
 * STEP 7: Uncomment remaining tests (marked with STEP 4)
 * STEP 8: Run tests - they should all pass if implementation is correct!
 * 
 * WHAT THIS CLASS SHOULD DO:
 * - Parse List<string> into List<TradeRecord>
 * - Validate trade data format and values
 * - Skip invalid lines with appropriate logging
 * - Transform currency pairs, amounts, and prices correctly
 * - Follow Single Responsibility Principle - ONLY handle parsing and validation
 */

/// <summary>
/// Unit tests for TradeParser class.
/// Tests the Parse method with various input scenarios including valid data, malformed input, and edge cases.
/// 
/// INSTRUCTIONS: Uncomment tests as you complete each implementation step:
/// 1. After creating ITradeParser interface -> Uncomment STEP 3 tests
/// 2. After creating TradeParser class -> Uncomment STEP 4 tests
/// </summary>
[TestFixture]
public class TradeParserTests
{
    // TODO: Uncomment after creating TradeParser class
    // private TradeParser _tradeParser;

    [SetUp]
    public void Setup()
    {
        // TODO: Uncomment after creating TradeParser class
        // _tradeParser = new TradeParser();
    }

    // ========== STEP 3: UNCOMMENT AFTER CREATING ITradeParser INTERFACE ==========
    /*
    [Test]
    public void Parse_WithValidTradeData_ReturnsCorrectTradeRecords()
    {
        // LEARNING: This test validates the basic contract of ITradeParser
        // It shows how to test data transformation from strings to objects
        
        // Arrange
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",
            "EURJPY,1500,178.13",
            "GBPJPY,800,188.71"
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        
        // Verify first trade
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[0].DestinationCurrency, Is.EqualTo("USD"));
        Assert.That(result[0].Lots, Is.EqualTo(10.0f)); // 1000 / 100 (LotSize)
        Assert.That(result[0].Price, Is.EqualTo(1.51m));
        
        // Verify second trade
        Assert.That(result[1].SourceCurrency, Is.EqualTo("EUR"));
        Assert.That(result[1].DestinationCurrency, Is.EqualTo("JPY"));
        Assert.That(result[1].Lots, Is.EqualTo(15.0f)); // 1500 / 100
        Assert.That(result[1].Price, Is.EqualTo(178.13m));
    }

    [Test]
    public void Parse_WithSingleValidTrade_ReturnsSingleTradeRecord()
    {
        // LEARNING: Edge case testing - single item processing
        
        // Arrange
        var lines = new List<string> { "GBPUSD,1000,1.51" };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[0].DestinationCurrency, Is.EqualTo("USD"));
        Assert.That(result[0].Lots, Is.EqualTo(10.0f));
        Assert.That(result[0].Price, Is.EqualTo(1.51m));
    }

    [Test]
    public void Parse_WithEmptyList_ReturnsEmptyList()
    {
        // LEARNING: Edge case testing - empty input handling
        
        // Arrange
        var lines = new List<string>();

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }
    */

    // ========== STEP 4: UNCOMMENT AFTER CREATING TradeParser CLASS ==========
    /*
    [Test]
    public void Parse_WithMalformedLines_SkipsInvalidLinesAndContinues()
    {
        // LEARNING: Error handling - skip invalid data but continue processing
        
        // Arrange
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",      // Valid
            "INVALID",               // Invalid - too few fields
            "EURJPY,1500,178.13",    // Valid
            "GBPJPY,800",           // Invalid - missing price
            "USDJPY,2000,110.25"    // Valid
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3)); // Only valid trades
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[1].SourceCurrency, Is.EqualTo("EUR"));
        Assert.That(result[2].SourceCurrency, Is.EqualTo("USD"));
    }

    [Test]
    public void Parse_WithInvalidCurrencyPairs_SkipsInvalidCurrencyPairs()
    {
        // LEARNING: Validation logic - currency pairs must be exactly 6 characters
        
        // Arrange
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",      // Valid - 6 characters
            "GBP,1000,1.51",         // Invalid - too short
            "GBPUSDEUR,1000,1.51",   // Invalid - too long
            "EURJPY,1500,178.13"     // Valid - 6 characters
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2)); // Only valid currency pairs
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[1].SourceCurrency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Parse_WithInvalidAmounts_SkipsInvalidAmounts()
    {
        // LEARNING: Validation logic - amounts must be valid integers
        
        // Arrange
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",      // Valid
            "EURJPY,abc,178.13",     // Invalid - non-numeric amount
            "GBPJPY,1000.5,188.71",  // Invalid - decimal amount
            "USDJPY,2000,110.25"     // Valid
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2)); // Only valid amounts
        Assert.That(result[0].Lots, Is.EqualTo(10.0f));
        Assert.That(result[1].Lots, Is.EqualTo(20.0f));
    }

    [Test]
    public void Parse_WithInvalidPrices_SkipsInvalidPrices()
    {
        // LEARNING: Validation logic - prices must be valid decimals
        
        // Arrange
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",      // Valid
            "EURJPY,1500,abc",       // Invalid - non-numeric price
            "GBPJPY,800,",           // Invalid - empty price
            "USDJPY,2000,110.25"     // Valid
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2)); // Only valid prices
        Assert.That(result[0].Price, Is.EqualTo(1.51m));
        Assert.That(result[1].Price, Is.EqualTo(110.25m));
    }

    [Test]
    public void Parse_WithNegativeAmounts_AllowsNegativeAmounts()
    {
        // LEARNING: Business logic - negative amounts might be valid (short positions)
        
        // Arrange
        var lines = new List<string>
        {
            "GBPUSD,-1000,1.51",
            "EURJPY,-1500,178.13"
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Lots, Is.EqualTo(-10.0f)); // -1000 / 100
        Assert.That(result[1].Lots, Is.EqualTo(-15.0f)); // -1500 / 100
    }

    [Test]
    public void Parse_WithNegativePrices_AllowsNegativePrices()
    {
        // LEARNING: Business logic - negative prices might be valid in some markets
        
        // Arrange
        var lines = new List<string>
        {
            "GBPUSD,1000,-1.51",
            "EURJPY,1500,-178.13"
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Price, Is.EqualTo(-1.51m));
        Assert.That(result[1].Price, Is.EqualTo(-178.13m));
    }

    [Test]
    public void Parse_WithZeroAmounts_AllowsZeroAmounts()
    {
        // LEARNING: Edge case - zero amounts might be valid
        
        // Arrange
        var lines = new List<string>
        {
            "GBPUSD,0,1.51",
            "EURJPY,0,178.13"
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Lots, Is.EqualTo(0.0f));
        Assert.That(result[1].Lots, Is.EqualTo(0.0f));
    }

    [Test]
    public void Parse_WithZeroPrices_AllowsZeroPrices()
    {
        // LEARNING: Edge case - zero prices might be valid
        
        // Arrange
        var lines = new List<string>
        {
            "GBPUSD,1000,0",
            "EURJPY,1500,0.00"
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Price, Is.EqualTo(0.0m));
        Assert.That(result[1].Price, Is.EqualTo(0.0m));
    }

    [Test]
    public void Parse_WithLargeDataSet_ProcessesAllValidTrades()
    {
        // LEARNING: Performance testing with larger datasets
        
        // Arrange
        var lines = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
            lines.Add($"GBPUSD,{1000 + i},{1.51 + (i * 0.001):F3}");
        }

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1000));
        Assert.That(result[0].Lots, Is.EqualTo(10.0f)); // 1000 / 100
        Assert.That(result[999].Lots, Is.EqualTo(19.99f)); // 1999 / 100
    }

    [Test]
    public void Parse_WithEmptyLines_SkipsEmptyLines()
    {
        // LEARNING: Data cleaning - skip empty lines
        
        // Arrange
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",
            "",
            "   ",
            "EURJPY,1500,178.13",
            "\t\t",
            "GBPJPY,800,188.71"
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3)); // Only non-empty valid lines
    }

    [Test]
    public void Parse_WithExtraFields_SkipsLinesWithTooManyFields()
    {
        // LEARNING: Strict validation - exactly 3 fields required
        
        // Arrange
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",           // Valid - 3 fields
            "EURJPY,1500,178.13,extra",   // Invalid - 4 fields
            "GBPJPY,800,188.71"           // Valid - 3 fields
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2)); // Only lines with exactly 3 fields
    }

    [Test]
    public void Parse_WithNullInput_ThrowsNullReferenceException()
    {
        // LEARNING: Error handling for null inputs
        
        // Arrange
        List<string> nullLines = null;

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _tradeParser.Parse(nullLines));
    }

    [Test]
    public void Parse_CalculatesLotsCorrectly_UsingLotSizeConstant()
    {
        // LEARNING: Business logic - lots calculation using LotSize constant (100)
        
        // Arrange
        var lines = new List<string>
        {
            "GBPUSD,100,1.51",    // 100 / 100 = 1.0 lots
            "EURJPY,1500,178.13", // 1500 / 100 = 15.0 lots
            "GBPJPY,50,188.71"    // 50 / 100 = 0.5 lots
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result[0].Lots, Is.EqualTo(1.0f));
        Assert.That(result[1].Lots, Is.EqualTo(15.0f));
        Assert.That(result[2].Lots, Is.EqualTo(0.5f));
    }
    */
}

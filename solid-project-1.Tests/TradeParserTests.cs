using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace solid_project_1.Tests;

/// <summary>
/// Unit tests for TradeParser class.
/// Tests the Parse method with various input scenarios including valid data, malformed data, and edge cases.
/// </summary>
[TestFixture]
public class TradeParserTests
{
    private TradeParser _tradeParser;

    [SetUp]
    public void Setup()
    {
        _tradeParser = new TradeParser();
    }

    [Test]
    public void Parse_WithValidTradeData_ReturnsCorrectTradeRecords()
    {
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
        Assert.That(result[0].Lots, Is.EqualTo(10)); // 1000/100
        Assert.That(result[0].Price, Is.EqualTo(1.51m));

        // Verify second trade
        Assert.That(result[1].SourceCurrency, Is.EqualTo("EUR"));
        Assert.That(result[1].DestinationCurrency, Is.EqualTo("JPY"));
        Assert.That(result[1].Lots, Is.EqualTo(15)); // 1500/100
        Assert.That(result[1].Price, Is.EqualTo(178.13m));

        // Verify third trade
        Assert.That(result[2].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[2].DestinationCurrency, Is.EqualTo("JPY"));
        Assert.That(result[2].Lots, Is.EqualTo(8)); // 800/100
        Assert.That(result[2].Price, Is.EqualTo(188.71m));
    }

    [Test]
    public void Parse_WithSingleValidTrade_ReturnsSingleTradeRecord()
    {
        // Arrange
        var lines = new List<string> { "GBPUSD,1000,1.51" };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[0].DestinationCurrency, Is.EqualTo("USD"));
        Assert.That(result[0].Lots, Is.EqualTo(10));
        Assert.That(result[0].Price, Is.EqualTo(1.51m));
    }

    [Test]
    public void Parse_WithEmptyList_ReturnsEmptyList()
    {
        // Arrange
        var lines = new List<string>();

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Parse_WithNullList_ThrowsNullReferenceException()
    {
        // Arrange
        List<string> lines = null;

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _tradeParser.Parse(lines));
    }

    [Test]
    public void Parse_WithInvalidCurrencyCode_SkipsInvalidTrade()
    {
        // Arrange - Invalid currency code (not 6 characters)
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",    // Valid
            "INVALID,1500,178.13", // Invalid - not 6 chars
            "EURJPY,800,188.71"    // Valid
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[1].SourceCurrency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Parse_WithInvalidAmount_SkipsInvalidTrade()
    {
        // Arrange - Invalid amount (non-numeric)
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",      // Valid
            "EURJPY,INVALID,178.13", // Invalid amount
            "GBPJPY,800,188.71"      // Valid
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[1].SourceCurrency, Is.EqualTo("GBP"));
    }

    [Test]
    public void Parse_WithInvalidPrice_SkipsInvalidTrade()
    {
        // Arrange - Invalid price (non-decimal)
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",    // Valid
            "EURJPY,1500,INVALID", // Invalid price
            "GBPJPY,800,188.71"    // Valid
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[1].SourceCurrency, Is.EqualTo("GBP"));
    }

    [Test]
    public void Parse_WithWrongFieldCount_SkipsInvalidTrade()
    {
        // Arrange - Wrong number of fields
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",        // Valid (3 fields)
            "EURJPY,1500",             // Invalid (2 fields)
            "GBPJPY,800,188.71,EXTRA", // Invalid (4 fields)
            "USDJPY,2000,110.25"       // Valid (3 fields)
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[1].SourceCurrency, Is.EqualTo("USD"));
    }

    [Test]
    public void Parse_WithZeroAmount_IncludesZeroAmountTrade()
    {
        // Arrange - Zero amount is actually valid in the current implementation
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51", // Valid
            "EURJPY,0,178.13",  // Valid - zero amount allowed
            "GBPJPY,800,188.71" // Valid
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[1].SourceCurrency, Is.EqualTo("EUR"));
        Assert.That(result[1].Lots, Is.EqualTo(0)); // Zero lots from zero amount
        Assert.That(result[2].SourceCurrency, Is.EqualTo("GBP"));
    }

    [Test]
    public void Parse_WithNegativeAmount_IncludesNegativeAmountTrade()
    {
        // Arrange - Negative amount is actually valid in the current implementation
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",  // Valid
            "EURJPY,-1500,178.13", // Valid - negative amount allowed
            "GBPJPY,800,188.71"   // Valid
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[1].SourceCurrency, Is.EqualTo("EUR"));
        Assert.That(result[1].Lots, Is.EqualTo(-15)); // Negative lots from negative amount
        Assert.That(result[2].SourceCurrency, Is.EqualTo("GBP"));
    }

    [Test]
    public void Parse_WithNegativePrice_IncludesNegativePriceTrade()
    {
        // Arrange - Negative price is actually valid in the current implementation
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",   // Valid
            "EURJPY,1500,-178.13", // Valid - negative price allowed
            "GBPJPY,800,188.71"    // Valid
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[1].SourceCurrency, Is.EqualTo("EUR"));
        Assert.That(result[1].Price, Is.EqualTo(-178.13m)); // Negative price allowed
        Assert.That(result[2].SourceCurrency, Is.EqualTo("GBP"));
    }

    [Test]
    public void Parse_WithMixedValidAndInvalidTrades_ReturnsOnlyValidTrades()
    {
        // Arrange - Mix of valid and various invalid trades
        var lines = new List<string>
        {
            "GBPUSD,1000,1.51",      // Valid
            "INVALID",               // Invalid - wrong field count
            "EURJPY,INVALID,178.13", // Invalid - non-numeric amount
            "GBPJPY,800,INVALID",    // Invalid - non-numeric price
            "USDJPY,2000,110.25",    // Valid
            "INVALID_CURR,1000,1.51", // Valid - currency format not strictly validated
            "AUDCAD,0,1.35",         // Valid - zero amount allowed
            "NZDUSD,1200,0.67"       // Valid
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(4)); // Only truly invalid trades are skipped
        Assert.That(result[0].SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(result[1].SourceCurrency, Is.EqualTo("USD"));
        Assert.That(result[2].SourceCurrency, Is.EqualTo("AUD"));
        Assert.That(result[3].SourceCurrency, Is.EqualTo("NZD"));
    }

    [Test]
    public void Parse_WithDecimalAmounts_ConvertsToLotsCorrectly()
    {
        // Arrange - Test lot size conversion (amount / 100)
        var lines = new List<string>
        {
            "GBPUSD,100,1.51",   // 100/100 = 1 lot
            "EURJPY,1500,178.13", // 1500/100 = 15 lots
            "GBPJPY,2550,188.71"  // 2550/100 = 25.5 lots
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result[0].Lots, Is.EqualTo(1));
        Assert.That(result[1].Lots, Is.EqualTo(15));
        Assert.That(result[2].Lots, Is.EqualTo(25.5f));
    }

    [Test]
    public void Parse_WithHighPrecisionPrices_PreservesPrecision()
    {
        // Arrange - Test decimal precision preservation
        var lines = new List<string>
        {
            "GBPUSD,1000,1.123456789",
            "EURJPY,1500,178.987654321"
        };

        // Act
        var result = _tradeParser.Parse(lines);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Price, Is.EqualTo(1.123456789m));
        Assert.That(result[1].Price, Is.EqualTo(178.987654321m));
    }
}

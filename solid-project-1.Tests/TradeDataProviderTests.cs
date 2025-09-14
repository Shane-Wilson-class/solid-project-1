using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace solid_project_1.Tests;

/// <summary>
/// Unit tests for TradeDataProvider class.
/// Tests the GetTradeData method with various Stream scenarios to ensure proper data extraction.
/// </summary>
[TestFixture]
public class TradeDataProviderTests
{
    private TradeDataProvider _tradeDataProvider;

    [SetUp]
    public void Setup()
    {
        _tradeDataProvider = new TradeDataProvider();
    }

    [Test]
    public void GetTradeData_WithValidTradeData_ReturnsCorrectLines()
    {
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
        // Arrange
        var stream = new MemoryStream();

        // Act
        var result = _tradeDataProvider.GetTradeData(stream);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetTradeData_WithEmptyLines_ReturnsAllLines()
    {
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
        // Arrange
        Stream nullStream = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _tradeDataProvider.GetTradeData(nullStream));
    }

    [Test]
    public void GetTradeData_WithDisposedStream_ThrowsArgumentException()
    {
        // Arrange
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("GBPUSD,1000,1.51"));
        stream.Dispose();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _tradeDataProvider.GetTradeData(stream));
    }

    [Test]
    public void GetTradeData_WithSpecialCharacters_PreservesSpecialCharacters()
    {
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
}

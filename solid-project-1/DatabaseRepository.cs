using System;
using System.Collections.Generic;
using LiteDB;

namespace solid_project_1;

/// <summary>
/// Concrete database repository that wraps all LiteDB operations.
/// This implements IDatabaseRepository interface, demonstrating the complete
/// application of the Dependency Inversion Principle where both high-level
/// and low-level modules depend on abstractions.
/// </summary>
public class DatabaseRepository : IDatabaseRepository
{
    private readonly string _databasePath;

    public DatabaseRepository(string databasePath = @"trades.db")
    {
        _databasePath = databasePath;
    }

    /// <summary>
    /// Clears all existing trade records from the database.
    /// This prevents duplicate accumulation when the application runs multiple times.
    /// </summary>
    public void ClearAllTrades()
    {
        using var db = new LiteRepository(_databasePath);
        db.DeleteMany<TradeRecord>(_ => true);
    }

    /// <summary>
    /// Inserts a single trade record into the database.
    /// </summary>
    /// <param name="trade">The trade record to insert</param>
    public void InsertTrade(TradeRecord trade)
    {
        using var db = new LiteRepository(_databasePath);
        db.Insert(trade);
    }

    /// <summary>
    /// Inserts multiple trade records into the database efficiently.
    /// </summary>
    /// <param name="trades">The collection of trade records to insert</param>
    public void InsertTrades(IEnumerable<TradeRecord> trades)
    {
        using var db = new LiteRepository(_databasePath);
        foreach (var trade in trades)
        {
            db.Insert(trade);
        }
    }

    /// <summary>
    /// Retrieves all trade records from the database.
    /// </summary>
    /// <returns>List of all trade records in the database</returns>
    public List<TradeRecord> GetAllTrades()
    {
        using var db = new LiteRepository(_databasePath);
        return db.Query<TradeRecord>().ToList();
    }

    /// <summary>
    /// Gets the count of trade records in the database.
    /// </summary>
    /// <returns>Number of trade records</returns>
    public int GetTradeCount()
    {
        using var db = new LiteRepository(_databasePath);
        return db.Query<TradeRecord>().Count();
    }
}

using System.Collections.Generic;

namespace solid_project_1;

/// <summary>
/// Interface for database repository operations.
/// This abstraction follows the Dependency Inversion Principle by allowing
/// high-level modules (TradeStorage) to depend on this abstraction rather
/// than the concrete DatabaseRepository implementation.
/// </summary>
public interface IDatabaseRepository
{
    /// <summary>
    /// Clears all existing trade records from the database.
    /// This prevents duplicate accumulation when the application runs multiple times.
    /// </summary>
    void ClearAllTrades();

    /// <summary>
    /// Inserts a single trade record into the database.
    /// </summary>
    /// <param name="trade">The trade record to insert</param>
    void InsertTrade(TradeRecord trade);

    /// <summary>
    /// Inserts multiple trade records into the database efficiently.
    /// </summary>
    /// <param name="trades">The collection of trade records to insert</param>
    void InsertTrades(IEnumerable<TradeRecord> trades);

    /// <summary>
    /// Retrieves all trade records from the database.
    /// </summary>
    /// <returns>List of all trade records in the database</returns>
    List<TradeRecord> GetAllTrades();

    /// <summary>
    /// Gets the count of trade records in the database.
    /// </summary>
    /// <returns>Number of trade records</returns>
    int GetTradeCount();
}

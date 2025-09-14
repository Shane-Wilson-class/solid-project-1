using System.Collections.Generic;

namespace solid_project_1;

/// <summary>
/// TradeStorage implementation that depends on IDatabaseRepository interface for data persistence.
/// This demonstrates the complete application of the Dependency Inversion Principle where
/// high-level modules depend on abstractions rather than concrete implementations.
/// This makes the code more testable, flexible, and follows SOLID principles completely.
/// </summary>
public class TradeStorage : ITradeStorage
{
    private readonly IDatabaseRepository _databaseRepository;

    /// <summary>
    /// Constructor that accepts IDatabaseRepository interface dependency.
    /// This follows the Dependency Inversion Principle by depending on an abstraction
    /// rather than a concrete implementation, making the code more testable and flexible.
    /// </summary>
    /// <param name="databaseRepository">The database repository interface for data operations</param>
    public TradeStorage(IDatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    /// <summary>
    /// Persists trade records to the database and returns a status message.
    /// This method now returns a string instead of void to enable better testability
    /// and provide meaningful feedback about the operation.
    /// </summary>
    /// <param name="trades">The list of trade records to persist</param>
    /// <returns>Status message indicating the result of the operation</returns>
    public string Persist(List<TradeRecord> trades)
    {
        // Clear existing trades before inserting new ones to prevent duplicate trade records
        // from accumulating when the application is run multiple times. This ensures the
        // database contains only the current set of trades from the input file, making the
        // behavior predictable for educational purposes.
        _databaseRepository.ClearAllTrades();

        // Insert all trade records
        _databaseRepository.InsertTrades(trades);

        // Return meaningful status message instead of using Console.WriteLine
        return $"INFO: {trades.Count} trades processed";
    }
}
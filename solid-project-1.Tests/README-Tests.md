# Unit Testing Guide for SOLID Principles Project

## Overview

This directory contains comprehensive unit tests that will guide you through implementing the SOLID principles. The tests are organized using **Progressive Test Enabling** - you'll uncomment tests as you complete each implementation step.

## Test-Driven Development Workflow

### Phase 1: Understanding the Current State
1. Run `dotnet test` to see current test results
2. **DatabaseRepositoryTests** - All tests should pass (class already exists)
3. **InterfaceContractTests** - Will fail until you create interfaces
4. **ImplementationTests** - Will fail until you create implementation classes
5. **Core unit tests** - All commented out, waiting for your implementation

### Phase 2: Create Interfaces (Step 3 of Assignment)
After creating each interface, uncomment the corresponding "STEP 3" tests:

1. **Create ITradeDataProvider interface**
   - Uncomment STEP 3 tests in `TradeDataProviderTests.cs`
   - Run tests - they should fail (no implementation yet)

2. **Create ITradeParser interface**
   - Uncomment STEP 3 tests in `TradeParserTests.cs`
   - Run tests - they should fail (no implementation yet)

3. **Create ITradeStorage interface**
   - Uncomment STEP 3 tests in `TradeStorageTests.cs`
   - Run tests - they should fail (no implementation yet)

### Phase 3: Create Implementation Classes (Step 4 of Assignment)
After creating each implementation class, uncomment the corresponding "STEP 4" tests:

1. **Create TradeDataProvider class**
   - Move `ReadTradData()` method from TradeProcessor
   - Uncomment STEP 4 tests in `TradeDataProviderTests.cs`
   - Run tests - they should pass if implementation is correct

2. **Create TradeParser class**
   - Move `Parse()`, `MapTradeDataToTradeRecord()`, `ValidateTradeData()` methods
   - Move `LotSize` constant
   - Uncomment STEP 4 tests in `TradeParserTests.cs`
   - Run tests - they should pass if implementation is correct

3. **Create TradeStorage class**
   - Move `StoreTrades()` method functionality
   - Uncomment STEP 4 tests in `TradeStorageTests.cs`
   - Run tests - they should pass if implementation is correct

### Phase 4: Refactor TradeProcessor (Step 5 of Assignment)
After refactoring TradeProcessor with dependency injection:

1. **Update TradeProcessor class**
   - Add constructor with interface dependencies
   - Change `ProcessTrades()` to instance method
   - Uncomment STEP 5 tests in `TradeProcessorTests.cs`
   - Run tests - they should pass if refactoring is correct

### Phase 5: Update Main Program (Step 6 of Assignment)
After updating the main program:
- All tests should pass
- You should have 85+ passing tests total

## Test File Organization

Each test file follows this structure:

```csharp
/*
 * LEARNING OBJECTIVES:
 * - Specific learning goals for this class
 * - SOLID principles demonstrated
 * - Professional testing patterns shown
 */

[TestFixture]
public class ClassNameTests
{
    // STEP 3: Uncomment after creating interface
    /*
    [Test]
    public void BasicFunctionalityTest()
    {
        // Tests that validate interface contract
    }
    */

    // STEP 4: Uncomment after creating implementation
    /*
    [Test]
    public void ComprehensiveTest()
    {
        // Tests that validate full implementation
    }
    */

    // STEP 5: Uncomment after dependency injection (TradeProcessor only)
    /*
    [Test]
    public void IntegrationTest()
    {
        // Tests that validate proper integration
    }
    */
}
```

## Test Categories Explained

### 1. **Interface Contract Tests** (InterfaceContractTests.cs)
- Validates that interfaces exist with correct signatures
- Tests pass when interfaces are properly defined
- **Always enabled** - provides immediate feedback

### 2. **Implementation Tests** (ImplementationTests.cs)
- Validates that classes exist and implement interfaces
- Tests pass when implementation classes are created
- **Always enabled** - provides immediate feedback

### 3. **Unit Tests** (Individual class test files)
- Comprehensive testing of each class's functionality
- **Progressively enabled** - uncomment as you implement
- Demonstrates professional testing patterns

### 4. **Database Tests** (DatabaseRepositoryTests.cs)
- Tests the existing DatabaseRepository class
- **Fully enabled** - shows working test examples
- Demonstrates integration testing with temporary databases

## Professional Testing Patterns Demonstrated

### AAA Pattern (Arrange-Act-Assert)
```csharp
[Test]
public void Method_WithCondition_ExpectedBehavior()
{
    // Arrange - Set up test data and dependencies
    var input = CreateTestData();
    var expectedResult = "expected value";

    // Act - Execute the method being tested
    var result = _classUnderTest.Method(input);

    // Assert - Verify the expected outcome
    Assert.That(result, Is.EqualTo(expectedResult));
}
```

### Mocking with Moq Framework
```csharp
// Arrange - Create mock dependencies
var mockDependency = new Mock<IDependency>();
mockDependency.Setup(x => x.Method(It.IsAny<string>()))
              .Returns("mocked result");

// Act - Use the mock in your test
var result = _classUnderTest.ProcessData(mockDependency.Object);

// Assert - Verify interactions
mockDependency.Verify(x => x.Method("expected parameter"), Times.Once);
```

### Edge Case Testing
- Null parameter handling
- Empty collections
- Invalid data formats
- Large datasets
- Error conditions

## Success Criteria

By the end of this exercise, you should have:

- ✅ **85+ passing tests** (14 existing + 71 new comprehensive tests)
- ✅ **Complete SOLID implementation** validated by tests
- ✅ **Professional testing knowledge** from reading and enabling tests
- ✅ **TDD experience** from the red-green-refactor cycle
- ✅ **Mocking expertise** from dependency isolation examples

## Tips for Success

1. **Read tests before implementing** - Tests show you exactly what to build
2. **Uncomment gradually** - Don't enable all tests at once
3. **Run tests frequently** - Get immediate feedback on your progress
4. **Study the test patterns** - Learn professional testing techniques
5. **Ask questions** - Use tests to understand requirements better

## Getting Help

If tests are failing:
1. Read the test name and comments carefully
2. Check that your class/interface names match exactly
3. Verify method signatures match the interface contracts
4. Ensure you've moved methods to the correct classes
5. Check that you've implemented all required functionality

Remember: **Failing tests are your friends** - they tell you exactly what needs to be fixed!

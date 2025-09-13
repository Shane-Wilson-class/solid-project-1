# Unit Test Templates

This directory contains test templates to help you verify your SOLID principles implementation.

## Setting Up Tests

To create a test project and run these tests:

```bash
# Navigate to the solution directory
cd solid-project-1

# Create a new test project
dotnet new xunit -n solid-project-1.Tests

# Add the test project to the solution
dotnet sln add solid-project-1.Tests/solid-project-1.Tests.csproj

# Add reference to the main project
cd solid-project-1.Tests
dotnet add reference ../solid-project-1.csproj

# Copy the test templates from this Tests folder to your test project
# Then run the tests
dotnet test
```

## Test Templates Provided

1. **InterfaceContractTests.cs** - Verifies that your interfaces have the correct method signatures
2. **ImplementationTests.cs** - Tests that your classes implement the interfaces correctly
3. **TradeProcessorTests.cs** - Validates the facade pattern implementation
4. **IntegrationTests.cs** - End-to-end tests to ensure functionality is preserved

## Using the Tests

1. **Before Implementation**: Most tests will fail - this is expected
2. **During Implementation**: Use failing tests to guide your development
3. **After Implementation**: All tests should pass

## Test-Driven Development Approach

Consider following this approach:
1. Look at the failing tests to understand what needs to be implemented
2. Implement just enough code to make one test pass
3. Refactor your code while keeping tests green
4. Repeat until all tests pass

This approach helps ensure you're building exactly what's required by the assignment.

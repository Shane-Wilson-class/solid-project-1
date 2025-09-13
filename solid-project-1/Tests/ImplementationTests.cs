using System;
using System.Reflection;
using Xunit;

namespace solid_project_1.Tests
{
    /// <summary>
    /// Tests to verify that implementation classes exist and implement the correct interfaces.
    /// These tests help ensure you've created the implementation classes as specified.
    /// </summary>
    public class ImplementationTests
    {
        [Fact]
        public void TradeDataProvider_Class_Should_Exist()
        {
            // Arrange & Act
            var classType = Type.GetType("solid_project_1.TradeDataProvider, solid-project-1");
            
            // Assert
            Assert.NotNull(classType);
            Assert.True(classType.IsClass);
            Assert.False(classType.IsAbstract);
        }

        [Fact]
        public void TradeDataProvider_Should_Implement_ITradeDataProvider()
        {
            // Arrange
            var classType = Type.GetType("solid_project_1.TradeDataProvider, solid-project-1");
            var interfaceType = Type.GetType("solid_project_1.ITradeDataProvider, solid-project-1");
            
            // Act & Assert
            Assert.NotNull(classType);
            Assert.NotNull(interfaceType);
            Assert.True(interfaceType.IsAssignableFrom(classType));
        }

        [Fact]
        public void TradeParser_Class_Should_Exist()
        {
            // Arrange & Act
            var classType = Type.GetType("solid_project_1.TradeParser, solid-project-1");
            
            // Assert
            Assert.NotNull(classType);
            Assert.True(classType.IsClass);
            Assert.False(classType.IsAbstract);
        }

        [Fact]
        public void TradeParser_Should_Implement_ITradeParser()
        {
            // Arrange
            var classType = Type.GetType("solid_project_1.TradeParser, solid-project-1");
            var interfaceType = Type.GetType("solid_project_1.ITradeParser, solid-project-1");
            
            // Act & Assert
            Assert.NotNull(classType);
            Assert.NotNull(interfaceType);
            Assert.True(interfaceType.IsAssignableFrom(classType));
        }

        [Fact]
        public void TradeStorage_Class_Should_Exist()
        {
            // Arrange & Act
            var classType = Type.GetType("solid_project_1.TradeStorage, solid-project-1");
            
            // Assert
            Assert.NotNull(classType);
            Assert.True(classType.IsClass);
            Assert.False(classType.IsAbstract);
        }

        [Fact]
        public void TradeStorage_Should_Implement_ITradeStorage()
        {
            // Arrange
            var classType = Type.GetType("solid_project_1.TradeStorage, solid-project-1");
            var interfaceType = Type.GetType("solid_project_1.ITradeStorage, solid-project-1");
            
            // Act & Assert
            Assert.NotNull(classType);
            Assert.NotNull(interfaceType);
            Assert.True(interfaceType.IsAssignableFrom(classType));
        }

        [Fact]
        public void TradeProcessor_Should_Not_Be_Static_Class()
        {
            // Arrange
            var classType = Type.GetType("solid_project_1.TradeProcessor, solid-project-1");
            
            // Act & Assert
            Assert.NotNull(classType);
            Assert.True(classType.IsClass);
            
            // Static classes are both abstract and sealed in .NET
            var isStaticClass = classType.IsAbstract && classType.IsSealed;
            Assert.False(isStaticClass, "TradeProcessor should not be a static class after refactoring");
        }

        [Fact]
        public void TradeProcessor_Should_Have_Constructor_With_Dependencies()
        {
            // Arrange
            var classType = Type.GetType("solid_project_1.TradeProcessor, solid-project-1");
            var tradeParserInterface = Type.GetType("solid_project_1.ITradeParser, solid-project-1");
            var tradeStorageInterface = Type.GetType("solid_project_1.ITradeStorage, solid-project-1");
            var tradeDataProviderInterface = Type.GetType("solid_project_1.ITradeDataProvider, solid-project-1");
            
            // Act
            var constructor = classType?.GetConstructor(new[] { 
                tradeParserInterface, 
                tradeStorageInterface, 
                tradeDataProviderInterface 
            });
            
            // Assert
            Assert.NotNull(constructor);
            Assert.Equal(3, constructor.GetParameters().Length);
        }
    }
}

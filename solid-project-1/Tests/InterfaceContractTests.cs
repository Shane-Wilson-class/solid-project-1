using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit;

namespace solid_project_1.Tests
{
    /// <summary>
    /// Tests to verify that the required interfaces exist with correct method signatures.
    /// These tests help ensure you've created the interfaces as specified in the assignment.
    /// </summary>
    public class InterfaceContractTests
    {
        [Fact]
        public void ITradeDataProvider_Interface_Should_Exist()
        {
            // Arrange & Act
            var interfaceType = Type.GetType("solid_project_1.ITradeDataProvider, solid-project-1");
            
            // Assert
            Assert.NotNull(interfaceType);
            Assert.True(interfaceType.IsInterface);
        }

        [Fact]
        public void ITradeDataProvider_Should_Have_GetTradeData_Method()
        {
            // Arrange
            var interfaceType = Type.GetType("solid_project_1.ITradeDataProvider, solid-project-1");
            
            // Act
            var method = interfaceType?.GetMethod("GetTradeData");
            
            // Assert
            Assert.NotNull(method);
            Assert.Equal(typeof(List<string>), method.ReturnType);
            
            var parameters = method.GetParameters();
            Assert.Single(parameters);
            Assert.Equal(typeof(Stream), parameters[0].ParameterType);
        }

        [Fact]
        public void ITradeParser_Interface_Should_Exist()
        {
            // Arrange & Act
            var interfaceType = Type.GetType("solid_project_1.ITradeParser, solid-project-1");
            
            // Assert
            Assert.NotNull(interfaceType);
            Assert.True(interfaceType.IsInterface);
        }

        [Fact]
        public void ITradeParser_Should_Have_Parse_Method()
        {
            // Arrange
            var interfaceType = Type.GetType("solid_project_1.ITradeParser, solid-project-1");
            
            // Act
            var method = interfaceType?.GetMethod("Parse");
            
            // Assert
            Assert.NotNull(method);
            Assert.Equal(typeof(List<TradeRecord>), method.ReturnType);
            
            var parameters = method.GetParameters();
            Assert.Single(parameters);
            Assert.Equal(typeof(List<string>), parameters[0].ParameterType);
        }

        [Fact]
        public void ITradeStorage_Interface_Should_Exist()
        {
            // Arrange & Act
            var interfaceType = Type.GetType("solid_project_1.ITradeStorage, solid-project-1");
            
            // Assert
            Assert.NotNull(interfaceType);
            Assert.True(interfaceType.IsInterface);
        }

        [Fact]
        public void ITradeStorage_Should_Have_Persist_Method()
        {
            // Arrange
            var interfaceType = Type.GetType("solid_project_1.ITradeStorage, solid-project-1");
            
            // Act
            var method = interfaceType?.GetMethod("Persist");
            
            // Assert
            Assert.NotNull(method);
            Assert.Equal(typeof(void), method.ReturnType);
            
            var parameters = method.GetParameters();
            Assert.Single(parameters);
            Assert.Equal(typeof(List<TradeRecord>), parameters[0].ParameterType);
        }
    }
}

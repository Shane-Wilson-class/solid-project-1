namespace solid_project_1.Tests
{
    /// <summary>
    /// Tests to verify that implementation classes exist and implement the correct interfaces.
    /// These tests help ensure you've created the implementation classes as specified.
    /// </summary>
    [TestFixture]
    public class ImplementationTests
    {
        [Test]
        public void TradeDataProvider_Class_Should_Exist()
        {
            // Arrange & Act
            var classType = Type.GetType("solid_project_1.TradeDataProvider, solid-project-1");
            
            // Assert
            Assert.That(classType, Is.Not.Null);
            Assert.That(classType.IsClass, Is.True);
            Assert.That(classType.IsAbstract, Is.False);
        }

        [Test]
        public void TradeDataProvider_Should_Implement_ITradeDataProvider()
        {
            // Arrange
            var classType = Type.GetType("solid_project_1.TradeDataProvider, solid-project-1");
            var interfaceType = Type.GetType("solid_project_1.ITradeDataProvider, solid-project-1");
            
            // Act & Assert
            Assert.That(classType, Is.Not.Null);
            Assert.That(interfaceType, Is.Not.Null);
            Assert.That(interfaceType.IsAssignableFrom(classType), Is.True);
        }

        [Test]
        public void TradeParser_Class_Should_Exist()
        {
            // Arrange & Act
            var classType = Type.GetType("solid_project_1.TradeParser, solid-project-1");
            
            // Assert
            Assert.That(classType, Is.Not.Null);
            Assert.That(classType.IsClass, Is.True);
            Assert.That(classType.IsAbstract, Is.False);
        }

        [Test]
        public void TradeParser_Should_Implement_ITradeParser()
        {
            // Arrange
            var classType = Type.GetType("solid_project_1.TradeParser, solid-project-1");
            var interfaceType = Type.GetType("solid_project_1.ITradeParser, solid-project-1");
            
            // Act & Assert
            Assert.That(classType, Is.Not.Null);
            Assert.That(interfaceType, Is.Not.Null);
            Assert.That(interfaceType.IsAssignableFrom(classType), Is.True);
        }

        [Test]
        public void TradeStorage_Class_Should_Exist()
        {
            // Arrange & Act
            var classType = Type.GetType("solid_project_1.TradeStorage, solid-project-1");
            
            // Assert
            Assert.That(classType, Is.Not.Null);
            Assert.That(classType.IsClass, Is.True);
            Assert.That(classType.IsAbstract, Is.False);
        }

        [Test]
        public void TradeStorage_Should_Implement_ITradeStorage()
        {
            // Arrange
            var classType = Type.GetType("solid_project_1.TradeStorage, solid-project-1");
            var interfaceType = Type.GetType("solid_project_1.ITradeStorage, solid-project-1");
            
            // Act & Assert
            Assert.That(classType, Is.Not.Null);
            Assert.That(interfaceType, Is.Not.Null);
            Assert.That(interfaceType.IsAssignableFrom(classType), Is.True);
        }

        [Test]
        public void TradeProcessor_Should_Not_Be_Static_Class()
        {
            // Arrange
            var classType = Type.GetType("solid_project_1.TradeProcessor, solid-project-1");
            
            // Act & Assert
            Assert.That(classType, Is.Not.Null);
            Assert.That(classType.IsClass, Is.True);
            
            // Static classes are both abstract and sealed in .NET
            var isStaticClass = classType.IsAbstract && classType.IsSealed;
            Assert.That(isStaticClass, Is.False, "TradeProcessor should not be a static class after refactoring");
        }

        [Test]
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
            Assert.That(constructor, Is.Not.Null);
            Assert.That(constructor.GetParameters().Length, Is.EqualTo(3));
        }
    }
}

namespace solid_project_1.Tests;

/// <summary>
/// Tests to verify that the required interfaces exist with correct method signatures.
/// These tests help ensure you've created the interfaces as specified in the assignment.
/// </summary>
[TestFixture]
public class InterfaceContractTests
{
    [Test]
    public void ITradeDataProvider_Interface_Should_Exist()
    {
        var interfaceType = Type.GetType("solid_project_1.ITradeDataProvider, solid-project-1");

        Assert.That(interfaceType, Is.Not.Null);
        Assert.That(interfaceType.IsInterface, Is.True);
    }

    [Test]
    public void ITradeDataProvider_Should_Have_GetTradeData_Method()
    {
        var interfaceType = Type.GetType("solid_project_1.ITradeDataProvider, solid-project-1");

        var method = interfaceType?.GetMethod("GetTradeData");

        Assert.That(method, Is.Not.Null);
        Assert.That(method.ReturnType, Is.EqualTo(typeof(List<string>)));

        var parameters = method.GetParameters();
        Assert.That(parameters.Length, Is.EqualTo(1));
        Assert.That(parameters[0].ParameterType, Is.EqualTo(typeof(Stream)));
    }

    [Test]
    public void ITradeParser_Interface_Should_Exist()
    {
        var interfaceType = Type.GetType("solid_project_1.ITradeParser, solid-project-1");

        Assert.That(interfaceType, Is.Not.Null);
        Assert.That(interfaceType.IsInterface, Is.True);
    }

    [Test]
    public void ITradeParser_Should_Have_Parse_Method()
    {
        var interfaceType = Type.GetType("solid_project_1.ITradeParser, solid-project-1");

        var method = interfaceType?.GetMethod("Parse");

        Assert.That(method, Is.Not.Null);
        Assert.That(method.ReturnType, Is.EqualTo(typeof(List<TradeRecord>)));

        var parameters = method.GetParameters();
        Assert.That(parameters.Length, Is.EqualTo(1));
        Assert.That(parameters[0].ParameterType, Is.EqualTo(typeof(List<string>)));
    }

    [Test]
    public void ITradeStorage_Interface_Should_Exist()
    {
        var interfaceType = Type.GetType("solid_project_1.ITradeStorage, solid-project-1");

        Assert.That(interfaceType, Is.Not.Null);
        Assert.That(interfaceType.IsInterface, Is.True);
    }

    [Test]
    public void ITradeStorage_Should_Have_Persist_Method()
    {
        var interfaceType = Type.GetType("solid_project_1.ITradeStorage, solid-project-1");

        var method = interfaceType?.GetMethod("Persist");

        Assert.That(method, Is.Not.Null);
        Assert.That(method.ReturnType, Is.EqualTo(typeof(string)));

        var parameters = method.GetParameters();
        Assert.That(parameters.Length, Is.EqualTo(1));
        Assert.That(parameters[0].ParameterType, Is.EqualTo(typeof(List<TradeRecord>)));
    }
}
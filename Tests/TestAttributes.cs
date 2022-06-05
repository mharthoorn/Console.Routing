using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleRouting.Tests;

[TestClass]
public class TestAttributes
{
    [TestMethod]
    public void OptionalBecauseAttribute()
    {
        Router router = new RouterBuilder().AddModule<AttributesModule>().Build();
        var args = router.Parse("optionaltryme");
        var result = router.Bind(args);
        
        Assert.AreEqual("OptionalTryMe", result.Bind.Route.Method.Name);
    }

    [TestMethod]
    public void OptionalBecauseNullable()
    {
        Router router = new RouterBuilder().AddModule<AttributesModule>().Build();

        var args = router.Parse("nullabletryme");
        var result = router.Bind(args);
        
        Assert.AreEqual("NullableTryMe", result.Bind.Route.Method.Name);
    }

    [TestMethod]
    public void OptionalBecauseDefault()
    {
        Router router = new RouterBuilder().AddModule<AttributesModule>().Build();

        var args = router.Parse("defaulttryme");
        var result = router.Bind(args);

        Assert.AreEqual("DefaultTryMe", result.Bind.Route.Method.Name);
    }

}

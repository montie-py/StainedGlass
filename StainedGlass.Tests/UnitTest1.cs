namespace StainedGlass.Tests;
using StainedGlass;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Class1 class1 = new Class1();
        Assert.Equal(5, class1.Add(2, 3));
    }
}
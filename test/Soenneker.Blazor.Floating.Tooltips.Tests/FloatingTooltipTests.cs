using Soenneker.Blazor.Floating.Tooltips.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Blazor.Floating.Tooltips.Tests;

[Collection("Collection")]
public class FloatingTooltipTests : FixturedUnitTest
{
    private readonly IFloatingTooltip _blazorlibrary;

    public FloatingTooltipTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _blazorlibrary = Resolve<IFloatingTooltip>(true);
    }

    [Fact]
    public void Default()
    {

    }
}

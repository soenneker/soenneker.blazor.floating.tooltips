using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.Floating.Tooltips.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class FloatingTooltipTests : HostedUnitTest
{
    public FloatingTooltipTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}

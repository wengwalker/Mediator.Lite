using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Mediator.Lite.Tests.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute(params object[] values)
        : base(new Func<IFixture>(() => new Fixture().Customize(new AutoMoqCustomization())))
    {
    }
}

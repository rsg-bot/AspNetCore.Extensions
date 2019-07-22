using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using FluentAssertions;
using Rocket.Surgery.Extensions.Autofac;
using Rocket.Surgery.Extensions.DependencyInjection;
using Xunit;

namespace Rocket.Surgery.AspNetCore.Tests
{
    public class ServicesExtensionsTests
    {
        [Fact]
        public void Should_AllowUsageWithApi()
        {
            var result = A.Fake<IAutofacConventionContext>().WithApi();
            result.Should().NotBeNull();
        }
    }
}

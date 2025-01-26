using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mews.Reusable.UnitTests.Customizations
{
    public class DateOnlyCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<DateOnly>(composer =>
                composer.FromFactory<DateTime>((dateTime) => DateOnly.FromDateTime(dateTime)));
        }
    }
}

using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Mews.Reusable.UnitTests.Customizations;
using NSubstitute.Routing.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Mews.Reusable.UnitTests.Attributes
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true)]
    public class InlineAutoNSubstituteDataAttribute(params object[] values) : AutoNSubstituteDataAttribute
    {
        private readonly object[] _values = values;

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var autoDataWithSubstitutes = base.GetData(testMethod);

            var arrayOfValues = autoDataWithSubstitutes.First();

            for (var i = 0; i < _values.Length; i++)
            {
                //types could be different
                if (arrayOfValues[i].GetType().Equals(_values[i].GetType()))
                    arrayOfValues[i] = _values[i];
                else throw new Exception($"Incorrect type for value of parameter {i}");
            }

            return autoDataWithSubstitutes;
        }
    }

    public class AutoNSubstituteDataAttribute() : AutoDataAttribute(() =>
        {
            var myCustomFixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            //More customizations can be added here            
            myCustomFixture.Customize(new DateOnlyCustomization());
            return myCustomFixture;
        }
    );
}

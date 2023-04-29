
namespace AleksandarWebDevelopment2Project.Infrastructure.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RangeUntilCurrentYearAttribute : RangeAttribute
    {
        public RangeUntilCurrentYearAttribute(int minYear) : base(minYear, DateTime.UtcNow.Year)
        {
        }
    }
}

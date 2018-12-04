using System;
using MyOnboardingApp.Contracts.Generators;

namespace MyOnboardingApp.Services.Generators
{
    internal class DateTimeGenerator : IDateTimeGenerator
    {
        public DateTime GetCurrentDateTime()
            => DateTime.UtcNow;
    }
}

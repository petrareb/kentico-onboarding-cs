using System;
using MyOnboardingApp.Contracts.Generators;

namespace MyOnboardingApp.Services.Generators
{
    internal class GuidGenerator : IIdGenerator<Guid>
    {
        public Guid GetNewId()
            => Guid.NewGuid();
    }
}
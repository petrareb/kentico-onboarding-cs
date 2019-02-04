using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Registration;

namespace MyOnboardingApp.Validation.Extensions
{
    public static class ValidationBootstrapperExtensions
    {
        public static IEnumerable<ConstructorInfo> GetConstructorsOfMappedTypes(this IEnumerable<IContainerRegistration> registrations)
            => registrations
                .SelectMany(registration => registration
                    .MappedToType
                    .GetConstructors());


        public static IEnumerable<ParameterInfo> GetParametersFromConstructors(this IEnumerable<ConstructorInfo> constructors)
            => constructors
                .SelectMany(constructor => constructor.GetParameters());


        public static IEnumerable<Type> GetParameterTypes(this IEnumerable<ParameterInfo> parameters)
            => parameters
                .Select(parameter => parameter.ParameterType);


        public static IEnumerable<Type> SelectGenericTypesImplementingType(this IEnumerable<Type> types, Type implementedType)
            => types
                .Where(type => type.IsGenericType
                               && type.GetGenericTypeDefinition() == implementedType);


        public static IEnumerable<Type> SelectArgumentsFromGenericTypes(this IEnumerable<Type> genericTypes)
            => genericTypes
                .Select(parameterType => parameterType.GetGenericArguments().First())
                .Distinct();


        public static IEnumerable<Type> GetRegisteredTypes(this IEnumerable<IContainerRegistration> registrations)
            => registrations
                .Select(registration => registration.RegisteredType);
    }
}

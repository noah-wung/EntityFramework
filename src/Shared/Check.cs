// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Internal;

namespace Microsoft.Data.Entity.Utilities
{
    [DebuggerStepThrough]
    internal static class Check
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>([NoEnumeration] T value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, "parameterName");
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            [NoEnumeration] T value,
            [InvokerParameterName] [NotNull] string parameterName,
            [NotNull] string propertyName)
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, "parameterName");
                NotEmpty(propertyName, "propertyName");
                throw new ArgumentException(Strings.ArgumentPropertyNull(propertyName, parameterName));
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static IReadOnlyList<T> NotEmpty<T>(IReadOnlyList<T> value, [InvokerParameterName] [NotNull] string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Count == 0)
            {
                NotEmpty(parameterName, "parameterName");
                throw new ArgumentException(Strings.CollectionArgumentIsEmpty(parameterName));
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string NotEmpty(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
            Exception e = null;
            if (ReferenceEquals(value, null))
            {
                e = new ArgumentNullException(parameterName);
            }
            else if (value.Trim().Length == 0)
            {
                e = new ArgumentException(Strings.ArgumentIsEmpty(parameterName));
            }

            if (e != null)
            {
                NotEmpty(parameterName, "parameterName");
                throw e;
            }

            return value;
        }

        public static string NullButNotEmpty(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (!ReferenceEquals(value, null)
                && value.Length == 0)
            {
                NotEmpty(parameterName, "parameterName");
                throw new ArgumentException(Strings.ArgumentIsEmpty(parameterName));
            }

            return value;
        }

        public static IReadOnlyList<T> HasNoNulls<T>(IReadOnlyList<T> value, [InvokerParameterName] [NotNull] string parameterName)
            where T : class
        {
            NotNull(value, parameterName);

            if (value.Any(e => e == null))
            {
                NotEmpty(parameterName, "parameterName");
                throw new ArgumentException(parameterName);
            }

            return value;
        }

        public static T IsDefined<T>(T value, [InvokerParameterName] [NotNull] string parameterName)
            where T : struct
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                NotEmpty(parameterName, "parameterName");
                throw new ArgumentException(Strings.InvalidEnumValue(parameterName, typeof(T)));
            }

            return value;
        }

        public static Type ValidEntityType(Type value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (!value.GetTypeInfo().IsClass)
            {
                NotEmpty(parameterName, "parameterName");
                throw new ArgumentException(Strings.InvalidEntityType(parameterName, value));
            }

            return value;
        }
    }
}

﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.Data.Entity.ChangeTracking.Internal;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Query;
using Microsoft.Data.Entity.Utilities;
using Microsoft.Framework.Logging;

namespace Microsoft.Data.Entity.InMemory.Query
{
    public class InMemoryQueryCompilationContext : QueryCompilationContext
    {
        public InMemoryQueryCompilationContext(
            [NotNull] IModel model,
            [NotNull] ILogger logger,
            [NotNull] EntityMaterializerSource entityMaterializerSource,
            [NotNull] EntityKeyFactorySource entityKeyFactorySource)
            : base(
                Check.NotNull(model, "model"),
                Check.NotNull(logger, "logger"),
                new LinqOperatorProvider(),
                new ResultOperatorHandler(),
                Check.NotNull(entityMaterializerSource, "entityMaterializerSource"),
                Check.NotNull(entityKeyFactorySource, "entityKeyFactorySource"))
        {
            Check.NotNull(entityKeyFactorySource, "entityKeyFactorySource");
        }

        public override EntityQueryModelVisitor CreateQueryModelVisitor(
            EntityQueryModelVisitor parentEntityQueryModelVisitor)
        {
            return new InMemoryQueryModelVisitor(this);
        }
    }
}

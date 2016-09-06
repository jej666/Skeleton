﻿using System.Collections.Generic;

namespace Skeleton.Abstraction
{
    public interface IFieldMetadata
    {
        int FieldsCount { get; }

        IEnumerable<IMemberAccessor> GetDeclaredOnlyFields();

        IMemberAccessor GetField(string name);

        IEnumerable<IMemberAccessor> GetFields();
    }
}
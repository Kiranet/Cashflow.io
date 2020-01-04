﻿using System;
using System.Collections.Generic;

namespace Cashflowio.Core.Utils
{
    public static class EnumFactory
    {
        public static IEnumerable<T> GetTypedValues<T>() where T : Enum
        {
            return (T[]) Enum.GetValues(typeof(T));
        }
    }
}
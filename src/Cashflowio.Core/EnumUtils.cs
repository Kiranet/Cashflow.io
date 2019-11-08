using System;
using System.Collections.Generic;

namespace Cashflowio.Core
{
    public static class EnumUtils
    {
        public static IEnumerable<T> GetTypedValues<T>() where T : Enum
        {
            return (T[]) Enum.GetValues(typeof(T));
        }
    }
}
using System.Collections.Generic;

namespace Cashflowio.Core.Utils
{
    public static class EnumUtils
    {
        public static IEnumerable<T> GetTypedValues<T>() where T : System.Enum
        {
            return (T[]) System.Enum.GetValues(typeof(T));
        }
    }
}
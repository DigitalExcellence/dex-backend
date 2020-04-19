using System;

namespace _3_Repositories.Tests.Extensions
{
    public static class CloneExtension
    {
#pragma warning disable IDE0060 // Remove unused parameter
        public static T CloneObject<T>(this object source)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            T result = Activator.CreateInstance<T>();
            return result;
        }
    }
}

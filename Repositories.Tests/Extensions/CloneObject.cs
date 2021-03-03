using System;

namespace Repositories.Tests.Extensions
{

    public static class CloneExtension
    {

        public static T CloneObject<T>(this object source)
        {
            T result = Activator.CreateInstance<T>();
            return result;
        }

    }

}

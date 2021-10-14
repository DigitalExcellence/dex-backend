using System.Collections;
using System.Collections.Generic;

namespace IntegrationTests
{
    public class IdentityIdSupplier : IEnumerable<object[]>
    {
        public static string DataOfficerIdentityId = "45675";
        public static string PRUserIdentityId = "2754457";

        public static List<object[]> Collection { get; set; } = new List<object[]> { new object[] { DataOfficerIdentityId }, new object[] { PRUserIdentityId } };

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach(object[] o in Collection)
            {
                yield return o;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

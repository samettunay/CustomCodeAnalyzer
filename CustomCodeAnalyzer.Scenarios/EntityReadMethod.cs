using System.Collections.Generic;

namespace CustomCodeAnalyzer.Scenarios
{
    public class EntityReadMethod : BaseEntity
    {
        public virtual void OkuTestMethodName(string value1)
        {

        }

        public virtual List<object> OkuListTestMethodName(string value1)
        {
            return new List<object>();
        }
    }
}

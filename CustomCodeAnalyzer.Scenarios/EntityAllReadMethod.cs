using System.Collections.Generic;

namespace CustomCodeAnalyzer.Scenarios
{
    public class EntityAllReadMethod : BaseEntity
    {
        public virtual void GetirTestMethodName(string value1)
        {

        }

        public virtual List<object> GetirListTestMethodName(string value1)
        {
            return new List<object>();
        }
    }
}

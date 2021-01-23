using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocialApp.Domain.Common.ValueObjects
{
    public class Identity : ValueObject
    {
        public string Value { get; private set; }

        private Identity(string value)
        {
            Value = value;
        }

        public static Identity Create(string id)
        {
            Guid.Parse(id);
            return new Identity(id);
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

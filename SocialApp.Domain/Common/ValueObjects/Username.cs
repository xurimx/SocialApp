using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocialApp.Domain.Common.ValueObjects
{
    public class Username : ValueObject
    {
        public string Value { get; private set; }

        private Username(string value)
        {
            Value = value;
        }

        public static Username Create(string username)
        {
            return new Username(username);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

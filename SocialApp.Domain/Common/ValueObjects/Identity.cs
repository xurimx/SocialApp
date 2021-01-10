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
        #region Properties
        public string Value { get; private set; }
        #endregion

        #region Constructors
        private Identity(string value)
        {
            Value = value;
        }
        #endregion

        #region Factories
        public static Identity Create(string id)
        {
            Guid.Parse(id);
            return new Identity(id);
        }
        #endregion

        #region Methods

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        #endregion
    }
}

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
        #region Properties
        public string Value { get; private set; }
        #endregion

        #region Constructors
        private Username(string value)
        {
            Value = value;
        }
        #endregion

        #region Factories
        public static Username Create(string username)
        {
            return new Username(username);
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

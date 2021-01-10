using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocialApp.Domain.Common.ValueObjects
{
    public class EmailAddress : ValueObject
    {
        #region Properties
        public string Value { get; private set; }
        #endregion

        #region Constructors
        private EmailAddress(string value)
        {
            Value = value;
        }
        #endregion

        #region Factories
        public static EmailAddress Create(string email)
        {
            Regex regex = new Regex(@"(^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$)");
            if (regex.IsMatch(email))
            {
                return new EmailAddress(email);
            }
            throw new ArgumentException($"The provided input: '{email}' is not a correct email!");
        }
        #endregion

        #region Methods

        public void SetEmail(string email)
        {
            Regex regex = new Regex(@"(^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$)");
            if (regex.IsMatch(email))
            {
                this.Value = email;
            }
            throw new ArgumentException($"The provided input: '{email}' is not a correct email!");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        #endregion
    }
}

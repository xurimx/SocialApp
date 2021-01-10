using SocialApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialApp.Domain.Enumerations
{
    public class VisibilityType : Enumeration
    {
        public static VisibilityType Friends = new VisibilityType(1, nameof(Friends));
        public static VisibilityType Everyone = new VisibilityType(2, nameof(Everyone));
        public static VisibilityType OnlyMe = new VisibilityType(3, nameof(OnlyMe));

        public VisibilityType(int id, string name) : base(id, name) {}
    }
}

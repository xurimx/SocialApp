using SocialApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialApp.Domain.Enumerations
{
    public class ReactionType : Enumeration
    {
        public static ReactionType Like = new ReactionType(1, nameof(Like));
        public static ReactionType Dislike = new ReactionType(2, nameof(Dislike));
        public static ReactionType Angry = new ReactionType(3, nameof(Angry));
        public static ReactionType Unamused = new ReactionType(4, nameof(Unamused));
        public static ReactionType Haha = new ReactionType(5, nameof(Haha));

        public ReactionType(int id, string name) : base(id, name) { }
    }
}

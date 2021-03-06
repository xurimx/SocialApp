﻿using SocialApp.Domain.Common;
using SocialApp.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialApp.Domain.Entities
{
    public class UserFriend : BaseEntity, IDomainEntity
    {
        public SocialUser User { get; }
        public SocialUser Friend { get; }
        public bool isFavorite { get; private set; }


        private UserFriend() { }

        protected UserFriend(SocialUser userId, SocialUser friendId, bool isFavorite)
        {
            User = userId;
            Friend = friendId;
            this.isFavorite = isFavorite;
        }

        protected UserFriend(Guid id, SocialUser user, SocialUser friend, bool isFavorite)
            :this(user, friend, isFavorite)
        {
            Id = id;
        }


        
        public static UserFriend Create(SocialUser user, SocialUser friend)
        {
            return new UserFriend(Guid.NewGuid(), user, friend, false);
        }


        public void MakeFavorite()
        {
            isFavorite = true;
        }

        public void RemoveFromFavorites()
        {
            isFavorite = false;
        }
    }
}

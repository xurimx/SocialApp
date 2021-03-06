﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocialApp.Domain.Common.ValueObjects
{
    public class Image : ValueObject
    {
        public static Image Default = new Image("default image url");

        public string Value { get; private set; }


        private Image(string value)
        {
            Value = value;
        }

        public static Image Create(string image)
        {
            return new Image(image);
        }


        public void SetImage(string image)
        {
            Value = image;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}

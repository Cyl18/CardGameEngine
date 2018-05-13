using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.Objects
{
    public abstract class User
    {
        protected static readonly Dictionary<string, User> Cache = new Dictionary<string, User>();
    }
}
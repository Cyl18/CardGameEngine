using System.Collections.Generic;
using CardGameEngine.Data;

namespace CardGameEngine.Objects
{
    public abstract class User : IIdentifiable<User>, IIdentifierOf<UserIdentifier>
    {
        protected static readonly Dictionary<string, User> Cache = new Dictionary<string, User>();

        public object InnerIdentifier => Identifier;
        public UserIdentifier Identifier { get; }
    }

    public struct UserIdentifier
    {
        public string UserName;
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using GammaLibrary;

namespace CardGameEngine.Storage
{
    [Configuration(nameof(UserStorage))]
    public class UserStorage : Configuration<UserStorage>
    {
        public Dictionary<UserID, UserData> UserData { get; } = new Dictionary<UserID, UserData>();
        
    }
}
using CardGameEngine.Data;

namespace CardGameEngine.Storage
{
    public class UserID : DataContainer<UserID, string>
    {
        public UserID(string data) : base(data)
        {
        }
    }
}
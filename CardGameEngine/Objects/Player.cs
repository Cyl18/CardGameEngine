using CardGameEngine.Data;

namespace CardGameEngine.Objects
{
    public class Player : GameObject, IIdentifiable<Player>, IIdentifierOf<PlayerIdentifier>
    {
        public Player(PlayerIdentifier identifier) // TODO: factory
        {
            Identifier = identifier;
        }

        public object InnerIdentifier => Identifier;
        public PlayerIdentifier Identifier { get; }
        public string Name => Identifier.UserIdentifier.UserName;
        public override string ToString() => Identifier.ToString();
    }

    public struct PlayerIdentifier
    {
        public UserIdentifier UserIdentifier;
        public GameIdentifier GameIdentifier;
        public override string ToString()
        {
            return $"[{UserIdentifier.UserName}:{GameIdentifier.GameGuid}]";
        }
    }

    public static class PlayerIdentifierExtensions
    {
        public static string GetPlayerName(this Identifier<Player> identifier) => ((PlayerIdentifier) identifier.InnerIdentifier).UserIdentifier.UserName;
    }
}

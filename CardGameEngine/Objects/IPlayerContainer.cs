using System.Collections.Generic;
using CardGameEngine.Data;

namespace CardGameEngine.Objects
{
    public interface IPlayerContainer
    {
        IReadOnlyDictionary<Identifier<Player>, Player> Players { get; }
        void AddPlayer(Player player);
        void RemovePlayer(Identifier<Player> playerIdentifier);
    }
}
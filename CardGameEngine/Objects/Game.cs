using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CardGameEngine.Data;
using CardGameEngine.Debug;
using CardGameEngine.Events;
using CardGameEngine.Games;

namespace CardGameEngine.Objects
{
    public abstract partial class Game : IPlayerContainer, IIdentifiable<Game>, IIdentifierOf<GameIdentifier>
    {
        public abstract IGoal Goal { get; }
        

        public GameIdentifier Identifier { get; } = GameIdentifier.CreateOne();
        public object InnerIdentifier => Identifier;
        public IReadOnlyDictionary<Identifier<Player>, Player> Players => PlayerDictionary;
        private ConcurrentDictionary<Identifier<Player>, Player> PlayerDictionary { get; } = new ConcurrentDictionary<Identifier<Player>, Player>();

        public event EventHandler<Player> PlayerAdded;
        public event EventHandler<Identifier<Player>> PlayerRemoved;
        public void AddPlayer(Player player)
        {
            var identifier = player.GetIdentifier();
            if (PlayerDictionary.ContainsKey(identifier)) throw new PlayerAlreadyExistsException(identifier.GetPlayerName());
            PlayerDictionary[identifier] = player;
            PlayerAdded?.Invoke(this, player);
        }

        public void RemovePlayer(Identifier<Player> playerIdentifier)
        {
            if (!PlayerDictionary.ContainsKey(playerIdentifier)) throw new PlayerNotExistsException(playerIdentifier.GetPlayerName());
            if (!PlayerDictionary.TryRemove(playerIdentifier, out _)) throw new OperationFailedException("remove player"); //TODO i18n
            PlayerRemoved?.Invoke(this, playerIdentifier);
        }

    }

    public class OperationFailedException : CardGameException
    {
        public OperationFailedException(string operationName) : base($"Operation {operationName} failed.") { }// TODO i18n
    }

    public class PlayerNotExistsException : CardGameException
    {
        public PlayerNotExistsException(string playerName) : base($"Player {playerName} don't exists.") { } // TODO i18n
    }

    public class PlayerAlreadyExistsException : CardGameException
    {
        public PlayerAlreadyExistsException(string playerName) : base($"Player {playerName} already exists.") { } // TODO i18n
    }
    public class PlayerNotExistsInTheWholeWorldException : CardGameException
    {
        public PlayerNotExistsInTheWholeWorldException(Player player) : base($"The player {player} that u are looking for do not exists in the whole world.") { } // TODO i18n
    }

    public abstract partial class Game
    {
        private ConcurrentDictionary<Identifier<Player>, Player> GlobalPlayersDictionary { get; } = new ConcurrentDictionary<Identifier<Player>, Player>();
        public event EventHandler<Game> GameEnded;

        private void InitialEvents()
        {
            GameEnded += OnGameEnded;
            PlayerAdded += OnPlayerAdded;
            PlayerRemoved += OnPlayerRemoved;
        }

        private void OnPlayerAdded(object sender, Player player)
        {
            GlobalPlayersDictionary.TryAdd(player.GetIdentifier(), player)
                .TraceWarningIfFalse($"Unable to add player {player} in the global players dictionary.");
        }

        private void OnPlayerRemoved(object sender, Identifier<Player> identifier)
        {
            GlobalPlayersDictionary.TryRemove(identifier, out _)
                .TraceWarningIfFalse($"Unable to remove player {identifier} in the global players dictionary");
        }

        private void OnGameEnded(object sender, Game e)
        {
            foreach (var identifier in e.Players.Select(kv => kv.Key))
            {
                GlobalPlayersDictionary.TryRemove(identifier, out _)
                    .TraceWarningIfFalse($"Unable to remove player {identifier} in the global players dictionary.");
            }
        }
    }

    public struct GameIdentifier
    {
        public Guid GameGuid;

        public GameIdentifier(Guid gameGuid)
        {
            GameGuid = gameGuid;
        }

        public static GameIdentifier CreateOne()
        {
            return new GameIdentifier(Guid.NewGuid());
        }
    }

    public interface IPlayerContainer
    {
        IReadOnlyDictionary<Identifier<Player>, Player> Players { get; }
        void AddPlayer(Player player);
        void RemovePlayer(Identifier<Player> playerIdentifier);
    }
}


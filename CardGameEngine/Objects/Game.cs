using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CardGameEngine.Data;
using CardGameEngine.Debugs;
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
        protected ConcurrentDictionary<Identifier<Player>, Player> PlayerDictionary { get; } = new ConcurrentDictionary<Identifier<Player>, Player>();
        protected EventManager EventManager { get; } = new EventManager();
        public class PlayerAdded : Event<PlayerAdded, Player> { }
        public class PlayerRemoved : Event<PlayerRemoved, Identifier<Player>> { }
        public void AddPlayer(Player player)
        {
            var identifier = player.GetIdentifier();
            if (PlayerDictionary.ContainsKey(identifier)) throw new PlayerAlreadyExistsException(identifier.GetPlayerName());
            PlayerDictionary[identifier] = player;
            EventManager.TriggerEvent<PlayerAdded>(this, player);
        }

        public void RemovePlayer(Identifier<Player> playerIdentifier)
        {
            if (!PlayerDictionary.ContainsKey(playerIdentifier)) throw new PlayerNotExistsException(playerIdentifier.GetPlayerName());
            if (!PlayerDictionary.TryRemove(playerIdentifier, out _)) throw new OperationFailedException("remove player"); //TODO i18n
            EventManager.TriggerEvent<PlayerRemoved>(this, playerIdentifier);
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
        private ConcurrentDictionary<Identifier<Game>, Game> GlobalGamesDictionary { get; } = new ConcurrentDictionary<Identifier<Game>, Game>();
        public class GameEnded : Event<GameEnded, Game> { }

        private void InitialEvents()
        {
            // TODO use reflection
            EventManager.RegisterAllInnerEvents<Game>();
            EventManager.RegisterEventHandler(GameEnded.Identifier, OnGameEnded);
            EventManager.RegisterEventHandler(PlayerAdded.Identifier, OnPlayerAdded);
            EventManager.RegisterEventHandler(PlayerRemoved.Identifier, OnPlayerRemoved);
        }

        private void OnPlayerAdded(object sender, Player player)
        {

        }

        private void OnPlayerRemoved(object sender, Identifier<Player> identifier)
        {

        }
        private void OnGameEnded(object sender, Game e)
        {
            GlobalGamesDictionary.TryRemove(e.GetIdentifier(), out _).TraceWarningIfFalse($"Unable to remove game {this}");
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
}


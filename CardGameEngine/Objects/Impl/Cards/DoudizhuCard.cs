using System;
using System.Collections.Generic;
using System.Text;
using CardGameEngine.Games;

namespace CardGameEngine.Objects.Impl.Cards
{
    public class DoudizhuCard : Card
    {
        internal DoudizhuCard(DoudizhuCardValue value, DoudizhuCardColor color, int index)
        {
            Value = value;
            Color = color;
            Index = index;
        }

        public DoudizhuCardValue Value { get; }
        public override int ValueIndex => (int)Value;

        public DoudizhuCardColor Color { get; }
        public override int ColorIndex => (int)Color;
        
        public override int Index { get; }
    }

    public enum DoudizhuCardValue
    {
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace,
        Two,
        ColoredJoker,
        BlackJoker
    }

    public enum DoudizhuCardColor
    {
        NoColor = -1,
        Heart,
        Tile,
        Clover,
        Pike
    }
}

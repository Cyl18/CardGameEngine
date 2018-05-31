using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using CardGameEngine.Data;
using CardGameEngine.Exceptions;

namespace CardGameEngine.Objects
{
    public abstract class Card : IIdentifiable<Card>, IIdentifierOf<CardIdentifier?>, IComparable, IComparable<Card>
    {
        public abstract int ValueIndex { get; }
        public abstract int ColorIndex { get; }
        public abstract int Index { get; }
        public object InnerIdentifier => Identifier;
        public CardIdentifier? Identifier { get; private set; }

        protected bool Equals(Card other)
        {
            return (Identifier ?? 
                    throw new Exception($"LG hAI_rEn_bU_QiaN. Check if you have card identifier initialized. Type is {this.GetType().Name}."))
                .Equals(other.Identifier);
        }


       

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Card) obj);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return (Identifier ?? 
                    throw new Exception($"LG hAI_rEn_bU_QiaN. Check if you have card identifier initialized. Type is {this.GetType().Name}."))
                .GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (!(obj is Card card)) throw new TypeMismatchException(obj.GetType().ToString());
            return CompareTo(card);
        }

        public int CompareTo(Card other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return Index.CompareTo(other.Index);
        }

        public static bool operator <(Card left, Card right)
        {
            return Comparer<Card>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(Card left, Card right)
        {
            return Comparer<Card>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(Card left, Card right)
        {
            return Comparer<Card>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(Card left, Card right)
        {
            return Comparer<Card>.Default.Compare(left, right) >= 0;
        }

        public static bool operator ==(Card left, Card right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Card left, Card right)
        {
            return !Equals(left, right);
        }

        protected void Initialize()
        {
            Identifier = new CardIdentifier(ValueIndex, ColorIndex);
        }
    }

    public struct CardIdentifier
    {
        public int ValueIndex;
        public int ColorIndex;
        private const int NoColor = -1;

        public CardIdentifier(int valueIndex, int? colorIndex = null)
        {
            ValueIndex = valueIndex;
            ColorIndex = colorIndex ?? NoColor;
        }
    }
}

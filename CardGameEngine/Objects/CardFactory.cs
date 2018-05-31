using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.Objects
{
    public interface ICardFactory<TCard> where TCard : Card
    {
        TCard[] Deck { get; }
    }
}

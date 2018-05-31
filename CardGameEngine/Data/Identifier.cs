using System;

namespace CardGameEngine.Data
{
    public class Identifier<T> where T : IIdentifiable<T>
    {
        private Identifier(IIdentifiable<T> identifiable)
        {
            InnerIdentifier = identifiable.InnerIdentifier;
        }
        public object InnerIdentifier { get; }
        public override int GetHashCode() => InnerIdentifier.GetHashCode();
        public override bool Equals(object obj) => Equals(InnerIdentifier, (obj as Identifier<T>)?.InnerIdentifier);
        public static Identifier<T> Of(IIdentifiable<T> identifiable) => new Identifier<T>(identifiable);
    }
    
    public interface IIdentifiable<T> where T : IIdentifiable<T>
    {
        object InnerIdentifier { get; }
    }

    public interface IIdentifierOf<T>
    {
        T Identifier { get; }
    }

    public static class IdentifierExtensions
    {
        public static Identifier<T> GetIdentifier<T>(this IIdentifiable<T> identifiable) where T : IIdentifiable<T>
        {
            return Identifier<T>.Of(identifiable);
        }

    }

}

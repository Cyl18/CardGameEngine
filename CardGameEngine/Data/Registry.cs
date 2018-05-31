using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.Data
{
    public abstract class Registry<TRegistry, TRegistrable> : IIdentifiable<TRegistrable>, IIdentifierOf<RegistryIdentifier>, IRegister<TRegistrable> where TRegistry : Registry<TRegistry, TRegistrable>, new() where TRegistrable : IRegistrable<TRegistrable>, IIdentifiable<TRegistrable>
    {
        public IReadOnlyDictionary<Identifier<TRegistrable>, TRegistrable> Entries => InternalRegistry;
        private ConcurrentDictionary<Identifier<TRegistrable>, TRegistrable> InternalRegistry { get; } = new ConcurrentDictionary<Identifier<TRegistrable>, TRegistrable>();
        public static readonly TRegistry Instance = new TRegistry();
        
        public static void Register(TRegistrable registrable) => Instance.InternalRegistry.TryAdd(registrable.GetIdentifier(), registrable);
        public static void Unregister(TRegistrable registrable) => Instance.InternalRegistry.TryRemove(registrable.GetIdentifier(), out _);
        
        public static TRegistrable  Get(Identifier<TRegistrable> key) => Instance.InternalRegistry[key];

        public object InnerIdentifier => Identifier;
        public RegistryIdentifier Identifier { get; } = new RegistryIdentifier(typeof(TRegistry).Name);
    }

    public interface IRegister<TRegistrable> where TRegistrable : IIdentifiable<TRegistrable>
    {
        IReadOnlyDictionary<Identifier<TRegistrable>, TRegistrable> Entries { get; }
    }
    

    public struct RegistryIdentifier
    {
        public string Name;

        public RegistryIdentifier(string name) => Name = name;
    }

    
    public interface IRegistrable<T> { }
}

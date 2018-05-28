using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameEngine.Data
{
    public class DataContainer<TContainer, TData>
    {
        public TData Content { get; }

        public DataContainer(TData data) => Content = data;

        public static implicit operator TData(DataContainer<TContainer, TData> container) => container.Content;

        public static DataContainer<TContainer, TData> Of(TData data) => new DataContainer<TContainer, TData>(data);

        public override string ToString() => Content.ToString();

        public static bool operator ==(DataContainer<TContainer, TData> left, DataContainer<TContainer, TData> right) => Equals(left, right);

        public static bool operator !=(DataContainer<TContainer, TData> left, DataContainer<TContainer, TData> right) => !Equals(left, right);

        private bool Equals(DataContainer<TContainer, TData> other) => Content.Equals(other.Content);

        public override bool Equals(object obj) => !(obj is null) && (ReferenceEquals(this, obj) || Equals((DataContainer<TContainer, TData>)obj));

        public override int GetHashCode() => Content.GetHashCode();
    }
}
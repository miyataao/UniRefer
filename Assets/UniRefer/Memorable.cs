using System;
using UnityEngine;

namespace UniRefer
{
    public interface IMemorable { }

    public interface IReadOnlyMemorable<T> : IReadOnlyRefer<T>, IMemorable
    {
        T Current { get; }
        T Previous { get; }
        bool EqualPrevious { get; }
        bool Changed { get; }
        void Deconstruct(out T c, out T p);
        (T previous, T current) Deconstructed { get; }
    }

    public interface IMemorable<T> : IReadOnlyMemorable<T>, IRefer<T> { }

    public interface IMemorableSetting
    {
        bool AllowOverwriteSame { get; set; }
    }

    [Serializable]
    public abstract class MemorableBase<T> : IMemorable<T>
    {
        public bool allowOverwriteSame = false;
        [SerializeField] private T current;
        [SerializeField] private T previous;

        public T Current { get => current; protected set => current = value; }
        public T Previous { get => previous; protected set => previous = value; }

        public virtual T Value
        {
            get => Current;
            set => Update(value);
        }

        protected bool Update(T newValue)
        {
            if(!allowOverwriteSame && Equals(newValue, Value)) return false;
            Previous = Current;
            Current = newValue;
            return true;
        }

        public bool EqualPrevious => Previous.Equals(Value);
        public bool Changed => !Previous.Equals(Value);

        public void Deconstruct(out T p, out T c)
        {
            c = Current;
            p = Previous;
        }
        public (T previous, T current) Deconstructed => (Previous, Current);
    }

    [Serializable]
    public class Memorable<T> : MemorableBase<T>
    {
        public Memorable(T _value = default)
        {
            Current = _value;
        }
    }


    [Serializable]
    public class MemorableBool : Memorable<bool>
    {
        public MemorableBool(bool _value = default) : base(_value) { }
    }
    [Serializable]
    public class MemorableInt : Memorable<int>
    {
        public MemorableInt(int _value = default) : base(_value) { }
    }
}

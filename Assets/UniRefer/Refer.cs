using System;

namespace UniRefer
{
    using UnityEngine;

    public interface IRefer { }

    public interface IRR<T> : IRefer
    {
        T Value { get; }
    }

    public interface IReadOnlyRefer<T> : IRR<T> { }

    public interface IRefer<T> : IReadOnlyRefer<T>
    {
        new T Value { get; set; }
    }

    [Serializable]
    public class Refer<T> : IRefer<T>
    {
        [SerializeField] protected T value;
        public virtual T Value
        {
            get => value;
            set => this.value = value;
        }
        public Refer() { Value = default; }
        public Refer(T value = default) { Value = value; }
    }

    namespace Refer
    {
        public interface IRef
        {
            Type ValueType { get; }
            object ObjectValue { get; }
        }

        [Serializable]
        public abstract class Ref<T> : Refer<T>, IRef
        {
            public Type ValueType => Value.GetType();
            public object ObjectValue => Value;
            protected Ref(T value) : base(value) { }
        }

        [Serializable] public class StringRef : Ref<string>
        {
            public StringRef(string value = default) : base(value) { }
        }

        [Serializable] public class IntRef : Ref<int>
        {
            public IntRef(int value = default) : base(value) { }
        }

        [Serializable] public class FloatRef : Ref<float>
        {
            public FloatRef(float value = default) : base(value) { }
        }

        [Serializable] public class BoolRef : Ref<bool>
        {
            public BoolRef(bool value = default) : base(value) { }
        }

        [Serializable] public class Vector2Ref : Ref<Vector2>
        {
            public Vector2Ref(Vector2 value = default) : base(value) { }
        }

        [Serializable] public class Vector3Ref : Ref<Vector3>
        {
            public Vector3Ref(Vector3 value = default) : base(value) { }
        }

        [Serializable] public class ColorRef : Ref<Color>
        {
            public ColorRef(Color value = default) : base(value) { }
        }

    }
}

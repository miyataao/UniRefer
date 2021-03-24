using System;
using UniRx;
using UnityEngine;

namespace UniRefer
{
    public interface IObservableRefer : IRefer { }

    public interface IReadOnlyObservableRefer<T> : IObservableRefer, IReadOnlyRefer<T>, IObservable<T>
    {
        IObservable<T> OnAssignment { get; }
        IObservable<T> OnChanged { get; }
    }

    public interface IObservableRefer<T> : IReadOnlyObservableRefer<T>, IRefer<T>,
                                           IDisposable, IReactiveProperty<T>, IOptimizedObservable<T> { }

    [Serializable]
    public class ObservableRefer<T> : Refer<T>, IObservableRefer<T>
    {
        public IDisposable Subscribe(IObserver<T> observer) => onAssignment.Subscribe(observer);
        public IObservable<T> OnAssignment => onAssignment;
        private readonly Subject<T> onAssignment = new Subject<T>();

        public IObservable<T> OnChanged => onChanged; // onAssignment.DistinctUntilChanged();
        private readonly Subject<T> onChanged = new Subject<T>();

        public override T Value
        {
            get => value;
            set
            {
                bool eq = this.value.Equals(value);
                this.value = value;
                if(!eq) onChanged.OnNext(value);
                onAssignment.OnNext(value);
            }
        }
        public bool HasValue => true;
        public bool IsRequiredSubscribeOnCurrentThread() => false;

        public ObservableRefer(T value) : base(value) { }

        public void Dispose()
        {
            onAssignment.OnCompleted();
            onChanged.OnCompleted();
            onAssignment.Dispose();
            onChanged.Dispose();
        }
    }

    namespace Refer
    {

        [Serializable]
        public abstract class ObservableRef<T> : ObservableRefer<T>, IRef
        {
            public Type ValueType => Value.GetType();
            public object ObjectValue => Value;
            protected ObservableRef(T value) : base(value) { }
        }

        [Serializable] public class ObservableStringRef : ObservableRef<string>
        {
            public ObservableStringRef(string value = default) : base(value) { }
        }

        [Serializable] public class ObservableIntRef : ObservableRef<int>
        {
            public ObservableIntRef(int value = default) : base(value) { }
        }

        [Serializable] public class ObservableFloatRef : ObservableRef<float>
        {
            public ObservableFloatRef(float value = default) : base(value) { }
        }

        [Serializable] public class ObservableBoolRef : ObservableRef<bool>
        {
            public ObservableBoolRef(bool value = default) : base(value) { }
        }

        [Serializable] public class ObservableVector2Ref : ObservableRef<Vector2>
        {
            public ObservableVector2Ref(Vector2 value = default) : base(value) { }
        }

        [Serializable] public class ObservableVector3Ref : ObservableRef<Vector3>
        {
            public ObservableVector3Ref(Vector3 value = default) : base(value) { }
        }

        [Serializable] public class ObservableColorRef : ObservableRef<Color>
        {
            public ObservableColorRef(Color value = default) : base(value) { }
        }
    }
}

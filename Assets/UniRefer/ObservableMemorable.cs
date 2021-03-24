using System;
using UniRx;

namespace UniRefer
{

    public interface IReadOnlyObservableMemorable<T> : IReadOnlyMemorable<T>, IReadOnlyObservableRefer<T>
    {
        IOptimizedObservable<(T previous, T current)> OnValueChanged { get; }
    }

    public interface IObservableMemorable<T> : IReadOnlyObservableMemorable<T>, IObservableRefer<T> { }


    [Serializable]
    public class ObservableMemorable<T> : MemorableBase<T>, IObservableMemorable<T>
    {
        public IDisposable Subscribe(IObserver<T> observer) => onAssignment.Subscribe(observer);
        public IObservable<T> OnAssignment => onAssignment;
        private readonly Subject<T> onAssignment = new Subject<T>();

        public IObservable<T> OnChanged => onChanged;
        private readonly Subject<T> onChanged = new Subject<T>();

        public IOptimizedObservable<(T previous, T current)> OnValueChanged => onValueChanged;
        private readonly Subject<(T previous, T current)> onValueChanged = new Subject<(T previous, T current)>();


        public override T Value
        {
            get => Current;
            set
            {
                onAssignment.OnNext(value);
                if(Update(value))
                {
                    onChanged.OnNext(Current);
                    onValueChanged.OnNext(Deconstructed);
                }
            }
        }

        public ObservableMemorable(T _value = default)
        {
            Current = _value;
        }

        public void SetValueToDefault()
        {
            Value = default;
        }

        public bool HasValue => true;
        public bool IsRequiredSubscribeOnCurrentThread() => false;
        public void Dispose()
        {
            onAssignment.OnCompleted();
            onChanged.OnCompleted();
            onValueChanged.OnCompleted();
            onAssignment.Dispose();
            onChanged.Dispose();
            onValueChanged.Dispose();
        }
    }
}

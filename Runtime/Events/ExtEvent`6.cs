namespace ExtEvents
{
    using System;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    [Serializable]
    public class ExtEvent<T1, T2, T3, T4, T5, T6> : BaseExtEvent
    {
        private readonly unsafe void*[] _arguments = new void*[6];

        private Type[] _eventParamTypes;
        protected override Type[] EventParamTypes => _eventParamTypes ??= new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) };

        /// <summary>
        /// The dynamic listeners list that you can add your listener to.
        /// </summary>
        [PublicAPI]
        public event Action<T1, T2, T3, T4, T5, T6> DynamicListeners;
        internal override Delegate _dynamicListeners => DynamicListeners;

        /// <summary>
        /// Invokes all listeners of the event.
        /// </summary>
        [PublicAPI]
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            unsafe
            {
                _arguments[0] = Unsafe.AsPointer(ref arg1);
                _arguments[1] = Unsafe.AsPointer(ref arg2);
                _arguments[2] = Unsafe.AsPointer(ref arg3);
                _arguments[3] = Unsafe.AsPointer(ref arg4);
                _arguments[4] = Unsafe.AsPointer(ref arg5);
                _arguments[5] = Unsafe.AsPointer(ref arg6);

                // ReSharper disable once ForCanBeConvertedToForeach
                for (int index = 0; index < _persistentListeners.Length; index++)
                {
                    _persistentListeners[index].Invoke(_arguments);
                }
            }

            DynamicListeners?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static ExtEvent<T1, T2, T3, T4, T5, T6> operator +(ExtEvent<T1, T2, T3, T4, T5, T6> extEvent, Action<T1, T2, T3, T4, T5, T6> listener)
        {
            if (extEvent == null)
                return null;

            extEvent.DynamicListeners += listener;
            return extEvent;
        }

        public static ExtEvent<T1, T2, T3, T4, T5, T6> operator -(ExtEvent<T1, T2, T3, T4, T5, T6> extEvent, Action<T1, T2, T3, T4, T5, T6> listener)
        {
            if (extEvent == null)
                return null;

            extEvent.DynamicListeners -= listener;
            return extEvent;
        }
    }
}
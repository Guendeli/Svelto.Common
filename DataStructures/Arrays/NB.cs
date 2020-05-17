using System;
using System.Runtime.CompilerServices;
using Svelto.Common;

namespace Svelto.DataStructures
{
    /// <summary>
    /// NB stands for NB
    /// NativeBuffers are current designed to be used inside Jobs. They wrap an EntityDB array of components
    /// but do not track it. Hence it's meant to be used temporary and locally as the array can become invalid
    /// after a submission of entities.
    ///
    /// NB are wrappers of native arrays. Are not meant to resize or free
    ///
    /// NBs cannot have a count, because a count of the meaningful number of items is not tracked.
    /// Example: an MB could be initialized with a size 10 and count 0. Then the buffer is used to fill entities
    /// but the count will stay zero. It's not the MB responsibility to track the count
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct NB<T>:IBuffer<T> where T:struct
    {
        static NB()
        {
            if (UnmanagedTypeExtensions.IsUnmanaged<T>() == false)
                throw new Exception("NativeBuffer (NB) supports only unmanaged types");
        }
        
        public NB(IntPtr array, uint capacity) : this()
        {
            _ptr = array;
            _capacity = capacity;
        }

        public void Set(IntPtr array, uint capacity)
        {
            _ptr      = array;
            _capacity = capacity;
        }

        public void CopyTo(uint sourceStartIndex, T[] destination, uint destinationStartIndex, uint size) { throw new NotImplementedException(); }
        public void Clear()
        {
            unsafe
            {
                Unsafe.InitBlock((void *)_ptr, 0, (uint) (_capacity * MemoryUtilities.SizeOf<T>()));
            }
        }

        public T[] ToManagedArray()
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr ToNativeArray(out int capacity)
        {
            capacity = (int) _capacity; return _ptr; 
        }

        public int capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int) _capacity;
        }

        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
            get
            {
                unsafe
                {
#if DEBUG && !PROFILE_SVELTO
                    if (index >= _capacity)
                        throw new Exception("NativeBuffer - out of bound access");
#endif                    
                    return ref Unsafe.AsRef<T>(Unsafe.Add<T>((void*) _ptr, (int) index));
                }
            }
        }

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
            get
            {
                unsafe
                {
#if DEBUG && !PROFILE_SVELTO
                    if (index >= _capacity)
                        throw new Exception("NativeBuffer - out of bound access");
#endif                    
                    
                    return ref Unsafe.AsRef<T>(Unsafe.Add<T>((void*) _ptr, (int) index));
                }
            }
        }

        uint _capacity;
#if UNITY_COLLECTIONS
        //todo can I remove this from here? it should be used outside
        [Unity.Burst.NoAlias]
        [Unity.Collections.LowLevel.Unsafe.NativeDisableUnsafePtrRestriction]
#endif
        public IntPtr _ptr;

        public NB<T> AsReader() { return this; }
        public NB<T> AsWriter() { return this; }
    }
}

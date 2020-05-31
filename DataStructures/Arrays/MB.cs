using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Svelto.DataStructures
{
    /// <summary>
    /// MB stands for ManagedBuffer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct MB<T>:IBuffer<T> 
    {
        public void Set(T[] array, uint count)
        {
            _count = count;
            _buffer = array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyFrom<TBuffer>(TBuffer array, uint startIndex, uint size) where TBuffer:IBuffer<T>
        {
            array.CopyTo(_buffer, 0, startIndex, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyFrom(T[] source, uint sourceStartIndex, uint destinationStartIndex, uint size)
        {
            Array.Copy(source, sourceStartIndex, _buffer, destinationStartIndex, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] destination, uint sourceStartIndex, uint destinationStartIndex, uint size)
        {
            Array.Copy(_buffer, sourceStartIndex, destination, destinationStartIndex, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyFrom(ICollection<T> source)
        {
            source.CopyTo(_buffer, source.Count); 
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(uint startIndex, uint count)
        {
            Array.Clear(_buffer, (int) startIndex, (int) count);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Array.Clear(_buffer, (int) 0, (int) _buffer.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnorderedRemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToManagedArray()
        {
            return _buffer;
        }

        public IntPtr ToNativeArray()
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GCHandle Pin()
        {
            return GCHandle.Alloc(_buffer, GCHandleType.Pinned);
        }

        public uint capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (uint) _buffer.Length;
        }
        
        public uint count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _count;
        }

        public void Dispose()
        {
            _buffer = null;
        }
        
        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _buffer[index];
        }
        
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _buffer[index];
        }

        T[] _buffer;
        uint _count;
    }
}
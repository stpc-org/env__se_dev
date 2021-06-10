// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.BitStream
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using ParallelTasks;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using VRage.Collections;
using VRage.Library.Memory;
using VRage.Library.Utils;

namespace VRage.Library.Collections
{
  public class BitStream : IDisposable
  {
    private static MyConcurrentBufferPool<NativeArray> BufferPool = new MyConcurrentBufferPool<NativeArray>("BitStreamBuffers", (IMyElementAllocator<NativeArray>) new NativeArrayAllocator(Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("BitStreamBuffers")));
    public const long MaxSize = 17179869176;
    private unsafe ulong* m_buffer;
    private GCHandle m_bufferHandle;
    private NativeArray m_ownedBuffer;
    private readonly int m_defaultByteSize;
    public const int TERMINATOR_SIZE = 2;
    private const ushort TERMINATOR = 51385;
    private const long Int64Msb = -9223372036854775808;
    private const int Int32Msb = -2147483648;

    public long BitPosition { get; private set; }

    public long BitLength { get; private set; }

    public int BytePosition => (int) MyLibraryUtils.GetDivisionCeil(this.BitPosition, 8L);

    public int ByteLength => (int) MyLibraryUtils.GetDivisionCeil(this.BitLength, 8L);

    private bool OwnsBuffer => this.m_ownedBuffer != null;

    public bool Reading => !this.Writing;

    public bool Writing { get; private set; }

    public unsafe IntPtr DataPointer => (IntPtr) (void*) this.m_buffer;

    public BitStream(int defaultByteSize = 1536) => this.m_defaultByteSize = Math.Max(16, MyLibraryUtils.GetDivisionCeil(defaultByteSize, 8) * 8);

    public void Dispose()
    {
      this.ReleaseInternalBuffer();
      GC.SuppressFinalize((object) this);
    }

    ~BitStream() => this.ReleaseInternalBuffer();

    private void EnsureSize(long bitCount)
    {
      if (this.BitLength >= bitCount)
        return;
      this.Resize(bitCount);
    }

    private unsafe void Resize(long bitSize)
    {
      if (!this.OwnsBuffer)
        throw new BitStreamException("BitStream cannot write more data. Buffer is full and it's not owned by BitStream", (Exception) new EndOfStreamException());
      if (bitSize > 17179869176L)
        throw new OutOfMemoryException("The maximum capacity for any bit stream would be exceeded by the operation.");
      int bytePosition = this.BytePosition;
      long num = Math.Max(Math.Min(this.BitLength * 2L, 17179869176L), bitSize);
      int bucketId = (int) MyLibraryUtils.GetDivisionCeil(num, 64L) * 8;
      NativeArray nativeArray = BitStream.BufferPool.Get(bucketId);
      Buffer.MemoryCopy((void*) this.m_buffer, nativeArray.Ptr.ToPointer(), (long) bucketId, (long) bytePosition);
      Unsafe.InitBlockUnaligned((nativeArray.Ptr + bytePosition).ToPointer(), (byte) 0, (uint) (bucketId - bytePosition));
      BitStream.BufferPool.Return(this.m_ownedBuffer);
      this.m_ownedBuffer = nativeArray;
      this.m_buffer = (ulong*) (void*) nativeArray.Ptr;
      this.BitLength = num;
    }

    private unsafe void ReleaseInternalBuffer()
    {
      this.FreeNotOwnedBuffer();
      if (!this.OwnsBuffer)
        return;
      this.m_buffer = (ulong*) null;
      this.BitLength = 0L;
      BitStream.BufferPool.Return(this.m_ownedBuffer);
      this.m_ownedBuffer = (NativeArray) null;
    }

    public unsafe void ResetRead()
    {
      this.FreeNotOwnedBuffer();
      this.BitLength = this.BitPosition;
      this.m_buffer = (ulong*) (void*) this.m_ownedBuffer?.Ptr.Value;
      this.Writing = false;
      this.BitPosition = 0L;
    }

    public unsafe void ResetRead(byte[] data, int byteOffset, long bitLength, bool copy = true)
    {
      fixed (byte* numPtr = &data[byteOffset])
      {
        this.ResetRead((IntPtr) (void*) numPtr, bitLength, copy);
        if (!copy)
          this.m_bufferHandle = GCHandle.Alloc((object) data, GCHandleType.Pinned);
      }
    }

    public void ResetRead(BitStream source, bool copy = true) => this.ResetRead(source.DataPointer + source.BytePosition, source.BitLength - (long) (source.BytePosition * 8), copy);

    public unsafe void ResetRead(IntPtr buffer, long bitLength, bool copy)
    {
      if (bitLength > 17179869176L)
        throw new OutOfMemoryException("The maximum capacity for any bit stream would be exceeded by the operation.");
      this.FreeNotOwnedBuffer();
      if (copy)
      {
        int divisionCeil = (int) MyLibraryUtils.GetDivisionCeil(bitLength, 8L);
        int bucketId = Math.Max(divisionCeil, this.m_defaultByteSize);
        if (this.m_ownedBuffer == null || (long) this.m_ownedBuffer.Size < bitLength)
        {
          if (this.m_ownedBuffer != null)
            BitStream.BufferPool.Return(this.m_ownedBuffer);
          this.m_ownedBuffer = BitStream.BufferPool.Get(bucketId);
        }
        Buffer.MemoryCopy(buffer.ToPointer(), this.m_ownedBuffer.Ptr.ToPointer(), (long) divisionCeil, (long) divisionCeil);
        this.m_buffer = (ulong*) (void*) this.m_ownedBuffer.Ptr;
        this.BitLength = bitLength;
        this.BitPosition = 0L;
        this.Writing = false;
      }
      else
      {
        this.m_buffer = (ulong*) (void*) buffer;
        this.BitLength = bitLength;
        this.BitPosition = 0L;
        this.Writing = false;
      }
    }

    private unsafe void FreeNotOwnedBuffer()
    {
      if (this.OwnsBuffer || (IntPtr) this.m_buffer == IntPtr.Zero || !this.m_bufferHandle.IsAllocated)
        return;
      this.m_bufferHandle.Free();
    }

    public unsafe void ResetWrite()
    {
      this.FreeNotOwnedBuffer();
      if (this.m_ownedBuffer == null)
        this.m_ownedBuffer = BitStream.BufferPool.Get(this.m_defaultByteSize);
      this.m_buffer = (ulong*) (void*) this.m_ownedBuffer.Ptr;
      this.BitLength = (long) (this.m_ownedBuffer.Size * 8);
      this.BitPosition = 0L;
      *this.m_buffer = 0UL;
      this.Writing = true;
    }

    public void Serialize(ref double value)
    {
      if (this.Writing)
        this.WriteDouble(value);
      else
        value = this.ReadDouble();
    }

    public void Serialize(ref float value)
    {
      if (this.Writing)
        this.WriteFloat(value);
      else
        value = this.ReadFloat();
    }

    public void Serialize(ref Decimal value)
    {
      if (this.Writing)
        this.WriteDecimal(value);
      else
        value = this.ReadDecimal();
    }

    public void Serialize(ref bool value)
    {
      if (this.Writing)
        this.WriteBool(value);
      else
        value = this.ReadBool();
    }

    public void Serialize(ref sbyte value, int bitCount = 8)
    {
      if (this.Writing)
        this.WriteSByte(value, bitCount);
      else
        value = this.ReadSByte(bitCount);
    }

    public void Serialize(ref short value, int bitCount = 16)
    {
      if (this.Writing)
        this.WriteInt16(value, bitCount);
      else
        value = this.ReadInt16(bitCount);
    }

    public void Serialize(ref int value, int bitCount = 32)
    {
      if (this.Writing)
        this.WriteInt32(value, bitCount);
      else
        value = this.ReadInt32(bitCount);
    }

    public void Serialize(ref long value, int bitCount = 64)
    {
      if (this.Writing)
        this.WriteInt64(value, bitCount);
      else
        value = this.ReadInt64(bitCount);
    }

    public void Serialize(ref byte value, int bitCount = 8)
    {
      if (this.Writing)
        this.WriteByte(value, bitCount);
      else
        value = this.ReadByte(bitCount);
    }

    public void Serialize(ref ushort value, int bitCount = 16)
    {
      if (this.Writing)
        this.WriteUInt16(value, bitCount);
      else
        value = this.ReadUInt16(bitCount);
    }

    public void Serialize(ref uint value, int bitCount = 32)
    {
      if (this.Writing)
        this.WriteUInt32(value, bitCount);
      else
        value = this.ReadUInt32(bitCount);
    }

    public void Serialize(ref ulong value, int bitCount = 64)
    {
      if (this.Writing)
        this.WriteUInt64(value, bitCount);
      else
        value = this.ReadUInt64(bitCount);
    }

    public void SerializeVariant(ref int value)
    {
      if (this.Writing)
        this.WriteVariantSigned(value);
      else
        value = this.ReadInt32Variant();
    }

    public void SerializeVariant(ref long value)
    {
      if (this.Writing)
        this.WriteVariantSigned(value);
      else
        value = this.ReadInt64Variant();
    }

    public void SerializeVariant(ref uint value)
    {
      if (this.Writing)
        this.WriteVariant(value);
      else
        value = this.ReadUInt32Variant();
    }

    public void SerializeVariant(ref ulong value)
    {
      if (this.Writing)
        this.WriteVariant(value);
      else
        value = this.ReadUInt64Variant();
    }

    public void Serialize(ref char value)
    {
      if (this.Writing)
        this.WriteChar(value);
      else
        value = this.ReadChar();
    }

    public void Serialize(StringBuilder value, ref char[] tmpArray, Encoding encoding)
    {
      if (this.Writing)
      {
        if (value.Length > tmpArray.Length)
          tmpArray = new char[Math.Max(value.Length, tmpArray.Length * 2)];
        value.CopyTo(0, tmpArray, 0, value.Length);
        this.WritePrefixLengthString(tmpArray, 0, value.Length, encoding);
      }
      else
      {
        value.Clear();
        int charCount = this.ReadPrefixLengthString(ref tmpArray, encoding);
        value.Append(tmpArray, 0, charCount);
      }
    }

    public void SerializeMemory(IntPtr ptr, long bitSize)
    {
      if (this.Writing)
        this.WriteMemory(ptr, bitSize);
      else
        this.ReadMemory(ptr, bitSize);
    }

    public unsafe void SerializeMemory(void* ptr, long bitSize)
    {
      if (this.Writing)
        this.WriteMemory(ptr, bitSize);
      else
        this.ReadMemory(ptr, bitSize);
    }

    public void SerializePrefixString(ref string str, Encoding encoding)
    {
      if (this.Writing)
        this.WritePrefixLengthString(str, 0, str.Length, encoding);
      else
        str = this.ReadPrefixLengthString(encoding);
    }

    public void SerializePrefixStringAscii(ref string str) => this.SerializePrefixString(ref str, Encoding.ASCII);

    public void SerializePrefixStringUtf8(ref string str) => this.SerializePrefixString(ref str, Encoding.UTF8);

    public void SerializePrefixBytes(ref byte[] bytes)
    {
      if (this.Writing)
      {
        this.WriteVariant((uint) bytes.Length);
        this.WriteBytes(bytes, 0, bytes.Length);
      }
      else
      {
        int count = (int) this.ReadUInt32Variant();
        bytes = new byte[count];
        this.ReadBytes(bytes, 0, count);
      }
    }

    public void SerializeBytes(ref byte[] bytes, int start, int count)
    {
      if (this.Writing)
        this.WriteBytes(bytes, start, count);
      else
        this.ReadBytes(bytes, start, count);
    }

    public void Terminate() => this.WriteUInt16((ushort) 51385);

    public bool CheckTerminator() => this.ReadUInt16() == (ushort) 51385;

    public Type ReadDynamicType(Type baseType, DynamicSerializerDelegate typeResolver)
    {
      Type type = (Type) null;
      typeResolver(this, baseType, ref type);
      return type;
    }

    public void WriteDynamicType(Type baseType, Type obj, DynamicSerializerDelegate typeResolver) => typeResolver(this, baseType, ref obj);

    [HandleProcessCorruptedStateExceptions]
    [SecurityCritical]
    private unsafe ulong ReadInternal(int bitSize)
    {
      long num1 = this.BitPosition >> 6;
      long num2 = this.BitPosition + (long) bitSize - 1L >> 6;
      ulong num3 = ulong.MaxValue >> 64 - bitSize;
      int num4 = (int) (this.BitPosition & 63L);
      ulong num5 = (ulong) *(long*) ((IntPtr) this.m_buffer + (IntPtr) (num1 * 8L)) >> num4;
      if (num2 != num1)
        num5 |= (ulong) (*(long*) ((IntPtr) this.m_buffer + (IntPtr) (num2 * 8L)) << 64 - num4);
      this.BitPosition += (long) bitSize;
      return num5 & num3;
    }

    public void SetBitPositionRead(long newReadBitPosition) => this.BitPosition = newReadBitPosition;

    public unsafe double ReadDouble() => *(double*) &this.ReadInternal(64);

    public unsafe float ReadFloat() => *(float*) &this.ReadInternal(32);

    public float ReadNormalizedSignedFloat(int bits) => MyLibraryUtils.DenormalizeFloatCenter(this.ReadUInt32(bits), -1f, 1f, bits);

    public unsafe Decimal ReadDecimal()
    {
      Decimal num;
      *(long*) &num = (long) this.ReadInternal(64);
      *(long*) ((IntPtr) &num + 8) = (long) this.ReadInternal(64);
      return num;
    }

    public bool ReadBool() => this.ReadInternal(1) > 0UL;

    public sbyte ReadSByte(int bitCount = 8) => (sbyte) this.ReadInternal(bitCount);

    public short ReadInt16(int bitCount = 16) => (short) this.ReadInternal(bitCount);

    public int ReadInt32(int bitCount = 32) => (int) this.ReadInternal(bitCount);

    public long ReadInt64(int bitCount = 64) => (long) this.ReadInternal(bitCount);

    public byte ReadByte(int bitCount = 8) => (byte) this.ReadInternal(bitCount);

    public ushort ReadUInt16(int bitCount = 16) => (ushort) this.ReadInternal(bitCount);

    public uint ReadUInt32(int bitCount = 32) => (uint) this.ReadInternal(bitCount);

    public ulong ReadUInt64(int bitCount = 64) => this.ReadInternal(bitCount);

    private static int Zag(uint ziggedValue)
    {
      int num = (int) ziggedValue;
      return -(num & 1) ^ num >> 1 & int.MaxValue;
    }

    private static long Zag(ulong ziggedValue)
    {
      long num = (long) ziggedValue;
      return -(num & 1L) ^ num >> 1 & long.MaxValue;
    }

    public int ReadInt32Variant() => BitStream.Zag(this.ReadUInt32Variant());

    public long ReadInt64Variant() => BitStream.Zag(this.ReadUInt64Variant());

    public uint ReadUInt32Variant()
    {
      uint num1 = (uint) this.ReadByte();
      if (((int) num1 & 128) == 0)
        return num1;
      uint num2 = num1 & (uint) sbyte.MaxValue;
      uint num3 = (uint) this.ReadByte();
      uint num4 = num2 | (uint) (((int) num3 & (int) sbyte.MaxValue) << 7);
      if (((int) num3 & 128) == 0)
        return num4;
      uint num5 = (uint) this.ReadByte();
      uint num6 = num4 | (uint) (((int) num5 & (int) sbyte.MaxValue) << 14);
      if (((int) num5 & 128) == 0)
        return num6;
      uint num7 = (uint) this.ReadByte();
      uint num8 = num6 | (uint) (((int) num7 & (int) sbyte.MaxValue) << 21);
      if (((int) num7 & 128) == 0)
        return num8;
      uint num9 = (uint) this.ReadByte();
      uint num10 = num8 | num9 << 28;
      if (((int) num9 & 240) == 0)
        return num10;
      throw new BitStreamException((Exception) new OverflowException("Error when deserializing variant uint32"));
    }

    public ulong ReadUInt64Variant()
    {
      ulong num1 = (ulong) this.ReadByte();
      if (((long) num1 & 128L) == 0L)
        return num1;
      ulong num2 = num1 & (ulong) sbyte.MaxValue;
      ulong num3 = (ulong) this.ReadByte();
      ulong num4 = num2 | (ulong) (((long) num3 & (long) sbyte.MaxValue) << 7);
      if (((long) num3 & 128L) == 0L)
        return num4;
      ulong num5 = (ulong) this.ReadByte();
      ulong num6 = num4 | (ulong) (((long) num5 & (long) sbyte.MaxValue) << 14);
      if (((long) num5 & 128L) == 0L)
        return num6;
      ulong num7 = (ulong) this.ReadByte();
      ulong num8 = num6 | (ulong) (((long) num7 & (long) sbyte.MaxValue) << 21);
      if (((long) num7 & 128L) == 0L)
        return num8;
      ulong num9 = (ulong) this.ReadByte();
      ulong num10 = num8 | (ulong) (((long) num9 & (long) sbyte.MaxValue) << 28);
      if (((long) num9 & 128L) == 0L)
        return num10;
      ulong num11 = (ulong) this.ReadByte();
      ulong num12 = num10 | (ulong) (((long) num11 & (long) sbyte.MaxValue) << 35);
      if (((long) num11 & 128L) == 0L)
        return num12;
      ulong num13 = (ulong) this.ReadByte();
      ulong num14 = num12 | (ulong) (((long) num13 & (long) sbyte.MaxValue) << 42);
      if (((long) num13 & 128L) == 0L)
        return num14;
      ulong num15 = (ulong) this.ReadByte();
      ulong num16 = num14 | (ulong) (((long) num15 & (long) sbyte.MaxValue) << 49);
      if (((long) num15 & 128L) == 0L)
        return num16;
      ulong num17 = (ulong) this.ReadByte();
      ulong num18 = num16 | (ulong) (((long) num17 & (long) sbyte.MaxValue) << 56);
      if (((long) num17 & 128L) == 0L)
        return num18;
      ulong num19 = (ulong) this.ReadByte();
      ulong num20 = num18 | num19 << 63;
      if (((long) num19 & -2L) != 0L)
        throw new BitStreamException((Exception) new OverflowException("Error when deserializing variant uint64"));
      return num20;
    }

    public char ReadChar(int bitCount = 16) => (char) this.ReadInternal(bitCount);

    public unsafe void ReadMemory(IntPtr ptr, long bitSize) => this.ReadMemory((void*) ptr, bitSize);

    public unsafe void ReadMemory(void* ptr, long bitSize)
    {
      int num = (int) (bitSize / 8L / 8L);
      ulong* numPtr1 = (ulong*) ptr;
      for (int index = 0; index < num; ++index)
        numPtr1[index] = this.ReadUInt64();
      int val1 = (int) (bitSize - (long) (num * 8 * 8));
      byte* numPtr2 = (byte*) (numPtr1 + num);
      while (val1 > 0)
      {
        int bitCount = Math.Min(val1, 8);
        *numPtr2 = this.ReadByte(bitCount);
        val1 -= bitCount;
        ++numPtr2;
      }
    }

    public string ReadString() => this.ReadPrefixLengthString(Encoding.UTF8);

    public unsafe string ReadPrefixLengthString(Encoding encoding)
    {
      int length = (int) this.ReadUInt32Variant();
      if (length == 0)
        return string.Empty;
      if (length <= 1024)
      {
        byte* bytes = stackalloc byte[length];
        this.ReadMemory((void*) bytes, (long) (length * 8));
        int charCount = encoding.GetCharCount(bytes, length);
        char* chars = stackalloc char[charCount];
        encoding.GetChars(bytes, length, chars, charCount);
        return new string(chars, 0, charCount);
      }
      byte[] bytes1 = new byte[length];
      fixed (byte* numPtr = bytes1)
        this.ReadMemory((void*) numPtr, (long) (length * 8));
      return new string(encoding.GetChars(bytes1));
    }

    public unsafe int ReadPrefixLengthString(ref char[] value, Encoding encoding)
    {
      int byteCount = (int) this.ReadUInt32Variant();
      if (byteCount == 0)
        return 0;
      if (byteCount > 1024)
      {
        fixed (byte* tmpBuffer = new byte[byteCount])
          return this.ReadChars(tmpBuffer, byteCount, ref value, encoding);
      }
      byte* tmpBuffer1 = stackalloc byte[byteCount];
      return this.ReadChars(tmpBuffer1, byteCount, ref value, encoding);
    }

    private unsafe int ReadChars(
      byte* tmpBuffer,
      int byteCount,
      ref char[] outputArray,
      Encoding encoding)
    {
      this.ReadMemory((void*) tmpBuffer, (long) (byteCount * 8));
      int charCount = encoding.GetCharCount(tmpBuffer, byteCount);
      if (charCount > outputArray.Length)
        outputArray = new char[Math.Max(charCount, outputArray.Length * 2)];
      fixed (char* chars = &outputArray[0])
        encoding.GetChars(tmpBuffer, byteCount, chars, charCount);
      return charCount;
    }

    public unsafe void ReadBytes(byte[] bytes, int start, int count)
    {
      fixed (byte* numPtr = bytes)
        this.ReadMemory((void*) (numPtr + start), (long) (count * 8));
    }

    public byte[] ReadPrefixBytes()
    {
      int count = (int) this.ReadUInt32Variant();
      byte[] bytes = new byte[count];
      this.ReadBytes(bytes, 0, count);
      return bytes;
    }

    private unsafe void WriteInternal(ulong value, int bitSize)
    {
      if (bitSize == 0)
        return;
      this.EnsureSize(this.BitPosition + (long) bitSize);
      long num1 = this.BitPosition >> 6;
      long num2 = this.BitPosition + (long) bitSize - 1L >> 6;
      ulong num3 = ulong.MaxValue >> 64 - bitSize;
      int num4 = (int) (this.BitPosition & 63L);
      value &= num3;
      IntPtr num5 = (IntPtr) this.m_buffer + (IntPtr) (num1 * 8L);
      *(long*) num5 = *(long*) num5 & ~((long) num3 << num4);
      IntPtr num6 = (IntPtr) this.m_buffer + (IntPtr) (num1 * 8L);
      *(long*) num6 = *(long*) num6 | (long) value << num4;
      if (num2 != num1)
      {
        IntPtr num7 = (IntPtr) this.m_buffer + (IntPtr) (num2 * 8L);
        *(long*) num7 = *(long*) num7 & ~(long) (num3 >> 64 - num4);
        IntPtr num8 = (IntPtr) this.m_buffer + (IntPtr) (num2 * 8L);
        *(long*) num8 = *(long*) num8 | (long) (value >> 64 - num4);
      }
      this.BitPosition += (long) bitSize;
    }

    private unsafe void Clear(int fromPosition)
    {
      int num1 = fromPosition >> 6;
      int num2 = fromPosition & 63;
      ulong* numPtr = this.m_buffer + num1;
      *numPtr = *numPtr & (ulong) ~(-1L << num2);
      int divisionCeil = (int) MyLibraryUtils.GetDivisionCeil(this.BitPosition, 64L);
      for (int index = num1 + 1; index < divisionCeil; ++index)
        this.m_buffer[index] = 0UL;
    }

    public void SetBitPositionWrite(long newBitPosition) => this.BitPosition = newBitPosition;

    public unsafe void WriteDouble(double value) => this.WriteInternal((ulong) *(long*) &value, 64);

    public unsafe void WriteFloat(float value) => this.WriteInternal((ulong) *(uint*) &value, 32);

    public void WriteNormalizedSignedFloat(float value, int bits) => this.WriteUInt32(MyLibraryUtils.NormalizeFloatCenter(value, -1f, 1f, bits), bits);

    public unsafe void WriteDecimal(Decimal value)
    {
      this.WriteInternal((ulong) *(long*) &value, 64);
      this.WriteInternal((ulong) *(long*) ((IntPtr) &value + 8), 64);
    }

    public void WriteBool(bool value) => this.WriteInternal(value ? ulong.MaxValue : 0UL, 1);

    public void WriteSByte(sbyte value, int bitCount = 8) => this.WriteInternal((ulong) value, bitCount);

    public void WriteInt16(short value, int bitCount = 16) => this.WriteInternal((ulong) value, bitCount);

    public void WriteInt32(int value, int bitCount = 32) => this.WriteInternal((ulong) value, bitCount);

    public void WriteInt64(long value, int bitCount = 64) => this.WriteInternal((ulong) value, bitCount);

    public void WriteByte(byte value, int bitCount = 8) => this.WriteInternal((ulong) value, bitCount);

    public void WriteUInt16(ushort value, int bitCount = 16) => this.WriteInternal((ulong) value, bitCount);

    public void WriteUInt32(uint value, int bitCount = 32) => this.WriteInternal((ulong) value, bitCount);

    public void WriteUInt64(ulong value, int bitCount = 64) => this.WriteInternal(value, bitCount);

    private static uint Zig(int value) => (uint) (value << 1 ^ value >> 31);

    private static ulong Zig(long value) => (ulong) (value << 1 ^ value >> 63);

    public void WriteVariantSigned(int value) => this.WriteVariant(BitStream.Zig(value));

    public void WriteVariantSigned(long value) => this.WriteVariant(BitStream.Zig(value));

    public unsafe void WriteVariant(uint value)
    {
      ulong num1;
      byte* numPtr1 = (byte*) &num1;
      int num2 = 0;
      int num3 = 0;
      do
      {
        numPtr1[num3++] = (byte) (value | 128U);
        ++num2;
      }
      while ((value >>= 7) != 0U);
      byte* numPtr2 = numPtr1 + (num3 - 1);
      *numPtr2 = (byte) ((uint) *numPtr2 & (uint) sbyte.MaxValue);
      this.WriteInternal(num1, num2 * 8);
    }

    public unsafe void WriteVariant(ulong value)
    {
      byte* numPtr1 = stackalloc byte[16];
      int num1 = 0;
      int num2 = 0;
      do
      {
        numPtr1[num2++] = (byte) (value & (ulong) sbyte.MaxValue | 128UL);
        ++num1;
      }
      while ((value >>= 7) != 0UL);
      byte* numPtr2 = numPtr1 + (num2 - 1);
      *numPtr2 = (byte) ((uint) *numPtr2 & (uint) sbyte.MaxValue);
      if (num1 > 8)
      {
        this.WriteInternal((ulong) *(long*) numPtr1, 64);
        this.WriteInternal((ulong) *(long*) (numPtr1 + 8), (num1 - 8) * 8);
      }
      else
        this.WriteInternal((ulong) *(long*) numPtr1, num1 * 8);
    }

    public void WriteChar(char value, int bitCount = 16) => this.WriteInternal((ulong) value, bitCount);

    public void WriteBitStream(BitStream readStream)
    {
      int bitCount;
      for (long val2 = readStream.BitLength - readStream.BitPosition; val2 > 0L; val2 -= (long) bitCount)
      {
        bitCount = (int) Math.Min(64L, val2);
        this.WriteUInt64(readStream.ReadUInt64(bitCount), bitCount);
      }
    }

    public unsafe void WriteMemory(IntPtr ptr, long bitSize) => this.WriteMemory((void*) ptr, bitSize);

    public unsafe void WriteMemory(void* ptr, long bitSize)
    {
      int num = (int) (bitSize / 8L / 8L);
      ulong* numPtr1 = (ulong*) ptr;
      for (int index = 0; index < num; ++index)
        this.WriteUInt64(numPtr1[index]);
      int val1 = (int) (bitSize - (long) (num * 8 * 8));
      byte* numPtr2 = (byte*) (numPtr1 + num);
      while (val1 > 0)
      {
        int bitCount = Math.Min(val1, 8);
        this.WriteByte(*numPtr2, bitCount);
        val1 -= bitCount;
        ++numPtr2;
      }
    }

    public void WriteString(string str) => this.WritePrefixLengthString(str, 0, str.Length, Encoding.UTF8);

    public unsafe void WritePrefixLengthString(
      string str,
      int characterStart,
      int characterCount,
      Encoding encoding)
    {
      fixed (char* ptr = str)
        this.WritePrefixLengthString(characterStart, characterCount, encoding, ptr);
    }

    public unsafe void WritePrefixLengthString(
      char[] str,
      int characterStart,
      int characterCount,
      Encoding encoding)
    {
      fixed (char* ptr = str)
        this.WritePrefixLengthString(characterStart, characterCount, encoding, ptr);
    }

    private unsafe void WritePrefixLengthString(
      int characterStart,
      int characterCount,
      Encoding encoding,
      char* ptr)
    {
      char* chars = ptr + characterStart;
      this.WriteVariant((uint) encoding.GetByteCount(chars, characterCount));
      byte* bytes1 = stackalloc byte[256];
      int val1 = 256 / encoding.GetMaxByteCount(1);
      int charCount;
      for (; characterCount > 0; characterCount -= charCount)
      {
        charCount = Math.Min(val1, characterCount);
        int bytes2 = encoding.GetBytes(chars, charCount, bytes1, 256);
        this.WriteMemory((void*) bytes1, (long) (bytes2 * 8));
        chars += charCount;
      }
    }

    public unsafe void WriteBytes(byte[] bytes, int start, int count)
    {
      fixed (byte* numPtr = bytes)
        this.WriteMemory((void*) (numPtr + start), (long) (count * 8));
    }

    public void WritePrefixBytes(byte[] bytes, int start, int count)
    {
      this.WriteVariant((uint) count);
      this.WriteBytes(bytes, start, count);
    }
  }
}

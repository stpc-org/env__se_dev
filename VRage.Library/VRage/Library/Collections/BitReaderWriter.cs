// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.BitReaderWriter
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Library.Collections
{
  public struct BitReaderWriter
  {
    private IBitSerializable m_writeData;
    private BitStream m_readStream;
    private long m_readStreamPosition;
    public readonly bool IsReading;

    public BitReaderWriter(IBitSerializable writeData)
    {
      this.m_writeData = writeData;
      this.m_readStream = (BitStream) null;
      this.m_readStreamPosition = 0L;
      this.IsReading = false;
    }

    private BitReaderWriter(BitStream readStream, long readPos)
    {
      this.m_writeData = (IBitSerializable) null;
      this.m_readStream = readStream;
      this.m_readStreamPosition = readPos;
      this.IsReading = true;
    }

    public static BitReaderWriter ReadFrom(BitStream stream)
    {
      uint num = stream.ReadUInt32Variant();
      BitReaderWriter bitReaderWriter = new BitReaderWriter(stream, stream.BitPosition);
      stream.SetBitPositionRead(stream.BitPosition + (long) (int) num);
      return bitReaderWriter;
    }

    public void Write(BitStream stream)
    {
      if (stream != null && this.m_writeData != null)
      {
        long bitPosition = stream.BitPosition;
        this.m_writeData.Serialize(stream, false);
        long num = stream.BitPosition - bitPosition;
        stream.SetBitPositionWrite(bitPosition);
        stream.WriteVariant((uint) num);
        this.m_writeData.Serialize(stream, false);
      }
    }

    public bool ReadData(IBitSerializable readDataInto, bool validate, bool acceptAndSetValue = true)
    {
      if (this.m_readStream == null)
        return false;
      long bitPosition = this.m_readStream.BitPosition;
      this.m_readStream.SetBitPositionRead(this.m_readStreamPosition);
      try
      {
        return readDataInto.Serialize(this.m_readStream, validate, acceptAndSetValue);
      }
      finally
      {
        this.m_readStream.SetBitPositionRead(bitPosition);
      }
    }
  }
}

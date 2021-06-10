// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.VoiceChat.OpusEncoder
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using System;

namespace SpaceEngineers.Game.VoiceChat
{
  public sealed class OpusEncoder : OpusDevice
  {
    private OpusEncoder.FrameSizes Frames;

    public int Channels { get; }

    public int TargetBitRate { get; }

    public OpusEncoder.FormatT Format { get; }

    public int SamplesPerSecond { get; }

    public OpusDevice.Application Application { get; }

    public int MaxPacketSize { get; }

    public unsafe OpusEncoder(
      int samplesPerSecond,
      OpusEncoder.FormatT format,
      int? targetBitRate = null,
      int maxPacketSize = 1276,
      int channels = 1,
      OpusDevice.Application application = OpusDevice.Application.Voip)
    {
      if (samplesPerSecond <= 12000)
      {
        if (samplesPerSecond == 8000 || samplesPerSecond == 12000)
          goto label_4;
      }
      else if (samplesPerSecond == 16000 || samplesPerSecond == 24000 || samplesPerSecond == 48000)
        goto label_4;
      throw new Exception("Unsupported sampling frequency " + (object) samplesPerSecond);
label_4:
      this.Format = format;
      this.Channels = channels;
      this.Application = application;
      this.MaxPacketSize = maxPacketSize;
      this.SamplesPerSecond = samplesPerSecond;
      int error;
      this.Device = OpusDevice.Native.opus_encoder_create(samplesPerSecond, channels, (int) application, &error);
      this.CheckError(error, "opus_encoder_create");
      if (targetBitRate.HasValue)
      {
        this.TargetBitRate = targetBitRate.Value;
        this.CheckError(OpusDevice.Native.opus_encoder_ctl(this.Device, OpusDevice.Ctl.SET_BITRATE_REQUEST, this.TargetBitRate), "opus_encoder_ctl SET_BITRATE_REQUEST");
      }
      else
      {
        int num;
        this.CheckError(OpusDevice.Native.opus_encoder_ctl(this.Device, OpusDevice.Ctl.GET_BITRATE_REQUEST, out num), "opus_encoder_ctl GET_BITRATE_REQUEST");
        this.TargetBitRate = num;
      }
      this.Frames.Bytes[0] = (int) format * channels * (samplesPerSecond / 1000 * 60);
      this.Frames.Bytes[1] = (int) format * channels * (samplesPerSecond / 1000 * 40);
      this.Frames.Bytes[2] = (int) format * channels * (samplesPerSecond / 1000 * 20);
      this.Frames.Bytes[3] = (int) format * channels * (samplesPerSecond / 1000 * 10);
      this.Frames.Bytes[4] = (int) format * channels * (samplesPerSecond / 1000 * 5);
      this.Frames.Bytes[5] = (int) format * channels * (int) ((double) (samplesPerSecond / 1000) * 2.5);
    }

    public unsafe void Encode(
      Span<byte> data,
      out int consumedBytes,
      out byte[] packet,
      out int packetSize)
    {
      int num1 = 0;
      for (int index = 0; index < 6; ++index)
      {
        int num2 = this.Frames.Bytes[index];
        if (num2 <= data.Length)
        {
          num1 = num2;
          break;
        }
      }
      consumedBytes = num1;
      if (num1 > 0)
      {
        byte[] tempBuffer = this.GetTempBuffer(this.MaxPacketSize);
        fixed (byte* pcm = &data.GetPinnableReference())
          fixed (byte* dataOut = tempBuffer)
          {
            int frame_size = num1 / this.Channels / (int) this.Format;
            packetSize = this.Format != OpusEncoder.FormatT.SHORT ? OpusDevice.Native.opus_encode_float(this.Device, pcm, frame_size, dataOut, this.MaxPacketSize) : OpusDevice.Native.opus_encode(this.Device, pcm, frame_size, dataOut, this.MaxPacketSize);
            if (packetSize < 0)
              this.CheckError(packetSize, "opus_encode");
            if (packetSize >= 2)
            {
              packet = tempBuffer;
              return;
            }
            // ISSUE: __unpin statement
            __unpin(dataOut);
            // ISSUE: __unpin statement
            __unpin(pcm);
          }
      }
      packetSize = 0;
      packet = (byte[]) null;
    }

    protected override void DisposeDevice(IntPtr device) => OpusDevice.Native.opus_encoder_destroy(device);

    private struct FrameSizes
    {
      public const int Count = 6;
      public unsafe fixed int Bytes[6];
    }

    public enum FormatT : byte
    {
      SHORT = 2,
      FLOAT = 4,
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.VoiceChat.OpusDecoder
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using System;

namespace SpaceEngineers.Game.VoiceChat
{
  public sealed class OpusDecoder : OpusDevice
  {
    public int Channels { get; }

    public int SamplesPerSecond { get; }

    public unsafe OpusDecoder(int samplesPerSecond = 48000, int channels = 1)
    {
      this.Channels = 1;
      this.SamplesPerSecond = samplesPerSecond;
      int error;
      this.Device = OpusDevice.Native.opus_decoder_create(samplesPerSecond, channels, &error);
      this.CheckError(error, "opus_decoder_create");
    }

    public unsafe Span<byte> Decode(Span<byte> packet)
    {
      fixed (byte* numPtr = &packet.GetPinnableReference())
      {
        int nbSamples = OpusDevice.Native.opus_packet_get_nb_samples(numPtr, packet.Length, this.SamplesPerSecond);
        int minSize = 2 * nbSamples * this.Channels;
        byte[] tempBuffer;
        byte[] numArray = tempBuffer = this.GetTempBuffer(minSize);
        byte* pcmOut = tempBuffer == null || numArray.Length == 0 ? (byte*) null : &numArray[0];
        int error = OpusDevice.Native.opus_decode(this.Device, numPtr, packet.Length, pcmOut, nbSamples, 0);
        if (error != nbSamples)
          this.CheckError(error, "opus_decode");
        numArray = (byte[]) null;
        return tempBuffer.Span<byte>(0, new int?(minSize));
      }
    }

    protected override void DisposeDevice(IntPtr device) => OpusDevice.Native.opus_decoder_destroy(device);
  }
}

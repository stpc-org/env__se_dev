// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.VoiceChat.OpusDevice
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using System;
using System.Runtime.InteropServices;

namespace SpaceEngineers.Game.VoiceChat
{
  public abstract class OpusDevice : IDisposable
  {
    public IntPtr Device;
    private byte[] TempBuffer;

    protected OpusDevice() => this.Device = IntPtr.Zero;

    protected abstract void DisposeDevice(IntPtr device);

    protected void CheckError(int error, string message)
    {
      if (error != 0)
        throw new Exception(string.Format("Error #{0} {1}", (object) error, (object) message));
    }

    protected byte[] GetTempBuffer(int minSize)
    {
      byte[] tempBuffer = this.TempBuffer;
      int num = tempBuffer != null ? tempBuffer.Length : 0;
      if (num < minSize)
        Array.Resize<byte>(ref this.TempBuffer, Math.Max(Math.Max((int) ((double) num * 1.5), minSize), 1024));
      return this.TempBuffer;
    }

    public void Dispose()
    {
      if (!(this.Device != IntPtr.Zero))
        return;
      this.DisposeDevice(this.Device);
      this.Device = IntPtr.Zero;
    }

    protected class Native
    {
      private const string OPUS_DLL = "Opus.dll";
      private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;
      internal const int max_packet_size = 1276;

      [DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern unsafe IntPtr opus_encoder_create(
        int Fs,
        int channels,
        int application,
        int* error);

      [DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void opus_encoder_destroy(IntPtr encoder);

      [DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern unsafe int opus_encode(
        IntPtr st,
        byte* pcm,
        int frame_size,
        byte* dataOut,
        int max_data_bytes);

      [DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern unsafe int opus_encode_float(
        IntPtr st,
        byte* pcm,
        int frame_size,
        byte* dataOut,
        int out_data_bytes);

      [DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern unsafe IntPtr opus_decoder_create(
        int Fs,
        int channels,
        int* error);

      [DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void opus_decoder_destroy(IntPtr decoder);

      [DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern unsafe int opus_decode(
        IntPtr st,
        byte* data,
        int len,
        byte* pcmOut,
        int frame_size,
        int decode_fec);

      [DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int opus_encoder_ctl(IntPtr st, OpusDevice.Ctl request, int value);

      [DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern int opus_encoder_ctl(IntPtr st, OpusDevice.Ctl request, out int value);

      [DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
      internal static extern unsafe int opus_packet_get_nb_samples(
        byte* packet,
        int packetLen,
        int Fs);
    }

    public enum Ctl
    {
      SET_APPLICATION_REQUEST = 4000, // 0x00000FA0
      GET_APPLICATION_REQUEST = 4001, // 0x00000FA1
      SET_BITRATE_REQUEST = 4002, // 0x00000FA2
      GET_BITRATE_REQUEST = 4003, // 0x00000FA3
      SET_MAX_BANDWIDTH_REQUEST = 4004, // 0x00000FA4
      GET_MAX_BANDWIDTH_REQUEST = 4005, // 0x00000FA5
      SET_VBR_REQUEST = 4006, // 0x00000FA6
      GET_VBR_REQUEST = 4007, // 0x00000FA7
      SET_BANDWIDTH_REQUEST = 4008, // 0x00000FA8
      GET_BANDWIDTH_REQUEST = 4009, // 0x00000FA9
      SET_COMPLEXITY_REQUEST = 4010, // 0x00000FAA
      GET_COMPLEXITY_REQUEST = 4011, // 0x00000FAB
      SET_INBAND_FEC_REQUEST = 4012, // 0x00000FAC
      GET_INBAND_FEC_REQUEST = 4013, // 0x00000FAD
      SET_PACKET_LOSS_PERC_REQUEST = 4014, // 0x00000FAE
      GET_PACKET_LOSS_PERC_REQUEST = 4015, // 0x00000FAF
      SET_DTX_REQUEST = 4016, // 0x00000FB0
      GET_DTX_REQUEST = 4017, // 0x00000FB1
      SET_VBR_CONSTRAINT_REQUEST = 4020, // 0x00000FB4
      GET_VBR_CONSTRAINT_REQUEST = 4021, // 0x00000FB5
      SET_FORCE_CHANNELS_REQUEST = 4022, // 0x00000FB6
      GET_FORCE_CHANNELS_REQUEST = 4023, // 0x00000FB7
      SET_SIGNAL_REQUEST = 4024, // 0x00000FB8
      GET_SIGNAL_REQUEST = 4025, // 0x00000FB9
      GET_LOOKAHEAD_REQUEST = 4027, // 0x00000FBB
      GET_SAMPLE_RATE_REQUEST = 4029, // 0x00000FBD
      GET_FINAL_RANGE_REQUEST = 4031, // 0x00000FBF
      GET_PITCH_REQUEST = 4033, // 0x00000FC1
      SET_GAIN_REQUEST = 4034, // 0x00000FC2
      SET_LSB_DEPTH_REQUEST = 4036, // 0x00000FC4
      GET_LSB_DEPTH_REQUEST = 4037, // 0x00000FC5
      GET_LAST_PACKET_DURATION_REQUEST = 4039, // 0x00000FC7
      SET_EXPERT_FRAME_DURATION_REQUEST = 4040, // 0x00000FC8
      GET_EXPERT_FRAME_DURATION_REQUEST = 4041, // 0x00000FC9
      SET_PREDICTION_DISABLED_REQUEST = 4042, // 0x00000FCA
      GET_PREDICTION_DISABLED_REQUEST = 4043, // 0x00000FCB
      GET_GAIN_REQUEST = 4045, // 0x00000FCD
      SET_PHASE_INVERSION_DISABLED_REQUEST = 4046, // 0x00000FCE
      GET_PHASE_INVERSION_DISABLED_REQUEST = 4047, // 0x00000FCF
      GET_IN_DTX_REQUEST = 4049, // 0x00000FD1
    }

    public enum Application
    {
      Voip = 2048, // 0x00000800
      Audio = 2049, // 0x00000801
      Restricted_LowLatency = 2051, // 0x00000803
    }

    public enum Errors
    {
      AllocFail = -7, // 0xFFFFFFF9
      InvalidState = -6, // 0xFFFFFFFA
      Unimplemented = -5, // 0xFFFFFFFB
      InvalidPacket = -4, // 0xFFFFFFFC
      InternalError = -3, // 0xFFFFFFFD
      BufferToSmall = -2, // 0xFFFFFFFE
      BadArg = -1, // 0xFFFFFFFF
      OK = 0,
    }
  }
}

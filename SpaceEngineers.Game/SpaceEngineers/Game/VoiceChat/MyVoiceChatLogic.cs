// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.VoiceChat.MyVoiceChatLogic
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.GameSystems;
using Sandbox.Game.VoiceChat;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage.Data.Audio;
using VRage.Library.Utils;
using VRageMath;

namespace SpaceEngineers.Game.VoiceChat
{
  public class MyVoiceChatLogic : IMyVoiceChatLogic, IDisposable
  {
    private MyTimeSpan LastSentData = MyTimeSpan.Zero;
    private OpusEncoder m_encoder;
    private readonly Dictionary<long, OpusDecoder> m_decoders = new Dictionary<long, OpusDecoder>();

    public bool ShouldSendVoice(MyPlayer sender, MyPlayer receiver) => MyAntennaSystem.Static.CheckConnection(sender.Identity, receiver.Identity);

    public bool ShouldPlayVoice(
      MyPlayer player,
      MyTimeSpan timestamp,
      MyTimeSpan lastPlaybackSubmission,
      out MySoundDimensions dimension,
      out float maxDistance)
    {
      MyTimeSpan totalTime = MySandboxGame.Static.TotalTime;
      double chatPlaybackDelay = (double) MyFakes.VOICE_CHAT_PLAYBACK_DELAY;
      double num = Math.Max(500.0, chatPlaybackDelay * 3.0);
      if ((totalTime - timestamp).Milliseconds > chatPlaybackDelay || (totalTime - lastPlaybackSubmission).Milliseconds < num)
      {
        dimension = MyPlatformGameSettings.VOICE_CHAT_3D_SOUND ? MySoundDimensions.D3 : MySoundDimensions.D2;
        maxDistance = float.MaxValue;
        return true;
      }
      dimension = MySoundDimensions.D2;
      maxDistance = 0.0f;
      return false;
    }

    public bool ShouldSendData(
      byte[] data,
      int dataSize,
      Span<byte> format,
      out int bytesToRemember)
    {
      MyMultiplayerBase myMultiplayerBase = MyMultiplayer.Static;
      if ((myMultiplayerBase != null ? (!myMultiplayerBase.IsSomeoneElseConnected ? 1 : 0) : 1) != 0)
      {
        bytesToRemember = 0;
        return false;
      }
      if (!MyGameService.IsMicrophoneFilteringSilence())
        return this.ShouldSendDataLogic(data, dataSize, format, out bytesToRemember);
      bytesToRemember = 0;
      return true;
    }

    public bool ShouldSendDataLogic(
      byte[] data,
      int dataSize,
      Span<byte> format,
      out int bytesToRemember)
    {
      float num1 = MyVoiceChatLogic.ComputeRMS(data, dataSize, format, 0.75f, out bytesToRemember);
      if ((double) num1 < 0.0)
      {
        bool flag = true;
        for (int index = 0; index < dataSize; ++index)
          flag &= data[index] == (byte) 0;
        num1 = flag ? 0.0f : 1f;
      }
      MyTimeSpan totalTime = MySandboxGame.Static.TotalTime;
      double milliseconds = (totalTime - this.LastSentData).Milliseconds;
      float num2 = (float) (0.100000001490116 - 0.0500000007450581 * (((double) MyMath.Clamp(MyFakes.VOICE_CHAT_MIC_SENSITIVITY, 0.0f, 1f) - 0.5) * 2.0));
      bool flag1 = milliseconds < 1000.0;
      if ((double) num1 > (double) num2 || flag1 && (double) num1 > (double) num2 * 0.850000023841858)
      {
        this.LastSentData = totalTime;
        return true;
      }
      return flag1;
    }

    private static bool TryReadFormat(
      Span<byte> formatBytes,
      out MyVoiceChatLogic.WAVEFORMATEX format,
      out bool isIEEE_Float)
    {
      int index = Unsafe.SizeOf<MyVoiceChatLogic.WAVEFORMATEX>();
      if (formatBytes.Length >= index)
      {
        format = Unsafe.ReadUnaligned<MyVoiceChatLogic.WAVEFORMATEX>(ref formatBytes[0]);
        if (format.FormatTag == (ushort) 65534)
        {
          if (formatBytes.Length >= index + Unsafe.SizeOf<MyVoiceChatLogic.WAVEFORMATEXTENSIBLE>())
          {
            MyVoiceChatLogic.WAVEFORMATEXTENSIBLE waveformatextensible = Unsafe.ReadUnaligned<MyVoiceChatLogic.WAVEFORMATEXTENSIBLE>(ref formatBytes[index]);
            isIEEE_Float = waveformatextensible.SubFormat == new Guid(3, (short) 0, (short) 16, (byte) 128, (byte) 0, (byte) 0, (byte) 170, (byte) 0, (byte) 56, (byte) 155, (byte) 113);
          }
          else
            goto label_6;
        }
        else
          isIEEE_Float = false;
        return true;
      }
label_6:
      isIEEE_Float = false;
      format = new MyVoiceChatLogic.WAVEFORMATEX();
      return false;
    }

    private static float ComputeRMS(
      byte[] data,
      int dataSize,
      Span<byte> formatBytes,
      float secondsToSave,
      out int bytesToSave)
    {
      MyVoiceChatLogic.WAVEFORMATEX format;
      bool isIEEE_Float;
      if (MyVoiceChatLogic.TryReadFormat(formatBytes, out format, out isIEEE_Float) & isIEEE_Float && format.BitsPerSample == (ushort) 32)
      {
        float num1 = 0.0f;
        for (int index = 0; index < dataSize; index += 4)
        {
          float num2 = Unsafe.ReadUnaligned<float>(ref data[index]);
          num1 += num2 * num2;
        }
        int num3 = (int) ((double) secondsToSave * (double) format.SamplesPerSec);
        bytesToSave = 4 * num3;
        return (float) Math.Sqrt((double) num1 / (double) (dataSize / 4));
      }
      bytesToSave = 0;
      return -1f;
    }

    public void Compress(
      Span<byte> data,
      Span<byte> formatBytes,
      out int consumedBytes,
      out byte[] packet,
      out int packetSize)
    {
      MyVoiceChatLogic.WAVEFORMATEX format;
      bool isIEEE_Float;
      if (!MyVoiceChatLogic.TryReadFormat(formatBytes, out format, out isIEEE_Float) || !isIEEE_Float && format.BitsPerSample != (ushort) 16)
        throw new Exception(string.Format("Unsupported format {0};{1};{2};{3}", (object) format.FormatTag, (object) format.BitsPerSample, (object) format.Channels, (object) format.Size));
      if (this.m_encoder == null || isIEEE_Float != (this.m_encoder.Format == OpusEncoder.FormatT.FLOAT) || this.m_encoder.TargetBitRate != MyFakes.VOICE_CHAT_TARGET_BIT_RATE)
      {
        this.m_encoder?.Dispose();
        this.m_encoder = new OpusEncoder((int) format.SamplesPerSec, isIEEE_Float ? OpusEncoder.FormatT.FLOAT : OpusEncoder.FormatT.SHORT, MyFakes.VOICE_CHAT_TARGET_BIT_RATE == 0 ? new int?() : new int?(MyFakes.VOICE_CHAT_TARGET_BIT_RATE), 1000);
        MyFakes.VOICE_CHAT_TARGET_BIT_RATE = this.m_encoder.TargetBitRate;
      }
      this.m_encoder.Encode(data, out consumedBytes, out packet, out packetSize);
    }

    public Span<byte> Decompress(Span<byte> packet, long sender)
    {
      OpusDecoder opusDecoder;
      if (!this.m_decoders.TryGetValue(sender, out opusDecoder))
      {
        opusDecoder = new OpusDecoder(24000);
        this.m_decoders.Add(sender, opusDecoder);
      }
      return opusDecoder.Decode(packet);
    }

    public void Dispose()
    {
      if (this.m_encoder != null)
      {
        this.m_encoder.Dispose();
        this.m_encoder = (OpusEncoder) null;
      }
      foreach (KeyValuePair<long, OpusDecoder> decoder in this.m_decoders)
      {
        OpusDecoder v;
        decoder.Deconstruct<long, OpusDecoder>(out long _, out v);
        v.Dispose();
      }
      this.m_decoders.Clear();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private readonly struct WAVEFORMATEX
    {
      public readonly ushort FormatTag;
      public readonly ushort Channels;
      public readonly uint SamplesPerSec;
      public readonly uint AvgBytesPerSec;
      public readonly ushort BlockAlign;
      public readonly ushort BitsPerSample;
      public readonly ushort Size;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct WAVEFORMATEXTENSIBLE
    {
      public ushort ValidBitsPerSample;
      public uint ChannelMask;
      public Guid SubFormat;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.VoiceChat.IMyVoiceChatLogic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using System;
using VRage.Data.Audio;
using VRage.Library.Utils;

namespace Sandbox.Game.VoiceChat
{
  public interface IMyVoiceChatLogic : IDisposable
  {
    bool ShouldSendData(byte[] data, int dataSize, Span<byte> format, out int bytesToRemember);

    bool ShouldSendVoice(MyPlayer sender, MyPlayer receiver);

    bool ShouldPlayVoice(
      MyPlayer player,
      MyTimeSpan timestamp,
      MyTimeSpan lastPlaybackSubmission,
      out MySoundDimensions dimension,
      out float maxDistance);

    void Compress(
      Span<byte> data,
      Span<byte> formatBytes,
      out int consumedBytes,
      out byte[] packet,
      out int packetSize);

    Span<byte> Decompress(Span<byte> packet, long sender);
  }
}

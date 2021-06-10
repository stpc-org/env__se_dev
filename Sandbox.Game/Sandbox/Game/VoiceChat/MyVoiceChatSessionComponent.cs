// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.VoiceChat.MyVoiceChatSessionComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Data.Audio;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.GameServices;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.VoiceChat
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  [StaticEventOwner]
  public class MyVoiceChatSessionComponent : MySessionComponentBase
  {
    private bool m_recording;
    private Dictionary<ulong, MyEntity3DSoundEmitter> m_voices;
    private Dictionary<ulong, MyVoiceChatSessionComponent.ReceivedData> m_receivedVoiceData;
    private int m_frameCount;
    private List<ulong> m_keys;
    private IMyVoiceChatLogic m_voiceChatLogic;
    private bool m_enabled;
    private const uint UNCOMPRESSED_SIZE = 22528;
    private Dictionary<ulong, bool> m_debugSentVoice = new Dictionary<ulong, bool>();
    private Dictionary<ulong, MyTuple<int, TimeSpan>> m_debugReceivedVoice = new Dictionary<ulong, MyTuple<int, TimeSpan>>();
    private static MyVoiceChatSessionComponent.SendBuffer Recievebuffer = new MyVoiceChatSessionComponent.SendBuffer();
    private Task m_dataRecordingTask;
    private MyVoiceResult m_lastRecordingResult = MyVoiceResult.NoData;
    private byte[] m_voiceDataBufferCache;
    private Action m_recordVoiceDataFunc;
    private Queue<byte> m_rememberedVoiceBytes = new Queue<byte>(22528);
    private int m_inactiveRecordingFrames;
    private static List<byte[]> RecievedBuffers = new List<byte[]>();

    public event Action<ulong, bool> OnPlayerMutedStateChanged;

    public static MyVoiceChatSessionComponent Static { get; private set; }

    public bool IsRecording => this.m_recording;

    public override void LoadData()
    {
      base.LoadData();
      MyVoiceChatSessionComponent.Static = this;
      MyGameService.InitializeVoiceRecording();
      this.m_voiceChatLogic = Activator.CreateInstance(MyPerGameSettings.VoiceChatLogic) as IMyVoiceChatLogic;
      this.m_recording = false;
      this.m_voiceDataBufferCache = new byte[22528];
      this.m_voices = new Dictionary<ulong, MyEntity3DSoundEmitter>();
      this.m_receivedVoiceData = new Dictionary<ulong, MyVoiceChatSessionComponent.ReceivedData>();
      this.m_keys = new List<ulong>();
      Sync.Players.PlayerRemoved += new Action<MyPlayer.PlayerId>(this.Players_PlayerRemoved);
      Sync.Players.PlayersChanged += new Action<bool, MyPlayer.PlayerId>(this.OnOnlinePlayersChanged);
      this.m_enabled = MyAudio.Static.EnableVoiceChat;
      MyAudio.Static.VoiceChatEnabled += new Action<bool>(this.MyAudio_VoiceChatEnabled);
      MyHud.VoiceChat.VisibilityChanged += new Action<bool>(this.VoiceChat_VisibilityChanged);
      MyGameService.OnOverlayActivated += new Action<bool>(this.OnOverlayActivated);
      ICollection<MyPlayer> onlinePlayers = Sync.Players.GetOnlinePlayers();
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
      {
        ulong steamId = myPlayer.Id.SteamId;
        if (MySandboxGame.Config.MutedPlayers.Contains(steamId))
          MyGameService.SetPlayerMuted(steamId, true);
      }
      if (onlinePlayers.Count <= 0)
        return;
      MyGameService.UpdateMutedPlayersFromCloud();
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      if (this.m_recording)
        this.StopRecording();
      foreach (ulong playerId in this.m_voices.Keys.ToArray<ulong>())
        this.DisposePlayerEmitter(playerId);
      this.m_voiceChatLogic?.Dispose();
      this.m_voiceChatLogic = (IMyVoiceChatLogic) null;
      MyGameService.DisposeVoiceRecording();
      MyVoiceChatSessionComponent.Static = (MyVoiceChatSessionComponent) null;
      this.m_receivedVoiceData = (Dictionary<ulong, MyVoiceChatSessionComponent.ReceivedData>) null;
      this.m_voices = (Dictionary<ulong, MyEntity3DSoundEmitter>) null;
      this.m_keys = (List<ulong>) null;
      Sync.Players.PlayerRemoved -= new Action<MyPlayer.PlayerId>(this.Players_PlayerRemoved);
      Sync.Players.PlayersChanged -= new Action<bool, MyPlayer.PlayerId>(this.OnOnlinePlayersChanged);
      MyAudio.Static.VoiceChatEnabled -= new Action<bool>(this.MyAudio_VoiceChatEnabled);
      MyHud.VoiceChat.VisibilityChanged -= new Action<bool>(this.VoiceChat_VisibilityChanged);
      MyGameService.OnOverlayActivated -= new Action<bool>(this.OnOverlayActivated);
    }

    private void OnOverlayActivated(bool enabled)
    {
      if (enabled)
        return;
      MyGameService.UpdateMutedPlayersFromCloud((Action) (() =>
      {
        if (!this.Loaded || MySession.Static == null)
          return;
        foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
        {
          if (!onlinePlayer.IsLocalPlayer)
          {
            ulong steamId = onlinePlayer.Id.SteamId;
            bool muted = MyGameService.IsPlayerMutedInCloud(steamId);
            this.SetPlayerMuted(steamId, muted);
          }
        }
      }));
    }

    public override bool IsRequiredByGame => MyPerGameSettings.VoiceChatEnabled;

    private void OnOnlinePlayersChanged(bool connected, MyPlayer.PlayerId player)
    {
      ulong steamId = player.SteamId;
      if (!connected || !MySandboxGame.Config.MutedPlayers.Contains(steamId))
        return;
      MyGameService.SetPlayerMuted(steamId, true);
    }

    private void Players_PlayerRemoved(MyPlayer.PlayerId pid)
    {
      if (pid.SerialId != 0)
        return;
      ulong steamId = pid.SteamId;
      if (this.m_receivedVoiceData.ContainsKey(steamId))
        this.m_receivedVoiceData.Remove(steamId);
      this.DisposePlayerEmitter(steamId);
    }

    private void DisposePlayerEmitter(ulong playerId)
    {
      MyEntity3DSoundEmitter entity3DsoundEmitter;
      if (!this.m_voices.TryGetValue(playerId, out entity3DsoundEmitter))
        return;
      entity3DsoundEmitter.StopSound(true, cleanupSound: true);
      this.m_voices.Remove(playerId);
    }

    private void MyAudio_VoiceChatEnabled(bool isEnabled)
    {
      this.m_enabled = isEnabled;
      if (this.m_enabled)
      {
        if (!MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION || this.m_recording)
          return;
        this.StartRecording();
      }
      else
      {
        if (this.m_recording)
        {
          this.m_recording = false;
          this.StopRecording();
        }
        foreach (ulong playerId in this.m_voices.Keys.ToArray<ulong>())
          this.DisposePlayerEmitter(playerId);
        this.m_voices.Clear();
        this.m_receivedVoiceData.Clear();
      }
    }

    private void VoiceChat_VisibilityChanged(bool isVisible)
    {
      if (MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION || this.m_recording == isVisible)
        return;
      if (this.m_recording)
      {
        this.m_recording = false;
        this.StopRecording();
      }
      else
        this.StartRecording();
    }

    public void StartRecording()
    {
      if (!this.m_enabled)
        return;
      this.m_recording = true;
      MyGameService.StartVoiceRecording();
      if (MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION)
        return;
      MyHud.VoiceChat.Show();
    }

    public void StopRecording()
    {
      if (!this.m_enabled)
        return;
      MyGameService.StopVoiceRecording();
      MyHud.VoiceChat.Hide();
      this.DummyUpdate();
    }

    public void ClearDebugData() => this.m_debugSentVoice.Clear();

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (!this.m_enabled || !MyVoiceChatSessionComponent.IsCharacterValid(MySession.Static.LocalCharacter))
        return;
      if (this.m_recording)
        this.UpdateRecording();
      else if (MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION)
        this.StartRecording();
      this.UpdatePlayback();
    }

    private static bool IsCharacterValid(MyCharacter character) => character != null && !character.IsDead && !character.MarkedForClose;

    private void VoiceMessageReceived(MyVoiceChatSessionComponent.SendBuffer data)
    {
      if (!this.m_enabled)
        return;
      MyMultiplayerBase myMultiplayerBase = MyMultiplayer.Static;
      if ((myMultiplayerBase != null ? (!myMultiplayerBase.IsVoiceChatAvailable ? 1 : 0) : 1) != 0 || !MyVoiceChatSessionComponent.IsCharacterValid(MySession.Static.LocalCharacter))
        return;
      ulong senderUserId = (ulong) data.SenderUserId;
      if (!MyGameService.Networking.Chat.IsVoiceChatAvailableForUserId(senderUserId, MyMultiplayer.Static.IsCrossMember(senderUserId)) || MyGameService.GetPlayerMutedState(senderUserId) == MyPlayerChatState.Muted)
        return;
      this.ProcessBuffer(data.VoiceDataBuffer.Span<byte>(0, new int?(data.NumElements)), senderUserId);
    }

    private void PlayVoice(
      byte[] uncompressedBuffer,
      ulong playerId,
      MySoundDimensions dimension,
      float maxDistance)
    {
      if (uncompressedBuffer.Length == 0)
        return;
      MyEntity3DSoundEmitter entity3DsoundEmitter;
      if (this.m_voices.TryGetValue(playerId, out entity3DsoundEmitter) && entity3DsoundEmitter.Entity is MyCharacter entity && !MyVoiceChatSessionComponent.IsCharacterValid(entity))
        this.DisposePlayerEmitter(playerId);
      if (!this.m_voices.TryGetValue(playerId, out entity3DsoundEmitter))
      {
        MyCharacter character = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(playerId)).Character;
        if (!MyVoiceChatSessionComponent.IsCharacterValid(character))
          return;
        entity3DsoundEmitter = new MyEntity3DSoundEmitter((MyEntity) character);
        this.m_voices[playerId] = entity3DsoundEmitter;
      }
      this.m_voices[playerId].PlaySound(uncompressedBuffer, MyAudio.Static.VolumeVoiceChat, maxDistance, dimension);
    }

    private void ProcessBuffer(Span<byte> compressedData, ulong sender)
    {
      MyVoiceChatSessionComponent.ReceivedData receivedData;
      if (!this.m_receivedVoiceData.TryGetValue(sender, out receivedData))
        receivedData = new MyVoiceChatSessionComponent.ReceivedData()
        {
          UncompressedBuffer = new MyList<byte>(),
          ReceivedDataTimestamp = MyTimeSpan.Zero,
          SpeakerTimestamp = MyTimeSpan.Zero
        };
      try
      {
        Span<byte> data = this.m_voiceChatLogic.Decompress(compressedData, (long) sender);
        receivedData.UncompressedBuffer.Insert(data);
      }
      catch
      {
      }
      MyTimeSpan totalTime = MySandboxGame.Static.TotalTime;
      if (receivedData.ReceivedDataTimestamp == MyTimeSpan.Zero)
        receivedData.ReceivedDataTimestamp = totalTime;
      receivedData.SpeakerTimestamp = totalTime;
      this.m_receivedVoiceData[sender] = receivedData;
    }

    private void UpdatePlayback()
    {
      if (this.m_voiceChatLogic == null)
        return;
      MyTimeSpan totalTime = MySandboxGame.Static.TotalTime;
      float num = 1000f;
      this.m_keys.AddRange((IEnumerable<ulong>) this.m_receivedVoiceData.Keys);
      foreach (ulong key in this.m_keys)
      {
        bool flag = false;
        MyVoiceChatSessionComponent.ReceivedData receivedData = this.m_receivedVoiceData[key];
        if (receivedData.ReceivedDataTimestamp != MyTimeSpan.Zero)
        {
          MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(key));
          MySoundDimensions dimension;
          float maxDistance;
          if (playerById != null && this.m_voiceChatLogic.ShouldPlayVoice(playerById, receivedData.ReceivedDataTimestamp, receivedData.LastPlaybackSubmissionTimestamp, out dimension, out maxDistance) && MyGameService.GetPlayerMutedState(playerById.Id.SteamId) != MyPlayerChatState.Muted)
          {
            this.PlayVoice(receivedData.GetDataForPlayback(), key, dimension, maxDistance);
            flag = true;
          }
        }
        if (receivedData.SpeakerTimestamp != MyTimeSpan.Zero && (totalTime - receivedData.SpeakerTimestamp).Milliseconds > (double) num)
        {
          receivedData.ClearSpeakerTimestamp();
          flag = true;
        }
        if (flag)
          this.m_receivedVoiceData[key] = receivedData;
      }
      this.m_keys.Clear();
    }

    private void RecordVoiceDataParallel()
    {
      uint size1;
      MyVoiceResult myVoiceResult = MyGameService.GetAvailableVoice(ref this.m_voiceDataBufferCache, out size1);
      int size = (int) size1;
      byte[] numArray = (byte[]) null;
      if (myVoiceResult == MyVoiceResult.OK)
      {
        numArray = MyGameService.GetVoiceDataFormat();
        if (MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION)
        {
          int bytesToRemember;
          if (this.m_voiceChatLogic.ShouldSendData(this.m_voiceDataBufferCache, size, (Span<byte>) numArray, out bytesToRemember))
          {
            TrimOldData(bytesToRemember);
          }
          else
          {
            myVoiceResult = MyVoiceResult.NoData;
            TrimOldData(bytesToRemember);
            SaveOldData(0);
          }
        }
      }
      if (myVoiceResult == MyVoiceResult.OK)
      {
        ConsumeOldData();
        MyVoiceChatSessionComponent.SendBuffer receiveBuffer = new MyVoiceChatSessionComponent.SendBuffer()
        {
          SenderUserId = (long) MySession.Static.LocalHumanPlayer.Id.SteamId
        };
        int num = 0;
        bool isServer = Sync.IsServer;
        while (true)
        {
          byte[] packet;
          int packetSize;
          do
          {
            int consumedBytes;
            this.m_voiceChatLogic.Compress(this.m_voiceDataBufferCache.Span<byte>(num, new int?(size - num)), (Span<byte>) numArray, out consumedBytes, out packet, out packetSize);
            if (consumedBytes != 0)
            {
              num += consumedBytes;
              receiveBuffer.NumElements = packetSize;
              receiveBuffer.VoiceDataBuffer = packet;
            }
            else
              goto label_14;
          }
          while (packetSize <= 0);
          if (MyFakes.VOICE_CHAT_ECHO)
            MyVoiceChatSessionComponent.RecievedBuffers.Add(packet.Span<byte>(0, new int?(packetSize)).ToArray());
          if (isServer)
            MyVoiceChatSessionComponent.SendVoice(receiveBuffer);
          else
            MyMultiplayer.RaiseStaticEvent<BitReaderWriter>((Func<IMyEventOwner, Action<BitReaderWriter>>) (x => new Action<BitReaderWriter>(MyVoiceChatSessionComponent.SendVoice)), (BitReaderWriter) receiveBuffer);
        }
label_14:
        SaveOldData(num);
      }
      this.m_lastRecordingResult = myVoiceResult;

      void SaveOldData(int i)
      {
        byte[] voiceDataBufferCache = this.m_voiceDataBufferCache;
        for (; i < size; ++i)
          this.m_rememberedVoiceBytes.Enqueue(voiceDataBufferCache[i]);
      }

      void TrimOldData(int maxBytesToRemember)
      {
        int num1 = Math.Max(0, maxBytesToRemember - size);
        while (this.m_rememberedVoiceBytes.Count > num1)
        {
          int num2 = (int) this.m_rememberedVoiceBytes.Dequeue();
        }
      }

      void ConsumeOldData()
      {
        int count = this.m_rememberedVoiceBytes.Count;
        if (count <= 0)
          return;
        int size = size + count;
        ArrayExtensions.EnsureCapacity<byte>(ref this.m_voiceDataBufferCache, size);
        Array.Copy((Array) this.m_voiceDataBufferCache, 0, (Array) this.m_voiceDataBufferCache, count, size);
        this.m_rememberedVoiceBytes.CopyTo(this.m_voiceDataBufferCache, 0);
        this.m_rememberedVoiceBytes.Clear();
        size = size;
      }
    }

    private void UpdateRecording()
    {
      if (Sync.IsDedicated)
        return;
      if (this.m_recordVoiceDataFunc == null)
        this.m_recordVoiceDataFunc = new Action(this.RecordVoiceDataParallel);
      else
        this.m_dataRecordingTask.WaitOrExecute();
      this.m_dataRecordingTask = Parallel.Start(this.m_recordVoiceDataFunc);
      MyVoiceResult lastRecordingResult = this.m_lastRecordingResult;
      if (lastRecordingResult == MyVoiceResult.OK)
      {
        if (!MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION)
          return;
        this.m_inactiveRecordingFrames = 0;
        if (MyHud.VoiceChat.Visible)
          return;
        MyHud.VoiceChat.Show();
      }
      else
      {
        if (MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION && MyHud.VoiceChat.Visible)
        {
          ++this.m_inactiveRecordingFrames;
          if ((double) this.m_inactiveRecordingFrames > 30.0)
            MyHud.VoiceChat.Hide();
        }
        if (lastRecordingResult == MyVoiceResult.NotRecording)
          this.m_recording = false;
      }
    }

    public void SetPlayerMuted(ulong playerId, bool muted)
    {
      HashSet<ulong> mutedPlayers = MySandboxGame.Config.MutedPlayers;
      if (!(!muted ? mutedPlayers.Remove(playerId) : mutedPlayers.Add(playerId)))
        return;
      MySandboxGame.Config.MutedPlayers = mutedPlayers;
      MySandboxGame.Config.Save();
      MyGameService.SetPlayerMuted(playerId, muted);
      this.OnPlayerMutedStateChanged.InvokeIfNotNull<ulong, bool>(playerId, muted);
    }

    [Event(null, 712)]
    [Server]
    private static void SendVoice(BitReaderWriter data)
    {
      if (MyVoiceChatSessionComponent.Static == null || !data.ReadData((IBitSerializable) MyVoiceChatSessionComponent.Recievebuffer, false))
        return;
      MyVoiceChatSessionComponent.SendVoice(MyVoiceChatSessionComponent.Recievebuffer);
    }

    private static void SendVoice(
      MyVoiceChatSessionComponent.SendBuffer receiveBuffer)
    {
      ulong senderUserId = (ulong) receiveBuffer.SenderUserId;
      MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(senderUserId));
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if (onlinePlayer.Id.SerialId == 0 && (long) onlinePlayer.Id.SteamId != (long) senderUserId && (MyVoiceChatSessionComponent.IsCharacterValid(onlinePlayer.Character) && MyVoiceChatSessionComponent.Static.m_voiceChatLogic.ShouldSendVoice(playerById, onlinePlayer)))
        {
          MyMultiplayer.RaiseStaticEvent<ulong, BitReaderWriter>((Func<IMyEventOwner, Action<ulong, BitReaderWriter>>) (x => new Action<ulong, BitReaderWriter>(MyVoiceChatSessionComponent.SendVoicePlayer)), onlinePlayer.Id.SteamId, (BitReaderWriter) receiveBuffer, new EndpointId(onlinePlayer.Id.SteamId));
          if (MyFakes.ENABLE_VOICE_CHAT_DEBUGGING)
            MyVoiceChatSessionComponent.Static.m_debugSentVoice[onlinePlayer.Id.SteamId] = true;
        }
        else if (MyFakes.ENABLE_VOICE_CHAT_DEBUGGING)
          MyVoiceChatSessionComponent.Static.m_debugSentVoice[onlinePlayer.Id.SteamId] = false;
      }
    }

    private void DummyUpdate()
    {
      foreach (byte[] recievedBuffer in MyVoiceChatSessionComponent.RecievedBuffers)
        this.ProcessBuffer((Span<byte>) recievedBuffer, MySession.Static.LocalHumanPlayer.Id.SteamId);
    }

    [Event(null, 758)]
    [Client]
    private static void SendVoicePlayer(ulong user, BitReaderWriter data)
    {
      data.ReadData((IBitSerializable) MyVoiceChatSessionComponent.Recievebuffer, false);
      MyVoiceChatSessionComponent.Static.VoiceMessageReceived(MyVoiceChatSessionComponent.Recievebuffer);
    }

    public override void Draw()
    {
      base.Draw();
      if (this.m_receivedVoiceData == null)
        return;
      if (MyDebugDrawSettings.DEBUG_DRAW_VOICE_CHAT && MyFakes.ENABLE_VOICE_CHAT_DEBUGGING)
        this.DebugDraw();
      BoundingSphereD boundingSphereD = new BoundingSphereD(MySector.MainCamera.Position, 500.0);
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if (onlinePlayer.Character != null && !onlinePlayer.IsLocalPlayer)
        {
          MyPositionComponentBase positionComp = onlinePlayer.Character.PositionComp;
          MatrixD matrixD = positionComp.WorldMatrixRef;
          if (boundingSphereD.Contains(matrixD.Translation) != ContainmentType.Disjoint)
          {
            ulong steamId = onlinePlayer.Id.SteamId;
            switch (MyGameService.GetPlayerMutedState(steamId))
            {
              case MyPlayerChatState.Talking:
                Vector3D position = matrixD.Translation + (double) positionComp.LocalAABB.Height * matrixD.Up + matrixD.Up * 0.200000002980232;
                MyGuiPaddedTexture textureVoiceChat = MyGuiConstants.TEXTURE_VOICE_CHAT;
                MatrixD matrix = MySector.MainCamera.ViewMatrix * MySector.MainCamera.ProjectionMatrix;
                Vector3D vector3D = Vector3D.Transform(position, matrix);
                if (vector3D.Z < 1.0)
                {
                  Vector2 hudPos = new Vector2((float) vector3D.X, (float) vector3D.Y);
                  hudPos = hudPos * 0.5f + 0.5f * Vector2.One;
                  hudPos.Y = 1f - hudPos.Y;
                  MyGuiManager.DrawSpriteBatch(textureVoiceChat.Texture, MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos), textureVoiceChat.SizeGui * 0.5f, Color.White, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
                  continue;
                }
                continue;
              case MyPlayerChatState.Muted:
                continue;
              default:
                MyVoiceChatSessionComponent.ReceivedData receivedData;
                if (!this.m_receivedVoiceData.TryGetValue(steamId, out receivedData) || !(receivedData.SpeakerTimestamp != MyTimeSpan.Zero))
                  continue;
                goto case MyPlayerChatState.Talking;
            }
          }
        }
      }
    }

    private void DebugDraw()
    {
      Vector2 screenCoord = new Vector2(300f, 100f);
      MyRenderProxy.DebugDrawText2D(screenCoord, "Sent voice to:", Color.White, 1f);
      screenCoord.Y += 30f;
      foreach (KeyValuePair<ulong, bool> keyValuePair in this.m_debugSentVoice)
      {
        string text = string.Format("id: {0} => {1}", (object) keyValuePair.Key, keyValuePair.Value ? (object) "SENT" : (object) "NOT");
        MyRenderProxy.DebugDrawText2D(screenCoord, text, Color.White, 1f);
        screenCoord.Y += 30f;
      }
      MyRenderProxy.DebugDrawText2D(screenCoord, "Received voice from:", Color.White, 1f);
      screenCoord.Y += 30f;
      foreach (KeyValuePair<ulong, MyTuple<int, TimeSpan>> keyValuePair in this.m_debugReceivedVoice)
      {
        string text = string.Format("id: {0} => size: {1} (timestamp {2})", (object) keyValuePair.Key, (object) keyValuePair.Value.Item1, (object) keyValuePair.Value.Item2.ToString());
        MyRenderProxy.DebugDrawText2D(screenCoord, text, Color.White, 1f);
        screenCoord.Y += 30f;
      }
      MyRenderProxy.DebugDrawText2D(screenCoord, "Uncompressed buffers:", Color.White, 1f);
      screenCoord.Y += 30f;
      foreach (KeyValuePair<ulong, MyVoiceChatSessionComponent.ReceivedData> keyValuePair in this.m_receivedVoiceData)
      {
        string text = string.Format("id: {0} => size: {1}", (object) keyValuePair.Key, (object) keyValuePair.Value.UncompressedBuffer.Count);
        MyRenderProxy.DebugDrawText2D(screenCoord, text, Color.White, 1f);
        screenCoord.Y += 30f;
      }
    }

    private class SendBuffer : IBitSerializable
    {
      public byte[] VoiceDataBuffer;
      public int NumElements;
      public long SenderUserId;

      public bool Serialize(BitStream stream, bool validate, bool acceptAndSetValue = true)
      {
        if (stream.Reading)
        {
          this.SenderUserId = stream.ReadInt64();
          this.NumElements = stream.ReadInt32();
          ArrayExtensions.EnsureCapacity<byte>(ref this.VoiceDataBuffer, this.NumElements);
          stream.ReadBytes(this.VoiceDataBuffer, 0, this.NumElements);
        }
        else
        {
          stream.WriteInt64(this.SenderUserId);
          stream.WriteInt32(this.NumElements);
          stream.WriteBytes(this.VoiceDataBuffer, 0, this.NumElements);
        }
        return true;
      }

      public static implicit operator BitReaderWriter(
        MyVoiceChatSessionComponent.SendBuffer buffer)
      {
        return new BitReaderWriter((IBitSerializable) buffer);
      }
    }

    private struct ReceivedData
    {
      public MyList<byte> UncompressedBuffer;
      public MyTimeSpan ReceivedDataTimestamp;
      public MyTimeSpan SpeakerTimestamp;
      public MyTimeSpan LastPlaybackSubmissionTimestamp;

      public byte[] GetDataForPlayback()
      {
        byte[] array = this.UncompressedBuffer.ToArray();
        this.UncompressedBuffer.Clear();
        this.ReceivedDataTimestamp = MyTimeSpan.Zero;
        this.LastPlaybackSubmissionTimestamp = MySandboxGame.Static.TotalTime;
        return array;
      }

      public void ClearSpeakerTimestamp() => this.SpeakerTimestamp = MyTimeSpan.Zero;
    }

    protected sealed class SendVoice\u003C\u003EVRage_Library_Collections_BitReaderWriter : ICallSite<IMyEventOwner, BitReaderWriter, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in BitReaderWriter data,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVoiceChatSessionComponent.SendVoice(data);
      }
    }

    protected sealed class SendVoicePlayer\u003C\u003ESystem_UInt64\u0023VRage_Library_Collections_BitReaderWriter : ICallSite<IMyEventOwner, ulong, BitReaderWriter, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong user,
        in BitReaderWriter data,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyVoiceChatSessionComponent.SendVoicePlayer(user, data);
      }
    }
  }
}

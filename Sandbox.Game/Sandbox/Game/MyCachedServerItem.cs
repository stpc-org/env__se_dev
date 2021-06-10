// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyCachedServerItem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game
{
  public class MyCachedServerItem
  {
    public readonly bool AllowedInGroup;
    public readonly MyGameServerItem Server;
    public Dictionary<string, string> Rules;
    private MyCachedServerItem.MyServerData m_data = new MyCachedServerItem.MyServerData();
    private const int RULE_LENGTH = 93;

    public MyObjectBuilder_SessionSettings Settings => this.m_data.Settings;

    public bool ExperimentalMode => this.m_data.ExperimentalMode;

    public string Description => this.m_data.Description;

    public List<WorkshopId> Mods => this.m_data.Mods;

    public MyCachedServerItem()
    {
    }

    public MyCachedServerItem(MyGameServerItem server)
    {
      this.Server = server;
      this.Rules = (Dictionary<string, string>) null;
      ulong tagByPrefixUlong = server.GetGameTagByPrefixUlong("groupId");
      this.AllowedInGroup = tagByPrefixUlong == 0UL || MyGameService.IsUserInGroup(tagByPrefixUlong);
    }

    public static void SendSettingsToSteam()
    {
      if (!Sandbox.Engine.Platform.Game.IsDedicated || MyGameService.GameServer == null)
        return;
      byte[] numArray;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        SerializableDictionary<string, short> blockTypeLimits = MySession.Static.Settings.BlockTypeLimits;
        MyCachedServerItem.MyServerData instance = new MyCachedServerItem.MyServerData()
        {
          Settings = MySession.Static.Settings,
          ExperimentalMode = MySession.Static.IsSettingsExperimental(),
          Mods = MySession.Static.Mods.Where<MyObjectBuilder_Checkpoint.ModItem>((Func<MyObjectBuilder_Checkpoint.ModItem, bool>) (m => !m.IsDependency)).Select<MyObjectBuilder_Checkpoint.ModItem, WorkshopId>((Func<MyObjectBuilder_Checkpoint.ModItem, WorkshopId>) (m => new WorkshopId(m.PublishedFileId, m.PublishedServiceName))).Distinct<WorkshopId>().ToList<WorkshopId>(),
          Description = MySandboxGame.ConfigDedicated == null ? (string) null : MySandboxGame.ConfigDedicated.ServerDescription
        };
        instance.Settings.BlockTypeLimits = new SerializableDictionary<string, short>();
        Serializer.Serialize<MyCachedServerItem.MyServerData>((Stream) memoryStream, instance);
        numArray = MyCompression.Compress(memoryStream.ToArray());
        instance.Settings.BlockTypeLimits = blockTypeLimits;
      }
      MyGameService.GameServer.SetKeyValue("sc", numArray.Length.ToString());
      for (int index = 0; (double) index < Math.Ceiling((double) numArray.Length / 93.0); ++index)
      {
        byte[] inArray = new byte[93];
        int length = numArray.Length - 93 * index;
        if (length >= 93)
        {
          Array.Copy((Array) numArray, index * 93, (Array) inArray, 0, 93);
        }
        else
        {
          inArray = new byte[length];
          Array.Copy((Array) numArray, index * 93, (Array) inArray, 0, length);
        }
        MyGameService.GameServer.SetKeyValue("sc" + (object) index, Convert.ToBase64String(inArray));
      }
    }

    public void DeserializeSettings()
    {
      string str = (string) null;
      try
      {
        if (!this.Rules.TryGetValue("sc", out str))
          return;
        int length = int.Parse(str);
        byte[] gzBuffer = new byte[length];
        for (int index = 0; (double) index < Math.Ceiling((double) length / 93.0); ++index)
        {
          byte[] numArray = Convert.FromBase64String(this.Rules["sc" + (object) index]);
          Array.Copy((Array) numArray, 0, (Array) gzBuffer, index * 93, numArray.Length);
        }
        using (MemoryStream memoryStream = new MemoryStream(MyCompression.Decompress(gzBuffer)))
          this.m_data = Serializer.Deserialize<MyCachedServerItem.MyServerData>((Stream) memoryStream);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLineAndConsole("Failed to deserialize session settings for server!");
        MyLog.Default.WriteLineAndConsole(str);
        MyLog.Default.WriteLineAndConsole(ex.ToString());
      }
    }

    [ProtoContract]
    public class MyServerData
    {
      [ProtoMember(1)]
      public MyObjectBuilder_SessionSettings Settings;
      [ProtoMember(4)]
      public bool ExperimentalMode;
      [ProtoMember(7)]
      public List<WorkshopId> Mods = new List<WorkshopId>();
      [ProtoMember(10)]
      public string Description;

      protected class Sandbox_Game_MyCachedServerItem\u003C\u003EMyServerData\u003C\u003ESettings\u003C\u003EAccessor : IMemberAccessor<MyCachedServerItem.MyServerData, MyObjectBuilder_SessionSettings>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyCachedServerItem.MyServerData owner,
          in MyObjectBuilder_SessionSettings value)
        {
          owner.Settings = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCachedServerItem.MyServerData owner,
          out MyObjectBuilder_SessionSettings value)
        {
          value = owner.Settings;
        }
      }

      protected class Sandbox_Game_MyCachedServerItem\u003C\u003EMyServerData\u003C\u003EExperimentalMode\u003C\u003EAccessor : IMemberAccessor<MyCachedServerItem.MyServerData, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCachedServerItem.MyServerData owner, in bool value) => owner.ExperimentalMode = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCachedServerItem.MyServerData owner, out bool value) => value = owner.ExperimentalMode;
      }

      protected class Sandbox_Game_MyCachedServerItem\u003C\u003EMyServerData\u003C\u003EMods\u003C\u003EAccessor : IMemberAccessor<MyCachedServerItem.MyServerData, List<WorkshopId>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyCachedServerItem.MyServerData owner,
          in List<WorkshopId> value)
        {
          owner.Mods = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyCachedServerItem.MyServerData owner,
          out List<WorkshopId> value)
        {
          value = owner.Mods;
        }
      }

      protected class Sandbox_Game_MyCachedServerItem\u003C\u003EMyServerData\u003C\u003EDescription\u003C\u003EAccessor : IMemberAccessor<MyCachedServerItem.MyServerData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyCachedServerItem.MyServerData owner, in string value) => owner.Description = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyCachedServerItem.MyServerData owner, out string value) => value = owner.Description;
      }

      private class Sandbox_Game_MyCachedServerItem\u003C\u003EMyServerData\u003C\u003EActor : IActivator, IActivator<MyCachedServerItem.MyServerData>
      {
        object IActivator.CreateInstance() => (object) new MyCachedServerItem.MyServerData();

        MyCachedServerItem.MyServerData IActivator<MyCachedServerItem.MyServerData>.CreateInstance() => new MyCachedServerItem.MyServerData();
      }
    }
  }
}

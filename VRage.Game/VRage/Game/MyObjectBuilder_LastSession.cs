// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_LastSession
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Net;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_LastSession : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public string Path;
    [ProtoMember(4)]
    public bool IsContentWorlds;
    [ProtoMember(7)]
    public bool IsOnline;
    [ProtoMember(10)]
    public bool IsLobby;
    [ProtoMember(13)]
    public string GameName;
    [ProtoMember(16)]
    public string ServerIP;
    [ProtoMember(19)]
    public int ServerPort;
    [ProtoMember(21)]
    public string ConnectionString;

    public string GetConnectionString()
    {
      if (this.ConnectionString != null)
        return this.ConnectionString;
      IPAddress address;
      if (this.ServerIP != null && this.ServerPort > 0 && IPAddress.TryParse(this.ServerIP, out address))
        return new IPEndPoint(address, this.ServerPort).ToString();
      return this.ServerIP != null ? this.ServerIP : (string) null;
    }

    protected class VRage_Game_MyObjectBuilder_LastSession\u003C\u003EPath\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LastSession owner, in string value) => owner.Path = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LastSession owner, out string value) => value = owner.Path;
    }

    protected class VRage_Game_MyObjectBuilder_LastSession\u003C\u003EIsContentWorlds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LastSession owner, in bool value) => owner.IsContentWorlds = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LastSession owner, out bool value) => value = owner.IsContentWorlds;
    }

    protected class VRage_Game_MyObjectBuilder_LastSession\u003C\u003EIsOnline\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LastSession owner, in bool value) => owner.IsOnline = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LastSession owner, out bool value) => value = owner.IsOnline;
    }

    protected class VRage_Game_MyObjectBuilder_LastSession\u003C\u003EIsLobby\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LastSession owner, in bool value) => owner.IsLobby = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LastSession owner, out bool value) => value = owner.IsLobby;
    }

    protected class VRage_Game_MyObjectBuilder_LastSession\u003C\u003EGameName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LastSession owner, in string value) => owner.GameName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LastSession owner, out string value) => value = owner.GameName;
    }

    protected class VRage_Game_MyObjectBuilder_LastSession\u003C\u003EServerIP\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LastSession owner, in string value) => owner.ServerIP = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LastSession owner, out string value) => value = owner.ServerIP;
    }

    protected class VRage_Game_MyObjectBuilder_LastSession\u003C\u003EServerPort\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LastSession owner, in int value) => owner.ServerPort = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LastSession owner, out int value) => value = owner.ServerPort;
    }

    protected class VRage_Game_MyObjectBuilder_LastSession\u003C\u003EConnectionString\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LastSession, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LastSession owner, in string value) => owner.ConnectionString = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LastSession owner, out string value) => value = owner.ConnectionString;
    }

    protected class VRage_Game_MyObjectBuilder_LastSession\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LastSession, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LastSession owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LastSession owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_LastSession\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LastSession, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LastSession owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LastSession owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_LastSession\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LastSession, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LastSession owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LastSession owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_LastSession\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LastSession, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LastSession owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LastSession owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_LastSession\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_LastSession>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_LastSession();

      MyObjectBuilder_LastSession IActivator<MyObjectBuilder_LastSession>.CreateInstance() => new MyObjectBuilder_LastSession();
    }
  }
}

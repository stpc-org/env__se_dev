// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WorldConfiguration
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
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
  public class MyObjectBuilder_WorldConfiguration : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    [XmlElement("Settings", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_SessionSettings>))]
    public MyObjectBuilder_SessionSettings Settings = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SessionSettings>();
    [ProtoMember(3)]
    public List<MyObjectBuilder_Checkpoint.ModItem> Mods;
    [ProtoMember(5)]
    public string SessionName;
    [ProtoMember(7)]
    public DateTime? LastSaveTime;

    protected class VRage_Game_MyObjectBuilder_WorldConfiguration\u003C\u003ESettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldConfiguration, MyObjectBuilder_SessionSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldConfiguration owner,
        in MyObjectBuilder_SessionSettings value)
      {
        owner.Settings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldConfiguration owner,
        out MyObjectBuilder_SessionSettings value)
      {
        value = owner.Settings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldConfiguration\u003C\u003EMods\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldConfiguration, List<MyObjectBuilder_Checkpoint.ModItem>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldConfiguration owner,
        in List<MyObjectBuilder_Checkpoint.ModItem> value)
      {
        owner.Mods = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldConfiguration owner,
        out List<MyObjectBuilder_Checkpoint.ModItem> value)
      {
        value = owner.Mods;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldConfiguration\u003C\u003ESessionName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldConfiguration, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WorldConfiguration owner, in string value) => owner.SessionName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WorldConfiguration owner, out string value) => value = owner.SessionName;
    }

    protected class VRage_Game_MyObjectBuilder_WorldConfiguration\u003C\u003ELastSaveTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldConfiguration, DateTime?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WorldConfiguration owner, in DateTime? value) => owner.LastSaveTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WorldConfiguration owner, out DateTime? value) => value = owner.LastSaveTime;
    }

    protected class VRage_Game_MyObjectBuilder_WorldConfiguration\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldConfiguration, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WorldConfiguration owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WorldConfiguration owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WorldConfiguration\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldConfiguration, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WorldConfiguration owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WorldConfiguration owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WorldConfiguration\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldConfiguration, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WorldConfiguration owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WorldConfiguration owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WorldConfiguration\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldConfiguration, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WorldConfiguration owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WorldConfiguration owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_WorldConfiguration\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WorldConfiguration>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WorldConfiguration();

      MyObjectBuilder_WorldConfiguration IActivator<MyObjectBuilder_WorldConfiguration>.CreateInstance() => new MyObjectBuilder_WorldConfiguration();
    }
  }
}

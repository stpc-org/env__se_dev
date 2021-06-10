// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Gui.MyObjectBuilder_ServerFilterOptions
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Gui
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ServerFilterOptions : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public bool AllowedGroups;
    [ProtoMember(4)]
    public bool SameVersion;
    [ProtoMember(7)]
    public bool SameData;
    [ProtoMember(10)]
    public bool? HasPassword;
    [ProtoMember(13)]
    public bool CreativeMode;
    [ProtoMember(16)]
    public bool SurvivalMode;
    [ProtoMember(19)]
    public bool CheckPlayer;
    [ProtoMember(22)]
    public SerializableRange PlayerCount;
    [ProtoMember(25)]
    public bool CheckMod;
    [ProtoMember(28)]
    public SerializableRange ModCount;
    [ProtoMember(31)]
    public bool CheckDistance;
    [ProtoMember(34)]
    public SerializableRange ViewDistance;
    [ProtoMember(37)]
    public bool Advanced;
    [ProtoMember(40)]
    public int Ping;
    [ProtoMember(43)]
    public bool ModsExclusive;
    [ProtoMember(48)]
    public List<WorkshopId> WorkshopMods;
    [ProtoMember(49)]
    public SerializableDictionary<byte, string> Filters;

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003EAllowedGroups\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value) => owner.AllowedGroups = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value) => value = owner.AllowedGroups;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003ESameVersion\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value) => owner.SameVersion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value) => value = owner.SameVersion;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003ESameData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value) => owner.SameData = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value) => value = owner.SameData;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003EHasPassword\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool? value) => owner.HasPassword = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool? value) => value = owner.HasPassword;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003ECreativeMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value) => owner.CreativeMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value) => value = owner.CreativeMode;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003ESurvivalMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value) => owner.SurvivalMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value) => value = owner.SurvivalMode;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003ECheckPlayer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value) => owner.CheckPlayer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value) => value = owner.CheckPlayer;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003EPlayerCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ServerFilterOptions owner,
        in SerializableRange value)
      {
        owner.PlayerCount = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ServerFilterOptions owner,
        out SerializableRange value)
      {
        value = owner.PlayerCount;
      }
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003ECheckMod\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value) => owner.CheckMod = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value) => value = owner.CheckMod;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003EModCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ServerFilterOptions owner,
        in SerializableRange value)
      {
        owner.ModCount = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ServerFilterOptions owner,
        out SerializableRange value)
      {
        value = owner.ModCount;
      }
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003ECheckDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value) => owner.CheckDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value) => value = owner.CheckDistance;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003EViewDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ServerFilterOptions owner,
        in SerializableRange value)
      {
        owner.ViewDistance = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ServerFilterOptions owner,
        out SerializableRange value)
      {
        value = owner.ViewDistance;
      }
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003EAdvanced\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value) => owner.Advanced = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value) => value = owner.Advanced;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003EPing\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in int value) => owner.Ping = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out int value) => value = owner.Ping;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003EModsExclusive\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in bool value) => owner.ModsExclusive = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out bool value) => value = owner.ModsExclusive;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003EWorkshopMods\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, List<WorkshopId>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ServerFilterOptions owner,
        in List<WorkshopId> value)
      {
        owner.WorkshopMods = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ServerFilterOptions owner,
        out List<WorkshopId> value)
      {
        value = owner.WorkshopMods;
      }
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003EFilters\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ServerFilterOptions, SerializableDictionary<byte, string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ServerFilterOptions owner,
        in SerializableDictionary<byte, string> value)
      {
        owner.Filters = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ServerFilterOptions owner,
        out SerializableDictionary<byte, string> value)
      {
        value = owner.Filters;
      }
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ServerFilterOptions, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ServerFilterOptions, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ServerFilterOptions, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ServerFilterOptions, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ServerFilterOptions owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ServerFilterOptions owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ServerFilterOptions\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ServerFilterOptions>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ServerFilterOptions();

      MyObjectBuilder_ServerFilterOptions IActivator<MyObjectBuilder_ServerFilterOptions>.CreateInstance() => new MyObjectBuilder_ServerFilterOptions();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_Localization
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Localization : MyObjectBuilder_Base
  {
    public uint Id;
    public string Language = "English";
    public string Context = "VRage";
    public string ResourceName = "Default Name";
    public bool Default;
    public string ResXName;
    [XmlIgnore]
    public List<MyObjectBuilder_Localization.KeyEntry> Entries = new List<MyObjectBuilder_Localization.KeyEntry>();
    [XmlIgnore]
    public bool Modified;

    public override string ToString() => this.ResourceName + " " + (object) this.Id;

    public struct KeyEntry
    {
      public string Key;
      public string Value;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Localization owner, in uint value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Localization owner, out uint value) => value = owner.Id;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003ELanguage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Localization owner, in string value) => owner.Language = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Localization owner, out string value) => value = owner.Language;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003EContext\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Localization owner, in string value) => owner.Context = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Localization owner, out string value) => value = owner.Context;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003EResourceName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Localization owner, in string value) => owner.ResourceName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Localization owner, out string value) => value = owner.ResourceName;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003EDefault\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Localization owner, in bool value) => owner.Default = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Localization owner, out bool value) => value = owner.Default;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003EResXName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Localization owner, in string value) => owner.ResXName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Localization owner, out string value) => value = owner.ResXName;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003EEntries\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, List<MyObjectBuilder_Localization.KeyEntry>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Localization owner,
        in List<MyObjectBuilder_Localization.KeyEntry> value)
      {
        owner.Entries = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Localization owner,
        out List<MyObjectBuilder_Localization.KeyEntry> value)
      {
        value = owner.Entries;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003EModified\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Localization owner, in bool value) => owner.Modified = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Localization owner, out bool value) => value = owner.Modified;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Localization, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Localization owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Localization owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Localization, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Localization owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Localization owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Localization, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Localization owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Localization owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Localization, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Localization owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Localization owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Localization>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Localization();

      MyObjectBuilder_Localization IActivator<MyObjectBuilder_Localization>.CreateInstance() => new MyObjectBuilder_Localization();
    }
  }
}

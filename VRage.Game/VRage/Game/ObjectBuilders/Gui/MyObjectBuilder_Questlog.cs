// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Gui.MyObjectBuilder_Questlog
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Gui
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  [ProtoContract]
  public class MyObjectBuilder_Questlog : MyObjectBuilder_Base
  {
    [ProtoMember(5)]
    public List<MultilineData> LineData = new List<MultilineData>();
    [ProtoMember(10)]
    public string Title = string.Empty;
    [ProtoMember(20)]
    public bool Visible = true;
    [ProtoMember(30)]
    public bool IsUsedByVisualScripting;

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog\u003C\u003ELineData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Questlog, List<MultilineData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Questlog owner, in List<MultilineData> value) => owner.LineData = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Questlog owner, out List<MultilineData> value) => value = owner.LineData;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog\u003C\u003ETitle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Questlog, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Questlog owner, in string value) => owner.Title = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Questlog owner, out string value) => value = owner.Title;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog\u003C\u003EVisible\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Questlog, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Questlog owner, in bool value) => owner.Visible = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Questlog owner, out bool value) => value = owner.Visible;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog\u003C\u003EIsUsedByVisualScripting\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Questlog, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Questlog owner, in bool value) => owner.IsUsedByVisualScripting = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Questlog owner, out bool value) => value = owner.IsUsedByVisualScripting;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Questlog, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Questlog owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Questlog owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Questlog, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Questlog owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Questlog owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Questlog, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Questlog owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Questlog owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Questlog, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Questlog owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Questlog owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Questlog>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Questlog();

      MyObjectBuilder_Questlog IActivator<MyObjectBuilder_Questlog>.CreateInstance() => new MyObjectBuilder_Questlog();
    }
  }
}

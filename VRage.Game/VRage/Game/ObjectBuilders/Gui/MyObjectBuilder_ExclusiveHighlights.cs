// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Gui.MyObjectBuilder_ExclusiveHighlights
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
  public class MyObjectBuilder_ExclusiveHighlights : MyObjectBuilder_Base
  {
    [ProtoMember(5)]
    public List<MyHighlightData> ExclusiveHighlightData = new List<MyHighlightData>();

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ExclusiveHighlights\u003C\u003EExclusiveHighlightData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ExclusiveHighlights, List<MyHighlightData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ExclusiveHighlights owner,
        in List<MyHighlightData> value)
      {
        owner.ExclusiveHighlightData = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ExclusiveHighlights owner,
        out List<MyHighlightData> value)
      {
        value = owner.ExclusiveHighlightData;
      }
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ExclusiveHighlights\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ExclusiveHighlights, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ExclusiveHighlights owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ExclusiveHighlights owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ExclusiveHighlights\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ExclusiveHighlights, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ExclusiveHighlights owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ExclusiveHighlights owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ExclusiveHighlights\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ExclusiveHighlights, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ExclusiveHighlights owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ExclusiveHighlights owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ExclusiveHighlights\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ExclusiveHighlights, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ExclusiveHighlights owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ExclusiveHighlights owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_ExclusiveHighlights\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ExclusiveHighlights>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ExclusiveHighlights();

      MyObjectBuilder_ExclusiveHighlights IActivator<MyObjectBuilder_ExclusiveHighlights>.CreateInstance() => new MyObjectBuilder_ExclusiveHighlights();
    }
  }
}

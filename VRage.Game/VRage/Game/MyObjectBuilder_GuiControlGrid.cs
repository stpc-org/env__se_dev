// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_GuiControlGrid
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_GuiControlGrid : MyObjectBuilder_GuiControlBase
  {
    [ProtoMember(1)]
    public MyGuiControlGridStyleEnum VisualStyle;
    [ProtoMember(4)]
    public int DisplayColumnsCount = 1;
    [ProtoMember(7)]
    public int DisplayRowsCount = 1;

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003EVisualStyle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlGrid, MyGuiControlGridStyleEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlGrid owner,
        in MyGuiControlGridStyleEnum value)
      {
        owner.VisualStyle = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlGrid owner,
        out MyGuiControlGridStyleEnum value)
      {
        value = owner.VisualStyle;
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003EDisplayColumnsCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlGrid, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in int value) => owner.DisplayColumnsCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlGrid owner, out int value) => value = owner.DisplayColumnsCount;
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003EDisplayRowsCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlGrid, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in int value) => owner.DisplayRowsCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlGrid owner, out int value) => value = owner.DisplayRowsCount;
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlGrid, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in Vector2 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlGrid owner, out Vector2 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlGrid, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in Vector2 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlGrid owner, out Vector2 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlGrid, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in string value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlGrid owner, out string value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003EBackgroundColor\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EBackgroundColor\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlGrid, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in Vector4 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlGrid owner, out Vector4 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003EControlTexture\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlTexture\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlGrid, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in string value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlGrid owner, out string value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003EOriginAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EOriginAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlGrid, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in MyGuiDrawAlignEnum value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlGrid owner,
        out MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlGrid, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlGrid owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlGrid, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlGrid owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003EControlAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlGrid, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in int value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlGrid owner, out int value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlGrid, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlGrid owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlGrid, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlGrid owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlGrid owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_GuiControlGrid\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlGrid>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_GuiControlGrid();

      MyObjectBuilder_GuiControlGrid IActivator<MyObjectBuilder_GuiControlGrid>.CreateInstance() => new MyObjectBuilder_GuiControlGrid();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_GuiControlSeparatorList
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
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
  public class MyObjectBuilder_GuiControlSeparatorList : MyObjectBuilder_GuiControlBase
  {
    [ProtoMember(13)]
    public List<MyObjectBuilder_GuiControlSeparatorList.Separator> Separators = new List<MyObjectBuilder_GuiControlSeparatorList.Separator>();

    [ProtoContract]
    public struct Separator
    {
      [ProtoMember(1)]
      [DefaultValue(0.0f)]
      [XmlAttribute]
      public float StartX { get; set; }

      [ProtoMember(4)]
      [DefaultValue(0.0f)]
      [XmlAttribute]
      public float StartY { get; set; }

      [ProtoMember(7)]
      [DefaultValue(0.0f)]
      [XmlAttribute]
      public float SizeX { get; set; }

      [ProtoMember(10)]
      [DefaultValue(0.0f)]
      [XmlAttribute]
      public float SizeY { get; set; }

      protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003ESeparator\u003C\u003EStartX\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList.Separator, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_GuiControlSeparatorList.Separator owner,
          in float value)
        {
          owner.StartX = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_GuiControlSeparatorList.Separator owner,
          out float value)
        {
          value = owner.StartX;
        }
      }

      protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003ESeparator\u003C\u003EStartY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList.Separator, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_GuiControlSeparatorList.Separator owner,
          in float value)
        {
          owner.StartY = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_GuiControlSeparatorList.Separator owner,
          out float value)
        {
          value = owner.StartY;
        }
      }

      protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003ESeparator\u003C\u003ESizeX\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList.Separator, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_GuiControlSeparatorList.Separator owner,
          in float value)
        {
          owner.SizeX = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_GuiControlSeparatorList.Separator owner,
          out float value)
        {
          value = owner.SizeX;
        }
      }

      protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003ESeparator\u003C\u003ESizeY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList.Separator, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_GuiControlSeparatorList.Separator owner,
          in float value)
        {
          owner.SizeY = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_GuiControlSeparatorList.Separator owner,
          out float value)
        {
          value = owner.SizeY;
        }
      }

      private class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003ESeparator\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlSeparatorList.Separator>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_GuiControlSeparatorList.Separator();

        MyObjectBuilder_GuiControlSeparatorList.Separator IActivator<MyObjectBuilder_GuiControlSeparatorList.Separator>.CreateInstance() => new MyObjectBuilder_GuiControlSeparatorList.Separator();
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003ESeparators\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList, List<MyObjectBuilder_GuiControlSeparatorList.Separator>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlSeparatorList owner,
        in List<MyObjectBuilder_GuiControlSeparatorList.Separator> value)
      {
        owner.Separators = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlSeparatorList owner,
        out List<MyObjectBuilder_GuiControlSeparatorList.Separator> value)
      {
        value = owner.Separators;
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlSeparatorList owner, in Vector2 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlSeparatorList owner, out Vector2 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlSeparatorList owner, in Vector2 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlSeparatorList owner, out Vector2 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlSeparatorList owner, in string value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlSeparatorList owner, out string value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003EBackgroundColor\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EBackgroundColor\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlSeparatorList owner, in Vector4 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlSeparatorList owner, out Vector4 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003EControlTexture\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlTexture\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlSeparatorList owner, in string value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlSeparatorList owner, out string value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003EOriginAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EOriginAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlSeparatorList owner,
        in MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlSeparatorList owner,
        out MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlSeparatorList owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlSeparatorList owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlSeparatorList owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlSeparatorList owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003EControlAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlSeparatorList owner, in int value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlSeparatorList owner, out int value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlSeparatorList owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlSeparatorList owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlSeparatorList, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlSeparatorList owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlSeparatorList owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_GuiControlSeparatorList\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlSeparatorList>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_GuiControlSeparatorList();

      MyObjectBuilder_GuiControlSeparatorList IActivator<MyObjectBuilder_GuiControlSeparatorList>.CreateInstance() => new MyObjectBuilder_GuiControlSeparatorList();
    }
  }
}

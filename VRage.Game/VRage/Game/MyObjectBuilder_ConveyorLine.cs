// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ConveyorLine
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
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ConveyorLine : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public SerializableVector3I StartPosition;
    [ProtoMember(4)]
    public Base6Directions.Direction StartDirection;
    [ProtoMember(7)]
    public SerializableVector3I EndPosition;
    [ProtoMember(10)]
    public Base6Directions.Direction EndDirection;
    [ProtoMember(13)]
    public List<MyObjectBuilder_ConveyorPacket> PacketsForward = new List<MyObjectBuilder_ConveyorPacket>();
    [ProtoMember(16)]
    public List<MyObjectBuilder_ConveyorPacket> PacketsBackward = new List<MyObjectBuilder_ConveyorPacket>();
    [ProtoMember(19)]
    [DefaultValue(null)]
    [XmlArrayItem("Section")]
    [Serialize(MyObjectFlags.DefaultZero)]
    public List<SerializableLineSectionInformation> Sections;
    [ProtoMember(22)]
    [DefaultValue(MyObjectBuilder_ConveyorLine.LineType.DEFAULT_LINE)]
    public MyObjectBuilder_ConveyorLine.LineType ConveyorLineType;
    [ProtoMember(25)]
    [DefaultValue(MyObjectBuilder_ConveyorLine.LineConductivity.FULL)]
    public MyObjectBuilder_ConveyorLine.LineConductivity ConveyorLineConductivity;

    public bool ShouldSerializePacketsForward() => (uint) this.PacketsForward.Count > 0U;

    public bool ShouldSerializePacketsBackward() => (uint) this.PacketsBackward.Count > 0U;

    public bool ShouldSerializeSections() => this.Sections != null;

    public MyObjectBuilder_ConveyorLine() => this.Sections = new List<SerializableLineSectionInformation>();

    public enum LineType
    {
      DEFAULT_LINE,
      SMALL_LINE,
      LARGE_LINE,
    }

    public enum LineConductivity
    {
      FULL,
      FORWARD,
      BACKWARD,
      NONE,
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003EStartPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConveyorLine owner, in SerializableVector3I value) => owner.StartPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConveyorLine owner,
        out SerializableVector3I value)
      {
        value = owner.StartPosition;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003EStartDirection\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ConveyorLine owner,
        in Base6Directions.Direction value)
      {
        owner.StartDirection = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConveyorLine owner,
        out Base6Directions.Direction value)
      {
        value = owner.StartDirection;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003EEndPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConveyorLine owner, in SerializableVector3I value) => owner.EndPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConveyorLine owner,
        out SerializableVector3I value)
      {
        value = owner.EndPosition;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003EEndDirection\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ConveyorLine owner,
        in Base6Directions.Direction value)
      {
        owner.EndDirection = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConveyorLine owner,
        out Base6Directions.Direction value)
      {
        value = owner.EndDirection;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003EPacketsForward\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, List<MyObjectBuilder_ConveyorPacket>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ConveyorLine owner,
        in List<MyObjectBuilder_ConveyorPacket> value)
      {
        owner.PacketsForward = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConveyorLine owner,
        out List<MyObjectBuilder_ConveyorPacket> value)
      {
        value = owner.PacketsForward;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003EPacketsBackward\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, List<MyObjectBuilder_ConveyorPacket>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ConveyorLine owner,
        in List<MyObjectBuilder_ConveyorPacket> value)
      {
        owner.PacketsBackward = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConveyorLine owner,
        out List<MyObjectBuilder_ConveyorPacket> value)
      {
        value = owner.PacketsBackward;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003ESections\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, List<SerializableLineSectionInformation>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ConveyorLine owner,
        in List<SerializableLineSectionInformation> value)
      {
        owner.Sections = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConveyorLine owner,
        out List<SerializableLineSectionInformation> value)
      {
        value = owner.Sections;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003EConveyorLineType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, MyObjectBuilder_ConveyorLine.LineType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ConveyorLine owner,
        in MyObjectBuilder_ConveyorLine.LineType value)
      {
        owner.ConveyorLineType = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConveyorLine owner,
        out MyObjectBuilder_ConveyorLine.LineType value)
      {
        value = owner.ConveyorLineType;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003EConveyorLineConductivity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConveyorLine, MyObjectBuilder_ConveyorLine.LineConductivity>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ConveyorLine owner,
        in MyObjectBuilder_ConveyorLine.LineConductivity value)
      {
        owner.ConveyorLineConductivity = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConveyorLine owner,
        out MyObjectBuilder_ConveyorLine.LineConductivity value)
      {
        value = owner.ConveyorLineConductivity;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ConveyorLine, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConveyorLine owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConveyorLine owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ConveyorLine, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConveyorLine owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConveyorLine owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ConveyorLine, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConveyorLine owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConveyorLine owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ConveyorLine, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConveyorLine owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConveyorLine owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ConveyorLine\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ConveyorLine>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ConveyorLine();

      MyObjectBuilder_ConveyorLine IActivator<MyObjectBuilder_ConveyorLine>.CreateInstance() => new MyObjectBuilder_ConveyorLine();
    }
  }
}

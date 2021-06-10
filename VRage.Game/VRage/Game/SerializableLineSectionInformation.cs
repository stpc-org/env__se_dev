// Decompiled with JetBrains decompiler
// Type: VRage.Game.SerializableLineSectionInformation
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  public struct SerializableLineSectionInformation
  {
    [ProtoMember(1)]
    [XmlAttribute]
    public Base6Directions.Direction Direction;
    [ProtoMember(4)]
    [XmlAttribute]
    public int Length;

    protected class VRage_Game_SerializableLineSectionInformation\u003C\u003EDirection\u003C\u003EAccessor : IMemberAccessor<SerializableLineSectionInformation, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref SerializableLineSectionInformation owner,
        in Base6Directions.Direction value)
      {
        owner.Direction = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref SerializableLineSectionInformation owner,
        out Base6Directions.Direction value)
      {
        value = owner.Direction;
      }
    }

    protected class VRage_Game_SerializableLineSectionInformation\u003C\u003ELength\u003C\u003EAccessor : IMemberAccessor<SerializableLineSectionInformation, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableLineSectionInformation owner, in int value) => owner.Length = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableLineSectionInformation owner, out int value) => value = owner.Length;
    }

    private class VRage_Game_SerializableLineSectionInformation\u003C\u003EActor : IActivator, IActivator<SerializableLineSectionInformation>
    {
      object IActivator.CreateInstance() => (object) new SerializableLineSectionInformation();

      SerializableLineSectionInformation IActivator<SerializableLineSectionInformation>.CreateInstance() => new SerializableLineSectionInformation();
    }
  }
}

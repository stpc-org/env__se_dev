// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetMaterialLayer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct MyPlanetMaterialLayer
  {
    [ProtoMember(1)]
    [XmlAttribute(AttributeName = "Material")]
    public string Material;
    [ProtoMember(2)]
    [XmlAttribute(AttributeName = "Depth")]
    public float Depth;

    protected class VRage_Game_MyPlanetMaterialLayer\u003C\u003EMaterial\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialLayer, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialLayer owner, in string value) => owner.Material = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialLayer owner, out string value) => value = owner.Material;
    }

    protected class VRage_Game_MyPlanetMaterialLayer\u003C\u003EDepth\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialLayer, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialLayer owner, in float value) => owner.Depth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialLayer owner, out float value) => value = owner.Depth;
    }

    private class VRage_Game_MyPlanetMaterialLayer\u003C\u003EActor : IActivator, IActivator<MyPlanetMaterialLayer>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetMaterialLayer();

      MyPlanetMaterialLayer IActivator<MyPlanetMaterialLayer>.CreateInstance() => new MyPlanetMaterialLayer();
    }
  }
}

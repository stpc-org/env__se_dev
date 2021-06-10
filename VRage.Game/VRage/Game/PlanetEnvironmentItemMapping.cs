// Decompiled with JetBrains decompiler
// Type: VRage.Game.PlanetEnvironmentItemMapping
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
  public struct PlanetEnvironmentItemMapping
  {
    [ProtoMember(65)]
    [XmlArrayItem("Material")]
    public string[] Materials;
    [ProtoMember(66)]
    [XmlArrayItem("Biome")]
    public int[] Biomes;
    [ProtoMember(67)]
    [XmlArrayItem("Item")]
    public MyPlanetEnvironmentItemDef[] Items;
    [ProtoMember(68)]
    public MyPlanetSurfaceRule Rule;

    protected class VRage_Game_PlanetEnvironmentItemMapping\u003C\u003EMaterials\u003C\u003EAccessor : IMemberAccessor<PlanetEnvironmentItemMapping, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlanetEnvironmentItemMapping owner, in string[] value) => owner.Materials = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlanetEnvironmentItemMapping owner, out string[] value) => value = owner.Materials;
    }

    protected class VRage_Game_PlanetEnvironmentItemMapping\u003C\u003EBiomes\u003C\u003EAccessor : IMemberAccessor<PlanetEnvironmentItemMapping, int[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlanetEnvironmentItemMapping owner, in int[] value) => owner.Biomes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlanetEnvironmentItemMapping owner, out int[] value) => value = owner.Biomes;
    }

    protected class VRage_Game_PlanetEnvironmentItemMapping\u003C\u003EItems\u003C\u003EAccessor : IMemberAccessor<PlanetEnvironmentItemMapping, MyPlanetEnvironmentItemDef[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref PlanetEnvironmentItemMapping owner,
        in MyPlanetEnvironmentItemDef[] value)
      {
        owner.Items = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref PlanetEnvironmentItemMapping owner,
        out MyPlanetEnvironmentItemDef[] value)
      {
        value = owner.Items;
      }
    }

    protected class VRage_Game_PlanetEnvironmentItemMapping\u003C\u003ERule\u003C\u003EAccessor : IMemberAccessor<PlanetEnvironmentItemMapping, MyPlanetSurfaceRule>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlanetEnvironmentItemMapping owner, in MyPlanetSurfaceRule value) => owner.Rule = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlanetEnvironmentItemMapping owner, out MyPlanetSurfaceRule value) => value = owner.Rule;
    }

    private class VRage_Game_PlanetEnvironmentItemMapping\u003C\u003EActor : IActivator, IActivator<PlanetEnvironmentItemMapping>
    {
      object IActivator.CreateInstance() => (object) new PlanetEnvironmentItemMapping();

      PlanetEnvironmentItemMapping IActivator<PlanetEnvironmentItemMapping>.CreateInstance() => new PlanetEnvironmentItemMapping();
    }
  }
}

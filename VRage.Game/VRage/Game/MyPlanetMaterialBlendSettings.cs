// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetMaterialBlendSettings
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct MyPlanetMaterialBlendSettings
  {
    [ProtoMember(72)]
    public string Texture;
    [ProtoMember(73)]
    public int CellSize;

    protected class VRage_Game_MyPlanetMaterialBlendSettings\u003C\u003ETexture\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialBlendSettings, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialBlendSettings owner, in string value) => owner.Texture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialBlendSettings owner, out string value) => value = owner.Texture;
    }

    protected class VRage_Game_MyPlanetMaterialBlendSettings\u003C\u003ECellSize\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaterialBlendSettings, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaterialBlendSettings owner, in int value) => owner.CellSize = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaterialBlendSettings owner, out int value) => value = owner.CellSize;
    }

    private class VRage_Game_MyPlanetMaterialBlendSettings\u003C\u003EActor : IActivator, IActivator<MyPlanetMaterialBlendSettings>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetMaterialBlendSettings();

      MyPlanetMaterialBlendSettings IActivator<MyPlanetMaterialBlendSettings>.CreateInstance() => new MyPlanetMaterialBlendSettings();
    }
  }
}

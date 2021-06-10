// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.MyItemInfo
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRageMath;

namespace VRage.Game.ModAPI.Ingame
{
  public struct MyItemInfo
  {
    public float Mass;
    public Vector3 Size;
    public float Volume;
    public MyFixedPoint MaxStackAmount;
    public bool UsesFractions;
    public bool IsOre;
    public bool IsIngot;
    public bool IsComponent;
    public bool IsTool;
    public bool IsAmmo;
  }
}

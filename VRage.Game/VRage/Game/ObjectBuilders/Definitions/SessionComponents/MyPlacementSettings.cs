// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.SessionComponents.MyPlacementSettings
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game.ObjectBuilders.Definitions.SessionComponents
{
  public struct MyPlacementSettings
  {
    public MyGridPlacementSettings SmallGrid;
    public MyGridPlacementSettings SmallStaticGrid;
    public MyGridPlacementSettings LargeGrid;
    public MyGridPlacementSettings LargeStaticGrid;
    public bool StaticGridAlignToCenter;

    public MyGridPlacementSettings GetGridPlacementSettings(
      MyCubeSize cubeSize,
      bool isStatic)
    {
      switch (cubeSize)
      {
        case MyCubeSize.Large:
          return !isStatic ? this.LargeGrid : this.LargeStaticGrid;
        case MyCubeSize.Small:
          return !isStatic ? this.SmallGrid : this.SmallStaticGrid;
        default:
          return this.LargeGrid;
      }
    }

    public MyGridPlacementSettings GetGridPlacementSettings(
      MyCubeSize cubeSize)
    {
      return cubeSize == MyCubeSize.Large || cubeSize != MyCubeSize.Small ? this.LargeGrid : this.SmallGrid;
    }
  }
}

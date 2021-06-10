// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyRadialMenuItemVoxelHand
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.SessionComponents;
using VRage.Game;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Screens.Helpers
{
  [MyRadialMenuItemDescriptor(typeof (MyObjectBuilder_RadialMenuItemVoxelHand))]
  internal class MyRadialMenuItemVoxelHand : MyRadialMenuItem
  {
    public SerializableDefinitionId Material;

    public override void Init(MyObjectBuilder_RadialMenuItem builder)
    {
      base.Init(builder);
      this.Material = ((MyObjectBuilder_RadialMenuItemVoxelHand) builder).Material;
    }

    public override void Activate(params object[] parameters)
    {
      MySessionComponentVoxelHand.Static.SetMaterial(this.Material);
      MySessionComponentVoxelHand.Static.CurrentMaterialTextureName = this.GetIcon();
    }
  }
}

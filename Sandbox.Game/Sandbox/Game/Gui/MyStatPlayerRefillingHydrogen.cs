// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatPlayerRefillingHydrogen
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character.Components;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatPlayerRefillingHydrogen : MyStatPlayerGasRefillingBase
  {
    public MyStatPlayerRefillingHydrogen() => this.Id = MyStringHash.GetOrCompute("player_refilling_hydrogen");

    protected override float GetGassLevel(MyCharacterOxygenComponent oxygenComp) => oxygenComp.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId);
  }
}

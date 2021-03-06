// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatPlayerHydrogen
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatPlayerHydrogen : MyStatBase
  {
    public MyStatPlayerHydrogen() => this.Id = MyStringHash.GetOrCompute("player_hydrogen");

    public override void Update()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null || localCharacter.OxygenComponent == null)
        return;
      this.CurrentValue = localCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId);
    }

    public override string ToString() => string.Format("{0:0}", (object) (float) ((double) this.CurrentValue * 100.0));
  }
}

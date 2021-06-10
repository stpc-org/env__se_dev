// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatPlayerEnergy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatPlayerEnergy : MyStatBase
  {
    public MyStatPlayerEnergy() => this.Id = MyStringHash.GetOrCompute("player_energy");

    public override void Update()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null)
        return;
      this.CurrentValue = localCharacter.SuitEnergyLevel;
    }

    public override string ToString() => string.Format("{0:0}", (object) (float) ((double) this.CurrentValue * 100.0));
  }
}

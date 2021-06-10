// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatPlayerHealth
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatPlayerHealth : MyStatBase
  {
    public MyStatPlayerHealth() => this.Id = MyStringHash.GetOrCompute("player_health");

    public override void Update()
    {
      MyCharacter myCharacter = MySession.Static != null ? MySession.Static.LocalCharacter : (MyCharacter) null;
      if (myCharacter == null || myCharacter.StatComp == null)
        return;
      this.CurrentValue = myCharacter.StatComp.HealthRatio;
    }

    public override string ToString() => string.Format("{0:0}", (object) (float) ((double) this.CurrentValue * 100.0));
  }
}

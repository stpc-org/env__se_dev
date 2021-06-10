// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatPlayerJetpack
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatPlayerJetpack : MyStatBase
  {
    public MyStatPlayerJetpack() => this.Id = MyStringHash.GetOrCompute("player_jetpack");

    public override void Update()
    {
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter != null && localCharacter.JetpackComp != null)
        this.CurrentValue = localCharacter.JetpackComp.TurnedOn ? 1f : 0.0f;
      else
        this.CurrentValue = 0.0f;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatPlayerHelmet
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatPlayerHelmet : MyStatBase
  {
    public MyStatPlayerHelmet() => this.Id = MyStringHash.GetOrCompute("player_helmet");

    public override void Update()
    {
      if (MySession.Static == null)
        return;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter == null || localCharacter.OxygenComponent == null)
        return;
      this.CurrentValue = localCharacter.OxygenComponent.HelmetEnabled ? 1f : 0.0f;
    }
  }
}

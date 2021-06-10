// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyGhostCharacterDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GhostCharacterDefinition), null)]
  public class MyGhostCharacterDefinition : MyDefinitionBase
  {
    public List<MyDefinitionId> LeftHandWeapons = new List<MyDefinitionId>();
    public List<MyDefinitionId> RightHandWeapons = new List<MyDefinitionId>();

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_GhostCharacterDefinition characterDefinition = builder as MyObjectBuilder_GhostCharacterDefinition;
      if (characterDefinition.LeftHandWeapons != null)
      {
        foreach (SerializableDefinitionId leftHandWeapon in characterDefinition.LeftHandWeapons)
          this.LeftHandWeapons.Add((MyDefinitionId) leftHandWeapon);
      }
      if (characterDefinition.RightHandWeapons == null)
        return;
      foreach (SerializableDefinitionId rightHandWeapon in characterDefinition.RightHandWeapons)
        this.RightHandWeapons.Add((MyDefinitionId) rightHandWeapon);
    }

    private class Sandbox_Definitions_MyGhostCharacterDefinition\u003C\u003EActor : IActivator, IActivator<MyGhostCharacterDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGhostCharacterDefinition();

      MyGhostCharacterDefinition IActivator<MyGhostCharacterDefinition>.CreateInstance() => new MyGhostCharacterDefinition();
    }
  }
}

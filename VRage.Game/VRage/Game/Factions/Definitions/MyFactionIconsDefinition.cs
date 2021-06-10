// Decompiled with JetBrains decompiler
// Type: VRage.Game.Factions.Definitions.MyFactionIconsDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions.Factions;
using VRage.Network;
using VRageMath;

namespace VRage.Game.Factions.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_FactionIconsDefinition), null)]
  public class MyFactionIconsDefinition : MyDefinitionBase
  {
    public List<Vector3> BackgroundColorRanges;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_FactionIconsDefinition factionIconsDefinition = (MyObjectBuilder_FactionIconsDefinition) builder;
      if (factionIconsDefinition.BackgroundColorRanges == null)
        return;
      this.BackgroundColorRanges = new List<Vector3>(factionIconsDefinition.BackgroundColorRanges.Count);
      foreach (SerializableVector3 backgroundColorRange in factionIconsDefinition.BackgroundColorRanges)
        this.BackgroundColorRanges.Add((Vector3) backgroundColorRange);
    }

    private class VRage_Game_Factions_Definitions_MyFactionIconsDefinition\u003C\u003EActor : IActivator, IActivator<MyFactionIconsDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyFactionIconsDefinition();

      MyFactionIconsDefinition IActivator<MyFactionIconsDefinition>.CreateInstance() => new MyFactionIconsDefinition();
    }
  }
}

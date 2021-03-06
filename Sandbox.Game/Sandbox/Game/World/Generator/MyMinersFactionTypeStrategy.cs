// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyMinersFactionTypeStrategy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;

namespace Sandbox.Game.World.Generator
{
  internal class MyMinersFactionTypeStrategy : MyFactionTypeBaseStrategy
  {
    private static MyDefinitionId MINER_ID = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FactionTypeDefinition), "Miner");

    public MyMinersFactionTypeStrategy()
      : base(MyMinersFactionTypeStrategy.MINER_ID)
    {
    }

    public override void UpdateStationsStoreItems(MyFaction faction, bool firstGeneration) => base.UpdateStationsStoreItems(faction, firstGeneration);
  }
}

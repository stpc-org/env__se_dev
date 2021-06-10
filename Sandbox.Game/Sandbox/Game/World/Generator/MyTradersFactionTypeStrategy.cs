// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyTradersFactionTypeStrategy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ObjectBuilders;

namespace Sandbox.Game.World.Generator
{
  internal class MyTradersFactionTypeStrategy : MyFactionTypeBaseStrategy
  {
    private static MyDefinitionId TRADER_ID = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FactionTypeDefinition), "Trader");

    public MyTradersFactionTypeStrategy()
      : base(MyTradersFactionTypeStrategy.TRADER_ID)
    {
    }

    public override void UpdateStationsStoreItems(MyFaction faction, bool firstGeneration) => base.UpdateStationsStoreItems(faction, firstGeneration);
  }
}

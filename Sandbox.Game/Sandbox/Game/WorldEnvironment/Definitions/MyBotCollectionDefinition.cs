// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Definitions.MyBotCollectionDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_BotCollectionDefinition), null)]
  public class MyBotCollectionDefinition : MyDefinitionBase
  {
    public MyDiscreteSampler<MyDefinitionId> Bots;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_BotCollectionDefinition collectionDefinition))
        return;
      List<MyDefinitionId> values = new List<MyDefinitionId>();
      List<float> floatList = new List<float>();
      for (int index = 0; index < collectionDefinition.Bots.Length; ++index)
      {
        MyObjectBuilder_BotCollectionDefinition.BotDefEntry bot = collectionDefinition.Bots[index];
        values.Add((MyDefinitionId) bot.Id);
        floatList.Add(bot.Probability);
      }
      this.Bots = new MyDiscreteSampler<MyDefinitionId>(values, (IEnumerable<float>) floatList);
    }

    private class Sandbox_Game_WorldEnvironment_Definitions_MyBotCollectionDefinition\u003C\u003EActor : IActivator, IActivator<MyBotCollectionDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyBotCollectionDefinition();

      MyBotCollectionDefinition IActivator<MyBotCollectionDefinition>.CreateInstance() => new MyBotCollectionDefinition();
    }
  }
}

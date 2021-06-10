// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyAgentDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_AgentDefinition), null)]
  public class MyAgentDefinition : MyBotDefinition
  {
    public string BotModel;
    public string TargetType;
    public bool InventoryContentGenerated;
    public MyDefinitionId InventoryContainerTypeId;
    public bool RemoveAfterDeath;
    public int RespawnTimeMs;
    public int RemoveTimeMs;
    public string FactionTag;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_AgentDefinition builderAgentDefinition = builder as MyObjectBuilder_AgentDefinition;
      this.BotModel = builderAgentDefinition.BotModel;
      this.TargetType = builderAgentDefinition.TargetType;
      this.InventoryContentGenerated = builderAgentDefinition.InventoryContentGenerated;
      if (builderAgentDefinition.InventoryContainerTypeId.HasValue)
        this.InventoryContainerTypeId = (MyDefinitionId) builderAgentDefinition.InventoryContainerTypeId.Value;
      this.RemoveAfterDeath = builderAgentDefinition.RemoveAfterDeath;
      this.RespawnTimeMs = builderAgentDefinition.RespawnTimeMs;
      this.RemoveTimeMs = builderAgentDefinition.RemoveTimeMs;
      this.FactionTag = builderAgentDefinition.FactionTag;
    }

    public override void AddItems(MyCharacter character)
    {
      MyEntityExtensions.GetInventory(character).Clear(true);
      if (!this.InventoryContentGenerated)
        return;
      MyContainerTypeDefinition containerTypeDefinition = MyDefinitionManager.Static.GetContainerTypeDefinition(this.InventoryContainerTypeId.SubtypeName);
      if (containerTypeDefinition == null)
        return;
      MyEntityExtensions.GetInventory(character).GenerateContent(containerTypeDefinition);
    }

    private class Sandbox_Definitions_MyAgentDefinition\u003C\u003EActor : IActivator, IActivator<MyAgentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAgentDefinition();

      MyAgentDefinition IActivator<MyAgentDefinition>.CreateInstance() => new MyAgentDefinition();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyToolItemDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ToolItemDefinition), null)]
  public class MyToolItemDefinition : MyPhysicalItemDefinition
  {
    public MyVoxelMiningDefinition[] VoxelMinings;
    public List<MyToolActionDefinition> PrimaryActions = new List<MyToolActionDefinition>();
    public List<MyToolActionDefinition> SecondaryActions = new List<MyToolActionDefinition>();
    public float HitDistance;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ToolItemDefinition toolItemDefinition = builder as MyObjectBuilder_ToolItemDefinition;
      if (toolItemDefinition.VoxelMinings != null && toolItemDefinition.VoxelMinings.Length != 0)
      {
        this.VoxelMinings = new MyVoxelMiningDefinition[toolItemDefinition.VoxelMinings.Length];
        for (int index = 0; index < toolItemDefinition.VoxelMinings.Length; ++index)
        {
          this.VoxelMinings[index].MinedOre = toolItemDefinition.VoxelMinings[index].MinedOre;
          this.VoxelMinings[index].HitCount = toolItemDefinition.VoxelMinings[index].HitCount;
          this.VoxelMinings[index].PhysicalItemId = (MyDefinitionId) toolItemDefinition.VoxelMinings[index].PhysicalItemId;
          this.VoxelMinings[index].RemovedRadius = toolItemDefinition.VoxelMinings[index].RemovedRadius;
          this.VoxelMinings[index].OnlyApplyMaterial = toolItemDefinition.VoxelMinings[index].OnlyApplyMaterial;
        }
      }
      this.CopyActions(toolItemDefinition.PrimaryActions, this.PrimaryActions);
      this.CopyActions(toolItemDefinition.SecondaryActions, this.SecondaryActions);
      this.HitDistance = toolItemDefinition.HitDistance;
    }

    private void CopyActions(
      MyObjectBuilder_ToolItemDefinition.MyToolActionDefinition[] sourceActions,
      List<MyToolActionDefinition> targetList)
    {
      if (sourceActions == null || sourceActions.Length == 0)
        return;
      for (int index1 = 0; index1 < sourceActions.Length; ++index1)
      {
        MyToolActionDefinition actionDefinition = new MyToolActionDefinition();
        actionDefinition.Name = MyStringId.GetOrCompute(sourceActions[index1].Name);
        actionDefinition.StartTime = sourceActions[index1].StartTime;
        actionDefinition.EndTime = sourceActions[index1].EndTime;
        actionDefinition.Efficiency = sourceActions[index1].Efficiency;
        actionDefinition.StatsEfficiency = sourceActions[index1].StatsEfficiency;
        actionDefinition.SwingSound = sourceActions[index1].SwingSound;
        actionDefinition.SwingSoundStart = sourceActions[index1].SwingSoundStart;
        actionDefinition.HitStart = sourceActions[index1].HitStart;
        actionDefinition.HitDuration = sourceActions[index1].HitDuration;
        actionDefinition.HitSound = sourceActions[index1].HitSound;
        actionDefinition.CustomShapeRadius = sourceActions[index1].CustomShapeRadius;
        actionDefinition.Crosshair = sourceActions[index1].Crosshair;
        if (sourceActions[index1].HitConditions != null)
        {
          actionDefinition.HitConditions = new MyToolHitCondition[sourceActions[index1].HitConditions.Length];
          for (int index2 = 0; index2 < actionDefinition.HitConditions.Length; ++index2)
          {
            actionDefinition.HitConditions[index2].EntityType = sourceActions[index1].HitConditions[index2].EntityType;
            actionDefinition.HitConditions[index2].Animation = sourceActions[index1].HitConditions[index2].Animation;
            actionDefinition.HitConditions[index2].AnimationTimeScale = sourceActions[index1].HitConditions[index2].AnimationTimeScale;
            actionDefinition.HitConditions[index2].StatsAction = sourceActions[index1].HitConditions[index2].StatsAction;
            actionDefinition.HitConditions[index2].StatsActionIfHit = sourceActions[index1].HitConditions[index2].StatsActionIfHit;
            actionDefinition.HitConditions[index2].StatsModifier = sourceActions[index1].HitConditions[index2].StatsModifier;
            actionDefinition.HitConditions[index2].StatsModifierIfHit = sourceActions[index1].HitConditions[index2].StatsModifierIfHit;
            actionDefinition.HitConditions[index2].Component = sourceActions[index1].HitConditions[index2].Component;
          }
        }
        targetList.Add(actionDefinition);
      }
    }

    private class Sandbox_Definitions_MyToolItemDefinition\u003C\u003EActor : IActivator, IActivator<MyToolItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyToolItemDefinition();

      MyToolItemDefinition IActivator<MyToolItemDefinition>.CreateInstance() => new MyToolItemDefinition();
    }
  }
}

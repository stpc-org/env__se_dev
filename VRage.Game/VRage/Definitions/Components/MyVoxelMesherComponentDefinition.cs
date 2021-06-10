// Decompiled with JetBrains decompiler
// Type: VRage.Definitions.Components.MyVoxelMesherComponentDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders.Definitions.Components;

namespace VRage.Definitions.Components
{
  [MyDefinitionType(typeof (MyObjectBuilder_VoxelMesherComponentDefinition), null)]
  public class MyVoxelMesherComponentDefinition : MyDefinitionBase
  {
    public List<MyObjectBuilder_VoxelPostprocessing> PostProcessingSteps = new List<MyObjectBuilder_VoxelPostprocessing>();

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_VoxelMesherComponentDefinition componentDefinition = (MyObjectBuilder_VoxelMesherComponentDefinition) builder;
      if (componentDefinition.PostprocessingSteps == null)
        return;
      this.PostProcessingSteps = componentDefinition.PostprocessingSteps;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder() => base.GetObjectBuilder();

    private class VRage_Definitions_Components_MyVoxelMesherComponentDefinition\u003C\u003EActor : IActivator, IActivator<MyVoxelMesherComponentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyVoxelMesherComponentDefinition();

      MyVoxelMesherComponentDefinition IActivator<MyVoxelMesherComponentDefinition>.CreateInstance() => new MyVoxelMesherComponentDefinition();
    }
  }
}

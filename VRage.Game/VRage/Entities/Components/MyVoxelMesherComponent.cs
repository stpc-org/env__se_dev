// Decompiled with JetBrains decompiler
// Type: VRage.Entities.Components.MyVoxelMesherComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Definitions.Components;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.Voxels;
using VRage.Voxels.DualContouring;
using VRageMath;

namespace VRage.Entities.Components
{
  public class MyVoxelMesherComponent : MyEntityComponentBase
  {
    private List<MyVoxelPostprocessing> m_postprocessingSteps = new List<MyVoxelPostprocessing>();

    public VRage.Game.Voxels.IMyStorage Storage => this.Entity is IMyVoxelBase entity ? (VRage.Game.Voxels.IMyStorage) entity.Storage : (VRage.Game.Voxels.IMyStorage) null;

    public ListReader<MyVoxelPostprocessing> PostprocessingSteps => (ListReader<MyVoxelPostprocessing>) this.m_postprocessingSteps;

    public void Init(MyVoxelMesherComponentDefinition def)
    {
      if (def == null)
        throw new Exception("Definition {0} is not a valid MyVoxelMesherComponentDefinition.");
      foreach (MyObjectBuilder_VoxelPostprocessing postProcessingStep in def.PostProcessingSteps)
      {
        MyVoxelPostprocessing instance = MyVoxelPostprocessing.Factory.CreateInstance(postProcessingStep.TypeId);
        instance.Init(postProcessingStep);
        this.m_postprocessingSteps.Add(instance);
      }
    }

    public override void OnAddedToScene() => base.OnAddedToScene();

    public override void OnRemovedFromScene() => base.OnRemovedFromScene();

    public override string ComponentTypeDebugString => nameof (MyVoxelMesherComponent);

    public virtual MyMesherResult CalculateMesh(
      int lod,
      Vector3I lodVoxelMin,
      Vector3I lodVoxelMax,
      MyStorageDataTypeFlags properties = MyStorageDataTypeFlags.ContentAndMaterial,
      MyVoxelRequestFlags flags = (MyVoxelRequestFlags) 0,
      VrVoxelMesh target = null)
    {
      return MyDualContouringMesher.Static.Calculate(this, lod, lodVoxelMin, lodVoxelMax, properties, flags, target);
    }

    public virtual string StorageName => (this.Entity as IMyVoxelBase).StorageName;

    private class VRage_Entities_Components_MyVoxelMesherComponent\u003C\u003EActor : IActivator, IActivator<MyVoxelMesherComponent>
    {
      object IActivator.CreateInstance() => (object) new MyVoxelMesherComponent();

      MyVoxelMesherComponent IActivator<MyVoxelMesherComponent>.CreateInstance() => new MyVoxelMesherComponent();
    }
  }
}

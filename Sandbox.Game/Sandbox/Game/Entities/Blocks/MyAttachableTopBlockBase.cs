// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyAttachableTopBlockBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Blocks
{
  public abstract class MyAttachableTopBlockBase : MySyncedBlock, Sandbox.ModAPI.IMyAttachableTopBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock
  {
    private long? m_parentId;
    private MyMechanicalConnectionBlockBase m_parentBlock;

    public Vector3 WheelDummy { get; private set; }

    public MyMechanicalConnectionBlockBase Stator => this.m_parentBlock;

    public virtual void Attach(MyMechanicalConnectionBlockBase parent) => this.m_parentBlock = parent;

    public virtual void Detach(bool isWelding)
    {
      if (isWelding)
        return;
      this.m_parentBlock = (MyMechanicalConnectionBlockBase) null;
    }

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      base.Init(builder, cubeGrid);
      if (builder is MyObjectBuilder_AttachableTopBlockBase attachableTopBlockBase)
      {
        if (!attachableTopBlockBase.YieldLastComponent)
          this.SlimBlock.DisableLastComponentYield();
        if (attachableTopBlockBase.ParentEntityId != 0L)
        {
          MyEntity entity;
          Sandbox.Game.Entities.MyEntities.TryGetEntityById(attachableTopBlockBase.ParentEntityId, out entity);
          if (entity is MyMechanicalConnectionBlockBase connectionBlockBase)
            connectionBlockBase.MarkForReattach();
        }
      }
      this.LoadDummies();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_CubeBlock builderCubeBlock = base.GetObjectBuilderCubeBlock(copy);
      if (!(builderCubeBlock is MyObjectBuilder_AttachableTopBlockBase attachableTopBlockBase))
        return builderCubeBlock;
      attachableTopBlockBase.ParentEntityId = this.m_parentBlock != null ? this.m_parentBlock.EntityId : 0L;
      attachableTopBlockBase.YieldLastComponent = this.SlimBlock.YieldLastComponent;
      return builderCubeBlock;
    }

    private void LoadDummies()
    {
      foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(this.BlockDefinition.Model).Dummies)
      {
        if (dummy.Key.ToLower().Contains("wheel"))
        {
          this.WheelDummy = (Matrix.Normalize(dummy.Value.Matrix) * this.PositionComp.LocalMatrixRef).Translation;
          break;
        }
      }
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      if (this.Stator != null)
        this.Stator.OnTopBlockCubeGridChanged(oldGrid);
      base.OnCubeGridChanged(oldGrid);
    }

    Sandbox.ModAPI.IMyMechanicalConnectionBlock Sandbox.ModAPI.IMyAttachableTopBlock.Base => (Sandbox.ModAPI.IMyMechanicalConnectionBlock) this.Stator;

    bool Sandbox.ModAPI.Ingame.IMyAttachableTopBlock.IsAttached => this.Stator != null;

    Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock Sandbox.ModAPI.Ingame.IMyAttachableTopBlock.Base => (Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock) this.Stator;
  }
}

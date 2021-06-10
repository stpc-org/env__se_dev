// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyRespawnComponent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRageMath;
using VRageRender.Import;

namespace VRage.Game.Components
{
  [MyComponentBuilder(typeof (MyObjectBuilder_RespawnComponent), true)]
  public class MyRespawnComponent : MyEntityRespawnComponentBase
  {
    private static HashSet<MyRespawnComponent> m_respawns = new HashSet<MyRespawnComponent>();

    public static HashSetReader<MyRespawnComponent> GetAllRespawns() => (HashSetReader<MyRespawnComponent>) MyRespawnComponent.m_respawns;

    public MyTerminalBlock Entity => (MyTerminalBlock) base.Entity;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      if (!this.Container.Entity.InScene)
        return;
      MyTerminalBlock entity = this.Entity;
      int num;
      if (entity == null)
      {
        num = 0;
      }
      else
      {
        bool? isPreview = entity.CubeGrid?.IsPreview;
        bool flag = false;
        num = isPreview.GetValueOrDefault() == flag & isPreview.HasValue ? 1 : 0;
      }
      if (num == 0)
        return;
      MyRespawnComponent.m_respawns.Add(this);
    }

    public override void OnBeforeRemovedFromContainer()
    {
      MyRespawnComponent.m_respawns.Remove(this);
      base.OnBeforeRemovedFromContainer();
    }

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      MyTerminalBlock entity = this.Entity;
      int num;
      if (entity == null)
      {
        num = 0;
      }
      else
      {
        bool? isPreview = entity.CubeGrid?.IsPreview;
        bool flag = true;
        num = isPreview.GetValueOrDefault() == flag & isPreview.HasValue ? 1 : 0;
      }
      if (num != 0)
        return;
      MyRespawnComponent.m_respawns.Add(this);
    }

    public override void OnRemovedFromScene()
    {
      MyRespawnComponent.m_respawns.Remove(this);
      base.OnRemovedFromScene();
    }

    public MatrixD GetSpawnPosition()
    {
      MatrixD matrixD = MatrixD.Identity;
      MyModelDummy myModelDummy;
      if (this.Entity.Model.Dummies.TryGetValue("dummy detector_respawn", out myModelDummy))
        matrixD = (MatrixD) ref myModelDummy.Matrix;
      return MatrixD.Multiply(MatrixD.CreateTranslation(matrixD.Translation), this.Entity.WorldMatrix);
    }

    public bool SpawnWithoutOxygen => !(this.Entity is MyMedicalRoom entity) || entity.SpawnWithoutOxygenEnabled;

    public float GetOxygenLevel()
    {
      if (!MySession.Static.Settings.EnableOxygen)
        return 0.0f;
      MyTerminalBlock entity = this.Entity;
      MyCubeGrid cubeGrid = entity.CubeGrid;
      MyGridGasSystem gasSystem = cubeGrid.GridSystems.GasSystem;
      if (gasSystem == null)
        return 0.0f;
      MyOxygenBlock oxygenBlock = gasSystem.GetOxygenBlock(entity.WorldMatrix.Translation);
      return oxygenBlock == null || oxygenBlock.Room == null || !oxygenBlock.Room.IsAirtight ? 0.0f : oxygenBlock.OxygenLevel(cubeGrid.GridSize);
    }

    public bool CanPlayerSpawn(long playerId, bool acceptPublicRespawn)
    {
      MyTerminalBlock entity = this.Entity;
      if (entity.HasPlayerAccess(playerId))
      {
        if (acceptPublicRespawn)
          return true;
        MyIDModule idModule = entity.IDModule;
        switch (MyIDModule.GetRelationPlayerBlock(idModule.Owner, playerId, idModule.ShareMode, defaultShareWithAllRelations: (acceptPublicRespawn ? MyRelationsBetweenPlayerAndBlock.FactionShare : MyRelationsBetweenPlayerAndBlock.Enemies)))
        {
          case MyRelationsBetweenPlayerAndBlock.Owner:
          case MyRelationsBetweenPlayerAndBlock.FactionShare:
            return true;
        }
      }
      return acceptPublicRespawn && entity is MyMedicalRoom myMedicalRoom && myMedicalRoom.SetFactionToSpawnee;
    }

    public override string ComponentTypeDebugString => "Respawn";
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.MyAiTargetBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.EnvironmentItems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.AI
{
  public class MyAiTargetBase
  {
    private const int UNREACHABLE_ENTITY_TIMEOUT = 1500;
    private const int UNREACHABLE_BLOCK_TIMEOUT = 60000;
    private const int UNREACHABLE_CHARACTER_TIMEOUT = 1500;
    protected IMyEntityBot m_user;
    protected MyAgentBot m_bot;
    protected MyEntity m_targetEntity;
    protected Vector3I m_targetCube = Vector3I.Zero;
    protected Vector3D m_targetPosition = Vector3D.Zero;
    protected Vector3I m_targetInVoxelCoord = Vector3I.Zero;
    protected ushort? m_compoundId;
    protected int m_targetTreeId;
    protected Dictionary<MyEntity, int> m_unreachableEntities = new Dictionary<MyEntity, int>();
    protected Dictionary<Tuple<MyEntity, int>, int> m_unreachableTrees = new Dictionary<Tuple<MyEntity, int>, int>();
    protected static List<MyEntity> m_tmpEntities;
    protected static List<Tuple<MyEntity, int>> m_tmpTrees;

    public MyAiTargetEnum TargetType { get; protected set; }

    public bool HasTarget() => (uint) this.TargetType > 0U;

    private void Clear()
    {
      this.TargetType = MyAiTargetEnum.NO_TARGET;
      this.m_targetEntity = (MyEntity) null;
      this.m_targetCube = Vector3I.Zero;
      this.m_targetPosition = Vector3D.Zero;
      this.m_targetInVoxelCoord = Vector3I.Zero;
      this.m_compoundId = new ushort?();
      this.m_targetTreeId = 0;
    }

    private void SetMTargetPosition(Vector3D pos) => this.m_targetPosition = pos;

    public MyCubeGrid TargetGrid => this.m_targetEntity as MyCubeGrid;

    public MyEntity TargetEntity => this.m_targetEntity;

    public Vector3D TargetPosition
    {
      get
      {
        switch (this.TargetType)
        {
          case MyAiTargetEnum.NO_TARGET:
            return Vector3D.Zero;
          case MyAiTargetEnum.GRID:
          case MyAiTargetEnum.CHARACTER:
          case MyAiTargetEnum.ENTITY:
            return this.m_targetEntity.PositionComp.GetPosition();
          case MyAiTargetEnum.CUBE:
          case MyAiTargetEnum.COMPOUND_BLOCK:
            if (!(this.m_targetEntity is MyCubeGrid targetEntity))
              return Vector3D.Zero;
            MySlimBlock cubeBlock = targetEntity.GetCubeBlock(this.m_targetCube);
            return cubeBlock == null ? Vector3D.Zero : targetEntity.GridIntegerToWorld(cubeBlock.Position);
          case MyAiTargetEnum.POSITION:
            return this.m_targetPosition;
          case MyAiTargetEnum.ENVIRONMENT_ITEM:
            return this.m_targetEntity.PositionComp.GetPosition();
          case MyAiTargetEnum.VOXEL:
            return this.m_targetPosition;
          default:
            return Vector3D.Zero;
        }
      }
    }

    public Vector3D TargetCubeWorldPosition
    {
      get
      {
        MySlimBlock cubeBlock = this.GetCubeBlock();
        return cubeBlock?.FatBlock != null ? cubeBlock.FatBlock.PositionComp.WorldAABB.Center : this.TargetGrid.GridIntegerToWorld(this.m_targetCube);
      }
    }

    public bool HasGotoFailed { get; set; }

    public bool IsTargetGridOrBlock(MyAiTargetEnum type) => type == MyAiTargetEnum.CUBE || type == MyAiTargetEnum.GRID;

    public virtual bool IsMemoryTargetValid(MyBBMemoryTarget targetMemory)
    {
      if (targetMemory == null)
        return false;
      switch (targetMemory.TargetType)
      {
        case MyAiTargetEnum.GRID:
        case MyAiTargetEnum.ENTITY:
          MyEntity entity1;
          return Sandbox.Game.Entities.MyEntities.TryGetEntityById(targetMemory.EntityId.Value, out entity1) && this.IsEntityReachable(entity1);
        case MyAiTargetEnum.CUBE:
        case MyAiTargetEnum.COMPOUND_BLOCK:
          MyCubeGrid entity2;
          if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(targetMemory.EntityId.Value, out entity2))
            return false;
          MySlimBlock cubeBlock = entity2.GetCubeBlock(targetMemory.BlockPosition);
          if (cubeBlock == null)
            return false;
          return cubeBlock.FatBlock != null ? this.IsEntityReachable((MyEntity) cubeBlock.FatBlock) : this.IsEntityReachable((MyEntity) entity2);
        case MyAiTargetEnum.CHARACTER:
          MyCharacter entity3;
          return Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCharacter>(targetMemory.EntityId.Value, out entity3) && this.IsEntityReachable((MyEntity) entity3) && !entity3.IsDead;
        case MyAiTargetEnum.ENVIRONMENT_ITEM:
        case MyAiTargetEnum.VOXEL:
          return true;
        default:
          return false;
      }
    }

    public Vector3D? GetMemoryTargetPosition(MyBBMemoryTarget targetMemory)
    {
      if (targetMemory == null)
        return new Vector3D?();
      switch (targetMemory.TargetType)
      {
        case MyAiTargetEnum.GRID:
        case MyAiTargetEnum.CHARACTER:
        case MyAiTargetEnum.ENTITY:
          MyCharacter entity1 = (MyCharacter) null;
          return Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCharacter>(targetMemory.EntityId.Value, out entity1) ? new Vector3D?(entity1.PositionComp.GetPosition()) : new Vector3D?();
        case MyAiTargetEnum.CUBE:
        case MyAiTargetEnum.COMPOUND_BLOCK:
          MyCubeGrid entity2 = (MyCubeGrid) null;
          return Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(targetMemory.EntityId.Value, out entity2) && entity2.CubeExists(targetMemory.BlockPosition) ? new Vector3D?(entity2.GridIntegerToWorld(targetMemory.BlockPosition)) : new Vector3D?();
        case MyAiTargetEnum.POSITION:
        case MyAiTargetEnum.ENVIRONMENT_ITEM:
        case MyAiTargetEnum.VOXEL:
          return new Vector3D?(this.m_targetPosition);
        default:
          return new Vector3D?();
      }
    }

    public virtual bool IsTargetValid()
    {
      switch (this.TargetType)
      {
        case MyAiTargetEnum.GRID:
        case MyAiTargetEnum.ENTITY:
          return this.IsEntityReachable(this.m_targetEntity);
        case MyAiTargetEnum.CUBE:
        case MyAiTargetEnum.COMPOUND_BLOCK:
          MySlimBlock mySlimBlock = this.m_targetEntity is MyCubeGrid targetEntity ? targetEntity.GetCubeBlock(this.m_targetCube) : (MySlimBlock) null;
          if (mySlimBlock == null)
            return false;
          return mySlimBlock.FatBlock != null ? this.IsEntityReachable((MyEntity) mySlimBlock.FatBlock) : this.IsEntityReachable((MyEntity) targetEntity);
        case MyAiTargetEnum.CHARACTER:
          return this.m_targetEntity is MyCharacter targetEntity && this.IsEntityReachable((MyEntity) targetEntity);
        case MyAiTargetEnum.ENVIRONMENT_ITEM:
        case MyAiTargetEnum.VOXEL:
          return true;
        default:
          return false;
      }
    }

    public MyAiTargetBase(IMyEntityBot bot)
    {
      this.m_user = bot;
      this.m_bot = bot as MyAgentBot;
      this.TargetType = MyAiTargetEnum.NO_TARGET;
      MyAiTargetManager.AddAiTarget(this);
    }

    public virtual void Init(MyObjectBuilder_AiTarget builder)
    {
      this.TargetType = builder.CurrentTarget;
      this.m_targetEntity = (MyEntity) null;
      if (builder.EntityId.HasValue)
      {
        if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(builder.EntityId.Value, out this.m_targetEntity))
          this.TargetType = MyAiTargetEnum.NO_TARGET;
      }
      else
        this.TargetType = MyAiTargetEnum.NO_TARGET;
      this.m_targetCube = builder.TargetCube;
      this.SetMTargetPosition(builder.TargetPosition);
      this.m_compoundId = builder.CompoundId;
      if (builder.UnreachableEntities == null)
        return;
      foreach (MyObjectBuilder_AiTarget.UnreachableEntitiesData unreachableEntity in builder.UnreachableEntities)
      {
        MyEntity entity;
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityById(unreachableEntity.UnreachableEntityId, out entity))
          this.m_unreachableEntities.Add(entity, MySandboxGame.TotalGamePlayTimeInMilliseconds + unreachableEntity.Timeout);
      }
    }

    public virtual MyObjectBuilder_AiTarget GetObjectBuilder()
    {
      MyObjectBuilder_AiTarget newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_AiTarget>();
      newObject.EntityId = this.m_targetEntity?.EntityId;
      newObject.CurrentTarget = this.TargetType;
      newObject.TargetCube = this.m_targetCube;
      newObject.TargetPosition = this.m_targetPosition;
      newObject.CompoundId = this.m_compoundId;
      newObject.UnreachableEntities = new List<MyObjectBuilder_AiTarget.UnreachableEntitiesData>();
      foreach (KeyValuePair<MyEntity, int> unreachableEntity in this.m_unreachableEntities)
      {
        MyObjectBuilder_AiTarget.UnreachableEntitiesData unreachableEntitiesData = new MyObjectBuilder_AiTarget.UnreachableEntitiesData()
        {
          UnreachableEntityId = unreachableEntity.Key.EntityId,
          Timeout = unreachableEntity.Value - MySandboxGame.TotalGamePlayTimeInMilliseconds
        };
        newObject.UnreachableEntities.Add(unreachableEntitiesData);
      }
      return newObject;
    }

    public virtual void UnsetTarget()
    {
      switch (this.TargetType)
      {
        case MyAiTargetEnum.NO_TARGET:
        case MyAiTargetEnum.GRID:
        case MyAiTargetEnum.CUBE:
        case MyAiTargetEnum.CHARACTER:
        case MyAiTargetEnum.ENTITY:
        case MyAiTargetEnum.ENVIRONMENT_ITEM:
        case MyAiTargetEnum.VOXEL:
          if (this.m_targetEntity != null)
          {
            this.UnsetTargetEntity();
            break;
          }
          break;
      }
      this.Clear();
    }

    public virtual void DebugDraw()
    {
    }

    public virtual void DrawLineToTarget(Vector3D from)
    {
    }

    public virtual void Cleanup() => MyAiTargetManager.RemoveAiTarget(this);

    public virtual void Update()
    {
      using (MyUtils.ReuseCollection<MyEntity>(ref MyAiTargetBase.m_tmpEntities))
      {
        foreach (KeyValuePair<MyEntity, int> unreachableEntity in this.m_unreachableEntities)
        {
          if (unreachableEntity.Value - MySandboxGame.TotalGamePlayTimeInMilliseconds < 0)
            MyAiTargetBase.m_tmpEntities.Add(unreachableEntity.Key);
        }
        foreach (MyEntity tmpEntity in MyAiTargetBase.m_tmpEntities)
          this.RemoveUnreachableEntity(tmpEntity);
      }
      using (MyUtils.ReuseCollection<Tuple<MyEntity, int>>(ref MyAiTargetBase.m_tmpTrees))
      {
        foreach (KeyValuePair<Tuple<MyEntity, int>, int> unreachableTree in this.m_unreachableTrees)
        {
          if (unreachableTree.Value - MySandboxGame.TotalGamePlayTimeInMilliseconds < 0)
            MyAiTargetBase.m_tmpTrees.Add(unreachableTree.Key);
        }
        foreach (Tuple<MyEntity, int> tmpTree in MyAiTargetBase.m_tmpTrees)
          this.RemoveUnreachableTree(tmpTree);
      }
    }

    private void AddUnreachableEntity(MyEntity entity, int timeout)
    {
      this.m_unreachableEntities[entity] = MySandboxGame.TotalGamePlayTimeInMilliseconds + timeout;
      entity.OnClosing -= new Action<MyEntity>(this.RemoveUnreachableEntity);
      entity.OnClosing += new Action<MyEntity>(this.RemoveUnreachableEntity);
    }

    private void AddUnreachableTree(MyEntity entity, int treeId, int timeout)
    {
      this.m_unreachableTrees[new Tuple<MyEntity, int>(entity, treeId)] = MySandboxGame.TotalGamePlayTimeInMilliseconds + timeout;
      entity.OnClosing -= new Action<MyEntity>(this.RemoveUnreachableTrees);
      entity.OnClosing += new Action<MyEntity>(this.RemoveUnreachableTrees);
    }

    public bool IsEntityReachable(MyEntity entity)
    {
      if (entity == null)
        return false;
      bool flag = true;
      if (entity.Parent != null)
        flag &= this.IsEntityReachable(entity.Parent);
      return flag && !this.m_unreachableEntities.ContainsKey(entity);
    }

    public bool IsTreeReachable(MyEntity entity, int treeId)
    {
      if (entity == null)
        return false;
      bool flag = true;
      if (entity.Parent != null)
        flag &= this.IsEntityReachable(entity.Parent);
      return flag && !this.m_unreachableTrees.ContainsKey(new Tuple<MyEntity, int>(entity, treeId));
    }

    private void RemoveUnreachableEntity(MyEntity entity)
    {
      entity.OnClosing -= new Action<MyEntity>(this.RemoveUnreachableEntity);
      this.m_unreachableEntities.Remove(entity);
    }

    private void RemoveUnreachableTree(Tuple<MyEntity, int> tree) => this.m_unreachableTrees.Remove(tree);

    private void RemoveUnreachableTrees(MyEntity entity)
    {
      entity.OnClosing -= new Action<MyEntity>(this.RemoveUnreachableTrees);
      using (MyUtils.ReuseCollection<Tuple<MyEntity, int>>(ref MyAiTargetBase.m_tmpTrees))
      {
        foreach (Tuple<MyEntity, int> key in this.m_unreachableTrees.Keys)
        {
          if (key.Item1 == entity)
            MyAiTargetBase.m_tmpTrees.Add(key);
        }
        foreach (Tuple<MyEntity, int> tmpTree in MyAiTargetBase.m_tmpTrees)
          this.RemoveUnreachableTree(tmpTree);
      }
    }

    public bool PositionIsNearTarget(Vector3D position, float radius)
    {
      if (!this.HasTarget())
        return false;
      Vector3D targetPosition;
      float radius1;
      this.GetTargetPosition(position, out targetPosition, out radius1);
      return Vector3D.Distance(position, targetPosition) <= (double) radius + (double) radius1;
    }

    public void ClearUnreachableEntities() => this.m_unreachableEntities.Clear();

    public void GotoTarget()
    {
      if (!this.HasTarget())
        return;
      if (this.TargetType == MyAiTargetEnum.POSITION || this.TargetType == MyAiTargetEnum.VOXEL)
      {
        this.m_bot.Navigation.Goto(this.m_targetPosition, relativeEntity: this.m_targetEntity);
      }
      else
      {
        Vector3D targetPosition;
        float radius;
        this.GetTargetPosition(this.m_bot.Navigation.PositionAndOrientation.Translation, out targetPosition, out radius);
        this.m_bot.Navigation.Goto(targetPosition, radius, this.m_targetEntity);
      }
    }

    public void GotoTargetNoPath(float radius, bool resetStuckDetection = true)
    {
      if (!this.HasTarget())
        return;
      if (this.TargetType == MyAiTargetEnum.POSITION || this.TargetType == MyAiTargetEnum.VOXEL)
      {
        this.m_bot.Navigation.GotoNoPath(this.m_targetPosition, radius);
      }
      else
      {
        Vector3D targetPosition;
        float radius1;
        this.GetTargetPosition(this.m_bot.Navigation.PositionAndOrientation.Translation, out targetPosition, out radius1);
        this.m_bot.Navigation.GotoNoPath(targetPosition, radius + radius1, resetStuckDetection: resetStuckDetection);
      }
    }

    public void GetTargetPosition(
      Vector3D startingPosition,
      out Vector3D targetPosition,
      out float radius)
    {
      targetPosition = new Vector3D();
      radius = 0.0f;
      if (!this.HasTarget())
        return;
      if (this.TargetType == MyAiTargetEnum.POSITION)
      {
        targetPosition = this.m_targetPosition;
      }
      else
      {
        Vector3D vector3D = this.m_targetEntity.PositionComp.GetPosition();
        radius = 0.75f;
        if (this.TargetType == MyAiTargetEnum.CUBE)
        {
          Vector3D projectedPosition = this.GetLocalCubeProjectedPosition(ref startingPosition);
          radius = (float) projectedPosition.Length() * 0.3f;
          vector3D = this.TargetCubeWorldPosition + projectedPosition;
        }
        else if (this.TargetType == MyAiTargetEnum.CHARACTER)
        {
          radius = 0.65f;
          vector3D = (this.m_targetEntity as MyCharacter).PositionComp.WorldVolume.Center;
        }
        else if (this.TargetType == MyAiTargetEnum.ENVIRONMENT_ITEM)
        {
          vector3D = this.m_targetPosition;
          radius = 0.75f;
        }
        else if (this.TargetType == MyAiTargetEnum.VOXEL)
          vector3D = this.m_targetPosition;
        else if (this.TargetType == MyAiTargetEnum.ENTITY)
        {
          if (this.m_targetPosition != Vector3D.Zero && this.m_targetEntity is MyFracturedPiece)
            vector3D = this.m_targetPosition;
          radius = this.m_targetEntity.PositionComp.LocalAABB.HalfExtents.Length();
        }
        targetPosition = vector3D;
      }
    }

    public Vector3D GetTargetPosition(Vector3D startingPosition)
    {
      Vector3D targetPosition;
      this.GetTargetPosition(startingPosition, out targetPosition, out float _);
      return targetPosition;
    }

    public void AimAtTarget()
    {
      if (!this.HasTarget())
        return;
      if (this.TargetType == MyAiTargetEnum.POSITION || this.TargetType == MyAiTargetEnum.VOXEL)
      {
        this.m_bot.Navigation.AimAt((MyEntity) null, new Vector3D?(this.m_targetPosition));
      }
      else
      {
        this.SetMTargetPosition(this.GetAimAtPosition(this.m_bot.Navigation.AimingPositionAndOrientation.Translation));
        this.m_bot.Navigation.AimAt(this.m_targetEntity, new Vector3D?(this.m_targetPosition));
      }
    }

    public void GotoFailed()
    {
      this.HasGotoFailed = true;
      if (this.TargetType == MyAiTargetEnum.CHARACTER)
        this.AddUnreachableEntity(this.m_targetEntity, 1500);
      else if (this.TargetType == MyAiTargetEnum.CUBE)
      {
        MyEntity targetEntity = this.m_targetEntity;
        MySlimBlock cubeBlock = this.GetCubeBlock();
        if (cubeBlock?.FatBlock != null)
          this.AddUnreachableEntity((MyEntity) cubeBlock.FatBlock, 60000);
      }
      else if (this.m_targetEntity is MyTrees)
        this.AddUnreachableTree(this.m_targetEntity, this.m_targetTreeId, 1500);
      else if (this.m_targetEntity != null && this.TargetType != MyAiTargetEnum.VOXEL)
        this.AddUnreachableEntity(this.m_targetEntity, 1500);
      this.UnsetTarget();
    }

    public virtual bool SetTargetFromMemory(MyBBMemoryTarget memoryTarget)
    {
      if (memoryTarget.TargetType == MyAiTargetEnum.POSITION)
      {
        if (!memoryTarget.Position.HasValue)
          return false;
        this.SetTargetPosition(memoryTarget.Position.Value);
        return true;
      }
      if (memoryTarget.TargetType == MyAiTargetEnum.ENVIRONMENT_ITEM)
      {
        if (!memoryTarget.TreeId.HasValue)
          return false;
        this.SetTargetTree(ref new MyEnvironmentItems.ItemInfo()
        {
          LocalId = memoryTarget.TreeId.Value,
          Transform = {
            Position = memoryTarget.Position.Value
          }
        }, memoryTarget.EntityId.Value);
        return true;
      }
      if (memoryTarget.TargetType != MyAiTargetEnum.NO_TARGET)
      {
        if (!memoryTarget.EntityId.HasValue)
          return false;
        MyEntity entity;
        if (Sandbox.Game.Entities.MyEntities.TryGetEntityById(memoryTarget.EntityId.Value, out entity))
        {
          if (memoryTarget.TargetType == MyAiTargetEnum.CUBE || memoryTarget.TargetType == MyAiTargetEnum.COMPOUND_BLOCK)
          {
            MySlimBlock slimBlock = (entity as MyCubeGrid).GetCubeBlock(memoryTarget.BlockPosition);
            if (slimBlock == null)
              return false;
            if (memoryTarget.TargetType == MyAiTargetEnum.COMPOUND_BLOCK)
            {
              MySlimBlock block = (slimBlock.FatBlock as MyCompoundCubeBlock).GetBlock(memoryTarget.CompoundId.Value);
              if (block == null)
                return false;
              slimBlock = block;
              this.m_compoundId = memoryTarget.CompoundId;
            }
            this.SetTargetBlock(slimBlock);
          }
          else if (memoryTarget.TargetType == MyAiTargetEnum.ENTITY)
          {
            if (memoryTarget.Position.HasValue && entity is MyFracturedPiece)
              this.SetMTargetPosition(memoryTarget.Position.Value);
            else
              this.SetMTargetPosition(entity.PositionComp.GetPosition());
            this.SetTargetEntity(entity);
            this.m_targetEntity = entity;
          }
          else if (memoryTarget.TargetType == MyAiTargetEnum.VOXEL)
          {
            MyVoxelMap voxelMap = entity as MyVoxelMap;
            if (!memoryTarget.Position.HasValue || voxelMap == null)
              return false;
            this.SetTargetVoxel(memoryTarget.Position.Value, voxelMap);
            this.m_targetEntity = (MyEntity) voxelMap;
          }
          else
            this.SetTargetEntity(entity);
          return true;
        }
        this.UnsetTarget();
        return false;
      }
      if (memoryTarget.TargetType == MyAiTargetEnum.NO_TARGET)
      {
        this.UnsetTarget();
        return true;
      }
      this.UnsetTarget();
      return false;
    }

    protected virtual void SetTargetEntity(MyEntity entity)
    {
      if (entity is MyCubeBlock)
      {
        this.SetTargetBlock((entity as MyCubeBlock).SlimBlock);
      }
      else
      {
        if (this.m_targetEntity != null)
          this.UnsetTargetEntity();
        this.m_targetEntity = entity;
        switch (entity)
        {
          case MyCubeGrid _:
            (entity as MyCubeGrid).OnBlockRemoved += new Action<MySlimBlock>(this.BlockRemoved);
            this.TargetType = MyAiTargetEnum.GRID;
            break;
          case MyCharacter _:
            this.TargetType = MyAiTargetEnum.CHARACTER;
            break;
          case MyVoxelBase _:
            this.TargetType = MyAiTargetEnum.VOXEL;
            break;
          case MyEnvironmentItems _:
            this.TargetType = MyAiTargetEnum.ENVIRONMENT_ITEM;
            break;
          case null:
            break;
          default:
            this.TargetType = MyAiTargetEnum.ENTITY;
            break;
        }
      }
    }

    protected virtual void UnsetTargetEntity()
    {
      if (this.IsTargetGridOrBlock(this.TargetType) && this.m_targetEntity is MyCubeGrid)
        (this.m_targetEntity as MyCubeGrid).OnBlockRemoved -= new Action<MySlimBlock>(this.BlockRemoved);
      this.m_compoundId = new ushort?();
      this.m_targetEntity = (MyEntity) null;
      this.TargetType = MyAiTargetEnum.NO_TARGET;
    }

    private void BlockRemoved(MySlimBlock block)
    {
      MyCubeGrid targetGrid = this.TargetGrid;
      if (this.GetCubeBlock() != null)
        return;
      this.UnsetTargetEntity();
    }

    public void SetTargetBlock(MySlimBlock slimBlock, ushort? compoundId = null)
    {
      if (this.m_targetEntity != slimBlock.CubeGrid)
        this.SetTargetEntity((MyEntity) slimBlock.CubeGrid);
      this.m_targetCube = slimBlock.Position;
      this.TargetType = MyAiTargetEnum.CUBE;
    }

    public MySlimBlock GetTargetBlock()
    {
      if (this.TargetType != MyAiTargetEnum.CUBE)
        return (MySlimBlock) null;
      return this.TargetGrid == null ? (MySlimBlock) null : this.GetCubeBlock();
    }

    public void SetTargetTree(ref MyEnvironmentItems.ItemInfo targetTree, long treesId)
    {
      MyEntity entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(treesId, out entity))
        return;
      this.UnsetTarget();
      this.SetMTargetPosition(targetTree.Transform.Position);
      this.m_targetEntity = entity;
      this.m_targetTreeId = targetTree.LocalId;
      this.SetTargetEntity(entity);
    }

    public void SetTargetPosition(Vector3D pos)
    {
      this.UnsetTarget();
      this.SetMTargetPosition(pos);
      this.TargetType = MyAiTargetEnum.POSITION;
    }

    public void SetTargetVoxel(Vector3D pos, MyVoxelMap voxelMap)
    {
      this.UnsetTarget();
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(voxelMap.PositionLeftBottomCorner, ref pos, out this.m_targetInVoxelCoord);
      this.SetMTargetPosition(pos);
      this.TargetType = MyAiTargetEnum.VOXEL;
    }

    protected Vector3D GetLocalCubeProjectedPosition(ref Vector3D toProject)
    {
      this.GetCubeBlock();
      Vector3D vector3D = Vector3D.Transform(toProject, this.TargetGrid.PositionComp.WorldMatrixNormalizedInv) - ((Vector3) this.m_targetCube + new Vector3(0.5f)) * this.TargetGrid.GridSize;
      float num = Math.Abs(vector3D.Y) <= Math.Abs(vector3D.Z) ? (Math.Abs(vector3D.Z) <= Math.Abs(vector3D.X) ? 1f / (float) Math.Abs(vector3D.X) : 1f / (float) Math.Abs(vector3D.Z)) : (Math.Abs(vector3D.Y) <= Math.Abs(vector3D.X) ? 1f / (float) Math.Abs(vector3D.X) : 1f / (float) Math.Abs(vector3D.Y));
      return vector3D * (double) num * ((double) this.TargetGrid.GridSize * 0.5);
    }

    public Vector3D GetAimAtPosition(Vector3D startingPosition)
    {
      if (!this.HasTarget())
        return Vector3D.Zero;
      if (this.TargetType == MyAiTargetEnum.POSITION || this.TargetType == MyAiTargetEnum.ENVIRONMENT_ITEM)
        return this.m_targetPosition;
      Vector3D vector3D = this.m_targetEntity.PositionComp.GetPosition();
      if (this.TargetType == MyAiTargetEnum.CUBE)
      {
        this.GetLocalCubeProjectedPosition(ref startingPosition);
        vector3D = this.TargetCubeWorldPosition;
      }
      else if (this.TargetType == MyAiTargetEnum.CHARACTER)
      {
        MyPositionComponentBase positionComp = (this.m_targetEntity as MyCharacter).PositionComp;
        vector3D = Vector3D.Transform(positionComp.LocalVolume.Center, positionComp.WorldMatrixRef);
      }
      else if (this.TargetType == MyAiTargetEnum.VOXEL)
        vector3D = this.m_targetPosition;
      else if (this.TargetType == MyAiTargetEnum.ENTITY && this.m_targetPosition != Vector3D.Zero && this.m_targetEntity is MyFracturedPiece)
        vector3D = this.m_targetPosition;
      return vector3D;
    }

    public virtual bool GetRandomDirectedPosition(
      Vector3D initPosition,
      Vector3D direction,
      out Vector3D outPosition)
    {
      outPosition = MySession.Static.LocalCharacter.PositionComp.GetPosition();
      return true;
    }

    protected MySlimBlock GetCubeBlock() => this.m_compoundId.HasValue ? (!(this.TargetGrid.GetCubeBlock(this.m_targetCube)?.FatBlock is MyCompoundCubeBlock fatBlock) ? (MySlimBlock) null : fatBlock.GetBlock(this.m_compoundId.Value)) : this.TargetGrid?.GetCubeBlock(this.m_targetCube);
  }
}

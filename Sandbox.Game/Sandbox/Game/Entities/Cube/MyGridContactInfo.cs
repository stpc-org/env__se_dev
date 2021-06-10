// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyGridContactInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Voxels;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public struct MyGridContactInfo
  {
    public HkContactPointEvent Event;
    public readonly Vector3D ContactPosition;
    public MyCubeGrid m_currentEntity;
    public MyEntity m_collidingEntity;
    private MySlimBlock m_currentBlock;
    private MySlimBlock m_otherBlock;
    private bool m_voxelSurfaceMaterialInitialized;
    private MyVoxelMaterialDefinition m_voxelSurfaceMaterial;
    public float ImpulseMultiplier;

    public MyCubeGrid CurrentEntity => this.m_currentEntity;

    public MyEntity CollidingEntity => this.m_collidingEntity;

    public MySlimBlock OtherBlock => this.m_otherBlock;

    public MyVoxelMaterialDefinition VoxelSurfaceMaterial
    {
      get
      {
        if (!this.m_voxelSurfaceMaterialInitialized)
        {
          this.ReadVoxelSurfaceMaterial();
          this.m_voxelSurfaceMaterialInitialized = true;
        }
        return this.m_voxelSurfaceMaterial;
      }
    }

    private MyGridContactInfo.ContactFlags Flags
    {
      get => (MyGridContactInfo.ContactFlags) this.Event.ContactProperties.UserData.AsUint;
      set => this.Event.ContactProperties.UserData = HkContactUserData.UInt((uint) value);
    }

    public bool EnableDeformation
    {
      get => (uint) (this.Flags & MyGridContactInfo.ContactFlags.Deformation) > 0U;
      set => this.SetFlag(MyGridContactInfo.ContactFlags.Deformation, value);
    }

    public bool RubberDeformation
    {
      get => (uint) (this.Flags & MyGridContactInfo.ContactFlags.RubberDeformation) > 0U;
      set => this.SetFlag(MyGridContactInfo.ContactFlags.RubberDeformation, value);
    }

    public bool EnableParticles
    {
      get => (uint) (this.Flags & MyGridContactInfo.ContactFlags.Particles) > 0U;
      set => this.SetFlag(MyGridContactInfo.ContactFlags.Particles, value);
    }

    public MyGridContactInfo(ref HkContactPointEvent evnt, MyCubeGrid grid)
      : this(ref evnt, grid, evnt.GetOtherEntity((IMyEntity) grid) as MyEntity)
    {
    }

    public MyGridContactInfo(
      ref HkContactPointEvent evnt,
      MyCubeGrid grid,
      MyEntity collidingEntity)
    {
      this.Event = evnt;
      this.ContactPosition = grid.Physics.ClusterToWorld(evnt.ContactPoint.Position);
      this.m_currentEntity = grid;
      this.m_collidingEntity = collidingEntity;
      this.m_currentBlock = (MySlimBlock) null;
      this.m_otherBlock = (MySlimBlock) null;
      this.ImpulseMultiplier = 1f;
      this.m_voxelSurfaceMaterial = (MyVoxelMaterialDefinition) null;
      this.m_voxelSurfaceMaterialInitialized = false;
    }

    public bool IsKnown => (uint) (this.Flags & MyGridContactInfo.ContactFlags.Known) > 0U;

    public void HandleEvents()
    {
      if ((this.Flags & MyGridContactInfo.ContactFlags.Known) != (MyGridContactInfo.ContactFlags) 0)
        return;
      this.Flags |= MyGridContactInfo.ContactFlags.Known | MyGridContactInfo.ContactFlags.Deformation | MyGridContactInfo.ContactFlags.Particles;
      this.m_currentBlock = MyGridContactInfo.GetContactBlock(this.CurrentEntity, this.ContactPosition, this.Event.ContactPoint.NormalAndDistance.W);
      if (this.CollidingEntity is MyCubeGrid collidingEntity)
      {
        Vector3D contactPosition = this.ContactPosition;
        double w = (double) this.Event.ContactPoint.NormalAndDistance.W;
        this.m_otherBlock = MyGridContactInfo.GetContactBlock(collidingEntity, contactPosition, (float) w);
      }
      if (this.m_currentBlock != null && this.m_currentBlock.FatBlock != null)
      {
        this.m_currentBlock.FatBlock.ContactPointCallback(ref this);
        this.ImpulseMultiplier *= this.m_currentBlock.BlockDefinition.PhysicalMaterial.CollisionMultiplier;
      }
      if (this.m_otherBlock == null || this.m_otherBlock.FatBlock == null)
        return;
      this.SwapEntities();
      this.Event.ContactPoint.Flip();
      this.ImpulseMultiplier *= this.m_currentBlock.BlockDefinition.PhysicalMaterial.CollisionMultiplier;
      this.m_currentBlock.FatBlock.ContactPointCallback(ref this);
      this.SwapEntities();
      this.Event.ContactPoint.Flip();
    }

    private void SetFlag(MyGridContactInfo.ContactFlags flag, bool value) => this.Flags = value ? this.Flags | flag : this.Flags & ~flag;

    private void SwapEntities()
    {
      MyCubeGrid currentEntity = this.m_currentEntity;
      this.m_currentEntity = (MyCubeGrid) this.m_collidingEntity;
      this.m_collidingEntity = (MyEntity) currentEntity;
      MySlimBlock currentBlock = this.m_currentBlock;
      this.m_currentBlock = this.m_otherBlock;
      this.m_otherBlock = currentBlock;
    }

    private static MySlimBlock GetContactBlock(
      MyCubeGrid grid,
      Vector3D worldPosition,
      float graceDistance)
    {
      HashSet<MySlimBlock> cubeBlocks = grid.CubeBlocks;
      if (cubeBlocks.Count == 1)
        return cubeBlocks.FirstElement<MySlimBlock>();
      MatrixD matrix = grid.PositionComp.WorldMatrixNormalizedInv;
      Vector3D result;
      Vector3D.Transform(ref worldPosition, ref matrix, out result);
      MySlimBlock mySlimBlock1 = (MySlimBlock) null;
      float num1 = float.MaxValue;
      if (cubeBlocks.Count < 10)
      {
        foreach (MySlimBlock mySlimBlock2 in cubeBlocks)
        {
          float num2 = (float) (mySlimBlock2.Position * grid.GridSize - result).LengthSquared();
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            mySlimBlock1 = mySlimBlock2;
          }
        }
        return mySlimBlock1;
      }
      bool flag = false;
      Vector3 linearVelocity = grid.Physics.LinearVelocity;
      float num3 = MyGridPhysics.ShipMaxLinearVelocity();
      if ((double) linearVelocity.LengthSquared() > (double) num3 * (double) num3 * 100.0)
      {
        flag = true;
        linearVelocity /= linearVelocity.Length() * num3;
      }
      graceDistance = Math.Max(Math.Abs(graceDistance), grid.GridSize * 0.2f);
      ++graceDistance;
      Vector3D vector3D = Vector3D.TransformNormal(linearVelocity * 0.01666667f, matrix);
      Vector3I vector3I1 = Vector3I.Round((result - (double) graceDistance - vector3D) / (double) grid.GridSize);
      Vector3I vector3I2 = Vector3I.Round((result + graceDistance + vector3D) / (double) grid.GridSize);
      Vector3I vector3I3 = Vector3I.Round((result + graceDistance - vector3D) / (double) grid.GridSize);
      Vector3I vector3I4 = Vector3I.Round((result - (double) graceDistance + vector3D) / (double) grid.GridSize);
      Vector3I vector3I5 = Vector3I.Min(Vector3I.Min(Vector3I.Min(vector3I1, vector3I2), vector3I3), vector3I4);
      Vector3I vector3I6 = Vector3I.Max(Vector3I.Max(Vector3I.Max(vector3I1, vector3I2), vector3I3), vector3I4);
      Vector3I pos;
      for (pos.X = vector3I5.X; pos.X <= vector3I6.X; ++pos.X)
      {
        for (pos.Y = vector3I5.Y; pos.Y <= vector3I6.Y; ++pos.Y)
        {
          for (pos.Z = vector3I5.Z; pos.Z <= vector3I6.Z; ++pos.Z)
          {
            MySlimBlock cubeBlock = grid.GetCubeBlock(pos);
            if (cubeBlock != null)
            {
              float num2 = (float) (pos * grid.GridSize - result).LengthSquared();
              if ((double) num2 < (double) num1)
              {
                num1 = num2;
                mySlimBlock1 = cubeBlock;
                if (flag)
                  return mySlimBlock1;
              }
            }
          }
        }
      }
      return mySlimBlock1;
    }

    private void ReadVoxelSurfaceMaterial()
    {
      if (!(this.m_collidingEntity.Physics is MyVoxelPhysicsBody physics))
        return;
      int bodyIndex = this.Event.GetPhysicsBody(0) == physics ? 0 : 1;
      uint triangleMaterial = physics.GetHitTriangleMaterial(ref this.Event, bodyIndex);
      if (triangleMaterial == uint.MaxValue)
        return;
      this.m_voxelSurfaceMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition((byte) triangleMaterial);
    }

    [System.Flags]
    public enum ContactFlags
    {
      Known = 1,
      Deformation = 8,
      Particles = 16, // 0x00000010
      RubberDeformation = 32, // 0x00000020
      PredictedCollision = 64, // 0x00000040
      PredictedCollision_Disabled = 128, // 0x00000080
    }
  }
}

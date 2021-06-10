// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyVoxelPhysics
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using VRage;
using VRage.Game.Components;
using VRage.Game.Voxels;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities
{
  internal class MyVoxelPhysics : MyVoxelBase
  {
    private MyPlanet m_parent;

    internal MyVoxelPhysicsBody Physics
    {
      get => base.Physics as MyVoxelPhysicsBody;
      set => this.Physics = (MyPhysicsComponentBase) value;
    }

    public override MyVoxelBase RootVoxel => (MyVoxelBase) this.m_parent;

    public bool Valid { get; set; }

    public MyVoxelPhysics() => this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentVoxelMap((MyVoxelBase) this));

    public override void Init(MyObjectBuilder_EntityBase builder, IMyStorage storage)
    {
    }

    public void Init(
      IMyStorage storage,
      Vector3D positionMinCorner,
      Vector3I storageMin,
      Vector3I storageMax,
      MyPlanet parent)
    {
      this.PositionLeftBottomCorner = positionMinCorner;
      this.m_storageMax = storageMax;
      this.m_storageMin = storageMin;
      this.m_storage = storage;
      this.SizeInMetres = this.Size * 1f;
      this.SizeInMetresHalf = this.SizeInMetres / 2f;
      MatrixD translation = MatrixD.CreateTranslation(positionMinCorner + this.SizeInMetresHalf);
      this.Init(storage, translation, storageMin, storageMax, parent);
    }

    public void Init(
      IMyStorage storage,
      MatrixD worldMatrix,
      Vector3I storageMin,
      Vector3I storageMax,
      MyPlanet parent)
    {
      this.m_parent = parent;
      this.EntityId = MyEntityIdentifier.ConstructId(MyEntityIdentifier.ID_OBJECT_TYPE.VOXEL_PHYSICS, ((((long) storageMin.X * 397L ^ (long) storageMin.Y) * 397L ^ (long) storageMin.Z) * 397L ^ parent.EntityId) & 72057594037927935L);
      this.Init((MyObjectBuilder_EntityBase) null);
      this.InitVoxelMap(worldMatrix, this.Size, false);
      this.Valid = true;
    }

    public MyPlanet Parent => this.m_parent;

    protected override void InitVoxelMap(MatrixD worldMatrix, Vector3I size, bool useOffset = true)
    {
      base.InitVoxelMap(worldMatrix, size, useOffset);
      this.Physics = new MyVoxelPhysicsBody((MyVoxelBase) this, 1.5f, 7f);
      this.Physics.Enabled = true;
    }

    public void OnStorageChanged(
      Vector3I minChanged,
      Vector3I maxChanged,
      MyStorageDataTypeFlags dataChanged)
    {
      minChanged = Vector3I.Clamp(minChanged, this.m_storageMin, this.m_storageMax);
      maxChanged = Vector3I.Clamp(maxChanged, this.m_storageMin, this.m_storageMax);
      if ((dataChanged & MyStorageDataTypeFlags.Content) == MyStorageDataTypeFlags.None || this.Physics == null)
        return;
      this.Physics.InvalidateRange(minChanged, maxChanged);
      this.RaisePhysicsChanged();
    }

    public void RefreshPhysics(IMyStorage storage)
    {
      this.m_storage = storage;
      this.OnStorageChanged(this.m_storageMin, this.m_storageMax, MyStorageDataTypeFlags.Content);
      this.Valid = true;
    }

    protected override void BeforeDelete()
    {
      base.BeforeDelete();
      this.m_storage = (IMyStorage) null;
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      if (this.Physics == null)
        return;
      this.Physics.UpdateBeforeSimulation10();
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.Physics == null)
        return;
      this.Physics.UpdateAfterSimulation10();
    }

    public override int GetOrePriority() => 0;

    public void PrefetchShapeOnRay(ref LineD ray)
    {
      if (this.Physics == null)
        return;
      this.Physics.PrefetchShapeOnRay(ref ray);
    }

    private class Sandbox_Game_Entities_MyVoxelPhysics\u003C\u003EActor : IActivator, IActivator<MyVoxelPhysics>
    {
      object IActivator.CreateInstance() => (object) new MyVoxelPhysics();

      MyVoxelPhysics IActivator<MyVoxelPhysics>.CreateInstance() => new MyVoxelPhysics();
    }
  }
}

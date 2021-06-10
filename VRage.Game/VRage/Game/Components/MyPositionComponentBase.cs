// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyPositionComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageMath.Spatial;

namespace VRage.Game.Components
{
  public abstract class MyPositionComponentBase : MyEntityComponentBase
  {
    public static Action<IMyEntity> OnReportInvalidMatrix;
    protected MatrixD m_worldMatrix = MatrixD.Identity;
    public uint m_worldMatrixCounter;
    public uint m_lastParentWorldMatrixCounter;
    public bool m_worldMatrixDirty;
    protected Matrix m_localMatrix = Matrix.Identity;
    protected BoundingBox m_localAABB;
    protected BoundingSphere m_localVolume;
    protected Vector3 m_localVolumeOffset;
    protected BoundingBoxD m_worldAABB;
    protected BoundingSphereD m_worldVolume;
    protected bool m_worldVolumeDirty;
    protected bool m_worldAABBDirty;
    private float? m_scale;
    private MyPositionComponentBase m_parent;
    protected bool m_normalizedInvMatrixDirty = true;
    private MatrixD m_normalizedWorldMatrixInv;
    protected bool m_invScaledMatrixDirty = true;
    private MatrixD m_worldMatrixInvScaled;

    public ref readonly MatrixD WorldMatrixRef
    {
      get
      {
        this.RecalculateWorldMatrixHRIfNeeded();
        return ref this.m_worldMatrix;
      }
    }

    [Obsolete("Deprecated, use WorldMatrixRef instead.")]
    public MatrixD WorldMatrix
    {
      get
      {
        this.RecalculateWorldMatrixHRIfNeeded();
        return this.m_worldMatrix;
      }
      set => this.SetWorldMatrix(ref value);
    }

    public ref readonly Matrix LocalMatrixRef => ref this.m_localMatrix;

    [Obsolete("Deprecated, use WorldMatrixRef instead.")]
    public Matrix LocalMatrix
    {
      get => this.m_localMatrix;
      set => this.SetLocalMatrix(ref value);
    }

    public BoundingBoxD WorldAABB
    {
      get
      {
        if (this.m_worldAABBDirty || this.RecalculateWorldMatrixHRIfNeeded())
        {
          this.m_localAABB.Transform(ref this.m_worldMatrix, ref this.m_worldAABB);
          this.m_worldAABBDirty = false;
        }
        return this.m_worldAABB;
      }
      set
      {
        this.m_worldAABB = value;
        Vector3 result = (Vector3) (value.Center - this.WorldMatrixRef.Translation);
        MatrixD matrix = this.WorldMatrixInvScaled;
        Vector3.TransformNormal(ref result, ref matrix, out result);
        this.LocalAABB = new BoundingBox(result - (Vector3) value.HalfExtents, result + (Vector3) value.HalfExtents);
        this.m_worldAABBDirty = false;
      }
    }

    public BoundingSphereD WorldVolume
    {
      get
      {
        if (this.m_worldVolumeDirty || this.RecalculateWorldMatrixHRIfNeeded())
        {
          this.m_worldVolume.Center = Vector3D.Transform((Vector3D) this.m_localVolume.Center, ref this.m_worldMatrix);
          this.m_worldVolume.Radius = (double) this.m_localVolume.Radius;
          this.m_worldVolumeDirty = false;
        }
        return this.m_worldVolume;
      }
      set
      {
        this.m_worldVolume = value;
        Vector3 result = (Vector3) (value.Center - this.WorldMatrixRef.Translation);
        MatrixD matrix = this.WorldMatrixInvScaled;
        Vector3.TransformNormal(ref result, ref matrix, out result);
        this.LocalVolume = new BoundingSphere(result, (float) value.Radius);
        this.m_worldVolumeDirty = false;
      }
    }

    public virtual BoundingBox LocalAABB
    {
      get => this.m_localAABB;
      set
      {
        if (!(this.m_localAABB != value))
          return;
        this.m_localAABB = value;
        this.m_localVolume = BoundingSphere.CreateFromBoundingBox(this.m_localAABB);
        this.m_worldVolumeDirty = true;
        this.m_worldAABBDirty = true;
        if (this.OnLocalAABBChanged == null)
          return;
        this.OnLocalAABBChanged(this);
      }
    }

    public BoundingSphere LocalVolume
    {
      get => this.m_localVolume;
      set
      {
        this.m_localVolume = value;
        this.m_localAABB = MyMath.CreateFromInsideRadius(value.Radius);
        this.m_localAABB = this.m_localAABB.Translate(value.Center);
        this.m_worldVolumeDirty = true;
        this.m_worldAABBDirty = true;
      }
    }

    public Vector3 LocalVolumeOffset
    {
      get => this.m_localVolumeOffset;
      set
      {
        this.m_localVolumeOffset = value;
        this.m_worldVolumeDirty = true;
      }
    }

    public event Action<MyPositionComponentBase> OnPositionChanged;

    public event Action<MyPositionComponentBase> OnLocalAABBChanged;

    protected void RaiseOnPositionChanged(MyPositionComponentBase component) => this.OnPositionChanged.InvokeIfNotNull<MyPositionComponentBase>(component);

    protected virtual bool ShouldSync => this.Container.Get<MySyncComponentBase>() != null;

    public float? Scale
    {
      get => this.m_scale;
      set
      {
        float? scale = this.m_scale;
        float? nullable = value;
        if ((double) scale.GetValueOrDefault() == (double) nullable.GetValueOrDefault() & scale.HasValue == nullable.HasValue)
          return;
        this.m_scale = value;
        Matrix normalized1 = this.LocalMatrixRef;
        if (this.m_scale.HasValue)
        {
          MatrixD normalized2 = this.WorldMatrixRef;
          if (this.m_parent == null)
          {
            MyUtils.Normalize(ref normalized2, out normalized2);
            MatrixD worldMatrix = Matrix.CreateScale(this.m_scale.Value) * normalized2;
            this.SetWorldMatrix(ref worldMatrix);
          }
          else
          {
            MyUtils.Normalize(ref normalized1, out normalized1);
            Matrix localMatrix = Matrix.CreateScale(this.m_scale.Value) * normalized1;
            this.SetLocalMatrix(ref localMatrix);
          }
        }
        else
        {
          MyUtils.Normalize(ref normalized1, out normalized1);
          this.SetLocalMatrix(ref normalized1);
        }
        this.UpdateWorldMatrix();
      }
    }

    [Obsolete("SetWorldMatrix(MatrixD,...) is deprecated, please use SetWorldMatrix(ref MatrixD,...) instead.")]
    public void SetWorldMatrix(
      MatrixD worldMatrix,
      object source = null,
      bool forceUpdate = false,
      bool updateChildren = true,
      bool updateLocal = true,
      bool skipTeleportCheck = false,
      bool forceUpdateAllChildren = false,
      bool ignoreAssert = false)
    {
      this.SetWorldMatrix(ref worldMatrix, source, forceUpdate, updateChildren, updateLocal, skipTeleportCheck, forceUpdateAllChildren, ignoreAssert);
    }

    public void SetWorldMatrix(
      ref MatrixD worldMatrix,
      object source = null,
      bool forceUpdate = false,
      bool updateChildren = true,
      bool updateLocal = true,
      bool skipTeleportCheck = false,
      bool forceUpdateAllChildren = false,
      bool ignoreAssert = false)
    {
      if (MyPositionComponentBase.OnReportInvalidMatrix != null && !worldMatrix.IsValid())
        MyPositionComponentBase.OnReportInvalidMatrix(this.Entity);
      else if (!skipTeleportCheck && this.Entity.InScene && Vector3D.DistanceSquared(worldMatrix.Translation, this.WorldMatrixRef.Translation) > (double) MyClusterTree.IdealClusterSizeHalfSqr.X)
      {
        this.Entity.Teleport(worldMatrix, source, ignoreAssert);
      }
      else
      {
        if (this.Entity.Parent != null && source != this.Entity.Parent)
          return;
        if (this.Scale.HasValue)
        {
          MyUtils.Normalize(ref worldMatrix, out worldMatrix);
          worldMatrix = MatrixD.CreateScale((double) this.Scale.Value) * worldMatrix;
        }
        if (!forceUpdate && this.m_worldMatrix.EqualsFast(ref worldMatrix, 1E-06))
          return;
        if (this.m_parent == null)
        {
          this.m_worldMatrix = worldMatrix;
          this.m_localMatrix.SetFrom(in worldMatrix);
        }
        else if (updateLocal)
        {
          MatrixD matrixD1 = this.m_parent.WorldMatrixInvScaled;
          ref Matrix local1 = ref this.m_localMatrix;
          MatrixD matrixD2 = worldMatrix * matrixD1;
          ref MatrixD local2 = ref matrixD2;
          local1.SetFrom(in local2);
        }
        ++this.m_worldMatrixCounter;
        this.m_worldMatrixDirty = false;
        this.UpdateWorldMatrix(source, updateChildren, forceUpdateAllChildren);
      }
    }

    public bool NeedsRecalculateWorldMatrix
    {
      get
      {
        if (this.m_worldMatrixDirty)
          return true;
        if (this.Entity == null)
          return false;
        MyPositionComponentBase parent = this.m_parent;
        uint worldMatrixCounter = this.m_lastParentWorldMatrixCounter;
        for (; parent != null; parent = parent.m_parent)
        {
          if (worldMatrixCounter < parent.m_worldMatrixCounter)
            return true;
          worldMatrixCounter = parent.m_lastParentWorldMatrixCounter;
        }
        return false;
      }
    }

    protected bool RecalculateWorldMatrixHRIfNeeded(bool updateChildren = false)
    {
      if (this.m_parent == null)
        return false;
      this.m_parent.RecalculateWorldMatrixHRIfNeeded();
      if ((int) this.m_lastParentWorldMatrixCounter == (int) this.m_parent.m_worldMatrixCounter && !this.m_worldMatrixDirty)
        return false;
      this.m_lastParentWorldMatrixCounter = this.m_parent.m_worldMatrixCounter;
      MatrixD worldMatrix = this.m_worldMatrix;
      MatrixD.Multiply(ref this.m_localMatrix, ref this.m_parent.m_worldMatrix, out this.m_worldMatrix);
      this.m_worldMatrixDirty = false;
      if (this.m_worldMatrix.EqualsFast(ref worldMatrix))
        return false;
      ++this.m_worldMatrixCounter;
      this.m_worldVolumeDirty = true;
      this.m_worldAABBDirty = true;
      this.m_normalizedInvMatrixDirty = true;
      this.m_invScaledMatrixDirty = true;
      return true;
    }

    public void SetLocalMatrix(
      ref Matrix localMatrix,
      object source,
      bool updateWorld,
      ref Matrix renderLocal,
      bool forceUpdateRender = false)
    {
      if (!(this.SetLocalMatrix(ref localMatrix, source, updateWorld) | forceUpdateRender))
        return;
      this.Entity.Render?.UpdateRenderObjectLocal(renderLocal);
    }

    public bool SetLocalMatrix(ref Matrix localMatrix, object source = null, bool updateWorld = true)
    {
      int num = !this.m_localMatrix.EqualsFast(ref localMatrix) ? 1 : 0;
      if (num != 0)
      {
        this.m_localMatrix = localMatrix;
        ++this.m_worldMatrixCounter;
        this.m_worldMatrixDirty = true;
      }
      if (!this.NeedsRecalculateWorldMatrix)
        return num != 0;
      if (!updateWorld)
        return num != 0;
      this.UpdateWorldMatrix(source);
      return num != 0;
    }

    public Vector3D GetPosition()
    {
      this.RecalculateWorldMatrixHRIfNeeded();
      return this.m_worldMatrix.Translation;
    }

    public void SetPosition(Vector3D pos, object source = null, bool forceUpdate = false, bool updateChildren = true)
    {
      if (MyUtils.IsZero(this.m_worldMatrix.Translation - pos))
        return;
      MatrixD worldMatrix = this.m_worldMatrix;
      worldMatrix.Translation = pos;
      this.SetWorldMatrix(ref worldMatrix, source, forceUpdate, updateChildren);
    }

    public MatrixD GetOrientation()
    {
      this.RecalculateWorldMatrixHRIfNeeded();
      return this.m_worldMatrix.GetOrientation();
    }

    public ref readonly MatrixD WorldMatrixNormalizedInv
    {
      get
      {
        if (this.m_normalizedInvMatrixDirty || this.RecalculateWorldMatrixHRIfNeeded())
        {
          MatrixD worldMatrix = this.m_worldMatrix;
          if (!MyUtils.IsZero(worldMatrix.Left.LengthSquared() - 1.0))
          {
            MatrixD matrix = MatrixD.Normalize(worldMatrix);
            MatrixD.Invert(ref matrix, out this.m_normalizedWorldMatrixInv);
          }
          else
            MatrixD.Invert(ref worldMatrix, out this.m_normalizedWorldMatrixInv);
          this.m_normalizedInvMatrixDirty = false;
          if (!this.Scale.HasValue)
          {
            this.m_worldMatrixInvScaled = this.m_normalizedWorldMatrixInv;
            this.m_invScaledMatrixDirty = false;
          }
        }
        return ref this.m_normalizedWorldMatrixInv;
      }
    }

    public ref readonly MatrixD WorldMatrixInvScaled
    {
      get
      {
        if (this.m_invScaledMatrixDirty || this.RecalculateWorldMatrixHRIfNeeded())
        {
          MatrixD matrix = this.m_worldMatrix;
          if (!MyUtils.IsZero(matrix.Left.LengthSquared() - 1.0))
            matrix = MatrixD.Normalize(matrix);
          float? scale1 = this.Scale;
          if (scale1.HasValue)
          {
            MatrixD matrixD = matrix;
            scale1 = this.Scale;
            Matrix scale2 = Matrix.CreateScale(scale1.Value);
            matrix = matrixD * scale2;
          }
          MatrixD.Invert(ref matrix, out this.m_worldMatrixInvScaled);
          this.m_invScaledMatrixDirty = false;
          scale1 = this.Scale;
          if (!scale1.HasValue)
          {
            this.m_normalizedWorldMatrixInv = this.m_worldMatrixInvScaled;
            this.m_normalizedInvMatrixDirty = false;
          }
        }
        return ref this.m_worldMatrixInvScaled;
      }
    }

    public virtual MatrixD GetViewMatrix() => this.WorldMatrixNormalizedInv;

    protected virtual void UpdateWorldMatrix(
      object source = null,
      bool updateChildren = true,
      bool forceUpdateAllChildren = false)
    {
      if (this.m_parent != null)
      {
        MatrixD parentWorldMatrix = this.m_parent.WorldMatrixRef;
        this.UpdateWorldMatrix(ref parentWorldMatrix, source, updateChildren, forceUpdateAllChildren);
      }
      else
        this.OnWorldPositionChanged(source, updateChildren, forceUpdateAllChildren);
    }

    public virtual void UpdateWorldMatrix(
      ref MatrixD parentWorldMatrix,
      object source = null,
      bool updateChildren = true,
      bool forceUpdateAllChildren = false)
    {
      if (this.m_parent == null)
        return;
      MatrixD.Multiply(ref this.m_localMatrix, ref parentWorldMatrix, out this.m_worldMatrix);
      this.m_lastParentWorldMatrixCounter = this.m_parent.m_worldMatrixCounter;
      ++this.m_worldMatrixCounter;
      this.m_worldMatrixDirty = false;
      this.OnWorldPositionChanged(source, updateChildren, forceUpdateAllChildren);
    }

    protected virtual void OnWorldPositionChanged(
      object source,
      bool updateChildren = true,
      bool forceUpdateAllChildren = false)
    {
      this.m_worldVolumeDirty = true;
      this.m_worldAABBDirty = true;
      this.m_normalizedInvMatrixDirty = true;
      this.m_invScaledMatrixDirty = true;
      this.RaiseOnPositionChanged(this);
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.Container.ComponentAdded += new Action<Type, MyEntityComponentBase>(this.ContainerOnComponentAdded);
      this.Container.ComponentRemoved += new Action<Type, MyEntityComponentBase>(this.ContainerOnComponentRemoved);
      this.HookHierarchy(this.Entity.Hierarchy);
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      this.Container.ComponentAdded -= new Action<Type, MyEntityComponentBase>(this.ContainerOnComponentAdded);
      this.Container.ComponentRemoved -= new Action<Type, MyEntityComponentBase>(this.ContainerOnComponentRemoved);
      this.UnHookHierarchy(this.Entity.Hierarchy);
    }

    private void ContainerOnComponentAdded(Type arg1, MyEntityComponentBase arg2)
    {
      if (!(arg2 is MyHierarchyComponentBase hierarchy))
        return;
      this.HookHierarchy(hierarchy);
    }

    private void ContainerOnComponentRemoved(Type arg1, MyEntityComponentBase arg2)
    {
      if (!(arg2 is MyHierarchyComponentBase hierarchy))
        return;
      this.UnHookHierarchy(hierarchy);
    }

    private void HookHierarchy(MyHierarchyComponentBase hierarchy)
    {
      if (hierarchy == null)
        return;
      hierarchy.OnParentChanged += new Action<MyHierarchyComponentBase, MyHierarchyComponentBase>(this.Hierarchy_OnParentChanged);
      this.m_parent = this.Entity.Parent?.PositionComp;
    }

    private void UnHookHierarchy(MyHierarchyComponentBase hierarchy)
    {
      if (hierarchy == null)
        return;
      hierarchy.OnParentChanged -= new Action<MyHierarchyComponentBase, MyHierarchyComponentBase>(this.Hierarchy_OnParentChanged);
      this.m_parent = (MyPositionComponentBase) null;
    }

    private void Hierarchy_OnParentChanged(
      MyHierarchyComponentBase arg1,
      MyHierarchyComponentBase arg2)
    {
      this.m_parent = arg2?.Entity?.PositionComp;
      this.m_lastParentWorldMatrixCounter = 0U;
    }

    public override string ComponentTypeDebugString => "Position";

    public override string ToString() => "worldpos=" + (object) this.GetPosition() + ", worldmat=" + (object) this.WorldMatrixRef;
  }
}

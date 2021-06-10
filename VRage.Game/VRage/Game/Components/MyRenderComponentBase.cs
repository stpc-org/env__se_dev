// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyRenderComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Game.Components
{
  public abstract class MyRenderComponentBase : MyEntityComponentBase
  {
    public static uint[] UNINITIALIZED_IDs = new uint[1]
    {
      uint.MaxValue
    };
    public static readonly Vector3 OldRedToHSV = new Vector3(0.0f, 0.0f, 0.05f);
    public static readonly Vector3 OldYellowToHSV = new Vector3(0.1222222f, -0.1f, 0.26f);
    public static readonly Vector3 OldBlueToHSV = new Vector3(0.575f, 0.0f, 0.0f);
    public static readonly Vector3 OldGreenToHSV = new Vector3(0.3333333f, -0.48f, -0.25f);
    public static readonly Vector3 OldBlackToHSV = new Vector3(0.0f, -0.96f, -0.5f);
    public static readonly Vector3 OldWhiteToHSV = new Vector3(0.0f, -0.95f, 0.4f);
    public static readonly Vector3 OldGrayToHSV = new Vector3(0.0f, -1f, 0.0f);
    protected bool m_enableColorMaskHsv;
    protected Vector3 m_colorMaskHsv = MyRenderComponentBase.OldGrayToHSV;
    protected Dictionary<string, MyTextureChange> m_textureChanges;
    protected Color m_diffuseColor = Color.White;
    public int LastMomentUpdateIndex = -1;
    public Action NeedForDrawFromParentChanged;
    public bool FadeIn;
    public bool FadeOut;
    protected uint[] m_parentIDs = MyRenderComponentBase.UNINITIALIZED_IDs;
    protected uint[] m_renderObjectIDs = MyRenderComponentBase.UNINITIALIZED_IDs;
    public float Transparency;
    public byte DepthBias;
    private bool m_visibilityUpdates;

    public abstract object ModelStorage { get; set; }

    public bool EnableColorMaskHsv
    {
      get => this.m_enableColorMaskHsv;
      set
      {
        this.m_enableColorMaskHsv = value;
        if (!this.EnableColorMaskHsv)
          return;
        this.UpdateRenderEntity(this.m_colorMaskHsv);
        this.Container.Entity.EnableColorMaskForSubparts(value);
      }
    }

    public Vector3 ColorMaskHsv
    {
      get => this.m_colorMaskHsv;
      set
      {
        this.m_colorMaskHsv = value;
        if (!this.EnableColorMaskHsv)
          return;
        this.UpdateRenderEntity(this.m_colorMaskHsv);
        this.Container.Entity.SetColorMaskForSubparts(value);
      }
    }

    public Dictionary<string, MyTextureChange> TextureChanges
    {
      get => this.m_textureChanges;
      set
      {
        this.m_textureChanges = value;
        if (!this.EnableColorMaskHsv)
          return;
        this.UpdateRenderTextureChanges(value);
        this.Container.Entity.SetTextureChangesForSubparts(value);
      }
    }

    public MyPersistentEntityFlags2 PersistentFlags { get; set; }

    public uint[] ParentIDs => this.m_parentIDs;

    public uint[] RenderObjectIDs => this.m_renderObjectIDs;

    public abstract void SetRenderObjectID(int index, uint ID);

    public uint GetRenderObjectID() => this.m_renderObjectIDs.Length != 0 ? this.m_renderObjectIDs[0] : uint.MaxValue;

    public virtual void RemoveRenderObjects()
    {
      for (int index = 0; index < this.m_renderObjectIDs.Length; ++index)
        this.ReleaseRenderObjectID(index);
    }

    public abstract void ReleaseRenderObjectID(int index);

    public void ResizeRenderObjectArray(int newSize)
    {
      int length = this.m_renderObjectIDs.Length;
      Array.Resize<uint>(ref this.m_renderObjectIDs, newSize);
      Array.Resize<uint>(ref this.m_parentIDs, newSize);
      for (int index = length; index < newSize; ++index)
      {
        this.m_renderObjectIDs[index] = uint.MaxValue;
        this.m_parentIDs[index] = uint.MaxValue;
      }
    }

    public bool IsRenderObjectAssigned(int index) => this.m_renderObjectIDs[index] != uint.MaxValue;

    public virtual void InvalidateRenderObjects()
    {
      if (MyRenderProxy.SkipRenderMessages)
        return;
      MyEntity entity = (MyEntity) this.Entity;
      if (!this.Visible && !this.CastShadows || (!entity.InScene || !entity.InvalidateOnMove))
        return;
      ref readonly MatrixD local = ref entity.PositionComp.WorldMatrixRef;
      for (int index = 0; index < this.m_renderObjectIDs.Length; ++index)
      {
        if (this.m_renderObjectIDs[index] != uint.MaxValue)
          MyRenderProxy.UpdateRenderObject(this.m_renderObjectIDs[index], in local, in BoundingBox.Invalid, false, this.LastMomentUpdateIndex);
      }
    }

    public void UpdateRenderObjectLocal(Matrix renderLocalMatrix)
    {
      if (MyRenderProxy.SkipRenderMessages || !this.Container.Entity.Visible && !this.Container.Entity.CastShadows)
        return;
      for (int index = 0; index < this.m_renderObjectIDs.Length; ++index)
      {
        if (this.RenderObjectIDs[index] != uint.MaxValue)
          MyRenderProxy.UpdateRenderObjectLocal(this.RenderObjectIDs[index], in renderLocalMatrix, in BoundingBox.Invalid, false, this.LastMomentUpdateIndex);
      }
    }

    public virtual void UpdateRenderEntity(Vector3 colorMaskHSV)
    {
      this.m_colorMaskHsv = colorMaskHSV;
      if (MyRenderProxy.SkipRenderMessages || this.m_renderObjectIDs[0] == uint.MaxValue)
        return;
      MyRenderProxy.UpdateRenderEntity(this.m_renderObjectIDs[0], new Color?(this.m_diffuseColor), new Vector3?(this.m_colorMaskHsv));
    }

    public virtual void UpdateRenderTextureChanges(
      Dictionary<string, MyTextureChange> skinTextureChanges)
    {
      this.m_textureChanges = skinTextureChanges;
      if (MyRenderProxy.SkipRenderMessages || this.m_renderObjectIDs[0] == uint.MaxValue)
        return;
      MyRenderProxy.ChangeMaterialTexture(this.m_renderObjectIDs[0], this.m_textureChanges);
    }

    public bool Visible
    {
      get => (uint) (this.Container.Entity.Flags & EntityFlags.Visible) > 0U;
      set
      {
        int flags1 = (int) this.Container.Entity.Flags;
        if (value)
          this.Container.Entity.Flags |= EntityFlags.Visible;
        else
          this.Container.Entity.Flags &= ~EntityFlags.Visible;
        int flags2 = (int) this.Container.Entity.Flags;
        if (flags1 == flags2)
          return;
        this.UpdateRenderObjectVisibilityIncludingChildren(value);
      }
    }

    protected virtual bool CanBeAddedToRender() => true;

    public void UpdateRenderObject(bool visible, bool updateChildren = true)
    {
      if (!this.Container.Entity.InScene & visible)
        return;
      MyHierarchyComponentBase hierarchyComponentBase = this.Container.Get<MyHierarchyComponentBase>();
      if (visible)
      {
        if (hierarchyComponentBase != null && this.Visible && (hierarchyComponentBase.Parent == null || hierarchyComponentBase.Parent.Container.Entity.Visible) && this.CanBeAddedToRender())
        {
          if (!this.IsRenderObjectAssigned(0))
            this.AddRenderObjects();
          else
            this.UpdateRenderObjectVisibility(visible);
        }
      }
      else
      {
        if (this.m_renderObjectIDs[0] != uint.MaxValue)
          this.UpdateRenderObjectVisibility(visible);
        this.RemoveRenderObjects();
      }
      if (!updateChildren || hierarchyComponentBase == null)
        return;
      foreach (MyHierarchyComponentBase child in hierarchyComponentBase.Children)
      {
        MyRenderComponentBase component = (MyRenderComponentBase) null;
        if (child.Container.TryGet<MyRenderComponentBase>(out component))
          component.UpdateRenderObject(visible);
      }
    }

    protected virtual void UpdateRenderObjectVisibility(bool visible)
    {
      if (MyRenderProxy.SkipRenderMessages)
        return;
      foreach (uint renderObjectId in this.m_renderObjectIDs)
      {
        if (renderObjectId != uint.MaxValue)
          MyRenderProxy.UpdateRenderObjectVisibility(renderObjectId, visible, this.Container.Entity.NearFlag);
      }
    }

    private void UpdateRenderObjectVisibilityIncludingChildren(bool visible)
    {
      if (MyRenderProxy.SkipRenderMessages)
        return;
      this.UpdateRenderObjectVisibility(visible);
      MyHierarchyComponentBase component1;
      if (!this.Container.TryGet<MyHierarchyComponentBase>(out component1))
        return;
      foreach (MyHierarchyComponentBase child in component1.Children)
      {
        MyRenderComponentBase component2 = (MyRenderComponentBase) null;
        if (child.Container.Entity.InScene && child.Container.TryGet<MyRenderComponentBase>(out component2))
          component2.UpdateRenderObjectVisibilityIncludingChildren(visible);
      }
    }

    public Color GetDiffuseColor() => this.m_diffuseColor;

    public bool DrawInAllCascades { get; set; }

    public bool MetalnessColorable { get; set; }

    public virtual bool NearFlag
    {
      get => (uint) (this.Container.Entity.Flags & EntityFlags.Near) > 0U;
      set
      {
        int num = value != this.NearFlag ? 1 : 0;
        if (value)
          this.Container.Entity.Flags |= EntityFlags.Near;
        else
          this.Container.Entity.Flags &= ~EntityFlags.Near;
        if (num != 0)
        {
          for (int index = 0; index < this.m_renderObjectIDs.Length; ++index)
          {
            if (this.m_renderObjectIDs[index] != uint.MaxValue)
              MyRenderProxy.UpdateRenderObjectVisibility(this.m_renderObjectIDs[index], this.Visible, this.NearFlag);
          }
        }
        MyHierarchyComponentBase component1;
        if (!this.Container.TryGet<MyHierarchyComponentBase>(out component1))
          return;
        foreach (MyHierarchyComponentBase child in component1.Children)
        {
          MyRenderComponentBase component2 = (MyRenderComponentBase) null;
          if (child.Container.Entity.InScene && child.Container.TryGet<MyRenderComponentBase>(out component2))
            component2.NearFlag = value;
        }
      }
    }

    public bool NeedsDrawFromParent
    {
      get => (uint) (this.Container.Entity.Flags & EntityFlags.NeedsDrawFromParent) > 0U;
      set
      {
        if (value == this.NeedsDrawFromParent)
          return;
        this.Container.Entity.Flags &= ~EntityFlags.NeedsDrawFromParent;
        if (value)
          this.Container.Entity.Flags |= EntityFlags.NeedsDrawFromParent;
        if (this.NeedForDrawFromParentChanged == null)
          return;
        this.NeedForDrawFromParentChanged();
      }
    }

    public bool CastShadows
    {
      get => (uint) (this.PersistentFlags & MyPersistentEntityFlags2.CastShadows) > 0U;
      set
      {
        if (value)
          this.PersistentFlags |= MyPersistentEntityFlags2.CastShadows;
        else
          this.PersistentFlags &= ~MyPersistentEntityFlags2.CastShadows;
      }
    }

    public bool NeedsResolveCastShadow
    {
      get => (uint) (this.Container.Entity.Flags & EntityFlags.NeedsResolveCastShadow) > 0U;
      set
      {
        if (value)
          this.Container.Entity.Flags |= EntityFlags.NeedsResolveCastShadow;
        else
          this.Container.Entity.Flags &= ~EntityFlags.NeedsResolveCastShadow;
      }
    }

    public bool FastCastShadowResolve
    {
      get => (uint) (this.Container.Entity.Flags & EntityFlags.FastCastShadowResolve) > 0U;
      set
      {
        if (value)
          this.Container.Entity.Flags |= EntityFlags.FastCastShadowResolve;
        else
          this.Container.Entity.Flags &= ~EntityFlags.FastCastShadowResolve;
      }
    }

    public bool SkipIfTooSmall
    {
      get => (uint) (this.Container.Entity.Flags & EntityFlags.SkipIfTooSmall) > 0U;
      set
      {
        if (value)
          this.Container.Entity.Flags |= EntityFlags.SkipIfTooSmall;
        else
          this.Container.Entity.Flags &= ~EntityFlags.SkipIfTooSmall;
      }
    }

    public bool DrawOutsideViewDistance
    {
      get => (uint) (this.Container.Entity.Flags & EntityFlags.DrawOutsideViewDistance) > 0U;
      set
      {
        if (value)
          this.Container.Entity.Flags |= EntityFlags.DrawOutsideViewDistance;
        else
          this.Container.Entity.Flags &= ~EntityFlags.DrawOutsideViewDistance;
      }
    }

    public bool ShadowBoxLod
    {
      get => (uint) (this.Container.Entity.Flags & EntityFlags.ShadowBoxLod) > 0U;
      set
      {
        if (value)
          this.Container.Entity.Flags |= EntityFlags.ShadowBoxLod;
        else
          this.Container.Entity.Flags &= ~EntityFlags.ShadowBoxLod;
      }
    }

    public bool OffsetInVertexShader { get; set; }

    public virtual RenderFlags GetRenderFlags()
    {
      RenderFlags renderFlags = (RenderFlags) 0;
      if (this.NearFlag)
        renderFlags |= RenderFlags.Near;
      if (this.CastShadows)
        renderFlags |= RenderFlags.CastShadows | RenderFlags.CastShadowsOnLow;
      if (this.Visible)
        renderFlags |= RenderFlags.Visible;
      if (this.NeedsResolveCastShadow)
        renderFlags |= RenderFlags.NeedsResolveCastShadow;
      if (this.FastCastShadowResolve)
        renderFlags |= RenderFlags.FastCastShadowResolve;
      if (this.SkipIfTooSmall)
        renderFlags |= RenderFlags.SkipIfTooSmall;
      if (this.DrawOutsideViewDistance)
        renderFlags |= RenderFlags.DrawOutsideViewDistance;
      if (this.ShadowBoxLod)
        renderFlags |= RenderFlags.ShadowLodBox;
      if (this.DrawInAllCascades)
        renderFlags |= RenderFlags.DrawInAllCascades;
      if (this.MetalnessColorable)
        renderFlags |= RenderFlags.MetalnessColorable;
      return renderFlags;
    }

    public virtual CullingOptions GetRenderCullingOptions() => CullingOptions.Default;

    public abstract void AddRenderObjects();

    public abstract void Draw();

    public abstract bool IsVisible();

    public virtual bool NeedsDraw
    {
      get => (uint) (this.Container.Entity.Flags & EntityFlags.NeedsDraw) > 0U;
      set
      {
        if (value == this.NeedsDraw)
          return;
        this.Container.Entity.Flags &= ~EntityFlags.NeedsDraw;
        if (!value)
          return;
        this.Container.Entity.Flags |= EntityFlags.NeedsDraw;
      }
    }

    public override string ComponentTypeDebugString => "Render";

    public void SetParent(int index, uint cellParentCullObject, Matrix? childToParent = null)
    {
      if (this.m_parentIDs == MyRenderComponentBase.UNINITIALIZED_IDs)
        this.m_parentIDs = new uint[index + 1];
      this.m_parentIDs[index] = cellParentCullObject;
      MyRenderProxy.SetParentCullObject(this.RenderObjectIDs[index], cellParentCullObject, childToParent);
    }

    public bool IsChild(int index) => this.m_parentIDs[index] != uint.MaxValue;

    public void SetVisibilityUpdates(bool state)
    {
      this.m_visibilityUpdates = state;
      this.PropagateVisibilityUpdates(true);
    }

    protected void PropagateVisibilityUpdates(bool always = false)
    {
      if (MyRenderProxy.SkipRenderMessages || !always && !this.m_visibilityUpdates)
        return;
      foreach (uint renderObjectId in this.m_renderObjectIDs)
      {
        if (renderObjectId != uint.MaxValue)
          MyRenderProxy.SetVisibilityUpdates(renderObjectId, this.m_visibilityUpdates);
      }
    }

    public void UpdateTransparency()
    {
      if (MyRenderProxy.SkipRenderMessages || this.m_renderObjectIDs[0] == uint.MaxValue)
        return;
      MyRenderProxy.UpdateRenderEntity(this.m_renderObjectIDs[0], new Color?(), new Vector3?(), new float?(this.Transparency));
    }
  }
}

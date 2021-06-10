// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.Renders.MyRenderComponentVoxelMap
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Entities.Components;
using VRage.Factory;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Voxels.Clipmap;
using VRageMath;
using VRageRender;
using VRageRender.Voxels;

namespace Sandbox.Game.EntityComponents.Renders
{
  [MyDependency(typeof (MyVoxelMesherComponent), Critical = true)]
  public class MyRenderComponentVoxelMap : MyRenderComponent
  {
    public const string DefaultSettingsGroup = "Default";
    public const string PlanetSettingsGroup = "Planet";
    protected MyVoxelBase m_voxelMap;
    protected MyVoxelMesherComponent Mesher;

    static MyRenderComponentVoxelMap() => MyRenderComponentVoxelMap.SetLodQuality(MySandboxGame.Config.VoxelQuality.HasValue ? MySandboxGame.Config.VoxelQuality.Value : MyRenderQualityEnum.NORMAL);

    public static event Action TerrainQualityChange;

    public IMyLodController Clipmap { get; protected set; }

    public uint ClipmapId => this.m_renderObjectIDs[0];

    protected virtual IMyLodController CreateLodController()
    {
      Vector3D leftBottomCorner = this.m_voxelMap.PositionLeftBottomCorner;
      Matrix orientation = this.m_voxelMap.Orientation;
      Vector3 forward = orientation.Forward;
      orientation = this.m_voxelMap.Orientation;
      Vector3 up = orientation.Up;
      return (IMyLodController) new MyVoxelClipmap(this.m_voxelMap.Size, MatrixD.CreateWorld(leftBottomCorner, forward, up), this.Mesher, new float?(), Vector3D.Zero, "Default");
    }

    public override void OnAddedToScene()
    {
      this.m_voxelMap = this.Container.Entity as MyVoxelBase;
      this.Mesher = new MyVoxelMesherComponent();
      this.Mesher.SetContainer((MyComponentContainer) this.Entity.Components);
      this.Mesher.OnAddedToScene();
      base.OnAddedToScene();
    }

    public void ResetLoading() => MyRenderComponentVoxelMap.VoxelLoadingWaitStep.AddClipmap(this.Clipmap);

    public override void AddRenderObjects()
    {
      if (this.Mesher == null)
        return;
      MatrixD world = MatrixD.CreateWorld(this.m_voxelMap.PositionLeftBottomCorner, this.m_voxelMap.Orientation.Forward, this.m_voxelMap.Orientation.Up);
      this.Clipmap = this.CreateLodController();
      MyRenderComponentVoxelMap.VoxelLoadingWaitStep.AddClipmap(this.Clipmap);
      this.SetRenderObjectID(0, MyRenderProxy.RenderVoxelCreate(this.m_voxelMap.StorageName, world, this.Clipmap, this.GetRenderFlags(), this.Transparency));
    }

    public override void InvalidateRenderObjects()
    {
      if (!this.Visible || this.m_renderObjectIDs[0] == uint.MaxValue)
        return;
      MyRenderProxy.UpdateRenderObject(this.m_renderObjectIDs[0], new MatrixD?(MatrixD.CreateWorld(this.m_voxelMap.PositionLeftBottomCorner, this.m_voxelMap.Orientation.Forward, this.m_voxelMap.Orientation.Up)));
    }

    public void UpdateCells()
    {
      if (this.m_renderObjectIDs[0] == uint.MaxValue)
        return;
      Vector3D leftBottomCorner = this.m_voxelMap.PositionLeftBottomCorner;
      Matrix orientation = this.m_voxelMap.Orientation;
      Vector3 forward = orientation.Forward;
      orientation = this.m_voxelMap.Orientation;
      Vector3 up = orientation.Up;
      MyRenderProxy.UpdateRenderObject(this.m_renderObjectIDs[0], new MatrixD?(MatrixD.CreateWorld(leftBottomCorner, forward, up)));
    }

    public void InvalidateRange(Vector3I minVoxelChanged, Vector3I maxVoxelChanged)
    {
      minVoxelChanged -= 1;
      maxVoxelChanged += 1;
      this.m_voxelMap.Storage.ClampVoxelCoord(ref minVoxelChanged);
      this.m_voxelMap.Storage.ClampVoxelCoord(ref maxVoxelChanged);
      minVoxelChanged -= this.m_voxelMap.StorageMin;
      maxVoxelChanged -= this.m_voxelMap.StorageMin;
      if (this.Clipmap == null)
        return;
      this.Clipmap.InvalidateRange(minVoxelChanged, maxVoxelChanged);
    }

    public static void RefreshClipmapSettings()
    {
      if (!MyEntities.IsLoaded)
        return;
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (!entity.MarkedForClose && entity.Render is MyRenderComponentVoxelMap render && render.Clipmap is MyVoxelClipmap clipmap)
        {
          MyVoxelClipmapSettings settings = MyVoxelClipmapSettings.GetSettings(clipmap.SettingsGroup);
          clipmap.UpdateSettings(settings);
        }
      }
    }

    public static void SetLodQuality(MyRenderQualityEnum quality)
    {
      MyVoxelClipmapSettings.SetSettingsForGroup("Default", MyVoxelClipmapSettingsPresets.NormalSettings[(int) quality]);
      MyVoxelClipmapSettings.SetSettingsForGroup("Planet", MyVoxelClipmapSettingsPresets.PlanetSettings[(int) quality]);
      MyRenderComponentVoxelMap.RefreshClipmapSettings();
      if (MyRenderComponentVoxelMap.TerrainQualityChange == null)
        return;
      MyRenderComponentVoxelMap.TerrainQualityChange();
    }

    public static class VoxelLoadingWaitStep
    {
      public static readonly HashSet<IMyLodController> Clipmaps = new HashSet<IMyLodController>();
      public static int Total;

      public static bool Complete => MyRenderComponentVoxelMap.VoxelLoadingWaitStep.Clipmaps.Count == 0;

      public static float Progress => (float) MyRenderComponentVoxelMap.VoxelLoadingWaitStep.Clipmaps.Count / (float) MyRenderComponentVoxelMap.VoxelLoadingWaitStep.Total;

      public static void AddClipmap(IMyLodController controller)
      {
        lock (MyRenderComponentVoxelMap.VoxelLoadingWaitStep.Clipmaps)
        {
          if (!MyRenderComponentVoxelMap.VoxelLoadingWaitStep.Clipmaps.Add(controller))
            return;
          ++MyRenderComponentVoxelMap.VoxelLoadingWaitStep.Total;
          controller.Loaded += new Action<IMyLodController>(MyRenderComponentVoxelMap.VoxelLoadingWaitStep.RemoveClipmap);
        }
      }

      public static void RemoveClipmap(IMyLodController clipmap)
      {
        lock (MyRenderComponentVoxelMap.VoxelLoadingWaitStep.Clipmaps)
        {
          if (MyRenderComponentVoxelMap.VoxelLoadingWaitStep.Clipmaps.Remove(clipmap))
            clipmap.Loaded -= new Action<IMyLodController>(MyRenderComponentVoxelMap.VoxelLoadingWaitStep.RemoveClipmap);
        }
        if (!MyRenderComponentVoxelMap.VoxelLoadingWaitStep.Complete)
          return;
        MyRenderProxy.SendClipmapsReady();
      }
    }

    private class Sandbox_Game_EntityComponents_Renders_MyRenderComponentVoxelMap\u003C\u003EActor : IActivator, IActivator<MyRenderComponentVoxelMap>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentVoxelMap();

      MyRenderComponentVoxelMap IActivator<MyRenderComponentVoxelMap>.CreateInstance() => new MyRenderComponentVoxelMap();
    }
  }
}

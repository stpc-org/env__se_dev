// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentPlanet
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Voxels.Clipmap;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;
using VRageRender.Voxels;

namespace Sandbox.Game.Components
{
  internal class MyRenderComponentPlanet : MyRenderComponentVoxelMap
  {
    private MyPlanet m_planet;
    private int m_shadowHelperRenderObjectIndex = -1;
    private int m_atmosphereRenderIndex = -1;
    private readonly List<int> m_cloudLayerRenderObjectIndexList = new List<int>();
    private int m_fogUpdateCounter;
    private static bool lastSentFogFlag = true;
    private bool m_oldNeedsDraw;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_planet = this.Entity as MyPlanet;
      this.m_oldNeedsDraw = this.NeedsDraw;
      this.NeedsDraw = true;
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      this.NeedsDraw = this.m_oldNeedsDraw;
      this.m_planet = (MyPlanet) null;
    }

    protected override IMyLodController CreateLodController()
    {
      Vector3D leftBottomCorner = this.m_voxelMap.PositionLeftBottomCorner;
      Matrix orientation = this.m_voxelMap.Orientation;
      Vector3 forward = orientation.Forward;
      orientation = this.m_voxelMap.Orientation;
      Vector3 up = orientation.Up;
      return (IMyLodController) new MyVoxelClipmap(this.m_voxelMap.Size, MatrixD.CreateWorld(leftBottomCorner, forward, up), this.Mesher, new float?(this.m_planet.AverageRadius), this.m_planet.PositionComp.GetPosition(), "Planet")
      {
        Cache = MyVoxelClipmapCache.Instance
      };
    }

    public override void AddRenderObjects()
    {
      base.AddRenderObjects();
      int length = this.RenderObjectIDs.Length;
      this.ResizeRenderObjectArray(16);
      int num1 = length;
      Vector3D leftBottomCorner = this.m_planet.PositionLeftBottomCorner;
      Vector3 atmosphereWavelengths = new Vector3();
      atmosphereWavelengths.X = 1f / (float) Math.Pow((double) this.m_planet.AtmosphereWavelengths.X, 4.0);
      atmosphereWavelengths.Y = 1f / (float) Math.Pow((double) this.m_planet.AtmosphereWavelengths.Y, 4.0);
      atmosphereWavelengths.Z = 1f / (float) Math.Pow((double) this.m_planet.AtmosphereWavelengths.Z, 4.0);
      IMyEntity entity = this.Entity;
      if (this.m_planet.HasAtmosphere)
      {
        MatrixD worldMatrix = MatrixD.Identity * (double) this.m_planet.AtmosphereRadius;
        worldMatrix.M44 = 1.0;
        worldMatrix.Translation = this.Entity.PositionComp.GetPosition();
        this.m_atmosphereRenderIndex = num1;
        this.SetRenderObjectID(num1++, MyRenderProxy.CreateRenderEntityAtmosphere(this.Entity.GetFriendlyName() + " " + this.Entity.EntityId.ToString(), "Models\\Environment\\Atmosphere_sphere.mwm", worldMatrix, MyMeshDrawTechnique.ATMOSPHERE, RenderFlags.Visible | RenderFlags.DrawOutsideViewDistance, this.GetRenderCullingOptions(), this.m_planet.AtmosphereRadius, this.m_planet.AverageRadius, atmosphereWavelengths, fadeIn: this.FadeIn));
        this.UpdateAtmosphereSettings(this.m_planet.AtmosphereSettings);
      }
      this.m_shadowHelperRenderObjectIndex = num1;
      MatrixD scale = MatrixD.CreateScale((double) this.m_planet.MinimumRadius);
      scale.Translation = this.m_planet.WorldMatrix.Translation;
      int index1 = num1;
      int num2 = index1 + 1;
      this.SetRenderObjectID(index1, MyRenderProxy.CreateRenderEntity("Shadow helper", "Models\\Environment\\Sky\\ShadowHelperSphere.mwm", scale, MyMeshDrawTechnique.MESH, RenderFlags.CastShadows | RenderFlags.Visible | RenderFlags.DrawOutsideViewDistance | RenderFlags.NoBackFaceCulling | RenderFlags.SkipInMainView | RenderFlags.CastShadowsOnLow | RenderFlags.SkipInForward, CullingOptions.Default, Color.White, new Vector3(1f, 1f, 1f), fadeIn: this.FadeIn));
      MyPlanetGeneratorDefinition generator = this.m_planet.Generator;
      if (!MyFakes.ENABLE_PLANETARY_CLOUDS || generator == null || generator.CloudLayers == null)
        return;
      foreach (MyCloudLayerSettings cloudLayer in generator.CloudLayers)
      {
        double minScaledAltitude = ((double) this.m_planet.AverageRadius + (double) this.m_planet.MaximumRadius) / 2.0;
        double altitude = minScaledAltitude + ((double) this.m_planet.MaximumRadius - minScaledAltitude) * (double) cloudLayer.RelativeAltitude;
        Vector3D rotationAxis = Vector3D.Normalize(cloudLayer.RotationAxis == Vector3D.Zero ? Vector3D.Up : cloudLayer.RotationAxis);
        int index2 = num2 + this.m_cloudLayerRenderObjectIndexList.Count;
        this.SetRenderObjectID(index2, MyRenderProxy.CreateRenderEntityCloudLayer(this.m_atmosphereRenderIndex != -1 ? this.m_renderObjectIDs[this.m_atmosphereRenderIndex] : uint.MaxValue, this.Entity.GetFriendlyName() + " " + this.Entity.EntityId.ToString(), cloudLayer.Model, cloudLayer.Textures, this.Entity.PositionComp.GetPosition(), altitude, minScaledAltitude, cloudLayer.ScalingEnabled, (double) cloudLayer.FadeOutRelativeAltitudeStart, (double) cloudLayer.FadeOutRelativeAltitudeEnd, cloudLayer.ApplyFogRelativeDistance, (double) this.m_planet.MaximumRadius, rotationAxis, cloudLayer.AngularVelocity, cloudLayer.InitialRotation, cloudLayer.Color.ToLinearRGB(), this.FadeIn));
        this.m_cloudLayerRenderObjectIndexList.Add(index2);
      }
      int num3 = num2 + generator.CloudLayers.Count;
    }

    public override void Draw()
    {
      if (this.m_oldNeedsDraw)
        base.Draw();
      MatrixD scale = MatrixD.CreateScale((double) this.m_planet.MinimumRadius * Math.Min(Vector3D.Distance(MySector.MainCamera.Position, this.m_planet.WorldMatrix.Translation) / (double) this.m_planet.MinimumRadius, 1.0) * 0.996999979019165);
      scale.Translation = this.m_planet.PositionComp.WorldMatrixRef.Translation;
      MyRenderProxy.UpdateRenderObject(this.m_renderObjectIDs[this.m_shadowHelperRenderObjectIndex], new MatrixD?(scale));
      this.DrawFog();
    }

    private void DrawFog()
    {
      if (!MyFakes.ENABLE_CLOUD_FOG || this.m_fogUpdateCounter-- > 0)
        return;
      this.m_fogUpdateCounter = (int) (100.0 * (0.800000011920929 + (double) MyRandom.Instance.NextFloat() * 0.400000005960464));
      Vector3D position1 = MySector.MainCamera.Position;
      Vector3D position2 = this.m_planet.PositionComp.GetPosition();
      double num = (double) this.m_planet.AtmosphereRadius * 2.0;
      if ((position1 - position2).LengthSquared() > num * num)
        return;
      this.m_fogUpdateCounter = (int) ((double) this.m_fogUpdateCounter * 0.670000016689301);
      bool shouldDrawFog = !this.IsPointInAirtightSpace(position1);
      if (MyRenderComponentPlanet.lastSentFogFlag == shouldDrawFog)
        return;
      MyRenderComponentPlanet.lastSentFogFlag = shouldDrawFog;
      MyRenderProxy.UpdateCloudLayerFogFlag(shouldDrawFog);
    }

    public void UpdateAtmosphereSettings(MyAtmosphereSettings settings) => MyRenderProxy.UpdateAtmosphereSettings(this.m_renderObjectIDs[this.m_atmosphereRenderIndex], settings);

    private bool IsPointInAirtightSpace(Vector3D worldPosition)
    {
      if (!MySession.Static.Settings.EnableOxygen)
        return true;
      bool flag = false;
      BoundingSphereD boundingSphere = new BoundingSphereD(worldPosition, 0.1);
      List<MyEntity> myEntityList = (List<MyEntity>) null;
      try
      {
        myEntityList = MyEntities.GetEntitiesInSphere(ref boundingSphere);
        foreach (MyEntity myEntity in myEntityList)
        {
          if (myEntity is MyCubeGrid myCubeGrid && myCubeGrid.GridSystems.GasSystem != null)
          {
            MyOxygenBlock safeOxygenBlock = myCubeGrid.GridSystems.GasSystem.GetSafeOxygenBlock(worldPosition);
            if (safeOxygenBlock != null && safeOxygenBlock.Room != null && safeOxygenBlock.Room.IsAirtight)
            {
              flag = true;
              break;
            }
          }
        }
      }
      finally
      {
        myEntityList?.Clear();
      }
      return flag;
    }

    private class Sandbox_Game_Components_MyRenderComponentPlanet\u003C\u003EActor : IActivator, IActivator<MyRenderComponentPlanet>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentPlanet();

      MyRenderComponentPlanet IActivator<MyRenderComponentPlanet>.CreateInstance() => new MyRenderComponentPlanet();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyBlockBuilderRenderData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using System.Collections.Generic;
using VRage.Game.Models;
using VRage.Generics;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
  public class MyBlockBuilderRenderData
  {
    private static MyObjectsPool<MyBlockBuilderRenderData.MyEntities> m_entitiesPool = new MyObjectsPool<MyBlockBuilderRenderData.MyEntities>(1);
    private Dictionary<int, MyBlockBuilderRenderData.MyEntities> m_allEntities = new Dictionary<int, MyBlockBuilderRenderData.MyEntities>();
    private List<int> m_tmpRemovedModels = new List<int>();
    private float Transparency = MyFakes.ENABLE_TRANSPARENT_CUBE_BUILDER ? 0.25f : 0.0f;

    public void UnloadRenderObjects()
    {
      foreach (KeyValuePair<int, MyBlockBuilderRenderData.MyEntities> allEntity in this.m_allEntities)
        allEntity.Value.Clear();
      this.m_allEntities.Clear();
    }

    public void BeginCollectingInstanceData()
    {
      foreach (KeyValuePair<int, MyBlockBuilderRenderData.MyEntities> allEntity in this.m_allEntities)
        allEntity.Value.PrepareCollecting();
    }

    public void AddInstance(
      int model,
      MatrixD matrix,
      ref MatrixD invGridWorldMatrix,
      Vector3 colorMaskHsv = default (Vector3),
      MyStringHash? skinId = null,
      Vector3UByte[] bones = null,
      float gridSize = 1f)
    {
      MyBlockBuilderRenderData.MyEntities myEntities1;
      if (!this.m_allEntities.TryGetValue(model, out myEntities1))
      {
        MyBlockBuilderRenderData.m_entitiesPool.AllocateOrCreate(out myEntities1);
        this.m_allEntities.Add(model, myEntities1);
      }
      MyBlockBuilderRenderData.MyEntities myEntities2 = myEntities1;
      int model1 = model;
      MatrixD matrixD = matrix * invGridWorldMatrix;
      Matrix localMatrix = (Matrix) ref matrixD;
      Vector3 colorMaskHsv1 = colorMaskHsv;
      MyStringHash? skinId1 = skinId;
      double transparency = (double) this.Transparency;
      myEntities2.AddModel(model1, localMatrix, colorMaskHsv1, skinId1, (float) transparency);
    }

    public void EndCollectingInstanceData(MatrixD gridWorldMatrix, bool useTransparency)
    {
      foreach (KeyValuePair<int, MyBlockBuilderRenderData.MyEntities> allEntity in this.m_allEntities)
      {
        allEntity.Value.ShrinkRenderEnties();
        if (allEntity.Value.IsEmpty())
          this.m_tmpRemovedModels.Add(allEntity.Key);
      }
      foreach (int tmpRemovedModel in this.m_tmpRemovedModels)
        this.m_allEntities.Remove(tmpRemovedModel);
      this.m_tmpRemovedModels.Clear();
      float transparency = useTransparency ? this.Transparency : 0.0f;
      foreach (KeyValuePair<int, MyBlockBuilderRenderData.MyEntities> allEntity in this.m_allEntities)
        allEntity.Value.Update(gridWorldMatrix, transparency);
    }

    private struct MyRenderEntity
    {
      public uint RenderEntityId;
      public Matrix LocalMatrix;
      public Vector3 ColorMashHsv;
      public MyStringHash SkinId;
      public bool UpdateSkin;

      public void Update(MatrixD gridWorldMatrix, float transparency)
      {
        MyRenderProxy.UpdateRenderObject(this.RenderEntityId, new MatrixD?(this.LocalMatrix * gridWorldMatrix));
        MyRenderProxy.UpdateRenderEntity(this.RenderEntityId, new Color?((Color) Vector3.One), new Vector3?(this.ColorMashHsv), new float?(transparency));
        if (!this.UpdateSkin)
          return;
        MyDefinitionManager.MyAssetModifiers definitionForRender = MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(this.SkinId);
        if (definitionForRender.SkinTextureChanges != null)
          MyRenderProxy.ChangeMaterialTexture(this.RenderEntityId, definitionForRender.SkinTextureChanges);
        this.UpdateSkin = false;
      }
    }

    [GenerateActivator]
    private class MyEntities
    {
      private List<MyBlockBuilderRenderData.MyRenderEntity> RenderEntities = new List<MyBlockBuilderRenderData.MyRenderEntity>();
      private int NumUsedModels;

      public bool IsEmpty() => this.NumUsedModels == 0;

      public void ShrinkRenderEnties()
      {
        for (int numUsedModels = this.NumUsedModels; numUsedModels < this.RenderEntities.Count; ++numUsedModels)
          MyRenderProxy.RemoveRenderObject(this.RenderEntities[numUsedModels].RenderEntityId, MyRenderProxy.ObjectType.Entity);
        this.RenderEntities.RemoveRange(this.NumUsedModels, this.RenderEntities.Count - this.NumUsedModels);
      }

      public void Clear()
      {
        this.NumUsedModels = 0;
        this.ShrinkRenderEnties();
      }

      public void PrepareCollecting() => this.NumUsedModels = 0;

      public void Update(MatrixD gridWorldMatrix, float transparency)
      {
        foreach (MyBlockBuilderRenderData.MyRenderEntity renderEntity in this.RenderEntities)
          renderEntity.Update(gridWorldMatrix, transparency);
      }

      public void AddModel(
        int model,
        Matrix localMatrix,
        Vector3 colorMaskHsv,
        MyStringHash? skinId,
        float transparency)
      {
        RenderFlags flags = (RenderFlags) (0 | 16);
        if (skinId.HasValue)
        {
          if (MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(skinId.Value).MetalnessColorable)
            flags |= RenderFlags.MetalnessColorable;
          else
            flags &= ~RenderFlags.MetalnessColorable;
        }
        if (this.RenderEntities.Count < ++this.NumUsedModels)
        {
          this.AddRenderEntity(model, localMatrix, colorMaskHsv, skinId, transparency, flags);
        }
        else
        {
          MyBlockBuilderRenderData.MyRenderEntity renderEntity = this.RenderEntities[this.NumUsedModels - 1];
          renderEntity.LocalMatrix = localMatrix;
          renderEntity.ColorMashHsv = colorMaskHsv;
          if (skinId.HasValue)
          {
            MyStringHash? nullable = skinId;
            MyStringHash skinId1 = renderEntity.SkinId;
            if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != skinId1 ? 1 : 0) : 0) : 1) != 0)
            {
              this.RenderEntities.RemoveAt(this.NumUsedModels - 1);
              MyRenderProxy.RemoveRenderObject(renderEntity.RenderEntityId, MyRenderProxy.ObjectType.Entity);
              this.AddRenderEntity(model, localMatrix, colorMaskHsv, skinId, transparency, flags);
              return;
            }
          }
          renderEntity.UpdateSkin = false;
          this.RenderEntities[this.NumUsedModels - 1] = renderEntity;
        }
      }

      private void AddRenderEntity(
        int model,
        Matrix localMatrix,
        Vector3 colorMaskHsv,
        MyStringHash? skinId,
        float transparency,
        RenderFlags flags)
      {
        string byId = MyModel.GetById(model);
        uint renderEntity = MyRenderProxy.CreateRenderEntity("Cube builder, part: " + (object) model, byId, MatrixD.Identity, MyMeshDrawTechnique.MESH, flags, CullingOptions.Default, (Color) Vector3.One, colorMaskHsv, transparency);
        this.RenderEntities.Add(new MyBlockBuilderRenderData.MyRenderEntity()
        {
          LocalMatrix = localMatrix,
          RenderEntityId = renderEntity,
          ColorMashHsv = colorMaskHsv,
          SkinId = skinId.Value,
          UpdateSkin = true
        });
      }

      private class Sandbox_Game_Entities_Cube_MyBlockBuilderRenderData\u003C\u003EMyEntities\u003C\u003EActor : IActivator, IActivator<MyBlockBuilderRenderData.MyEntities>
      {
        object IActivator.CreateInstance() => (object) new MyBlockBuilderRenderData.MyEntities();

        MyBlockBuilderRenderData.MyEntities IActivator<MyBlockBuilderRenderData.MyEntities>.CreateInstance() => new MyBlockBuilderRenderData.MyEntities();
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.MyDestructionData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using System;
using System.Collections.Generic;
using System.IO;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Fractures;
using VRageRender.Messages;
using VRageRender.Utils;

namespace Sandbox
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MyDestructionData : MySessionComponentBase
  {
    private static List<HkdShapeInstanceInfo> m_tmpChildrenList = new List<HkdShapeInstanceInfo>();
    private static MyPhysicsMesh m_tmpMesh = new MyPhysicsMesh();
    private HkDestructionStorage Storage;
    private static Dictionary<string, MyPhysicalMaterialDefinition> m_physicalMaterials;

    public static MyDestructionData Static { get; set; }

    public HkWorld TemporaryWorld { get; private set; }

    public MyBlockShapePool BlockShapePool { get; private set; }

    public override bool IsRequiredByGame => MyPerGameSettings.Destruction;

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.BlockShapePool.RefillPools();
    }

    public override void LoadData()
    {
      if (!HkBaseSystem.DestructionEnabled)
      {
        MyLog.Default.WriteLine("Havok Destruction is not availiable in this build.");
        throw new InvalidOperationException("Havok Destruction is not availiable in this build.");
      }
      if (MyDestructionData.Static != null)
      {
        MyLog.Default.WriteLine("Destruction data was not freed. Unloading now...");
        this.UnloadData();
      }
      MyDestructionData.Static = this;
      this.BlockShapePool = new MyBlockShapePool();
      this.TemporaryWorld = new HkWorld(true, 50000f, MyPhysics.RestingVelocity, MyFakes.ENABLE_HAVOK_MULTITHREADING, 4);
      this.TemporaryWorld.MarkForWrite();
      this.TemporaryWorld.DestructionWorld = new HkdWorld(this.TemporaryWorld);
      this.TemporaryWorld.UnmarkForWrite();
      this.Storage = new HkDestructionStorage(this.TemporaryWorld.DestructionWorld);
      foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
      {
        MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName);
        if (definitionGroup.Large != null)
        {
          MyModel model = MyModels.GetModel(definitionGroup.Large.Model);
          if (model != null)
          {
            if (!MyFakes.LAZY_LOAD_DESTRUCTION || model != null && model.HavokBreakableShapes != null)
              this.LoadModelDestruction(definitionGroup.Large.Model, (MyPhysicalModelDefinition) definitionGroup.Large, definitionGroup.Large.Size * MyDefinitionManager.Static.GetCubeSize(definitionGroup.Large.CubeSize));
            foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in definitionGroup.Large.BuildProgressModels)
            {
              model = MyModels.GetModel(buildProgressModel.File);
              if (model != null && (!MyFakes.LAZY_LOAD_DESTRUCTION || model != null && model.HavokBreakableShapes != null))
                this.LoadModelDestruction(buildProgressModel.File, (MyPhysicalModelDefinition) definitionGroup.Large, definitionGroup.Large.Size * MyDefinitionManager.Static.GetCubeSize(definitionGroup.Large.CubeSize));
            }
            if (MyFakes.CHANGE_BLOCK_CONVEX_RADIUS && model != null && model.HavokBreakableShapes != null)
            {
              HkShape shape = model.HavokBreakableShapes[0].GetShape();
              if (shape.ShapeType != HkShapeType.Sphere && shape.ShapeType != HkShapeType.Capsule)
                this.SetConvexRadius(model.HavokBreakableShapes[0], 0.05f);
            }
          }
          else
            continue;
        }
        if (definitionGroup.Small != null)
        {
          MyModel model = MyModels.GetModel(definitionGroup.Small.Model);
          if (model != null)
          {
            if (!MyFakes.LAZY_LOAD_DESTRUCTION || model != null && model.HavokBreakableShapes != null)
              this.LoadModelDestruction(definitionGroup.Small.Model, (MyPhysicalModelDefinition) definitionGroup.Small, definitionGroup.Small.Size * MyDefinitionManager.Static.GetCubeSize(definitionGroup.Small.CubeSize));
            foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in definitionGroup.Small.BuildProgressModels)
            {
              model = MyModels.GetModel(buildProgressModel.File);
              if (model != null && (!MyFakes.LAZY_LOAD_DESTRUCTION || model != null && model.HavokBreakableShapes != null))
                this.LoadModelDestruction(buildProgressModel.File, (MyPhysicalModelDefinition) definitionGroup.Small, definitionGroup.Large.Size * MyDefinitionManager.Static.GetCubeSize(definitionGroup.Large.CubeSize));
            }
            if (MyFakes.CHANGE_BLOCK_CONVEX_RADIUS && model != null && model.HavokBreakableShapes != null)
            {
              HkShape shape = model.HavokBreakableShapes[0].GetShape();
              if (shape.ShapeType != HkShapeType.Sphere && shape.ShapeType != HkShapeType.Capsule)
                this.SetConvexRadius(model.HavokBreakableShapes[0], 0.05f);
            }
          }
        }
      }
      if (!MyFakes.LAZY_LOAD_DESTRUCTION)
        this.BlockShapePool.Preallocate();
      foreach (MyPhysicalModelDefinition allDefinition in MyDefinitionManager.Static.GetAllDefinitions<MyPhysicalModelDefinition>())
        this.LoadModelDestruction(allDefinition.Model, allDefinition, Vector3.One, false, true);
    }

    protected override void UnloadData()
    {
      this.TemporaryWorld.MarkForWrite();
      this.Storage.Dispose();
      this.Storage = (HkDestructionStorage) null;
      this.TemporaryWorld.DestructionWorld.Dispose();
      this.TemporaryWorld.Dispose();
      this.TemporaryWorld = (HkWorld) null;
      this.BlockShapePool.Free();
      this.BlockShapePool = (MyBlockShapePool) null;
      MyDestructionData.Static = (MyDestructionData) null;
    }

    private HkReferenceObject CreateGeometryFromSplitPlane(string splitPlane)
    {
      MyModel modelOnlyData = MyModels.GetModelOnlyData(splitPlane);
      return modelOnlyData != null ? this.Storage.CreateGeometry(this.CreatePhysicsMesh(modelOnlyData), Path.GetFileNameWithoutExtension(splitPlane)) : (HkReferenceObject) null;
    }

    private void FractureBreakableShape(
      HkdBreakableShape bShape,
      MyModelFractures modelFractures,
      string modPath)
    {
      HkdFracture fracture1 = (HkdFracture) null;
      HkReferenceObject data = (HkReferenceObject) null;
      if (modelFractures.Fractures[0] is RandomSplitFractureSettings)
      {
        RandomSplitFractureSettings fracture2 = (RandomSplitFractureSettings) modelFractures.Fractures[0];
        fracture1 = (HkdFracture) new HkdRandomSplitFracture()
        {
          NumObjectsOnLevel1 = fracture2.NumObjectsOnLevel1,
          NumObjectsOnLevel2 = fracture2.NumObjectsOnLevel2,
          RandomRange = fracture2.RandomRange,
          RandomSeed1 = fracture2.RandomSeed1,
          RandomSeed2 = fracture2.RandomSeed2,
          SplitGeometryScale = Vector4.One
        };
        if (!string.IsNullOrEmpty(fracture2.SplitPlane))
        {
          string str = fracture2.SplitPlane;
          if (!string.IsNullOrEmpty(modPath))
            str = Path.Combine(modPath, fracture2.SplitPlane);
          data = this.CreateGeometryFromSplitPlane(str);
          if (data != (HkReferenceObject) null)
          {
            ((HkdRandomSplitFracture) fracture1).SetGeometry(data);
            MyRenderProxy.PreloadMaterials(str);
          }
        }
      }
      if (modelFractures.Fractures[0] is VoronoiFractureSettings)
      {
        VoronoiFractureSettings fracture2 = (VoronoiFractureSettings) modelFractures.Fractures[0];
        fracture1 = (HkdFracture) new HkdVoronoiFracture()
        {
          Seed = fracture2.Seed,
          NumSitesToGenerate = fracture2.NumSitesToGenerate,
          NumIterations = fracture2.NumIterations
        };
        if (!string.IsNullOrEmpty(fracture2.SplitPlane))
        {
          string str = fracture2.SplitPlane;
          if (!string.IsNullOrEmpty(modPath))
            str = Path.Combine(modPath, fracture2.SplitPlane);
          data = this.CreateGeometryFromSplitPlane(str);
          MyModels.GetModel(str);
          if (data != (HkReferenceObject) null)
          {
            ((HkdVoronoiFracture) fracture1).SetGeometry(data);
            MyRenderProxy.PreloadMaterials(str);
          }
        }
      }
      if (modelFractures.Fractures[0] is WoodFractureSettings)
      {
        WoodFractureSettings fracture2 = (WoodFractureSettings) modelFractures.Fractures[0];
        fracture1 = (HkdFracture) new HkdWoodFracture();
      }
      if ((HkReferenceObject) fracture1 != (HkReferenceObject) null)
      {
        this.Storage.FractureShape(bShape, fracture1);
        fracture1.Dispose();
      }
      if (!(data != (HkReferenceObject) null))
        return;
      data.Dispose();
    }

    private IPhysicsMesh CreatePhysicsMesh(MyModel model)
    {
      IPhysicsMesh physicsMesh = (IPhysicsMesh) new MyPhysicsMesh();
      physicsMesh.SetAABB(model.BoundingBox.Min, model.BoundingBox.Max);
      for (int vertexIndex = 0; vertexIndex < model.GetVerticesCount(); ++vertexIndex)
      {
        Vector3 vertex = model.GetVertex(vertexIndex);
        Vector3 vertexNormal = model.GetVertexNormal(vertexIndex);
        Vector3 vertexTangent = model.GetVertexTangent(vertexIndex);
        if (model.TexCoords == null)
          model.LoadTexCoordData();
        Vector2 vector2 = model.TexCoords[vertexIndex].ToVector2();
        physicsMesh.AddVertex(vertex, vertexNormal, vertexTangent, vector2);
      }
      for (int index = 0; index < model.Indices16.Length; ++index)
        physicsMesh.AddIndex((int) model.Indices16[index]);
      for (int index = 0; index < model.GetMeshList().Count; ++index)
      {
        VRageRender.Models.MyMesh mesh = model.GetMeshList()[index];
        physicsMesh.AddSectionData(mesh.IndexStart, mesh.TriCount, mesh.Material.Name);
      }
      return physicsMesh;
    }

    private void CreateBreakableShapeFromCollisionShapes(
      MyModel model,
      Vector3 defaultSize,
      MyPhysicalModelDefinition modelDef)
    {
      HkShape shape;
      if (model.HavokCollisionShapes != null && model.HavokCollisionShapes.Length != 0)
      {
        if (model.HavokCollisionShapes.Length > 1)
        {
          shape = (HkShape) HkListShape.Create(model.HavokCollisionShapes, model.HavokCollisionShapes.Length, HkReferencePolicy.None);
        }
        else
        {
          shape = model.HavokCollisionShapes[0];
          shape.AddReference();
        }
      }
      else
        shape = (HkShape) new HkBoxShape(defaultSize * 0.5f, MyPerGameSettings.PhysicsConvexRadius);
      HkdBreakableShape hkdBreakableShape = new HkdBreakableShape(shape);
      hkdBreakableShape.Name = model.AssetName;
      hkdBreakableShape.SetMass(modelDef.Mass);
      model.HavokBreakableShapes = new HkdBreakableShape[1]
      {
        hkdBreakableShape
      };
      shape.RemoveReference();
    }

    public void LoadModelDestruction(
      string modelName,
      MyPhysicalModelDefinition modelDef,
      Vector3 defaultSize,
      bool destructionRequired = true,
      bool useShapeVolume = false)
    {
      MyModel modelOnlyData = MyModels.GetModelOnlyData(modelName);
      if (modelOnlyData.HavokBreakableShapes != null)
        return;
      bool flag1 = false;
      if (modelDef is MyCubeBlockDefinition cubeBlockDefinition)
        flag1 = !cubeBlockDefinition.CreateFracturedPieces;
      MyPhysicalMaterialDefinition physicalMaterial = modelDef.PhysicalMaterial;
      string str = modelName;
      if (modelOnlyData == null)
        return;
      bool flag2 = false;
      modelOnlyData.LoadUV = true;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      if (modelOnlyData.ModelFractures != null)
      {
        if (modelOnlyData.HavokCollisionShapes != null && modelOnlyData.HavokCollisionShapes.Length != 0)
        {
          this.CreateBreakableShapeFromCollisionShapes(modelOnlyData, defaultSize, modelDef);
          this.Storage.RegisterShapeWithGraphics(this.CreatePhysicsMesh(modelOnlyData), modelOnlyData.HavokBreakableShapes[0], str);
          string modPath = (string) null;
          if (Path.IsPathRooted(modelOnlyData.AssetName))
            modPath = modelOnlyData.AssetName.Remove(modelOnlyData.AssetName.LastIndexOf("Models"));
          this.FractureBreakableShape(modelOnlyData.HavokBreakableShapes[0], modelOnlyData.ModelFractures, modPath);
          flag4 = true;
          flag5 = true;
          flag3 = true;
        }
      }
      else if (modelOnlyData.HavokDestructionData != null)
      {
        if (!flag2)
        {
          try
          {
            if (modelOnlyData.HavokBreakableShapes == null)
            {
              modelOnlyData.HavokBreakableShapes = this.Storage.LoadDestructionDataFromBuffer(modelOnlyData.HavokDestructionData);
              flag3 = true;
              flag4 = true;
              flag5 = true;
            }
          }
          catch
          {
            modelOnlyData.HavokBreakableShapes = (HkdBreakableShape[]) null;
          }
        }
      }
      modelOnlyData.HavokDestructionData = (byte[]) null;
      modelOnlyData.HavokData = (byte[]) null;
      if (modelOnlyData.HavokBreakableShapes == null & destructionRequired)
      {
        MyLog.Default.WriteLine(modelOnlyData.AssetName + " does not have destruction data");
        this.CreateBreakableShapeFromCollisionShapes(modelOnlyData, defaultSize, modelDef);
        flag4 = true;
        flag5 = true;
      }
      if (modelOnlyData.HavokBreakableShapes == null)
      {
        MyLog.Default.WriteLine(string.Format("Model {0} - Unable to load havok destruction data", (object) modelOnlyData.AssetName), LoggingOptions.LOADING_MODELS);
      }
      else
      {
        HkdBreakableShape havokBreakableShape = modelOnlyData.HavokBreakableShapes[0];
        if (flag1)
          havokBreakableShape.SetFlagRecursively(HkdBreakableShape.Flags.DONT_CREATE_FRACTURE_PIECE);
        if (flag5)
        {
          havokBreakableShape.AddReference();
          this.Storage.RegisterShape(havokBreakableShape, str);
        }
        MyRenderProxy.PreloadMaterials(modelOnlyData.AssetName);
        if (flag3)
          this.CreatePieceData(modelOnlyData, havokBreakableShape);
        if (flag4)
        {
          float num = havokBreakableShape.CalculateGeometryVolume();
          if ((double) num <= 0.0 | useShapeVolume)
            num = havokBreakableShape.Volume;
          float m = num * physicalMaterial.Density;
          havokBreakableShape.SetMassRecursively(MyDestructionHelper.MassToHavok(m));
        }
        if ((double) modelDef.Mass > 0.0)
          havokBreakableShape.SetMassRecursively(MyDestructionHelper.MassToHavok(modelDef.Mass));
        this.DisableRefCountRec(havokBreakableShape);
        if (MyFakes.CHANGE_BLOCK_CONVEX_RADIUS && modelOnlyData != null && modelOnlyData.HavokBreakableShapes != null)
        {
          HkShape shape = modelOnlyData.HavokBreakableShapes[0].GetShape();
          if (shape.ShapeType != HkShapeType.Sphere && shape.ShapeType != HkShapeType.Capsule)
            this.SetConvexRadius(modelOnlyData.HavokBreakableShapes[0], 0.05f);
        }
        if (!MyFakes.LAZY_LOAD_DESTRUCTION)
          return;
        this.BlockShapePool.AllocateForDefinition(str, modelDef, 50);
      }
    }

    private void SetConvexRadius(HkdBreakableShape bShape, float radius)
    {
      HkShape shape = bShape.GetShape();
      if (shape.IsConvex)
      {
        HkConvexShape hkConvexShape = (HkConvexShape) shape;
        if ((double) hkConvexShape.ConvexRadius <= (double) radius)
          return;
        hkConvexShape.ConvexRadius = radius;
      }
      else
      {
        if (!shape.IsContainer())
          return;
        HkShapeContainerIterator container = shape.GetContainer();
        while (container.IsValid)
        {
          if (container.CurrentValue.IsConvex)
          {
            HkConvexShape currentValue = (HkConvexShape) container.CurrentValue;
            if ((double) currentValue.ConvexRadius > (double) radius)
              currentValue.ConvexRadius = radius;
          }
          container.Next();
        }
      }
    }

    private bool CheckVolumeMassRec(HkdBreakableShape bShape, float minVolume, float minMass)
    {
      if (bShape.Name.Contains("Fake"))
        return true;
      if ((double) bShape.Volume <= (double) minVolume)
        return false;
      HkMassProperties massProperties = new HkMassProperties();
      bShape.BuildMassProperties(ref massProperties);
      if ((double) massProperties.Mass <= (double) minMass || (double) massProperties.InertiaTensor.M11 == 0.0 || ((double) massProperties.InertiaTensor.M22 == 0.0 || (double) massProperties.InertiaTensor.M33 == 0.0))
        return false;
      for (int i = 0; i < bShape.GetChildrenCount(); ++i)
      {
        if (!this.CheckVolumeMassRec(bShape.GetChildShape(i), minVolume, minMass))
          return false;
      }
      return true;
    }

    public static MyPhysicalMaterialDefinition GetPhysicalMaterial(
      MyPhysicalModelDefinition modelDef,
      string physicalMaterial)
    {
      if (MyDestructionData.m_physicalMaterials == null)
      {
        MyDestructionData.m_physicalMaterials = new Dictionary<string, MyPhysicalMaterialDefinition>();
        foreach (MyPhysicalMaterialDefinition materialDefinition in MyDefinitionManager.Static.GetPhysicalMaterialDefinitions())
          MyDestructionData.m_physicalMaterials.Add(materialDefinition.Id.SubtypeName, materialDefinition);
        MyDestructionData.m_physicalMaterials["Default"] = new MyPhysicalMaterialDefinition()
        {
          Density = 1920f,
          HorisontalTransmissionMultiplier = 1f,
          HorisontalFragility = 2f,
          CollisionMultiplier = 1.4f,
          SupportMultiplier = 1.5f
        };
      }
      if (!string.IsNullOrEmpty(physicalMaterial))
      {
        if (MyDestructionData.m_physicalMaterials.ContainsKey(physicalMaterial))
          return MyDestructionData.m_physicalMaterials[physicalMaterial];
        MyLog.Default.WriteLine("ERROR: Physical material " + physicalMaterial + " does not exist!");
      }
      if (modelDef.Id.SubtypeName.Contains("Stone") && MyDestructionData.m_physicalMaterials.ContainsKey("Stone"))
        return MyDestructionData.m_physicalMaterials["Stone"];
      return modelDef.Id.SubtypeName.Contains("Wood") && MyDestructionData.m_physicalMaterials.ContainsKey("Wood") || modelDef.Id.SubtypeName.Contains("Timber") && MyDestructionData.m_physicalMaterials.ContainsKey("Timber") ? MyDestructionData.m_physicalMaterials["Wood"] : MyDestructionData.m_physicalMaterials["Default"];
    }

    private void DisableRefCountRec(HkdBreakableShape bShape)
    {
      bShape.DisableRefCount();
      List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
      bShape.GetChildren(list);
      foreach (HkdShapeInstanceInfo shapeInstanceInfo in list)
        this.DisableRefCountRec(shapeInstanceInfo.Shape);
    }

    private void CreatePieceData(MyModel model, HkdBreakableShape breakableShape)
    {
      MyRenderMessageAddRuntimeModel message = MyRenderProxy.PrepareAddRuntimeModel();
      MyDestructionData.m_tmpMesh.Data = message.ModelData;
      MyDestructionData.Static.Storage.GetDataFromShape(breakableShape, (IPhysicsMesh) MyDestructionData.m_tmpMesh);
      if (message.ModelData.Sections.Count > 0)
      {
        if (MyFakes.USE_HAVOK_MODELS)
          message.ReplacedModel = model.AssetName;
        MyRenderProxy.AddRuntimeModel(breakableShape.ShapeName, message);
      }
      using (MyDestructionData.m_tmpChildrenList.GetClearToken<HkdShapeInstanceInfo>())
      {
        breakableShape.GetChildren(MyDestructionData.m_tmpChildrenList);
        MyDestructionData.LoadChildrenShapes(MyDestructionData.m_tmpChildrenList);
      }
    }

    private static void LoadChildrenShapes(List<HkdShapeInstanceInfo> children)
    {
      foreach (HkdShapeInstanceInfo child in children)
      {
        if (child.IsValid())
        {
          MyRenderMessageAddRuntimeModel message = MyRenderProxy.PrepareAddRuntimeModel();
          MyDestructionData.m_tmpMesh.Data = message.ModelData;
          MyDestructionData.Static.Storage.GetDataFromShapeInstance(child, (IPhysicsMesh) MyDestructionData.m_tmpMesh);
          MyDestructionData.m_tmpMesh.Transform(child.GetTransform());
          if (message.ModelData.Sections.Count > 0)
            MyRenderProxy.AddRuntimeModel(child.ShapeName, message);
          List<HkdShapeInstanceInfo> shapeInstanceInfoList = new List<HkdShapeInstanceInfo>();
          child.GetChildren(shapeInstanceInfoList);
          MyDestructionData.LoadChildrenShapes(shapeInstanceInfoList);
        }
      }
    }

    public float GetBlockMass(string model, MyCubeBlockDefinition def)
    {
      HkdBreakableShape breakableShape = this.BlockShapePool.GetBreakableShape(model, def);
      double mass = (double) breakableShape.GetMass();
      this.BlockShapePool.EnqueShape(model, def.Id, breakableShape);
      return (float) mass;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyUseObjectsComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Components
{
  [MyComponentBuilder(typeof (MyObjectBuilder_UseObjectsComponent), true)]
  public class MyUseObjectsComponent : MyUseObjectsComponentBase
  {
    [ThreadStatic]
    private static Vector3[] m_detectorVertices;
    [ThreadStatic]
    private static MyList<HkShape> m_shapes;
    private Dictionary<uint, MyUseObjectsComponent.DetectorData> m_detectorInteractiveObjects = new Dictionary<uint, MyUseObjectsComponent.DetectorData>();
    private Dictionary<string, uint> m_detectorShapeKeys = new Dictionary<string, uint>();
    private List<uint> m_customAddedDetectors = new List<uint>();
    private MyPhysicsBody m_detectorPhysics;
    private MyObjectBuilder_UseObjectsComponent m_objectBuilder;
    private MyUseObjectsComponentDefinition m_definition;

    public override MyPhysicsComponentBase DetectorPhysics
    {
      get => (MyPhysicsComponentBase) this.m_detectorPhysics;
      protected set => this.m_detectorPhysics = value as MyPhysicsBody;
    }

    public override void LoadDetectorsFromModel()
    {
      this.m_detectors.Clear();
      this.m_detectorInteractiveObjects.Clear();
      if (this.m_detectorPhysics != null)
        this.m_detectorPhysics.Close();
      MyRenderComponentBase renderComponentBase = this.Container.Get<MyRenderComponentBase>();
      if (renderComponentBase.GetModel() != null)
      {
        foreach (KeyValuePair<string, MyModelDummy> dummy in renderComponentBase.GetModel().Dummies)
        {
          string lower = dummy.Key.ToLower();
          if (lower.StartsWith("detector_") && lower.Length > "detector_".Length)
          {
            string[] strArray = lower.Split('_');
            if (strArray.Length >= 2)
            {
              MyModelDummy dummyData = dummy.Value;
              int num = (int) this.AddDetector(strArray[1], lower, dummyData);
            }
          }
        }
      }
      if (this.m_detectorInteractiveObjects.Count <= 0)
        return;
      this.RecreatePhysics();
    }

    private IMyUseObject CreateInteractiveObject(
      string detectorName,
      string dummyName,
      MyModelDummy dummyData,
      uint shapeKey)
    {
      return this.Container.Entity is MyDoor && detectorName == "terminal" ? (IMyUseObject) new MyUseObjectDoorTerminal(this.Container.Entity, dummyName, dummyData, shapeKey) : MyUseObjectFactory.CreateUseObject(detectorName, this.Container.Entity, dummyName, dummyData, shapeKey);
    }

    private uint AddDetector(string detectorName, string dummyName, MyModelDummy dummyData)
    {
      List<Matrix> matrixList;
      if (!this.m_detectors.TryGetValue(detectorName, out matrixList))
      {
        matrixList = new List<Matrix>();
        this.m_detectors[detectorName] = matrixList;
      }
      Matrix matrix = dummyData.Matrix;
      if (this.Entity is MyCubeBlock)
      {
        float gridScale = (this.Entity as MyCubeBlock).CubeGrid.GridScale;
        matrix.Translation *= gridScale;
        Matrix.Rescale(ref matrix, gridScale);
      }
      matrixList.Add(Matrix.Invert(matrix));
      uint count = (uint) this.m_detectorInteractiveObjects.Count;
      IMyUseObject interactiveObject = this.CreateInteractiveObject(detectorName, dummyName, dummyData, count);
      if (interactiveObject != null)
      {
        this.m_detectorInteractiveObjects.Add(count, new MyUseObjectsComponent.DetectorData(interactiveObject, matrix, detectorName));
        this.m_detectorShapeKeys[detectorName] = count;
      }
      return count;
    }

    public override void RemoveDetector(uint id)
    {
      if (!this.m_detectorInteractiveObjects.ContainsKey(id))
        return;
      this.m_detectorShapeKeys.Remove(this.m_detectorInteractiveObjects[id].DetectorName);
      this.m_detectorInteractiveObjects.Remove(id);
    }

    public override uint AddDetector(string name, Matrix dummyMatrix)
    {
      string lower = name.ToLower();
      string str = "detector_" + lower;
      MyModel model = this.Container.Entity.Render.GetModel();
      Dictionary<string, object> dictionary = (Dictionary<string, object>) null;
      MyModelDummy myModelDummy;
      if (model != null && model.Dummies.TryGetValue(str, out myModelDummy))
        dictionary = myModelDummy.CustomData;
      MyModelDummy dummyData = new MyModelDummy()
      {
        Name = str,
        CustomData = dictionary,
        Matrix = dummyMatrix
      };
      uint num = this.AddDetector(lower, str, dummyData);
      this.m_customAddedDetectors.Add(num);
      return num;
    }

    public void SetUseObjectIDs(uint renderId, int instanceId)
    {
      foreach (KeyValuePair<uint, MyUseObjectsComponent.DetectorData> interactiveObject in this.m_detectorInteractiveObjects)
      {
        interactiveObject.Value.UseObject.SetRenderID(renderId);
        interactiveObject.Value.UseObject.SetInstanceID(instanceId);
      }
    }

    public override unsafe void RecreatePhysics()
    {
      if (this.m_detectorPhysics != null)
      {
        this.m_detectorPhysics.Close();
        this.m_detectorPhysics = (MyPhysicsBody) null;
      }
      if (MyUseObjectsComponent.m_shapes == null)
        MyUseObjectsComponent.m_shapes = new MyList<HkShape>();
      if (MyUseObjectsComponent.m_detectorVertices == null)
        MyUseObjectsComponent.m_detectorVertices = new Vector3[8];
      MyUseObjectsComponent.m_shapes.Clear();
      BoundingBox boundingBox = new BoundingBox(-Vector3.One / 2f, Vector3.One / 2f);
      MyPositionComponentBase positionComponentBase = this.Container.Get<MyPositionComponentBase>();
      foreach (KeyValuePair<uint, MyUseObjectsComponent.DetectorData> interactiveObject in this.m_detectorInteractiveObjects)
      {
        fixed (Vector3* corners = MyUseObjectsComponent.m_detectorVertices)
          boundingBox.GetCornersUnsafe(corners);
        for (int index = 0; index < 8; ++index)
          MyUseObjectsComponent.m_detectorVertices[index] = Vector3.Transform(MyUseObjectsComponent.m_detectorVertices[index], interactiveObject.Value.Matrix);
        MyUseObjectsComponent.m_shapes.Add((HkShape) new HkConvexVerticesShape(MyUseObjectsComponent.m_detectorVertices, 8, false, 0.0f));
      }
      if (MyUseObjectsComponent.m_shapes.Count <= 0)
        return;
      HkListShape hkListShape = new HkListShape(MyUseObjectsComponent.m_shapes.GetInternalArray(), MyUseObjectsComponent.m_shapes.Count, HkReferencePolicy.TakeOwnership);
      this.m_detectorPhysics = new MyPhysicsBody(this.Container.Entity, RigidBodyFlag.RBF_DISABLE_COLLISION_RESPONSE);
      this.m_detectorPhysics.CreateFromCollisionObject((HkShape) hkListShape, Vector3.Zero, positionComponentBase.WorldMatrixRef);
      hkListShape.Base.RemoveReference();
    }

    public override void PositionChanged(MyPositionComponentBase obj)
    {
      if (this.m_detectorPhysics == null)
        return;
      this.m_detectorPhysics.OnWorldPositionChanged((object) obj);
    }

    private void positionComponent_OnPositionChanged(MyPositionComponentBase obj) => this.m_detectorPhysics.OnWorldPositionChanged((object) obj);

    public override void ProcessComponentToUseObjectsAndDistances(
      ref Dictionary<IMyUseObject, Tuple<float, object>> output,
      Vector3D from,
      Vector3 dir,
      object hit)
    {
      foreach (KeyValuePair<uint, MyUseObjectsComponent.DetectorData> interactiveObject in this.m_detectorInteractiveObjects)
      {
        IMyUseObject useObject = interactiveObject.Value.UseObject;
        bool intersection;
        float? distanceFromLine = this.ComputeDistanceFromLine(interactiveObject.Value, from, dir, out intersection);
        if (!distanceFromLine.HasValue)
          break;
        if (output.ContainsKey(useObject))
        {
          double num = (double) output[useObject].Item1;
          float? nullable = distanceFromLine;
          double valueOrDefault = (double) nullable.GetValueOrDefault();
          if (num < valueOrDefault & nullable.HasValue)
            output[useObject] = new Tuple<float, object>((intersection ? -1f : 1f) * distanceFromLine.Value, hit);
        }
        else
          output.Add(useObject, new Tuple<float, object>((intersection ? -1f : 1f) * distanceFromLine.Value, hit));
      }
    }

    private float? ComputeDistanceFromLine(
      MyUseObjectsComponent.DetectorData detector,
      Vector3D from,
      Vector3 dir,
      out bool intersection)
    {
      MyPositionComponentBase positionComponentBase = this.Container.Get<MyPositionComponentBase>();
      RayD ray = new RayD(from, (Vector3D) dir);
      MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD(detector.Matrix * positionComponentBase.WorldMatrixRef);
      double? nullable = orientedBoundingBoxD.Intersects(ref ray);
      if (nullable.HasValue)
      {
        intersection = true;
        return new float?((float) nullable.Value);
      }
      double num = (double) orientedBoundingBoxD.Distance(ray);
      intersection = false;
      return new float?((float) num);
    }

    public override IMyUseObject RaycastDetectors(
      Vector3D worldFrom,
      Vector3D worldTo,
      out float parameter)
    {
      MyPositionComponentBase positionComponentBase = this.Container.Get<MyPositionComponentBase>();
      ref readonly MatrixD local = ref positionComponentBase.WorldMatrixNormalizedInv;
      RayD ray = new RayD(worldFrom, worldTo - worldFrom);
      IMyUseObject myUseObject = (IMyUseObject) null;
      parameter = float.MaxValue;
      foreach (KeyValuePair<uint, MyUseObjectsComponent.DetectorData> interactiveObject in this.m_detectorInteractiveObjects)
      {
        double? nullable = new MyOrientedBoundingBoxD(interactiveObject.Value.Matrix * positionComponentBase.WorldMatrixRef).Intersects(ref ray);
        if (nullable.HasValue && nullable.Value < (double) parameter)
        {
          parameter = (float) nullable.Value;
          myUseObject = interactiveObject.Value.UseObject;
        }
      }
      return myUseObject;
    }

    private uint? GetDetectorIdFromUseObject(IMyUseObject useObject)
    {
      foreach (KeyValuePair<uint, MyUseObjectsComponent.DetectorData> interactiveObject in this.m_detectorInteractiveObjects)
      {
        if (interactiveObject.Value.UseObject == useObject)
          return new uint?(interactiveObject.Key);
      }
      return new uint?();
    }

    public override MatrixD? GetDetectorTransformation(IMyUseObject useObject)
    {
      uint? detectorIdFromUseObject = this.GetDetectorIdFromUseObject(useObject);
      if (!detectorIdFromUseObject.HasValue)
        return new MatrixD?();
      uint? nullable1 = detectorIdFromUseObject;
      long? nullable2 = nullable1.HasValue ? new long?((long) nullable1.GetValueOrDefault()) : new long?();
      long count = (long) this.m_detectorInteractiveObjects.Count;
      if (!(nullable2.GetValueOrDefault() >= count & nullable2.HasValue))
      {
        uint? nullable3 = detectorIdFromUseObject;
        uint num = 0;
        if (!(nullable3.GetValueOrDefault() < num & nullable3.HasValue))
        {
          MyPositionComponentBase positionComponentBase = this.Container.Get<MyPositionComponentBase>();
          return new MatrixD?(this.m_detectorInteractiveObjects[detectorIdFromUseObject.Value].Matrix * positionComponentBase.WorldMatrixRef);
        }
      }
      return new MatrixD?();
    }

    public override IMyUseObject RaycastDetector(
      IMyUseObject useObject,
      Vector3D worldFrom,
      Vector3D worldTo,
      out float parameter)
    {
      parameter = 0.0f;
      uint? detectorIdFromUseObject = this.GetDetectorIdFromUseObject(useObject);
      if (!detectorIdFromUseObject.HasValue)
        return (IMyUseObject) null;
      uint? nullable1 = detectorIdFromUseObject;
      long? nullable2 = nullable1.HasValue ? new long?((long) nullable1.GetValueOrDefault()) : new long?();
      long count = (long) this.m_detectorInteractiveObjects.Count;
      if (!(nullable2.GetValueOrDefault() >= count & nullable2.HasValue))
      {
        nullable1 = detectorIdFromUseObject;
        uint num = 0;
        if (!(nullable1.GetValueOrDefault() < num & nullable1.HasValue))
        {
          MyPositionComponentBase positionComponentBase = this.Container.Get<MyPositionComponentBase>();
          RayD ray = new RayD(worldFrom, worldTo - worldFrom);
          IMyUseObject myUseObject = (IMyUseObject) null;
          parameter = float.MaxValue;
          MyUseObjectsComponent.DetectorData interactiveObject = this.m_detectorInteractiveObjects[detectorIdFromUseObject.Value];
          double? nullable3 = new MyOrientedBoundingBoxD(interactiveObject.Matrix * positionComponentBase.WorldMatrixRef).Intersects(ref ray);
          if (nullable3.HasValue && nullable3.Value < (double) parameter)
          {
            parameter = (float) nullable3.Value;
            myUseObject = interactiveObject.UseObject;
          }
          return myUseObject;
        }
      }
      return (IMyUseObject) null;
    }

    public override IMyUseObject GetInteractiveObject(uint shapeKey)
    {
      MyUseObjectsComponent.DetectorData detectorData;
      return !this.m_detectorInteractiveObjects.TryGetValue(shapeKey, out detectorData) ? (IMyUseObject) null : detectorData.UseObject;
    }

    public override IMyUseObject GetInteractiveObject(string detectorName)
    {
      uint shapeKey;
      return !this.m_detectorShapeKeys.TryGetValue(detectorName, out shapeKey) ? (IMyUseObject) null : this.GetInteractiveObject(shapeKey);
    }

    public override void GetInteractiveObjects<T>(List<T> objects)
    {
      foreach (KeyValuePair<uint, MyUseObjectsComponent.DetectorData> interactiveObject in this.m_detectorInteractiveObjects)
      {
        if (interactiveObject.Value.UseObject is T useObject)
          objects.Add(useObject);
      }
    }

    public override bool IsSerialized() => this.m_customAddedDetectors.Count > 0;

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_UseObjectsComponent objectBuilder = MyComponentFactory.CreateObjectBuilder((MyComponentBase) this) as MyObjectBuilder_UseObjectsComponent;
      objectBuilder.CustomDetectorsCount = (uint) this.m_customAddedDetectors.Count;
      int index = 0;
      if (objectBuilder.CustomDetectorsCount > 0U)
      {
        objectBuilder.CustomDetectorsMatrices = new Matrix[(int) objectBuilder.CustomDetectorsCount];
        objectBuilder.CustomDetectorsNames = new string[(int) objectBuilder.CustomDetectorsCount];
        foreach (uint customAddedDetector in this.m_customAddedDetectors)
        {
          objectBuilder.CustomDetectorsNames[index] = this.m_detectorInteractiveObjects[customAddedDetector].DetectorName;
          objectBuilder.CustomDetectorsMatrices[index] = this.m_detectorInteractiveObjects[customAddedDetector].Matrix;
          ++index;
        }
      }
      return (MyObjectBuilder_ComponentBase) objectBuilder;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      base.Deserialize(builder);
      this.m_objectBuilder = builder as MyObjectBuilder_UseObjectsComponent;
    }

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      if (this.m_definition != null)
      {
        if (this.m_definition.LoadFromModel)
          this.LoadDetectorsFromModel();
        if (this.m_definition.UseObjectFromModelBBox != null)
        {
          int num = (int) this.AddDetector(this.m_definition.UseObjectFromModelBBox, Matrix.CreateScale(this.Entity.PositionComp.LocalAABB.Size) * Matrix.CreateTranslation(this.Entity.PositionComp.LocalAABB.Center));
        }
      }
      if (this.m_objectBuilder != null)
      {
        for (int index = 0; (long) index < (long) this.m_objectBuilder.CustomDetectorsCount; ++index)
        {
          if (!this.m_detectors.ContainsKey(this.m_objectBuilder.CustomDetectorsNames[index]))
          {
            int num = (int) this.AddDetector(this.m_objectBuilder.CustomDetectorsNames[index], this.m_objectBuilder.CustomDetectorsMatrices[index]);
          }
        }
      }
      this.RecreatePhysics();
    }

    public override void Init(MyComponentDefinitionBase definition)
    {
      base.Init(definition);
      this.m_definition = definition as MyUseObjectsComponentDefinition;
    }

    public struct DetectorData
    {
      public IMyUseObject UseObject;
      public Matrix Matrix;
      public string DetectorName;

      public DetectorData(IMyUseObject useObject, Matrix mat, string name)
      {
        this.UseObject = useObject;
        this.Matrix = mat;
        this.DetectorName = name;
      }
    }

    private class Sandbox_Game_Components_MyUseObjectsComponent\u003C\u003EActor : IActivator, IActivator<MyUseObjectsComponent>
    {
      object IActivator.CreateInstance() => (object) new MyUseObjectsComponent();

      MyUseObjectsComponent IActivator<MyUseObjectsComponent>.CreateInstance() => new MyUseObjectsComponent();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyUseObjectsComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Entity.UseObject;
using VRageMath;

namespace VRage.Game.Components
{
  [MyComponentType(typeof (MyUseObjectsComponentBase))]
  public abstract class MyUseObjectsComponentBase : MyEntityComponentBase
  {
    protected Dictionary<string, List<Matrix>> m_detectors = new Dictionary<string, List<Matrix>>();

    public abstract MyPhysicsComponentBase DetectorPhysics { get; protected set; }

    public abstract uint AddDetector(string name, Matrix matrix);

    public abstract void RemoveDetector(uint id);

    public abstract void RecreatePhysics();

    public abstract void LoadDetectorsFromModel();

    public abstract IMyUseObject GetInteractiveObject(uint shapeKey);

    public abstract IMyUseObject GetInteractiveObject(string detectorName);

    public abstract void GetInteractiveObjects<T>(List<T> objects) where T : class, IMyUseObject;

    public string RaycastDetectors(Vector3D worldFrom, Vector3D worldTo)
    {
      MatrixD matrix1 = this.Container.Get<MyPositionComponentBase>().WorldMatrixNormalizedInv;
      Vector3D position1 = Vector3D.Transform(worldFrom, matrix1);
      Vector3D position2 = Vector3D.Transform(worldTo, matrix1);
      BoundingBox boundingBox = new BoundingBox(-Vector3.One, Vector3.One);
      string str = (string) null;
      float maxValue = float.MaxValue;
      foreach (KeyValuePair<string, List<Matrix>> detector in this.m_detectors)
      {
        foreach (Matrix matrix2 in detector.Value)
        {
          Vector3 position3 = (Vector3) Vector3D.Transform(position1, matrix2);
          Vector3 direction = (Vector3) Vector3D.Transform(position2, matrix2);
          float? nullable = boundingBox.Intersects(new Ray(position3, direction));
          if (nullable.HasValue && (double) nullable.Value < (double) maxValue)
          {
            maxValue = nullable.Value;
            str = detector.Key;
          }
        }
      }
      return str;
    }

    public abstract IMyUseObject RaycastDetectors(
      Vector3D worldFrom,
      Vector3D worldTo,
      out float parameter);

    public abstract IMyUseObject RaycastDetector(
      IMyUseObject useObject,
      Vector3D worldFrom,
      Vector3D worldTo,
      out float parameter);

    public abstract MatrixD? GetDetectorTransformation(IMyUseObject useObject);

    public ListReader<Matrix> GetDetectors(string detectorName)
    {
      List<Matrix> list = (List<Matrix>) null;
      this.m_detectors.TryGetValue(detectorName, out list);
      return list == null || list.Count == 0 ? ListReader<Matrix>.Empty : new ListReader<Matrix>(list);
    }

    public virtual void ProcessComponentToUseObjectsAndDistances(
      ref Dictionary<IMyUseObject, Tuple<float, object>> output,
      Vector3D from,
      Vector3 dir,
      object hit)
    {
    }

    public virtual void ClearPhysics()
    {
      if (this.DetectorPhysics == null)
        return;
      this.DetectorPhysics.Close();
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      this.ClearPhysics();
    }

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      if (this.DetectorPhysics == null)
        return;
      this.DetectorPhysics.Activate();
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
      if (this.DetectorPhysics == null)
        return;
      this.DetectorPhysics.Deactivate();
    }

    public abstract void PositionChanged(MyPositionComponentBase obj);

    public override string ComponentTypeDebugString => "Use Objects";
  }
}

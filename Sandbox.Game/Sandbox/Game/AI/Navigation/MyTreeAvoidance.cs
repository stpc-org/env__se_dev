// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Navigation.MyTreeAvoidance
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using System.Collections.Generic;
using System.Diagnostics;
using VRageMath;

namespace Sandbox.Game.AI.Navigation
{
  public class MyTreeAvoidance : MySteeringBase
  {
    private readonly List<HkBodyCollision> m_trees = new List<HkBodyCollision>();

    public MyTreeAvoidance(MyBotNavigation navigation, float weight)
      : base(navigation, weight)
    {
    }

    public override string GetName() => "Tree avoidance steering";

    public override void AccumulateCorrection(ref Vector3 correction, ref float weight)
    {
      if ((double) this.Parent.Speed < 0.01)
        return;
      Vector3D translation = this.Parent.PositionAndOrientation.Translation;
      Quaternion identity = Quaternion.Identity;
      MyPhysics.GetPenetrationsShape((HkShape) new HkSphereShape(6f), ref translation, ref identity, this.m_trees, 9);
      foreach (HkBodyCollision tree in this.m_trees)
      {
        if (!((HkReferenceObject) tree.Body == (HkReferenceObject) null) && tree.Body.UserObject is MyPhysicsBody userObject)
        {
          HkShape shape = tree.Body.GetShape();
          if (shape.ShapeType == HkShapeType.StaticCompound)
          {
            HkStaticCompoundShape staticCompoundShape = (HkStaticCompoundShape) shape;
            int instanceId;
            staticCompoundShape.DecomposeShapeKey(tree.ShapeKey, out instanceId, out uint _);
            Vector3D vector3D1 = (Vector3D) staticCompoundShape.GetInstanceTransform(instanceId).Translation + userObject.GetWorldMatrix().Translation;
            Vector3D vector3D2 = vector3D1 - translation;
            double num = vector3D2.Normalize();
            vector3D2 = Vector3D.Reject((Vector3D) this.Parent.ForwardVector, vector3D2);
            vector3D2.Y = 0.0;
            if (vector3D2.Z * vector3D2.Z + vector3D2.X * vector3D2.X < 0.1)
            {
              Vector3D vector3D3 = Vector3D.TransformNormal(vector3D2, this.Parent.PositionAndOrientationInverted);
              vector3D2 = translation - vector3D1;
              vector3D2 = Vector3D.Cross(Vector3D.Up, vector3D2);
              if (vector3D3.X < 0.0)
                vector3D2 = -vector3D2;
            }
            vector3D2.Normalize();
            correction = (Vector3) (correction + (6.0 - num) * (double) this.Weight * vector3D2);
            if (!correction.IsValid())
              Debugger.Break();
          }
        }
      }
      this.m_trees.Clear();
      weight += this.Weight;
    }
  }
}

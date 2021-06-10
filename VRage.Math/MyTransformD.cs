// Decompiled with JetBrains decompiler
// Type: VRageMath.MyTransformD
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using VRage.Serialization;

namespace VRageMath
{
  public struct MyTransformD
  {
    [Serialize(MyPrimitiveFlags.Normalized)]
    public Quaternion Rotation;
    public Vector3D Position;

    public MatrixD TransformMatrix
    {
      get
      {
        MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(this.Rotation);
        fromQuaternion.Translation = this.Position;
        return fromQuaternion;
      }
    }

    public MyTransformD(Vector3D position)
      : this(ref position)
    {
    }

    public MyTransformD(MatrixD matrix)
      : this(ref matrix)
    {
    }

    public MyTransformD(ref Vector3D position)
    {
      this.Rotation = Quaternion.Identity;
      this.Position = position;
    }

    public MyTransformD(ref MatrixD matrix)
    {
      Quaternion.CreateFromRotationMatrix(ref matrix, out this.Rotation);
      this.Position = matrix.Translation;
    }

    public static MyTransformD Transform(ref MyTransformD t1, ref MyTransformD t2)
    {
      MyTransformD result;
      MyTransformD.Transform(ref t1, ref t2, out result);
      return result;
    }

    public static void Transform(ref MyTransformD t1, ref MyTransformD t2, out MyTransformD result)
    {
      Vector3D result1;
      Vector3D.Transform(ref t1.Position, ref t2.Rotation, out result1);
      Vector3D vector3D = result1 + t2.Position;
      Quaternion result2;
      Quaternion.Multiply(ref t1.Rotation, ref t2.Rotation, out result2);
      result.Position = vector3D;
      result.Rotation = result2;
    }

    public static Vector3D Transform(ref Vector3D v, ref MyTransformD t2)
    {
      Vector3D result;
      MyTransformD.Transform(ref v, ref t2, out result);
      return result;
    }

    public static void Transform(ref Vector3D v, ref MyTransformD t2, out Vector3D result)
    {
      Vector3D.Transform(ref v, ref t2.Rotation, out result);
      result += t2.Position;
    }
  }
}

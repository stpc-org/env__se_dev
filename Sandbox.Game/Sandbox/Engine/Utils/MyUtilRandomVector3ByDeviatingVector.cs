// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyUtilRandomVector3ByDeviatingVector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
  internal struct MyUtilRandomVector3ByDeviatingVector
  {
    private Matrix m_matrix;

    public MyUtilRandomVector3ByDeviatingVector(Vector3 originalVector) => this.m_matrix = Matrix.CreateFromDir(originalVector);

    public Vector3 GetNext(float maxAngle)
    {
      float randomFloat1 = MyUtils.GetRandomFloat(-maxAngle, maxAngle);
      float randomFloat2 = MyUtils.GetRandomFloat(0.0f, 6.283185f);
      return Vector3.TransformNormal(-new Vector3(MyMath.FastSin(randomFloat1) * MyMath.FastCos(randomFloat2), MyMath.FastSin(randomFloat1) * MyMath.FastSin(randomFloat2), MyMath.FastCos(randomFloat1)), this.m_matrix);
    }

    public static Vector3 GetRandom(Vector3 originalVector, float maxAngle) => (double) maxAngle == 0.0 ? originalVector : new MyUtilRandomVector3ByDeviatingVector(originalVector).GetNext(maxAngle);
  }
}

// Decompiled with JetBrains decompiler
// Type: Havok.IPhysicsMesh
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using VRageMath;

namespace Havok
{
  public interface IPhysicsMesh
  {
    void SetAABB(Vector3 min, Vector3 max);

    void AddSectionData(int indexStart, int triCount, string materialName);

    void AddIndex(int index);

    void AddVertex(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 texCoord);

    int GetSectionsCount();

    bool GetSectionData(int idx, ref int indexStart, ref int triCount, ref string matIdx);

    int GetIndicesCount();

    int GetIndex(int idx);

    int GetVerticesCount();

    bool GetVertex(
      int vertexId,
      ref Vector3 position,
      ref Vector3 normal,
      ref Vector3 tangent,
      ref Vector2 texCoord);

    void Transform(Matrix m);
  }
}

// Decompiled with JetBrains decompiler
// Type: BulletXNA.BulletCollision.IPrimitiveManagerBase
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace BulletXNA.BulletCollision
{
  public interface IPrimitiveManagerBase
  {
    void Cleanup();

    bool IsTrimesh();

    int GetPrimitiveCount();

    void GetPrimitiveBox(int prim_index, out AABB primbox);

    void GetPrimitiveTriangle(int prim_index, PrimitiveTriangle triangle);
  }
}

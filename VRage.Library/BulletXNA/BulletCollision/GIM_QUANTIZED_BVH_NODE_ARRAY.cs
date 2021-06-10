// Decompiled with JetBrains decompiler
// Type: BulletXNA.BulletCollision.GIM_QUANTIZED_BVH_NODE_ARRAY
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using BulletXNA.LinearMath;

namespace BulletXNA.BulletCollision
{
  public class GIM_QUANTIZED_BVH_NODE_ARRAY : ObjectArray<BT_QUANTIZED_BVH_NODE>
  {
    public GIM_QUANTIZED_BVH_NODE_ARRAY()
    {
    }

    public GIM_QUANTIZED_BVH_NODE_ARRAY(int capacity)
      : base(capacity)
    {
    }
  }
}

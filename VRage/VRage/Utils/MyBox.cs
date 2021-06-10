// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyBox
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRage.Utils
{
  internal struct MyBox
  {
    public Vector3 Center;
    public Vector3 Size;

    public MyBox(Vector3 center, Vector3 size)
    {
      this.Center = center;
      this.Size = size;
    }
  }
}

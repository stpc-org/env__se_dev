// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyAtlasTextureCoordinate
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRage.Utils
{
  public class MyAtlasTextureCoordinate
  {
    public Vector2 Offset;
    public Vector2 Size;

    public MyAtlasTextureCoordinate(Vector2 offset, Vector2 size)
    {
      this.Offset = offset;
      this.Size = size;
    }
  }
}

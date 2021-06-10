// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.IMy2DClipmapNodeHandler
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
  public interface IMy2DClipmapNodeHandler
  {
    void Init(IMy2DClipmapManager parent, int x, int y, int lod, ref BoundingBox2D bounds);

    void Close();

    void InitJoin(IMy2DClipmapNodeHandler[] children);

    unsafe void Split(BoundingBox2D* childBoxes, ref IMy2DClipmapNodeHandler[] children);
  }
}

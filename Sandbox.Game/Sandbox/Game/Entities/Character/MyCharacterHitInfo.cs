// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.MyCharacterHitInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Models;
using VRageMath;

namespace Sandbox.Game.Entities.Character
{
  public class MyCharacterHitInfo
  {
    public MyCharacterHitInfo() => this.CapsuleIndex = -1;

    public int CapsuleIndex { get; set; }

    public int BoneIndex { get; set; }

    public CapsuleD Capsule { get; set; }

    public Vector3 HitNormalBindingPose { get; set; }

    public Vector3 HitPositionBindingPose { get; set; }

    public Matrix BindingTransformation { get; set; }

    public MyIntersectionResultLineTriangleEx Triangle { get; set; }

    public bool HitHead { get; set; }

    public void Reset()
    {
      this.CapsuleIndex = -1;
      this.BoneIndex = -1;
      this.Capsule = new CapsuleD();
      this.HitNormalBindingPose = new Vector3();
      this.HitPositionBindingPose = new Vector3();
      this.BindingTransformation = new Matrix();
      this.Triangle = new MyIntersectionResultLineTriangleEx();
      this.HitHead = false;
    }
  }
}

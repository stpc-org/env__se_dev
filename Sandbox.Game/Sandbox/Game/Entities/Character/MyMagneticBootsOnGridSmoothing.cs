// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.MyMagneticBootsOnGridSmoothing
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using System.Collections.Generic;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Entities.Character
{
  internal class MyMagneticBootsOnGridSmoothing
  {
    private MyCharacter m_character;
    private List<HkHitInfo> m_hkHitInfos = new List<HkHitInfo>();
    private List<Vector3D> m_rayCastOffsets = new List<Vector3D>();
    private MyPhysics.HitInfo m_hitInfo;
    private float m_minFraction;
    private float m_maxFraction;
    private const float MIN_NORMAL_DOT = 0.99999f;
    private const float MAX_HIT_FRACTION_DIFF = 0.03f;

    public Vector3 SupportNormal { get; internal set; }

    public MyMagneticBootsOnGridSmoothing(MyCharacter character) => this.m_character = character;

    public bool CanUseRayCastNormal()
    {
      this.m_hkHitInfos.Clear();
      this.m_minFraction = float.MaxValue;
      this.m_maxFraction = float.MinValue;
      return this.AreRayCastsNormalsCompatible();
    }

    private bool AreRayCastsNormalsCompatible()
    {
      float groundSearchDistance = MyConstants.DEFAULT_GROUND_SEARCH_DISTANCE;
      Vector3D vector3D1 = this.m_character.PositionComp.GetPosition() + this.m_character.PositionComp.WorldMatrixRef.Up * 0.5;
      Vector3D vector3D2 = vector3D1 + this.m_character.PositionComp.WorldMatrixRef.Down * (double) groundSearchDistance;
      this.m_rayCastOffsets.Clear();
      List<Vector3D> rayCastOffsets1 = this.m_rayCastOffsets;
      MatrixD matrixD1 = this.m_character.PositionComp.WorldMatrixRef;
      Vector3D vector3D3 = matrixD1.Forward * 0.100000001490116;
      matrixD1 = this.m_character.PositionComp.WorldMatrixRef;
      Vector3D vector3D4 = matrixD1.Left * 0.300000011920929;
      Vector3D vector3D5 = vector3D3 + vector3D4;
      rayCastOffsets1.Add(vector3D5);
      List<Vector3D> rayCastOffsets2 = this.m_rayCastOffsets;
      MatrixD matrixD2 = this.m_character.PositionComp.WorldMatrixRef;
      Vector3D vector3D6 = matrixD2.Forward * 0.100000001490116;
      matrixD2 = this.m_character.PositionComp.WorldMatrixRef;
      Vector3D vector3D7 = matrixD2.Right * 0.300000011920929;
      Vector3D vector3D8 = vector3D6 + vector3D7;
      rayCastOffsets2.Add(vector3D8);
      List<Vector3D> rayCastOffsets3 = this.m_rayCastOffsets;
      MatrixD matrixD3 = this.m_character.PositionComp.WorldMatrixRef;
      Vector3D vector3D9 = matrixD3.Backward * 0.100000001490116;
      matrixD3 = this.m_character.PositionComp.WorldMatrixRef;
      Vector3D vector3D10 = matrixD3.Left * 0.200000002980232;
      Vector3D vector3D11 = vector3D9 + vector3D10;
      rayCastOffsets3.Add(vector3D11);
      List<Vector3D> rayCastOffsets4 = this.m_rayCastOffsets;
      MatrixD matrixD4 = this.m_character.PositionComp.WorldMatrixRef;
      Vector3D vector3D12 = matrixD4.Backward * 0.100000001490116;
      matrixD4 = this.m_character.PositionComp.WorldMatrixRef;
      Vector3D vector3D13 = matrixD4.Right * 0.200000002980232;
      Vector3D vector3D14 = vector3D12 + vector3D13;
      rayCastOffsets4.Add(vector3D14);
      foreach (Vector3D rayCastOffset in this.m_rayCastOffsets)
      {
        this.RayCastGround(vector3D1 + rayCastOffset, vector3D2 + rayCastOffset);
        if (this.m_hkHitInfos.Count >= 2 && (!this.IsRayCastNormalDotCompatible() || !this.IsHitFractionCompatible()))
          return false;
      }
      this.SupportNormal = this.m_hkHitInfos[0].Normal;
      return true;
    }

    private bool IsHitFractionCompatible()
    {
      if ((double) this.m_minFraction > (double) this.m_hitInfo.HkHitInfo.HitFraction)
        this.m_minFraction = this.m_hitInfo.HkHitInfo.HitFraction;
      if ((double) this.m_maxFraction < (double) this.m_hitInfo.HkHitInfo.HitFraction)
        this.m_maxFraction = this.m_hitInfo.HkHitInfo.HitFraction;
      return (double) this.m_maxFraction - (double) this.m_minFraction <= 0.0299999993294477;
    }

    private void RayCastGround(Vector3D from, Vector3D to)
    {
      MyPhysics.CastRay(from, to, out this.m_hitInfo, 18U, true);
      this.m_hkHitInfos.Add(this.m_hitInfo.HkHitInfo);
    }

    private bool IsRayCastNormalDotCompatible() => 0.999989986419678 <= (double) this.m_hitInfo.HkHitInfo.Normal.Dot(this.m_hkHitInfos[0].Normal);
  }
}

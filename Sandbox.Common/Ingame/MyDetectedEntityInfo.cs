// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.MyDetectedEntityInfo
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
  public struct MyDetectedEntityInfo
  {
    public readonly long EntityId;
    public readonly string Name;
    public readonly MyDetectedEntityType Type;
    public readonly Vector3D? HitPosition;
    public readonly MatrixD Orientation;
    public readonly Vector3 Velocity;
    public readonly MyRelationsBetweenPlayerAndBlock Relationship;
    public readonly BoundingBoxD BoundingBox;
    public readonly long TimeStamp;

    public MyDetectedEntityInfo(
      long entityId,
      string name,
      MyDetectedEntityType type,
      Vector3D? hitPosition,
      MatrixD orientation,
      Vector3 velocity,
      MyRelationsBetweenPlayerAndBlock relationship,
      BoundingBoxD boundingBox,
      long timeStamp)
    {
      this.EntityId = entityId;
      this.Name = name;
      this.Type = type;
      this.HitPosition = hitPosition;
      this.Orientation = orientation;
      this.Velocity = velocity;
      this.Relationship = relationship;
      this.BoundingBox = boundingBox;
      this.TimeStamp = timeStamp;
    }

    public Vector3D Position => this.BoundingBox.Center;

    public bool IsEmpty() => this.EntityId == 0L;
  }
}

// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyVoxelMap
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyVoxelMap : IMyVoxelBase, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
  {
    void ClampVoxelCoord(ref Vector3I voxelCoord);

    void Init(MyObjectBuilder_EntityBase builder);

    new MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false);

    new void Close();

    new bool DoOverlapSphereTest(float sphereRadius, Vector3D spherePos);

    new bool GetIntersectionWithSphere(ref BoundingSphereD sphere);

    float GetVoxelContentInBoundingBox(BoundingBoxD worldAabb, out float cellCount);

    Vector3I GetVoxelCoordinateFromMeters(Vector3D pos);
  }
}

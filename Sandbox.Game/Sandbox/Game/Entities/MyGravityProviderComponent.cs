// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyGravityProviderComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Components;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public abstract class MyGravityProviderComponent : MyEntityComponentBase, IMyGravityProvider
  {
    public override string ComponentTypeDebugString => this.GetType().Name;

    public abstract bool IsWorking { get; }

    public abstract Vector3 GetWorldGravity(Vector3D worldPoint);

    public abstract bool IsPositionInRange(Vector3D worldPoint);

    public abstract float GetGravityMultiplier(Vector3D worldPoint);

    public abstract void GetProxyAABB(out BoundingBoxD aabb);
  }
}

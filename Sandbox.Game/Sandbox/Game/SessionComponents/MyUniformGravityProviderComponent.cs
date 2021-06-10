// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyUniformGravityProviderComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using System;
using VRage.Game.Components;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 666)]
  public class MyUniformGravityProviderComponent : MySessionComponentBase, IMyGravityProvider
  {
    public readonly Vector3 Gravity = Vector3.Down * 9.81f;

    public override void LoadData() => MyGravityProviderSystem.AddNaturalGravityProvider((IMyGravityProvider) this);

    protected override void UnloadData() => MyGravityProviderSystem.RemoveNaturalGravityProvider((IMyGravityProvider) this);

    public bool IsWorking => true;

    public Vector3 GetWorldGravity(Vector3D worldPoint) => this.Gravity;

    public bool IsPositionInRange(Vector3D worldPoint) => true;

    public Vector3 GetWorldGravityGrid(Vector3D worldPoint) => this.Gravity;

    public bool IsPositionInRangeGrid(Vector3D worldPoint) => true;

    public float GetGravityMultiplier(Vector3D worldPoint) => 1f;

    public void GetProxyAABB(out BoundingBoxD aabb) => throw new NotSupportedException();
  }
}

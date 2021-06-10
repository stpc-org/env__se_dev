// Decompiled with JetBrains decompiler
// Type: VRage.Game.Models.BulletXnaExtensions
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using BulletXNA.LinearMath;
using VRageMath;

namespace VRage.Game.Models
{
  public static class BulletXnaExtensions
  {
    public static IndexedVector3 ToBullet(this Vector3 v) => new IndexedVector3(v.X, v.Y, v.Z);

    public static IndexedVector3 ToBullet(this Vector3D v) => new IndexedVector3((float) v.X, (float) v.Y, (float) v.Z);
  }
}

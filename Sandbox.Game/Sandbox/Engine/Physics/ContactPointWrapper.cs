// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.ContactPointWrapper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Physics
{
  public class ContactPointWrapper
  {
    public MyPhysicsBody bodyA;
    public MyPhysicsBody bodyB;
    public Vector3 position;
    public Vector3 normal;
    public IMyEntity entityA;
    public IMyEntity entityB;
    public float separatingVelocity;

    public Vector3D WorldPosition { get; set; }

    public ContactPointWrapper(ref HkContactPointEvent e)
    {
      this.bodyA = e.Base.BodyA.GetBody();
      this.bodyB = e.Base.BodyB.GetBody();
      this.position = e.ContactPoint.Position;
      this.normal = e.ContactPoint.Normal;
      MyPhysicsBody physicsBody1 = e.GetPhysicsBody(0);
      MyPhysicsBody physicsBody2 = e.GetPhysicsBody(1);
      this.entityA = physicsBody1.Entity;
      this.entityB = physicsBody2.Entity;
      this.separatingVelocity = e.SeparatingVelocity;
    }

    public bool IsValid()
    {
      if (this.bodyA != null && this.bodyB != null)
      {
        Vector3 position = this.position;
        Vector3 normal = this.normal;
        if (this.entityA != null)
          return this.entityB != null;
      }
      return false;
    }
  }
}

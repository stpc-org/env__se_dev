// Decompiled with JetBrains decompiler
// Type: VRage.Game.Utils.MyCameraSpring
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRageMath;

namespace VRage.Game.Utils
{
  public class MyCameraSpring
  {
    public bool Enabled = true;
    private Vector3 m_springCenterLinearVelocity;
    private Vector3 m_springCenterLinearVelocityLast;
    private Vector3 m_springBodyVelocity;
    private Vector3 m_springBodyPosition;
    private float m_stiffness;
    private float m_weight;
    private float m_dampening;
    private float m_maxVelocityChange;
    private static float m_springMaxLength;

    public float SpringStiffness
    {
      get => this.m_stiffness;
      set => this.m_stiffness = MathHelper.Clamp(value, 0.0f, 50f);
    }

    public float SpringDampening
    {
      get => this.m_dampening;
      set => this.m_dampening = MathHelper.Clamp(value, 0.0f, 1f);
    }

    public float SpringMaxVelocity
    {
      get => this.m_maxVelocityChange;
      set => this.m_maxVelocityChange = MathHelper.Clamp(value, 0.0f, 10f);
    }

    public float SpringMaxLength
    {
      get => MyCameraSpring.m_springMaxLength;
      set => MyCameraSpring.m_springMaxLength = MathHelper.Clamp(value, 0.0f, 2f);
    }

    public MyCameraSpring() => this.Reset(true);

    public void Reset(bool resetSpringSettings)
    {
      this.m_springCenterLinearVelocity = Vector3.Zero;
      this.m_springCenterLinearVelocityLast = Vector3.Zero;
      this.m_springBodyVelocity = Vector3.Zero;
      this.m_springBodyPosition = Vector3.Zero;
      if (!resetSpringSettings)
        return;
      this.m_stiffness = 20f;
      this.m_weight = 1f;
      this.m_dampening = 0.7f;
      this.m_maxVelocityChange = 2f;
      MyCameraSpring.m_springMaxLength = 0.5f;
    }

    public void SetCurrentCameraControllerVelocity(Vector3 velocity) => this.m_springCenterLinearVelocity = velocity;

    public void AddCurrentCameraControllerVelocity(Vector3 velocity) => this.m_springCenterLinearVelocity += velocity;

    public bool Update(float timeStep, out Vector3 newCameraLocalOffset)
    {
      if (!this.Enabled)
      {
        newCameraLocalOffset = Vector3.Zero;
        this.m_springCenterLinearVelocity = Vector3.Zero;
        return false;
      }
      Vector3 vector3 = this.m_springCenterLinearVelocity - this.m_springCenterLinearVelocityLast;
      if ((double) vector3.LengthSquared() > (double) this.m_maxVelocityChange * (double) this.m_maxVelocityChange)
      {
        double num = (double) vector3.Normalize();
        vector3 *= this.m_maxVelocityChange;
      }
      this.m_springCenterLinearVelocityLast = this.m_springCenterLinearVelocity;
      this.m_springBodyPosition += vector3 * timeStep;
      this.m_springBodyVelocity += -this.m_springBodyPosition * this.m_stiffness / this.m_weight * timeStep;
      this.m_springBodyPosition += this.m_springBodyVelocity * timeStep;
      this.m_springBodyVelocity *= this.m_dampening;
      newCameraLocalOffset = MyCameraSpring.TransformLocalOffset(this.m_springBodyPosition);
      return true;
    }

    private static Vector3 TransformLocalOffset(Vector3 springBodyPosition)
    {
      float num = springBodyPosition.Length();
      return (double) num <= 9.99999974737875E-06 ? springBodyPosition : (float) ((double) MyCameraSpring.m_springMaxLength * (double) num / ((double) num + 2.0)) * springBodyPosition / num;
    }
  }
}

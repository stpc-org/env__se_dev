// Decompiled with JetBrains decompiler
// Type: VRage.MySpectator
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRage.Utils;
using VRageMath;

namespace VRage
{
  public class MySpectator
  {
    public static MySpectator Static;
    public const float DEFAULT_SPECTATOR_LINEAR_SPEED = 0.1f;
    public const float MIN_SPECTATOR_LINEAR_SPEED = 0.0001f;
    public const float MAX_SPECTATOR_LINEAR_SPEED = 8000f;
    public const float DEFAULT_SPECTATOR_ANGULAR_SPEED = 1f;
    public const float MIN_SPECTATOR_ANGULAR_SPEED = 0.0001f;
    public const float MAX_SPECTATOR_ANGULAR_SPEED = 6f;
    public Vector3D ThirdPersonCameraDelta = new Vector3D(-10.0, 10.0, -10.0);
    private MySpectatorCameraMovementEnum m_spectatorCameraMovement;
    private Vector3D m_position;
    private Vector3D m_targetDelta = Vector3D.Forward;
    private Vector3D? m_up;
    protected float m_speedModeLinear = 0.1f;
    protected float m_speedModeAngular = 1f;
    protected MatrixD m_orientation = MatrixD.Identity;
    protected bool m_orientationDirty = true;
    private float m_orbitY;
    private float m_orbitX;

    public MySpectatorCameraMovementEnum SpectatorCameraMovement
    {
      get => this.m_spectatorCameraMovement;
      set
      {
        if (this.m_spectatorCameraMovement != value)
          this.OnChangingMode(this.m_spectatorCameraMovement, value);
        this.m_spectatorCameraMovement = value;
      }
    }

    protected virtual void OnChangingMode(
      MySpectatorCameraMovementEnum oldMode,
      MySpectatorCameraMovementEnum newMode)
    {
    }

    public bool IsInFirstPersonView { get; set; }

    public bool ForceFirstPersonCamera { get; set; }

    public bool Initialized { get; set; }

    public MySpectator() => MySpectator.Static = this;

    public Vector3D Position
    {
      get => this.m_position;
      set => this.m_position = value;
    }

    public float SpeedModeLinear
    {
      get => this.m_speedModeLinear;
      set => this.m_speedModeLinear = value;
    }

    public float SpeedModeAngular
    {
      get => this.m_speedModeAngular;
      set => this.m_speedModeAngular = value;
    }

    public Vector3D Target
    {
      get => this.Position + this.m_targetDelta;
      set
      {
        Vector3D vector3D = value - this.Position;
        this.m_orientationDirty = this.m_targetDelta != vector3D;
        this.m_targetDelta = vector3D;
        this.m_up = new Vector3D?();
      }
    }

    public void SetTarget(Vector3D target, Vector3D? up)
    {
      this.Target = target;
      int num1 = this.m_orientationDirty ? 1 : 0;
      Vector3D? up1 = this.m_up;
      Vector3D? nullable = up;
      int num2 = up1.HasValue == nullable.HasValue ? (up1.HasValue ? (up1.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : 0) : 0) : 1;
      this.m_orientationDirty = (num1 | num2) != 0;
      this.m_up = up;
    }

    public void UpdateOrientation()
    {
      Vector3D vector3D = MyUtils.Normalize(this.m_targetDelta);
      this.m_orientation = MatrixD.CreateFromDir(vector3D.LengthSquared() > 0.0 ? vector3D : Vector3D.Forward, this.m_up.HasValue ? this.m_up.Value : Vector3D.Up);
    }

    public MatrixD Orientation
    {
      get
      {
        if (this.m_orientationDirty)
        {
          this.UpdateOrientation();
          this.m_orientationDirty = false;
        }
        return this.m_orientation;
      }
    }

    public void Rotate(Vector2 rotationIndicator, float rollIndicator) => this.MoveAndRotate(Vector3.Zero, rotationIndicator, rollIndicator);

    public void RotateStopped() => this.MoveAndRotateStopped();

    public virtual void MoveAndRotate(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      Vector3D position1 = this.Position;
      moveIndicator *= this.m_speedModeLinear;
      float num1 = 0.1f;
      float num2 = 1f / 400f * this.m_speedModeAngular;
      Vector3D position2 = (Vector3D) moveIndicator * (double) num1;
      switch (this.SpectatorCameraMovement)
      {
        case MySpectatorCameraMovementEnum.UserControlled:
          if ((double) rollIndicator != 0.0)
          {
            float angle = MathHelper.Clamp((float) ((double) rollIndicator * (double) this.m_speedModeLinear * 0.100000001490116), -0.02f, 0.02f);
            Vector3D xOut;
            Vector3D yOut;
            MyUtils.VectorPlaneRotation(this.m_orientation.Up, this.m_orientation.Right, out xOut, out yOut, angle);
            this.m_orientation.Right = yOut;
            this.m_orientation.Up = xOut;
          }
          if ((double) rotationIndicator.X != 0.0)
          {
            Vector3D xOut;
            Vector3D yOut;
            MyUtils.VectorPlaneRotation(this.m_orientation.Up, this.m_orientation.Forward, out xOut, out yOut, rotationIndicator.X * num2);
            this.m_orientation.Up = xOut;
            this.m_orientation.Forward = yOut;
          }
          if ((double) rotationIndicator.Y != 0.0)
          {
            Vector3D xOut;
            Vector3D yOut;
            MyUtils.VectorPlaneRotation(this.m_orientation.Right, this.m_orientation.Forward, out xOut, out yOut, -rotationIndicator.Y * num2);
            this.m_orientation.Right = xOut;
            this.m_orientation.Forward = yOut;
          }
          this.Position += Vector3D.Transform(position2, this.m_orientation);
          break;
        case MySpectatorCameraMovementEnum.ConstantDelta:
          this.m_orbitY += rotationIndicator.Y * 0.01f;
          this.m_orbitX += rotationIndicator.X * 0.01f;
          Vector3D position3 = -this.m_targetDelta;
          Vector3D vector3D1 = this.Position + this.m_targetDelta;
          MatrixD orientation1 = this.Orientation;
          Matrix matrix1 = Matrix.Invert((Matrix) ref orientation1);
          MatrixD matrix2 = (MatrixD) ref matrix1;
          Vector3D position4 = Vector3D.Transform(position3, matrix2);
          rotationIndicator *= 0.01f;
          MatrixD matrixD1 = MatrixD.CreateRotationX((double) this.m_orbitX) * MatrixD.CreateRotationY((double) this.m_orbitY) * MatrixD.CreateRotationZ((double) rollIndicator);
          MatrixD matrix3 = matrixD1;
          Vector3D vector3D2 = Vector3D.Transform(position4, matrix3);
          this.Position = vector3D1 + vector3D2;
          this.m_targetDelta = -vector3D2;
          this.m_orientation = matrixD1;
          break;
        case MySpectatorCameraMovementEnum.Orbit:
          this.m_orbitY += rotationIndicator.Y * 0.01f;
          this.m_orbitX += rotationIndicator.X * 0.01f;
          Vector3D position5 = -this.m_targetDelta;
          Vector3D vector3D3 = this.Position + this.m_targetDelta;
          MatrixD orientation2 = this.Orientation;
          Matrix matrix4 = Matrix.Invert((Matrix) ref orientation2);
          MatrixD matrix5 = (MatrixD) ref matrix4;
          Vector3D position6 = Vector3D.Transform(position5, matrix5);
          rotationIndicator *= 0.01f;
          MatrixD matrixD2 = MatrixD.CreateRotationX((double) this.m_orbitX) * MatrixD.CreateRotationY((double) this.m_orbitY) * MatrixD.CreateRotationZ((double) rollIndicator);
          MatrixD matrix6 = matrixD2;
          Vector3D vector3D4 = Vector3D.Transform(position6, matrix6);
          this.Position = vector3D3 + vector3D4;
          this.m_targetDelta = -vector3D4;
          this.Position += this.m_orientation.Right * position2.X + this.m_orientation.Up * position2.Y;
          Vector3D vector3D5 = this.m_orientation.Forward * -position2.Z;
          this.Position += vector3D5;
          this.m_targetDelta -= vector3D5;
          this.m_orientation = matrixD2;
          break;
      }
    }

    public virtual void Update()
    {
    }

    public virtual void MoveAndRotateStopped()
    {
    }

    public MatrixD GetViewMatrix()
    {
      Vector3D position = this.Position;
      MatrixD orientation = this.Orientation;
      Vector3D forward = orientation.Forward;
      orientation = this.Orientation;
      Vector3D up = orientation.Up;
      return MatrixD.Invert(MatrixD.CreateWorld(position, forward, up));
    }

    public void SetViewMatrix(MatrixD viewMatrix)
    {
      MatrixD matrixD = MatrixD.Invert(viewMatrix);
      this.Position = matrixD.Translation;
      this.m_orientation = MatrixD.Identity;
      this.m_orientation.Right = matrixD.Right;
      this.m_orientation.Up = matrixD.Up;
      this.m_orientation.Forward = matrixD.Forward;
      this.m_orientationDirty = false;
    }

    public void Reset()
    {
      this.m_position = (Vector3D) Vector3.Zero;
      this.m_targetDelta = (Vector3D) Vector3.Forward;
      this.ThirdPersonCameraDelta = new Vector3D(-10.0, 10.0, -10.0);
      this.m_orientationDirty = true;
      this.m_orbitX = 0.0f;
      this.m_orbitY = 0.0f;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: VRage.Game.Utils.MyCamera
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game.Utils
{
  public class MyCamera : IMyCamera
  {
    public const float DefaultFarPlaneDistance = 20000f;
    public float NearPlaneDistance = 0.05f;
    public float FarPlaneDistance = 20000f;
    public float FieldOfView;
    public Vector3D PreviousPosition;
    public MyViewport Viewport;
    public MatrixD WorldMatrix = MatrixD.Identity;
    public MatrixD ViewMatrix = MatrixD.Identity;
    public MatrixD ViewMatrixInverse = MatrixD.Identity;
    public MatrixD ProjectionMatrix = MatrixD.Identity;
    public MatrixD ProjectionMatrixFar = MatrixD.Identity;
    public MatrixD ViewProjectionMatrix = MatrixD.Identity;
    public MatrixD ViewProjectionMatrixFar = MatrixD.Identity;
    public BoundingBoxD BoundingBox;
    public BoundingSphereD BoundingSphere;
    public MyCameraZoomProperties Zoom;
    public BoundingFrustumD BoundingFrustum = new BoundingFrustumD(MatrixD.Identity);
    public BoundingFrustumD BoundingFrustumFar = new BoundingFrustumD(MatrixD.Identity);
    public readonly MyCameraShake CameraShake = new MyCameraShake();
    public readonly MyCameraSpring CameraSpring = new MyCameraSpring();
    private float m_fovSpring;
    private static float m_fovSpringDampening = 0.5f;
    public float FarFarPlaneDistance = 1000000f;

    public float AspectRatio { get; private set; }

    public MatrixD ViewMatrixAtZero
    {
      get
      {
        MatrixD projectionMatrix = this.ViewProjectionMatrix;
        projectionMatrix.M14 = 0.0;
        projectionMatrix.M24 = 0.0;
        projectionMatrix.M34 = 0.0;
        projectionMatrix.M41 = 0.0;
        projectionMatrix.M42 = 0.0;
        projectionMatrix.M43 = 0.0;
        projectionMatrix.M44 = 1.0;
        return projectionMatrix;
      }
    }

    public Vector3 ForwardVector => (Vector3) this.WorldMatrix.Forward;

    public Vector3 LeftVector => (Vector3) this.WorldMatrix.Left;

    public Vector3 UpVector => (Vector3) this.WorldMatrix.Up;

    public float FieldOfViewDegrees
    {
      get => MathHelper.ToDegrees(this.FieldOfView);
      set => this.FieldOfView = MathHelper.ToRadians(value);
    }

    public float FovWithZoom => this.Zoom.GetFOV() + this.m_fovSpring;

    public Vector3D Position => this.WorldMatrix.Translation;

    public bool SmoothMotion { get; private set; } = true;

    public MyCamera(float fieldOfView, MyViewport currentScreenViewport)
    {
      this.FieldOfView = fieldOfView;
      this.Zoom = new MyCameraZoomProperties(this);
      this.UpdateScreenSize(currentScreenViewport);
    }

    public void Update(float updateStepTime)
    {
      this.Zoom.Update(updateStepTime);
      Vector3 newCameraLocalOffset = Vector3.Zero;
      MatrixD viewMatrix = this.ViewMatrix;
      if (this.CameraSpring.Enabled)
        this.CameraSpring.Update(updateStepTime, out newCameraLocalOffset);
      if (this.CameraShake.ShakeEnabled)
      {
        Vector3 outPos;
        this.CameraShake.UpdateShake(updateStepTime, out outPos, out Vector3 _);
        newCameraLocalOffset += outPos;
      }
      if (newCameraLocalOffset != Vector3.Zero)
      {
        Vector3D vector = (Vector3D) newCameraLocalOffset;
        Vector3D result;
        Vector3D.Rotate(ref vector, ref viewMatrix, out result);
        viewMatrix.Translation += result;
        this.ViewMatrix = viewMatrix;
      }
      this.UpdatePropertiesInternal(this.ViewMatrix);
      this.m_fovSpring *= MyCamera.m_fovSpringDampening;
    }

    public void UpdateScreenSize(MyViewport currentScreenViewport)
    {
      this.Viewport = currentScreenViewport;
      this.PreviousPosition = Vector3D.Zero;
      this.BoundingFrustum = new BoundingFrustumD(MatrixD.Identity);
      this.AspectRatio = this.Viewport.Width / this.Viewport.Height;
    }

    public void SetViewMatrix(MatrixD newViewMatrix, bool smooth = true)
    {
      this.PreviousPosition = this.Position;
      this.SmoothMotion = smooth;
      this.UpdatePropertiesInternal(newViewMatrix);
    }

    public void UploadViewMatrixToRender() => MyRenderProxy.SetCameraViewMatrix(this.ViewMatrix, (Matrix) ref this.ProjectionMatrix, (Matrix) ref this.ProjectionMatrixFar, this.Zoom.GetFOV() + this.m_fovSpring, this.Zoom.GetFOV() + this.m_fovSpring, this.NearPlaneDistance, this.FarPlaneDistance, this.FarFarPlaneDistance, this.Position, smooth: this.SmoothMotion);

    private void UpdatePropertiesInternal(MatrixD newViewMatrix)
    {
      this.ViewMatrix = newViewMatrix;
      MatrixD.Invert(ref this.ViewMatrix, out this.WorldMatrix);
      this.ProjectionMatrix = MatrixD.CreatePerspectiveFieldOfView((double) this.FovWithZoom, (double) this.AspectRatio, (double) this.GetSafeNear(), (double) this.FarPlaneDistance);
      this.ProjectionMatrixFar = MatrixD.CreatePerspectiveFieldOfView((double) this.FovWithZoom, (double) this.AspectRatio, (double) this.GetSafeNear(), (double) this.FarFarPlaneDistance);
      this.ViewProjectionMatrix = this.ViewMatrix * this.ProjectionMatrix;
      this.ViewProjectionMatrixFar = this.ViewMatrix * this.ProjectionMatrixFar;
      MatrixD.Invert(ref this.ViewMatrix, out this.ViewMatrixInverse);
      this.UpdateBoundingFrustum();
    }

    private float GetSafeNear() => Math.Min(4f, this.NearPlaneDistance);

    private void UpdateBoundingFrustum()
    {
      this.BoundingFrustum.Matrix = this.ViewProjectionMatrix;
      this.BoundingFrustumFar.Matrix = this.ViewProjectionMatrixFar;
      this.BoundingBox = BoundingBoxD.CreateInvalid();
      this.BoundingBox.Include(ref this.BoundingFrustum);
      this.BoundingSphere = MyUtils.GetBoundingSphereFromBoundingBox(ref this.BoundingBox);
    }

    public bool IsInFrustum(ref BoundingBoxD boundingBox)
    {
      ContainmentType result;
      this.BoundingFrustum.Contains(ref boundingBox, out result);
      return (uint) result > 0U;
    }

    public bool IsInFrustum(BoundingBoxD boundingBox) => this.IsInFrustum(ref boundingBox);

    public bool IsInFrustum(ref BoundingSphereD boundingSphere)
    {
      ContainmentType result;
      this.BoundingFrustum.Contains(ref boundingSphere, out result);
      return (uint) result > 0U;
    }

    public double GetDistanceFromPoint(Vector3D position) => Vector3D.Distance(this.Position, position);

    public Vector3D WorldToScreen(ref Vector3D worldPos) => Vector3D.Transform(worldPos, this.ViewProjectionMatrix);

    public Vector3D ScreenToWorld(ref Vector3D screenPos)
    {
      MatrixD matrix = MatrixD.Invert(MatrixD.Multiply(MatrixD.Multiply(this.WorldMatrix, this.ViewMatrix), this.ProjectionMatrix));
      return Vector3D.Transform(screenPos, matrix);
    }

    public LineD WorldLineFromScreen(Vector2 screenCoords)
    {
      MatrixD matrix1 = MatrixD.Invert(this.ViewProjectionMatrix);
      Vector4D vector1 = new Vector4D(2.0 * (double) screenCoords.X / (double) this.Viewport.Width - 1.0, 1.0 - 2.0 * (double) screenCoords.Y / (double) this.Viewport.Height, 0.0, 1.0);
      Vector4D vector2 = new Vector4D(2.0 * (double) screenCoords.X / (double) this.Viewport.Width - 1.0, 1.0 - 2.0 * (double) screenCoords.Y / (double) this.Viewport.Height, 1.0, 1.0);
      Vector4D vector4D1 = Vector4D.Transform(vector1, matrix1);
      MatrixD matrix2 = matrix1;
      Vector4D vector4D2 = Vector4D.Transform(vector2, matrix2);
      return new LineD(new Vector3D(vector4D1 / vector4D1.W), new Vector3D(vector4D2 / vector4D2.W));
    }

    public void AddFovSpring(float fovAddition = 0.01f) => this.m_fovSpring += fovAddition;

    Vector3D IMyCamera.WorldToScreen(ref Vector3D worldPos) => Vector3D.Transform(worldPos, this.ViewProjectionMatrix);

    float IMyCamera.FieldOfViewAngle => this.FieldOfViewDegrees;

    float IMyCamera.FieldOfViewAngleForNearObjects => this.FieldOfViewDegrees;

    float IMyCamera.FovWithZoom => this.FovWithZoom;

    float IMyCamera.FovWithZoomForNearObjects => this.FovWithZoom;

    double IMyCamera.GetDistanceWithFOV(Vector3D position) => this.GetDistanceFromPoint(position);

    bool IMyCamera.IsInFrustum(ref BoundingBoxD boundingBox) => this.IsInFrustum(ref boundingBox);

    bool IMyCamera.IsInFrustum(ref BoundingSphereD boundingSphere) => this.IsInFrustum(ref boundingSphere);

    bool IMyCamera.IsInFrustum(BoundingBoxD boundingBox) => this.IsInFrustum(boundingBox);

    Vector3D IMyCamera.Position => this.Position;

    Vector3D IMyCamera.PreviousPosition => this.PreviousPosition;

    Vector2 IMyCamera.ViewportOffset => new Vector2(this.Viewport.OffsetX, this.Viewport.OffsetY);

    Vector2 IMyCamera.ViewportSize => new Vector2(this.Viewport.Width, this.Viewport.Height);

    MatrixD IMyCamera.ViewMatrix => this.ViewMatrix;

    MatrixD IMyCamera.WorldMatrix => this.WorldMatrix;

    MatrixD IMyCamera.ProjectionMatrix => this.ProjectionMatrix;

    MatrixD IMyCamera.ProjectionMatrixForNearObjects => this.ProjectionMatrix;

    float IMyCamera.NearPlaneDistance => this.NearPlaneDistance;

    float IMyCamera.FarPlaneDistance => this.FarPlaneDistance;
  }
}

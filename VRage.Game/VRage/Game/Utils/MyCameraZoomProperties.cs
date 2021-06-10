// Decompiled with JetBrains decompiler
// Type: VRage.Game.Utils.MyCameraZoomProperties
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRageMath;

namespace VRage.Game.Utils
{
  public class MyCameraZoomProperties
  {
    private static readonly float FIELD_OF_VIEW_MIN = MathHelper.ToRadians(40f);
    private float ZoomTime = 0.075f;
    private float m_currentZoomTime;
    private MyCameraZoomOperationType m_zoomType;
    private float m_FOV;
    private float m_zoomLevel;
    private MyCamera m_camera;

    public MyCameraZoomProperties(MyCamera camera)
    {
      this.m_camera = camera;
      this.Update(0.0f);
    }

    public void Update(float updateStepSize)
    {
      switch (this.m_zoomType)
      {
        case MyCameraZoomOperationType.ZoomingIn:
          if ((double) this.m_currentZoomTime <= (double) this.ZoomTime)
          {
            this.m_currentZoomTime += updateStepSize;
            if ((double) this.m_currentZoomTime >= (double) this.ZoomTime)
            {
              this.m_currentZoomTime = this.ZoomTime;
              this.m_zoomType = MyCameraZoomOperationType.Zoomed;
              break;
            }
            break;
          }
          break;
        case MyCameraZoomOperationType.ZoomingOut:
          if ((double) this.m_currentZoomTime >= 0.0)
          {
            this.m_currentZoomTime -= updateStepSize;
            if ((double) this.m_currentZoomTime <= 0.0)
            {
              this.m_currentZoomTime = 0.0f;
              this.m_zoomType = MyCameraZoomOperationType.NoZoom;
              break;
            }
            break;
          }
          break;
      }
      this.m_zoomLevel = (float) (1.0 - (double) this.m_currentZoomTime / (double) this.ZoomTime);
      this.m_FOV = this.ApplyToFov ? MathHelper.Lerp(MyCameraZoomProperties.FIELD_OF_VIEW_MIN, this.m_camera.FieldOfView, this.m_zoomLevel) : this.m_camera.FieldOfView;
    }

    public void ResetZoom()
    {
      this.m_zoomType = MyCameraZoomOperationType.NoZoom;
      this.m_currentZoomTime = 0.0f;
    }

    public void SetZoom(MyCameraZoomOperationType inZoomType) => this.m_zoomType = inZoomType;

    public float GetZoomLevel() => this.m_zoomLevel;

    public float GetFOV() => MyMath.Clamp(this.m_FOV, 1E-05f, 3.141583f);

    public bool IsZooming() => (uint) this.m_zoomType > 0U;

    public bool ApplyToFov { get; set; }
  }
}

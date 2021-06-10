// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MySectorLodding
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageRender;

namespace Sandbox.Game.World
{
  public class MySectorLodding
  {
    public MyNewLoddingSettings CurrentSettings = new MyNewLoddingSettings();
    private MyNewLoddingSettings m_lowSettings = new MyNewLoddingSettings();
    private MyNewLoddingSettings m_mediumSettings = new MyNewLoddingSettings();
    private MyNewLoddingSettings m_highSettings = new MyNewLoddingSettings();
    private MyNewLoddingSettings m_extremeSettings = new MyNewLoddingSettings();
    private MyRenderQualityEnum m_selectedQuality = MyRenderQualityEnum.HIGH;

    public MyNewLoddingSettings LowSettings => this.m_lowSettings;

    public MyNewLoddingSettings MediumSettings => this.m_mediumSettings;

    public MyNewLoddingSettings HighSettings => this.m_highSettings;

    public MyNewLoddingSettings ExtremeSettings => this.m_extremeSettings;

    public void UpdatePreset(
      MyNewLoddingSettings lowLoddingSettings,
      MyNewLoddingSettings mediumLoddingSettings,
      MyNewLoddingSettings highLoddingSettings,
      MyNewLoddingSettings extremeLoddingSettings)
    {
      this.m_lowSettings.CopyFrom(lowLoddingSettings);
      this.m_mediumSettings.CopyFrom(mediumLoddingSettings);
      this.m_highSettings.CopyFrom(highLoddingSettings);
      this.m_extremeSettings.CopyFrom(extremeLoddingSettings);
      this.SelectQuality(this.m_selectedQuality);
    }

    public void SelectQuality(MyRenderQualityEnum quality)
    {
      this.m_selectedQuality = quality;
      MyNewLoddingSettings settings;
      switch (quality)
      {
        case MyRenderQualityEnum.LOW:
          settings = this.LowSettings;
          break;
        case MyRenderQualityEnum.NORMAL:
          settings = this.MediumSettings;
          break;
        case MyRenderQualityEnum.HIGH:
          settings = this.HighSettings;
          break;
        case MyRenderQualityEnum.EXTREME:
          settings = this.ExtremeSettings;
          break;
        default:
          return;
      }
      this.CurrentSettings.CopyFrom(settings);
      MyRenderProxy.UpdateNewLoddingSettings(settings);
    }
  }
}

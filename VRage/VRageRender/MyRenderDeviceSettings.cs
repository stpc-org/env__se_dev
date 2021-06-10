// Decompiled with JetBrains decompiler
// Type: VRageRender.MyRenderDeviceSettings
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRage;

namespace VRageRender
{
  public struct MyRenderDeviceSettings : IEquatable<MyRenderDeviceSettings>
  {
    public int AdapterOrdinal;
    public int NewAdapterOrdinal;
    public bool DisableWindowedModeForOldDriver;
    public MyWindowModeEnum WindowMode;
    public int BackBufferWidth;
    public int BackBufferHeight;
    public int RefreshRate;
    public int VSync;
    public bool DebugDrawOnly;
    public bool UseStereoRendering;
    public bool SettingsMandatory;
    public bool InitParallel;

    public MyRenderDeviceSettings(
      int adapter,
      MyWindowModeEnum windowMode,
      int width,
      int height,
      int refreshRate,
      int vsync,
      bool useStereoRendering,
      bool settingsMandatory,
      bool initParallel = true,
      float spriteMainViewportScale = 1f)
    {
      this.AdapterOrdinal = adapter;
      this.NewAdapterOrdinal = adapter;
      this.DisableWindowedModeForOldDriver = false;
      this.WindowMode = windowMode;
      this.BackBufferWidth = width;
      this.BackBufferHeight = height;
      this.RefreshRate = refreshRate;
      this.VSync = vsync;
      this.UseStereoRendering = useStereoRendering;
      this.SettingsMandatory = settingsMandatory;
      this.InitParallel = initParallel;
      this.DebugDrawOnly = false;
    }

    bool IEquatable<MyRenderDeviceSettings>.Equals(
      MyRenderDeviceSettings other)
    {
      return this.Equals(ref other);
    }

    public bool Equals(ref MyRenderDeviceSettings other) => this.AdapterOrdinal == other.AdapterOrdinal && this.WindowMode == other.WindowMode && (this.BackBufferWidth == other.BackBufferWidth && this.BackBufferHeight == other.BackBufferHeight) && (this.RefreshRate == other.RefreshRate && this.VSync == other.VSync && this.UseStereoRendering == other.UseStereoRendering) && this.SettingsMandatory == other.SettingsMandatory;

    public override string ToString() => "MyRenderDeviceSettings: {\n" + "AdapterOrdinal: " + (object) this.AdapterOrdinal + "\n" + "NewAdapterOrdinal: " + (object) this.NewAdapterOrdinal + "\n" + "WindowMode: " + (object) this.WindowMode + "\n" + "BackBufferWidth: " + (object) this.BackBufferWidth + "\n" + "BackBufferHeight: " + (object) this.BackBufferHeight + "\n" + "RefreshRate: " + (object) this.RefreshRate + "\n" + "VSync: " + (object) this.VSync + "\n" + "DebugDrawOnly: " + this.DebugDrawOnly.ToString() + "\n" + "UseStereoRendering: " + this.UseStereoRendering.ToString() + "\n" + "SettingsMandatory: " + this.SettingsMandatory.ToString() + "\n" + "InitParallel: " + this.InitParallel.ToString() + "\n" + "}";
  }
}

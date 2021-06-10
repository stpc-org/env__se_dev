// Decompiled with JetBrains decompiler
// Type: VRageRender.MyAdapterInfo
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Diagnostics;
using VRageMath;

namespace VRageRender
{
  [DebuggerDisplay("DeviceName: '{Name}', Description: '{Description}'")]
  public struct MyAdapterInfo
  {
    public string Name;
    public string DeviceName;
    public MyDisplayMode[] SupportedDisplayModes;
    public string OutputName;
    public string Description;
    public int AdapterDeviceId;
    public int OutputId;
    public Rectangle DesktopBounds;
    public Vector2I DesktopResolution;
    public int MaxTextureSize;
    public bool Has512MBRam;
    public bool IsDx11Supported;
    public int Priority;
    public bool IsOutputAttached;
    public ulong VRAM;
    public ulong SVRAM;
    public bool MultithreadedRenderingSupported;
    public VendorIds VendorId;
    public int DeviceId;
    public string DriverVersion;
    public string DriverDate;
    public bool DriverUpdateNecessary;
    public string DriverUpdateLink;
    public bool IsNvidiaNotebookGpu;
    public bool AftermathSupported;
    public MyRenderPresetEnum Quality;
    public bool Mobile;
    public bool ParallelVertexBufferMapping;
    public bool BatchedConstantBufferMapping;

    public void LogInfo(Action<string> lineWriter)
    {
      lineWriter("Adapter: " + this.Name);
      lineWriter("VendorId: " + (object) this.VendorId);
      lineWriter("DeviceId: " + (object) this.DeviceId);
      lineWriter("Description: " + this.Description);
    }

    public override string ToString() => string.Format("DeviceName: '{0}', Description: '{1}'", (object) this.Name, (object) this.Description);

    public MyDisplayMode? GetDisplayMode(int width, int height, int refreshRate)
    {
      foreach (MyDisplayMode supportedDisplayMode in this.SupportedDisplayModes)
      {
        if (supportedDisplayMode.Width == width && supportedDisplayMode.Height == height && (refreshRate == 0 || (double) Math.Abs(supportedDisplayMode.RefreshRateF - (float) refreshRate / 1000f) < 0.100000001490116))
          return new MyDisplayMode?(supportedDisplayMode);
      }
      return new MyDisplayMode?();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudCameraInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Gui
{
  public class MyHudCameraInfo
  {
    private bool Visible { get; set; }

    private string CameraName { get; set; }

    private string ShipName { get; set; }

    private bool IsDirty { get; set; }

    public MyHudCameraInfo()
    {
      this.Visible = false;
      this.IsDirty = true;
    }

    public void Enable(string shipName, string cameraName)
    {
      this.Visible = true;
      this.ShipName = shipName;
      this.CameraName = cameraName;
      this.IsDirty = true;
    }

    public void Disable()
    {
      this.Visible = false;
      this.IsDirty = true;
    }

    public void Draw(MyGuiControlMultilineText control)
    {
      if (this.Visible)
      {
        if (!this.IsDirty)
          return;
        control.Clear();
        control.AppendText(this.CameraName);
        control.AppendLine();
        control.AppendText(this.ShipName);
        this.IsDirty = false;
      }
      else
      {
        if (!this.IsDirty)
          return;
        control.Clear();
        this.IsDirty = false;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MySCloudStorageQuotaBar
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Graphics.GUI;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MySCloudStorageQuotaBar : MyGuiControlInfoProgressBar
  {
    private int m_ticksTillNextUpdate;
    private bool m_innerControlsVisible;

    public MySCloudStorageQuotaBar(Vector2 position)
      : base(0.1758f, new Vector2?(position), textLeft: MyTexts.GetString(MyCommonTexts.CloudQuota_CloudUsage))
    {
      foreach (MyGuiControlBase control in this.Controls)
        control.Visible = false;
    }

    public override void Update()
    {
      base.Update();
      --this.m_ticksTillNextUpdate;
      if (this.m_ticksTillNextUpdate >= 0)
        return;
      this.m_ticksTillNextUpdate = 100;
      ulong totalBytes;
      ulong availableBytes;
      if (!MyGameService.GetRemoteStorageQuota(out totalBytes, out availableBytes))
        return;
      if ((double) availableBytes < (double) totalBytes * 0.1)
      {
        this.m_LeftLabel.ColorMask = this.m_RightLabel.ColorMask = (Vector4) Color.Red;
        this.m_BarInnerLine.ColorMask = (Vector4) Color.Red;
      }
      else
      {
        this.m_LeftLabel.ColorMask = this.m_RightLabel.ColorMask = (Vector4) Color.White;
        this.m_BarInnerLine.ColorMask = (Vector4) Color.White;
      }
      float num1 = (float) totalBytes / 1048576f;
      float num2 = (float) availableBytes / 1048576f;
      if (!this.m_innerControlsVisible)
      {
        this.m_innerControlsVisible = true;
        foreach (MyGuiControlBase control in this.Controls)
          control.Visible = true;
      }
      this.UpdateValues(num1 - num2, (long) ((double) num1 + 0.5), "{0:F1}/{1} MB");
    }

    private void UpdateValues(float current, long max, string format = null)
    {
      float num = 0.67f;
      this.m_LeftLabel.TextScale = num;
      this.m_RightLabel.TextScale = num;
      this.m_RightLabel.Text = string.Format(format ?? "{0} / {1}", (object) current, (object) max);
      this.m_BarInnerLine.Size = new Vector2(this.m_barSize.X * MathHelper.Clamp(current / (float) max, 0.0f, 1f), this.m_barSize.Y);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderDebug
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Render", "Debug")]
  internal class MyGuiScreenDebugRenderDebug : MyGuiScreenDebugBase
  {
    public static readonly StringBuilder ClipboardText = new StringBuilder();
    private List<MyGuiControlCheckbox> m_cbs = new List<MyGuiControlCheckbox>();

    public MyGuiScreenDebugRenderDebug()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Debug", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddSlider("Worker thread count", 1f, 5f, (Func<float>) (() => (float) MyRenderProxy.Settings.RenderThreadCount), (Action<float>) (f => MyRenderProxy.Settings.RenderThreadCount = (int) f));
      this.AddCheckBox("Force IC", MyRenderProxy.Settings.ForceImmediateContext, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.ForceImmediateContext = x.IsChecked));
      this.AddCheckBox("Render thread high priority", MyRenderProxy.Settings.RenderThreadHighPriority, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.RenderThreadHighPriority = x.IsChecked));
      this.AddCheckBox("Force Slow CPU", MyRenderProxy.Settings.ForceSlowCPU, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.ForceSlowCPU = x.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Total parrot view", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_MODEL_INFO)));
      this.AddButton("Copy to clipboard", new Action<MyGuiControlButton>(this.CopyClipboardTextToClipboard));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Debug missing file textures", MyRenderProxy.Settings.UseDebugMissingFileTextures, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.UseDebugMissingFileTextures = x.IsChecked));
      this.AddButton("Print textures log", new Action<MyGuiControlButton>(this.PrintUsedFileTexturesIntoLog));
      this.AddCheckBox("Skip global RO WM update", MyRenderProxy.Settings.SkipGlobalROWMUpdate, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.SkipGlobalROWMUpdate = x.IsChecked));
      this.AddCheckBox("HQ Depth", MyRenderProxy.Settings.User.HqDepth, (Action<MyGuiControlCheckbox>) (x =>
      {
        MyRenderProxy.Settings.User.HqDepth = x.IsChecked;
        MyRenderProxy.SetSettingsDirty();
      }));
      this.AddSlider("Resource streaming pool", 10f, 20480f, (Func<float>) (() => (float) ((double) MyRenderProxy.Settings.ResourceStreamingPool / 1024.0 / 1024.0)), (Action<float>) (x => MyRenderProxy.Settings.ResourceStreamingPool = (ulong) x * 1024UL * 1024UL));
      this.AddButton("Generate mip cache", (Action<MyGuiControlButton>) (_ =>
      {
        MyRenderProxy.Settings.ResourceStreamingPool = ulong.MaxValue;
        MyRenderProxy.SwitchRenderSettings(MyRenderProxy.Settings);
        MyRenderProxy.PreloadTextures((IEnumerable<string>) MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Textures")).ToList<string>(), TextureType.ColorMetal);
      }));
    }

    private void PrintUsedFileTexturesIntoLog(MyGuiControlButton sender) => MyRenderProxy.PrintAllFileTexturesIntoLog();

    private void CopyClipboardTextToClipboard(MyGuiControlButton sender)
    {
      string str = MyGuiScreenDebugRenderDebug.ClipboardText.ToString();
      if (string.IsNullOrEmpty(str))
        return;
      MyVRage.Platform.System.Clipboard = str;
    }

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();

    public override bool Update(bool hasFocus)
    {
      if (MyRenderProxy.SettingsDirty)
        MyRenderProxy.SwitchRenderSettings(MyRenderProxy.Settings);
      return base.Update(hasFocus);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderDebug);
  }
}

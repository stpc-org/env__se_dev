// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderDraw
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Render", "Draw")]
  internal class MyGuiScreenDebugRenderDraw : MyGuiScreenDebugBase
  {
    private List<MyGuiControlCheckbox> m_cbs = new List<MyGuiControlCheckbox>();

    public MyGuiScreenDebugRenderDraw()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Draw", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddCheckBox("Draw IDs", MyRenderProxy.Settings.DisplayIDs, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayIDs = x.IsChecked));
      this.AddCheckBox("Draw AABBs", MyRenderProxy.Settings.DisplayAabbs, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayAabbs = x.IsChecked));
      this.AddCheckBox("Draw Tree AABBs", MyRenderProxy.Settings.DisplayTreeAabbs, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayTreeAabbs = x.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Draw Wireframe", MyRenderProxy.Settings.Wireframe, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.Wireframe = x.IsChecked));
      this.AddCheckBox("Draw transparency heat map", MyRenderProxy.Settings.DisplayTransparencyHeatMap, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayTransparencyHeatMap = x.IsChecked));
      this.AddCheckBox("Draw transparency heat map in grayscale", MyRenderProxy.Settings.DisplayTransparencyHeatMapInGrayscale, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayTransparencyHeatMapInGrayscale = x.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Scene objects", Color.Yellow.ToVector4(), 1.2f);
      this.AddCheckBox("Draw non-merge-instanced", MyRenderProxy.Settings.DrawNonMergeInstanced, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawNonMergeInstanced = x.IsChecked));
      this.AddCheckBox("Draw merge-instanced", MyRenderProxy.Settings.DrawMergeInstanced, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawMergeInstanced = x.IsChecked));
      this.AddCheckBox("Draw groups", MyRenderProxy.Settings.DrawGroups, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawGroups = x.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Draw standard meshes", MyRenderProxy.Settings.DrawMeshes, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawMeshes = x.IsChecked));
      this.AddCheckBox("Draw standard instanced meshes", MyRenderProxy.Settings.DrawInstancedMeshes, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawInstancedMeshes = x.IsChecked));
      this.AddCheckBox("Draw dynamic instances", MyRenderProxy.Settings.DrawDynamicInstances, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawDynamicInstances = x.IsChecked));
      this.AddCheckBox("Draw glass", MyRenderProxy.Settings.DrawGlass, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawGlass = x.IsChecked));
      this.AddCheckBox("Draw transparent models", MyRenderProxy.Settings.DrawTransparentModels, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawTransparentModels = x.IsChecked));
      this.AddCheckBox("Draw transparent instanced models", MyRenderProxy.Settings.DrawTransparentModelsInstanced, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawTransparentModelsInstanced = x.IsChecked));
      this.AddCheckBox("Draw alphamasked", MyRenderProxy.Settings.DrawAlphamasked, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawAlphamasked = x.IsChecked));
      this.AddCheckBox("Draw billboards", MyRenderProxy.Settings.DrawBillboards, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawBillboards = x.IsChecked));
      this.AddCheckBox("Draw billboards top", MyRenderProxy.Settings.DrawBillboardsTop, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawBillboardsTop = x.IsChecked));
      this.AddCheckBox("Draw billboards standard", MyRenderProxy.Settings.DrawBillboardsStandard, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawBillboardsStandard = x.IsChecked));
      this.AddCheckBox("Draw billboards bottom", MyRenderProxy.Settings.DrawBillboardsBottom, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawBillboardsBottom = x.IsChecked));
      this.AddCheckBox("Draw billboards LDR", MyRenderProxy.Settings.DrawBillboardsLDR, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawBillboardsLDR = x.IsChecked));
      this.AddCheckBox("Draw billboards PostPP", MyRenderProxy.Settings.DrawBillboardsPostPP, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawBillboardsPostPP = x.IsChecked));
      this.AddCheckBox("Draw impostors", MyRenderProxy.Settings.DrawImpostors, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawImpostors = x.IsChecked));
      this.AddCheckBox("Draw voxels", MyRenderProxy.Settings.DrawVoxels, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawVoxels = x.IsChecked));
      this.AddCheckBox("Draw checker texture", MyRenderProxy.Settings.DrawCheckerTexture, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawCheckerTexture = x.IsChecked));
    }

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderDraw);
  }
}

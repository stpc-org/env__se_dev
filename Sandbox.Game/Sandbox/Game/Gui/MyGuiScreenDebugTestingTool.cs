// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugTestingTool
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Game", "Testing Tool")]
  internal class MyGuiScreenDebugTestingTool : MyGuiScreenDebugBase
  {
    private MyGuiControlListbox m_categoriesListbox;
    private MyGuiControlCheckbox m_smallGridCheckbox;
    private MyGuiControlCheckbox m_largeGridCheckbox;
    private List<string> buildCategoriesList = new List<string>();

    public MyGuiScreenDebugTestingTool()
      : base()
      => this.RecreateControls(true);

    private void CreateListOfBuildCategories()
    {
      foreach (KeyValuePair<string, MyGuiBlockCategoryDefinition> category in MyDefinitionManager.Static.GetCategories())
      {
        if (category.Key != null && category.Value.AvailableInSurvival && (!category.Value.IsToolCategory && !category.Value.IsAnimationCategory))
          this.buildCategoriesList.Add(category.Value.Name);
      }
      this.buildCategoriesList.Sort();
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Test Tool Control", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddButton("Generate Tests", (Action<MyGuiControlButton>) (x => MyTestingToolHelper.Instance.Action_SpawnBlockSaveTestReload()));
      this.AddButton("Stop Test Generation", (Action<MyGuiControlButton>) (x => MyTestingToolHelper.Instance.Action1_State3_Finish()));
      this.AddButton("Spawn monolith", (Action<MyGuiControlButton>) (x => MyTestingToolHelper.Instance.Action_Test()));
      this.AddSlider("Screenshot distance multiplier", 0.0f, 5f, (Func<float>) (() => MyTestingToolHelper.ScreenshotDistanceMultiplier), (Action<float>) (f => MyTestingToolHelper.ScreenshotDistanceMultiplier = f));
      this.AddSlider("Speed", 0.0f, 100f, (Func<float>) (() => (float) MyTestingToolHelper.m_timer_Max), (Action<float>) (f => MyTestingToolHelper.m_timer_Max = (int) f));
      this.AddLabel("Grid Selection for Automated Test Generator", Color.Yellow.ToVector4(), 1.2f);
      this.m_smallGridCheckbox = this.AddCheckBox("Small Grid", (Func<bool>) (() => MyTestingToolHelper.IsSmallGridSelected), (Action<bool>) (f => MyTestingToolHelper.IsSmallGridSelected = f));
      this.m_largeGridCheckbox = this.AddCheckBox("Large Grid", (Func<bool>) (() => MyTestingToolHelper.IsLargeGridSelected), (Action<bool>) (f => MyTestingToolHelper.IsLargeGridSelected = f));
      this.AddLabel("Group Selection for Automated Test Generator", Color.Yellow.ToVector4(), 1.2f);
      this.m_categoriesListbox = this.AddListBox(3f);
      this.m_categoriesListbox.MultiSelect = false;
      this.m_categoriesListbox.VisibleRowsCount = 17;
      this.CreateListOfBuildCategories();
      foreach (string buildCategories in this.buildCategoriesList)
        this.m_categoriesListbox.Items.Add(new MyGuiControlListbox.Item(new StringBuilder(buildCategories)));
      this.m_categoriesListbox.ItemsSelected += (Action<MyGuiControlListbox>) (e =>
      {
        if (e.SelectedItems.Count <= 0)
          return;
        MyTestingToolHelper.Instance.SelectedCategory = this.m_categoriesListbox.GetLastSelected().Text.ToString();
      });
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugTestingTool);
  }
}

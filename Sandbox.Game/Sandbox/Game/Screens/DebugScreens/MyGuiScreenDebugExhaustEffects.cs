// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugExhaustEffects
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VRage.FileSystem;
using VRage.Filesystem.FindFilesRegEx;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Render.Particles;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Game", "Exhaust Effects")]
  internal class MyGuiScreenDebugExhaustEffects : MyGuiScreenDebugBase
  {
    private MyGuiControlCombobox m_exhaustEffectsCombo;
    private MyGuiControlCombobox m_pipesCombo;
    private MyGuiControlTextbox m_dummyTextbox;
    private MyGuiControlCombobox m_effectsCombo;
    private MyGuiControlSlider m_effectIntensity;
    private MyGuiControlSlider m_powerToRadius;
    private MyGuiControlSlider m_powerToBirth;
    private MyGuiControlSlider m_powerToVelocity;
    private MyGuiControlSlider m_powerToColorIntensity;
    private MyGuiControlSlider m_powerToLife;
    private int m_selectedEffectIndex;
    private bool m_canUpdateValues = true;
    private MyExhaustEffectDefinition m_selectedEffectDefinition;
    private int m_selectedPipe;

    private MyObjectBuilder_ExhaustEffectDefinition.Pipe SelectedPipe
    {
      get
      {
        if (this.m_selectedEffectDefinition == null)
          return (MyObjectBuilder_ExhaustEffectDefinition.Pipe) null;
        return this.m_selectedPipe == -1 ? (MyObjectBuilder_ExhaustEffectDefinition.Pipe) null : this.m_selectedEffectDefinition.ExhaustPipes[this.m_selectedPipe];
      }
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugExhaustEffects);

    public MyGuiScreenDebugExhaustEffects()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Exhaust effects", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_exhaustEffectsCombo = this.AddCombo();
      this.AddExhaustEffects(this.m_exhaustEffectsCombo);
      this.m_exhaustEffectsCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.exhaustEffect_ItemSelected);
      this.AddLabel("Pipe: ", Color.Yellow.ToVector4(), 1.2f);
      this.m_pipesCombo = this.AddCombo();
      this.m_pipesCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.pipe_ItemSelected);
      this.AddLabel("Dummy: ", Color.Yellow.ToVector4(), 1.2f);
      this.m_dummyTextbox = this.AddTextbox("emitter", (Action<MyGuiControlTextbox>) null);
      this.m_dummyTextbox.FocusChanged += new Action<MyGuiControlBase, bool>(this.dummiesCombo_FocusChanged);
      this.m_dummyTextbox.EnterPressed += new Action<MyGuiControlTextbox>(this.dummiesCombo_EnterPressed);
      this.m_dummyTextbox.TextChanged += new Action<MyGuiControlTextbox>(this.dummyTextbox_TextChanged);
      this.AddLabel("Effect: ", Color.Yellow.ToVector4(), 1.2f);
      this.m_effectsCombo = this.AddCombo();
      this.AddEffects(this.m_effectsCombo);
      this.m_effectsCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.effect_ItemSelected);
      this.m_effectIntensity = this.AddSlider("Effect intensity", 0.0f, 0.0f, 1f);
      this.m_effectIntensity.ValueChanged = new Action<MyGuiControlSlider>(this.OnEffectValueChanged);
      this.m_currentPosition = this.m_currentPosition + new Vector2(0.0f, 0.015f);
      this.m_powerToRadius = this.AddSlider("Power to radius", 0.0f, 0.0f, 1f);
      this.m_powerToRadius.ValueChanged = new Action<MyGuiControlSlider>(this.OnEffectValueChanged);
      this.m_powerToBirth = this.AddSlider("Power to birth", 0.0f, 0.0f, 1f);
      this.m_powerToBirth.ValueChanged = new Action<MyGuiControlSlider>(this.OnEffectValueChanged);
      this.m_powerToVelocity = this.AddSlider("Power to velocity", 0.0f, 0.0f, 1f);
      this.m_powerToVelocity.ValueChanged = new Action<MyGuiControlSlider>(this.OnEffectValueChanged);
      this.m_powerToColorIntensity = this.AddSlider("Power to color intensity", 0.0f, 0.0f, 1f);
      this.m_powerToColorIntensity.ValueChanged = new Action<MyGuiControlSlider>(this.OnEffectValueChanged);
      this.m_powerToLife = this.AddSlider("Power to life", 0.0f, 0.0f, 1f);
      this.m_powerToLife.ValueChanged = new Action<MyGuiControlSlider>(this.OnEffectValueChanged);
      this.m_currentPosition = this.m_currentPosition + new Vector2(0.0f, 0.015f);
      this.AddButton(new StringBuilder("Save exhaust effects"), new Action<MyGuiControlButton>(this.onClick_Save)).VisualStyle = MyGuiControlButtonStyleEnum.Default;
      this.AddButton(new StringBuilder("Reload definitions"), new Action<MyGuiControlButton>(this.onClick_Reload)).VisualStyle = MyGuiControlButtonStyleEnum.Default;
      if (this.m_exhaustEffectsCombo.GetItemsCount() <= 0)
        return;
      this.m_exhaustEffectsCombo.SelectItemByIndex(this.m_selectedEffectIndex);
    }

    private void dummyTextbox_TextChanged(MyGuiControlTextbox obj) => this.SelectedPipe.Dummy = this.m_dummyTextbox.Text;

    private void dummiesCombo_EnterPressed(MyGuiControlTextbox obj) => this.CanHaveFocus = false;

    private void dummiesCombo_FocusChanged(MyGuiControlBase arg1, bool arg2) => this.CanHaveFocus = true;

    private void exhaustEffect_ItemSelected()
    {
      this.m_selectedEffectIndex = this.m_exhaustEffectsCombo.GetSelectedIndex();
      this.m_selectedEffectDefinition = MyDefinitionManager.Static.GetAllDefinitions<MyExhaustEffectDefinition>().Where<MyExhaustEffectDefinition>((Func<MyExhaustEffectDefinition, bool>) (x => x.Id.SubtypeName == this.m_exhaustEffectsCombo.GetSelectedValue().ToString())).FirstOrDefault<MyExhaustEffectDefinition>();
      if (this.m_selectedEffectDefinition == null)
        return;
      this.AddPipes();
    }

    private void AddPipes()
    {
      this.m_pipesCombo.Clear();
      if (this.m_selectedEffectDefinition.ExhaustPipes == null)
        return;
      long num = 0;
      foreach (MyObjectBuilder_ExhaustEffectDefinition.Pipe exhaustPipe in this.m_selectedEffectDefinition.ExhaustPipes)
        this.m_pipesCombo.AddItem(num++, exhaustPipe.Name);
      if (this.m_pipesCombo.GetItemsCount() <= 0)
        return;
      this.m_pipesCombo.SelectItemByIndex(this.m_selectedPipe);
    }

    private void pipe_ItemSelected()
    {
      this.m_selectedPipe = this.m_pipesCombo.GetSelectedIndex();
      this.m_effectsCombo.SelectItemByKey(this.m_selectedEffectDefinition.ExhaustPipes[this.m_selectedPipe].Effect.GetHashCode64());
      this.m_dummyTextbox.SetText(new StringBuilder(this.m_selectedEffectDefinition.ExhaustPipes[this.m_selectedPipe].Dummy));
      this.UpdateSliderValues();
    }

    private void effect_ItemSelected() => this.SelectedPipe.Effect = this.m_effectsCombo.GetSelectedValue().ToString();

    private void UpdateSliderValues()
    {
      if (!this.m_canUpdateValues)
        return;
      this.m_canUpdateValues = false;
      this.m_effectIntensity.Value = this.SelectedPipe.EffectIntensity;
      this.m_powerToRadius.Value = this.SelectedPipe.PowerToRadius;
      this.m_powerToBirth.Value = this.SelectedPipe.PowerToBirth;
      this.m_powerToVelocity.Value = this.SelectedPipe.PowerToVelocity;
      this.m_powerToColorIntensity.Value = this.SelectedPipe.PowerToColorIntensity;
      this.m_powerToLife.Value = this.SelectedPipe.PowerToLife;
      this.m_canUpdateValues = true;
    }

    private void AddExhaustEffects(MyGuiControlCombobox combo)
    {
      combo.Clear();
      IEnumerable<MyExhaustEffectDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyExhaustEffectDefinition>();
      if (allDefinitions == null)
        return;
      foreach (MyExhaustEffectDefinition effectDefinition in allDefinitions)
        combo.AddItem(effectDefinition.Id.GetHashCodeLong(), effectDefinition.Id.SubtypeName);
    }

    private void AddEffects(MyGuiControlCombobox combo)
    {
      combo.Clear();
      foreach (string name in MyParticleEffectsLibrary.GetNames())
        combo.AddItem(name.GetHashCode64(), name);
    }

    private void onClick_Save(MyGuiControlButton sender) => this.Save("ExhaustEffects.*");

    private void Save(string filePattern = "*.*")
    {
      Regex regex = FindFilesPatternToRegex.Convert(filePattern);
      Dictionary<string, List<MyDefinitionBase>> dictionary = new Dictionary<string, List<MyDefinitionBase>>();
      IEnumerable<MyExhaustEffectDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyExhaustEffectDefinition>();
      if (allDefinitions == null)
        return;
      List<MyDefinitionBase> myDefinitionBaseList = (List<MyDefinitionBase>) null;
      foreach (MyExhaustEffectDefinition effectDefinition in allDefinitions)
      {
        if (effectDefinition.Context != null && !string.IsNullOrEmpty(effectDefinition.Context.CurrentFile))
        {
          string fileName = Path.GetFileName(effectDefinition.Context.CurrentFile);
          if (regex.IsMatch(fileName))
          {
            if (!dictionary.ContainsKey(effectDefinition.Context.CurrentFile))
              dictionary.Add(effectDefinition.Context.CurrentFile, myDefinitionBaseList = new List<MyDefinitionBase>());
            else
              myDefinitionBaseList = dictionary[effectDefinition.Context.CurrentFile];
            myDefinitionBaseList.Add((MyDefinitionBase) effectDefinition);
          }
        }
      }
      MyObjectBuilder_Definitions newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
      List<MyObjectBuilder_DefinitionBase> source = new List<MyObjectBuilder_DefinitionBase>();
      foreach (MyDefinitionBase myDefinitionBase in myDefinitionBaseList)
      {
        MyObjectBuilder_DefinitionBase objectBuilder = myDefinitionBase.GetObjectBuilder();
        source.Add(objectBuilder);
      }
      newObject.Definitions = (MyObjectBuilder_DefinitionBase[]) source.OfType<MyObjectBuilder_ExhaustEffectDefinition>().ToArray<MyObjectBuilder_ExhaustEffectDefinition>();
      MyObjectBuilderSerializer.SerializeXML(Path.Combine(MyFileSystem.ContentPath, "Data\\ExhaustEffects.sbc"), false, (MyObjectBuilder_Base) newObject);
    }

    private void createEmptyExhaustEffect()
    {
      MyObjectBuilder_Definitions newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
      List<MyObjectBuilder_DefinitionBase> builderDefinitionBaseList = new List<MyObjectBuilder_DefinitionBase>();
      MyExhaustEffectDefinition effectDefinition = new MyExhaustEffectDefinition();
      effectDefinition.Id = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ExhaustEffectDefinition), "Exhaust");
      effectDefinition.ExhaustPipes = new List<MyObjectBuilder_ExhaustEffectDefinition.Pipe>();
      effectDefinition.ExhaustPipes.Add(new MyObjectBuilder_ExhaustEffectDefinition.Pipe()
      {
        Name = "Pipe0",
        Dummy = "emitter",
        Effect = "VehicleDust",
        EffectIntensity = 1f,
        PowerToBirth = 0.0f,
        PowerToRadius = 0.0f,
        PowerToLife = 0.0f,
        PowerToColorIntensity = 0.0f,
        PowerToVelocity = 0.0f
      });
      MyObjectBuilder_DefinitionBase objectBuilder = effectDefinition.GetObjectBuilder();
      string path = Path.Combine(MyFileSystem.ContentPath, "Data\\ExhaustEffects.sbc");
      builderDefinitionBaseList.Add(objectBuilder);
      newObject.Definitions = builderDefinitionBaseList.ToArray();
      MyObjectBuilder_Definitions builderDefinitions = newObject;
      MyObjectBuilderSerializer.SerializeXML(path, false, (MyObjectBuilder_Base) builderDefinitions);
    }

    private void onClick_Reload(MyGuiControlButton sender)
    {
      MyDefinitionManager.Static.UnloadData(true);
      MyDefinitionManager.Static.LoadData(new List<MyObjectBuilder_Checkpoint.ModItem>());
      this.RecreateControls(false);
    }

    private void OnEffectValueChanged(MyGuiControlSlider slider)
    {
      if (!this.m_canUpdateValues)
        return;
      this.SelectedPipe.EffectIntensity = this.m_effectIntensity.Value;
      this.SelectedPipe.PowerToBirth = this.m_powerToBirth.Value;
      this.SelectedPipe.PowerToColorIntensity = this.m_powerToColorIntensity.Value;
      this.SelectedPipe.PowerToLife = this.m_powerToLife.Value;
      this.SelectedPipe.PowerToRadius = this.m_powerToRadius.Value;
      this.SelectedPipe.PowerToVelocity = this.m_powerToVelocity.Value;
    }
  }
}

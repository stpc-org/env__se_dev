// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugAudio
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Game", "Audio")]
  internal class MyGuiScreenDebugAudio : MyGuiScreenDebugBase
  {
    private const string ALL_CATEGORIES = "_ALL_CATEGORIES_";
    private MyGuiControlCombobox m_categoriesCombo;
    private MyGuiControlCombobox m_cuesCombo;
    private static string m_currentCategorySelectedItem;
    private static int m_currentCueSelectedItem;
    private bool m_canUpdateValues = true;
    private IMySourceVoice m_sound;
    private MySoundData m_currentCue;
    private MyGuiControlSlider m_cueVolumeSlider;
    private MyGuiControlCombobox m_cueVolumeCurveCombo;
    private MyGuiControlSlider m_cueMaxDistanceSlider;
    private MyGuiControlSlider m_cueVolumeVariationSlider;
    private MyGuiControlSlider m_cuePitchVariationSlider;
    private MyGuiControlCheckbox m_soloCheckbox;
    private MyGuiControlButton m_applyVolumeToCategory;
    private MyGuiControlButton m_applyMaxDistanceToCategory;
    private MyGuiControlCombobox m_effects;
    private List<MyGuiControlCombobox> m_cues = new List<MyGuiControlCombobox>();
    private List<MyCueId> m_cueCache = new List<MyCueId>();

    public MyGuiScreenDebugAudio()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.m_scale = 0.7f;
      this.AddCaption("Audio FX", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      if (MyAudio.Static is MyNullAudio)
        return;
      this.m_categoriesCombo = this.AddCombo();
      List<MyStringId> categories = MyAudio.Static.GetCategories();
      this.m_categoriesCombo.AddItem(0L, new StringBuilder("_ALL_CATEGORIES_"));
      int num1 = 1;
      foreach (MyStringId myStringId in categories)
        this.m_categoriesCombo.AddItem((long) num1++, new StringBuilder(myStringId.ToString()));
      this.m_categoriesCombo.SortItemsByValueText();
      this.m_categoriesCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.categoriesCombo_OnSelect);
      this.m_cuesCombo = this.AddCombo();
      this.m_cuesCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.cuesCombo_OnSelect);
      this.m_cueVolumeSlider = this.AddSlider("Volume", 1f, 0.0f, 1f);
      this.m_cueVolumeSlider.ValueChanged = new Action<MyGuiControlSlider>(this.CueVolumeChanged);
      this.m_applyVolumeToCategory = this.AddButton(new StringBuilder("Apply to category"), new Action<MyGuiControlButton>(this.OnApplyVolumeToCategorySelected));
      this.m_applyVolumeToCategory.Enabled = false;
      this.m_cueVolumeCurveCombo = this.AddCombo();
      foreach (object obj in Enum.GetValues(typeof (MyCurveType)))
        this.m_cueVolumeCurveCombo.AddItem((long) (int) obj, new StringBuilder(obj.ToString()));
      this.m_effects = this.AddCombo();
      this.m_effects.AddItem(0L, new StringBuilder(""));
      int num2 = 1;
      foreach (MyAudioEffectDefinition effectDefinition in MyDefinitionManager.Static.GetAudioEffectDefinitions())
        this.m_effects.AddItem((long) num2++, new StringBuilder(effectDefinition.Id.SubtypeName));
      this.m_effects.SelectItemByIndex(0);
      this.m_effects.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.effects_ItemSelected);
      this.m_cueMaxDistanceSlider = this.AddSlider("Max distance", 0.0f, 0.0f, 2000f);
      this.m_cueMaxDistanceSlider.ValueChanged = new Action<MyGuiControlSlider>(this.MaxDistanceChanged);
      this.m_applyMaxDistanceToCategory = this.AddButton(new StringBuilder("Apply to category"), new Action<MyGuiControlButton>(this.OnApplyMaxDistanceToCategorySelected));
      this.m_applyMaxDistanceToCategory.Enabled = false;
      this.m_cueVolumeVariationSlider = this.AddSlider("Volume variation", 0.0f, 0.0f, 10f);
      this.m_cueVolumeVariationSlider.ValueChanged = new Action<MyGuiControlSlider>(this.VolumeVariationChanged);
      this.m_cuePitchVariationSlider = this.AddSlider("Pitch variation", 0.0f, 0.0f, 500f);
      this.m_cuePitchVariationSlider.ValueChanged = new Action<MyGuiControlSlider>(this.PitchVariationChanged);
      this.m_soloCheckbox = this.AddCheckBox("Solo", false, (Action<MyGuiControlCheckbox>) null);
      this.m_soloCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.SoloChanged);
      this.AddButton(new StringBuilder("Play selected"), new Action<MyGuiControlButton>(this.OnPlaySelected)).CueEnum = GuiSounds.None;
      this.AddButton(new StringBuilder("Stop selected"), new Action<MyGuiControlButton>(this.OnStopSelected));
      this.AddButton(new StringBuilder("Save"), new Action<MyGuiControlButton>(this.OnSave));
      this.AddButton(new StringBuilder("Reload"), new Action<MyGuiControlButton>(this.OnReload));
      if (this.m_categoriesCombo.GetItemsCount() > 0)
        this.m_categoriesCombo.SelectItemByIndex(0);
      this.m_currentPosition.Y -= MyGuiConstants.SCREEN_CAPTION_DELTA_Y;
      this.AddSubcaption("Voice chat");
      this.m_currentPosition.Y -= 0.035f;
      MyGuiControlCombobox preferredBitRate = this.AddCombo(size: new Vector2?(new Vector2(0.215f, 0.1f)));
      preferredBitRate.AddItem(0L, "Automatic");
      if (MyFakes.VOICE_CHAT_TARGET_BIT_RATE != 0)
        preferredBitRate.AddItem((long) MyFakes.VOICE_CHAT_TARGET_BIT_RATE, MyFakes.VOICE_CHAT_TARGET_BIT_RATE.ToString());
      for (int index = 3000; index <= 96000; index *= 2)
      {
        if (index != MyFakes.VOICE_CHAT_TARGET_BIT_RATE)
          preferredBitRate.AddItem((long) index, index.ToString());
      }
      preferredBitRate.SelectItemByKey((long) MyFakes.VOICE_CHAT_TARGET_BIT_RATE);
      preferredBitRate.ItemSelected += (MyGuiControlCombobox.ItemSelectedDelegate) (() => MyFakes.VOICE_CHAT_TARGET_BIT_RATE = (int) preferredBitRate.GetSelectedKey());
      this.AddSlider("Playback delay", 0.0f, 1500f, (object) null, MemberHelper.GetMember<float>((Expression<Func<float>>) (() => MyFakes.VOICE_CHAT_PLAYBACK_DELAY)));
      this.AddCheckBox("Automatic activation", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION)));
      this.AddCheckBox("Echo", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.VOICE_CHAT_ECHO)));
      this.AddButton("ResetMic", (Action<MyGuiControlButton>) (_ =>
      {
        IMyMicrophoneService service = MyServiceManager.Instance.GetService<IMyMicrophoneService>();
        service.DisposeVoiceRecording();
        service.InitializeVoiceRecording();
      }));
    }

    private void effects_ItemSelected()
    {
      foreach (MyGuiControlBase cue in this.m_cues)
        this.Controls.Remove(cue);
      this.m_cues.Clear();
      MyAudioEffectDefinition definition;
      if (!MyDefinitionManager.Static.TryGetDefinition<MyAudioEffectDefinition>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AudioEffectDefinition), MyStringHash.TryGet(this.m_effects.GetSelectedValue().ToString())), out definition))
        return;
      for (int index = 0; index < definition.Effect.SoundsEffects.Count - 1; ++index)
      {
        MyGuiControlCombobox box = this.AddCombo();
        this.UpdateCuesCombo(box);
        this.m_cues.Add(box);
      }
    }

    private void categoriesCombo_OnSelect()
    {
      MyGuiScreenDebugAudio.m_currentCategorySelectedItem = this.m_categoriesCombo.GetSelectedValue().ToString();
      this.m_applyVolumeToCategory.Enabled = MyGuiScreenDebugAudio.m_currentCategorySelectedItem != "_ALL_CATEGORIES_";
      this.m_applyMaxDistanceToCategory.Enabled = MyGuiScreenDebugAudio.m_currentCategorySelectedItem != "_ALL_CATEGORIES_";
      this.UpdateCuesCombo(this.m_cuesCombo);
      foreach (MyGuiControlCombobox cue in this.m_cues)
        this.UpdateCuesCombo(cue);
    }

    private void UpdateCuesCombo(MyGuiControlCombobox box)
    {
      box.ClearItems();
      long key = 0;
      foreach (MySoundData cueDefinition in MyAudio.Static.CueDefinitions)
      {
        if (MyGuiScreenDebugAudio.m_currentCategorySelectedItem == "_ALL_CATEGORIES_" || MyGuiScreenDebugAudio.m_currentCategorySelectedItem == cueDefinition.Category.ToString())
        {
          box.AddItem(key, new StringBuilder(cueDefinition.SubtypeId.ToString()));
          ++key;
        }
      }
      box.SortItemsByValueText();
      if (box.GetItemsCount() <= 0)
        return;
      box.SelectItemByIndex(0);
    }

    private void cuesCombo_OnSelect()
    {
      MyGuiScreenDebugAudio.m_currentCueSelectedItem = (int) this.m_cuesCombo.GetSelectedKey();
      this.m_currentCue = MyAudio.Static.GetCue(new MyCueId(MyStringHash.TryGet(this.m_cuesCombo.GetSelectedValue().ToString())));
      this.UpdateCueValues();
    }

    private void UpdateCueValues()
    {
      this.m_canUpdateValues = false;
      this.m_cueVolumeSlider.Value = this.m_currentCue.Volume;
      this.m_cueVolumeCurveCombo.SelectItemByKey((long) this.m_currentCue.VolumeCurve);
      this.m_cueMaxDistanceSlider.Value = this.m_currentCue.MaxDistance;
      this.m_cueVolumeVariationSlider.Value = this.m_currentCue.VolumeVariation;
      this.m_cuePitchVariationSlider.Value = this.m_currentCue.PitchVariation;
      this.m_soloCheckbox.IsChecked = this.m_currentCue == MyAudio.Static.SoloCue;
      this.m_canUpdateValues = true;
    }

    private void CueVolumeChanged(MyGuiControlSlider slider)
    {
      if (!this.m_canUpdateValues)
        return;
      this.m_currentCue.Volume = slider.Value;
    }

    private void CueVolumeCurveChanged(MyGuiControlCombobox combobox)
    {
      if (!this.m_canUpdateValues)
        return;
      this.m_currentCue.VolumeCurve = (MyCurveType) combobox.GetSelectedKey();
    }

    private void MaxDistanceChanged(MyGuiControlSlider slider)
    {
      if (!this.m_canUpdateValues)
        return;
      this.m_currentCue.MaxDistance = slider.Value;
    }

    private void VolumeVariationChanged(MyGuiControlSlider slider)
    {
      if (!this.m_canUpdateValues)
        return;
      this.m_currentCue.VolumeVariation = slider.Value;
    }

    private void PitchVariationChanged(MyGuiControlSlider slider)
    {
      if (!this.m_canUpdateValues)
        return;
      this.m_currentCue.PitchVariation = slider.Value;
    }

    private void SoloChanged(MyGuiControlCheckbox checkbox)
    {
      if (!this.m_canUpdateValues)
        return;
      if (checkbox.IsChecked)
        MyAudio.Static.SoloCue = this.m_currentCue;
      else
        MyAudio.Static.SoloCue = (MySoundData) null;
    }

    private void OnApplyVolumeToCategorySelected(MyGuiControlButton button)
    {
      this.m_canUpdateValues = false;
      foreach (MySoundData cueDefinition in MyAudio.Static.CueDefinitions)
      {
        if (MyGuiScreenDebugAudio.m_currentCategorySelectedItem == cueDefinition.Category.ToString())
          cueDefinition.Volume = this.m_cueVolumeSlider.Value;
      }
      this.m_canUpdateValues = true;
    }

    private void OnApplyMaxDistanceToCategorySelected(MyGuiControlButton button)
    {
      this.m_canUpdateValues = false;
      foreach (MySoundData cueDefinition in MyAudio.Static.CueDefinitions)
      {
        if (MyGuiScreenDebugAudio.m_currentCategorySelectedItem == cueDefinition.Category.ToString())
          cueDefinition.MaxDistance = this.m_cueMaxDistanceSlider.Value;
      }
      this.m_canUpdateValues = true;
    }

    private void OnPlaySelected(MyGuiControlButton button)
    {
      if (this.m_sound != null && this.m_sound.IsPlaying)
        this.m_sound.Stop(true);
      this.m_sound = MyAudio.Static.PlaySound(new MyCueId(MyStringHash.TryGet(this.m_cuesCombo.GetSelectedValue().ToString())));
      MyStringHash effect = MyStringHash.TryGet(this.m_effects.GetSelectedValue().ToString());
      if (!(effect != MyStringHash.NullOrEmpty))
        return;
      foreach (MyGuiControlCombobox cue in this.m_cues)
        this.m_cueCache.Add(new MyCueId(MyStringHash.TryGet(cue.GetSelectedValue().ToString())));
      this.m_sound = MyAudio.Static.ApplyEffect(this.m_sound, effect, this.m_cueCache.ToArray()).OutputSound;
      this.m_cueCache.Clear();
    }

    private void OnStopSelected(MyGuiControlButton button)
    {
      if (this.m_sound == null || !this.m_sound.IsPlaying)
        return;
      this.m_sound.Stop(true);
    }

    private void OnSave(MyGuiControlButton button)
    {
      MyObjectBuilder_Definitions builderDefinitions = new MyObjectBuilder_Definitions();
      DictionaryValuesReader<MyDefinitionId, MyAudioDefinition> soundDefinitions = MyDefinitionManager.Static.GetSoundDefinitions();
      builderDefinitions.Sounds = new MyObjectBuilder_AudioDefinition[soundDefinitions.Count];
      int num = 0;
      foreach (MyAudioDefinition myAudioDefinition in soundDefinitions)
        builderDefinitions.Sounds[num++] = (MyObjectBuilder_AudioDefinition) myAudioDefinition.GetObjectBuilder();
      MyObjectBuilderSerializer.SerializeXML(Path.Combine(MyFileSystem.ContentPath, "Data\\Audio.sbc"), false, (MyObjectBuilder_Base) builderDefinitions);
    }

    private void OnReload(MyGuiControlButton button)
    {
      MyAudio.Static.UnloadData();
      MyDefinitionManager.Static.PreloadDefinitions();
      MyAudio.Static.ReloadData(MyAudioExtensions.GetSoundDataFromDefinitions(), MyAudioExtensions.GetEffectData());
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugAudio);
  }
}

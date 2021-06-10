// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenServerSearchSpace
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenServerSearchSpace : MyGuiScreenServerSearchBase
  {
    private MySpaceServerFilterOptions SpaceFilters => this.FilterOptions as MySpaceServerFilterOptions;

    public MyGuiScreenServerSearchSpace(MyGuiScreenJoinGame joinScreen)
      : base(joinScreen)
    {
    }

    protected override void DrawTopControls()
    {
      base.DrawTopControls();
      this.AddNumericRangeOption(MySpaceTexts.MultiplayerJoinProductionMultipliers, MySpaceNumericOptionEnum.ProductionMultipliers);
      this.AddNumericRangeOption(MySpaceTexts.WorldSettings_InventorySize, MySpaceNumericOptionEnum.InventoryMultipier);
      this.m_currentPosition.Y += this.m_padding;
    }

    protected override void DrawBottomControls()
    {
      base.DrawBottomControls();
      float maxLabelWidth = 0.26f;
      this.AddCheckboxRow(new MyStringId?[2]
      {
        new MyStringId?(MySpaceTexts.WorldSettings_EnableCopyPaste),
        new MyStringId?(MySpaceTexts.WorldSettings_EnableIngameScripts)
      }, new MySpaceBoolOptionEnum[2]
      {
        MySpaceBoolOptionEnum.CopyPaste,
        MySpaceBoolOptionEnum.Scripts
      }, new MyStringId[2]
      {
        MySpaceTexts.ToolTipWorldSettingsEnableCopyPaste,
        MySpaceTexts.ToolTipWorldSettings_EnableIngameScripts
      });
      this.AddCheckboxRow(new MyStringId?[2]
      {
        new MyStringId?(MySpaceTexts.WorldSettings_PermanentDeath),
        new MyStringId?(MySpaceTexts.WorldSettings_EnableWeapons)
      }, new MySpaceBoolOptionEnum[2]
      {
        MySpaceBoolOptionEnum.PermanentDeath,
        MySpaceBoolOptionEnum.Weapons
      }, new MyStringId[2]
      {
        MySpaceTexts.ToolTipWorldSettingsPermanentDeath,
        MySpaceTexts.ToolTipWorldSettingsWeapons
      });
      this.AddCheckboxRow(new MyStringId?[2]
      {
        new MyStringId?(MySpaceTexts.WorldSettings_Enable3rdPersonCamera),
        new MyStringId?(MySpaceTexts.WorldSettings_EnableSpectator)
      }, new MySpaceBoolOptionEnum[2]
      {
        MySpaceBoolOptionEnum.ThirdPerson,
        MySpaceBoolOptionEnum.Spectator
      }, new MyStringId[2]
      {
        MySpaceTexts.ToolTipWorldSettings_Enable3rdPersonCamera,
        MySpaceTexts.ToolTipWorldSettingsEnableSpectator
      });
      this.AddCheckboxRow(new MyStringId?[2]
      {
        new MyStringId?(MySpaceTexts.World_Settings_EnableOxygenPressurization),
        new MyStringId?(MySpaceTexts.World_Settings_EnableOxygen)
      }, new MySpaceBoolOptionEnum[2]
      {
        MySpaceBoolOptionEnum.Airtightness,
        MySpaceBoolOptionEnum.Oxygen
      }, new MyStringId[2]
      {
        MySpaceTexts.ToolTipWorldSettings_EnableOxygenPressurization,
        MySpaceTexts.ToolTipWorldSettings_EnableOxygen
      });
      this.AddCheckboxRow(new MyStringId?[2]
      {
        new MyStringId?(MySpaceTexts.ServerDetails_ServerManagement),
        new MyStringId?(MySpaceTexts.WorldSettings_StationVoxelSupport)
      }, new MySpaceBoolOptionEnum[2]
      {
        MySpaceBoolOptionEnum.ExternalServerManagement,
        MySpaceBoolOptionEnum.UnsupportedStations
      }, new MyStringId[2]
      {
        MySpaceTexts.ServerDetails_ServerManagement,
        MySpaceTexts.ToolTipWorldSettings_StationVoxelSupport
      });
      this.AddCheckboxRow(new MyStringId?[2]
      {
        new MyStringId?(MySpaceTexts.WorldSettings_DestructibleBlocks),
        new MyStringId?(MySpaceTexts.WorldSettings_ThrusterDamage)
      }, new MySpaceBoolOptionEnum[2]
      {
        MySpaceBoolOptionEnum.BlockDestruction,
        MySpaceBoolOptionEnum.ThrusterDamage
      }, new MyStringId[2]
      {
        MySpaceTexts.ToolTipWorldSettingsDestructibleBlocks,
        MySpaceTexts.ToolTipWorldSettingsThrusterDamage
      }, maxLabelWidth);
      this.AddCheckboxRow(new MyStringId?[2]
      {
        new MyStringId?(MySpaceTexts.WorldSettings_EnableCargoShips),
        new MyStringId?(MySpaceTexts.WorldSettings_Encounters)
      }, new MySpaceBoolOptionEnum[2]
      {
        MySpaceBoolOptionEnum.CargoShips,
        MySpaceBoolOptionEnum.Encounters
      }, new MyStringId[2]
      {
        MySpaceTexts.ToolTipWorldSettingsEnableCargoShips,
        MySpaceTexts.ToolTipWorldSettings_EnableEncounters
      });
      this.AddCheckboxRow(new MyStringId?[2]
      {
        new MyStringId?(MySpaceTexts.WorldSettings_EnableSpiders),
        new MyStringId?(MySpaceTexts.WorldSettings_EnableRespawnShips)
      }, new MySpaceBoolOptionEnum[2]
      {
        MySpaceBoolOptionEnum.Spiders,
        MySpaceBoolOptionEnum.RespawnShips
      }, new MyStringId[2]
      {
        MySpaceTexts.ToolTipWorldSettings_EnableSpiders,
        MySpaceTexts.ToolTipWorldSettings_EnableRespawnShips
      }, maxLabelWidth);
      this.AddCheckboxRow(new MyStringId?[2]
      {
        new MyStringId?(MySpaceTexts.WorldSettings_EnableDrones),
        new MyStringId?(MySpaceTexts.WorldSettings_EnableWolfs)
      }, new MySpaceBoolOptionEnum[2]
      {
        MySpaceBoolOptionEnum.Drones,
        MySpaceBoolOptionEnum.Wolves
      }, new MyStringId[2]
      {
        MySpaceTexts.ToolTipWorldSettings_EnableDrones,
        MySpaceTexts.ToolTipWorldSettings_EnableWolfs
      });
    }

    protected override void DrawMidControls()
    {
      base.DrawMidControls();
      Vector2 currentPosition = this.m_currentPosition;
      this.m_currentPosition.Y = -0.102f;
      this.m_currentPosition.X += this.m_padding / 2.4f;
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: MyTexts.GetString(MySpaceTexts.WorldSettings_EnvironmentHostility)));
      MyGuiControlCombobox combo = new MyGuiControlCombobox(new Vector2?(this.m_currentPosition));
      combo.AddItem(-1L, MyCommonTexts.Any);
      combo.AddItem(0L, MySpaceTexts.WorldSettings_EnvironmentHostilitySafe);
      combo.AddItem(1L, MySpaceTexts.WorldSettings_EnvironmentHostilityNormal);
      combo.AddItem(2L, MySpaceTexts.WorldSettings_EnvironmentHostilityCataclysm);
      combo.AddItem(3L, MySpaceTexts.WorldSettings_EnvironmentHostilityCataclysmUnreal);
      combo.Size = new Vector2(0.295f, 1f);
      combo.PositionX += (float) ((double) combo.Size.X / 2.0 + (double) this.m_padding * 12.3000001907349);
      combo.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_EvnironmentHostility));
      MyFilterRange filter1 = this.SpaceFilters.GetFilter(MySpaceNumericOptionEnum.EnvionmentHostility);
      if (filter1.Active)
        combo.SelectItemByKey((long) filter1.Value.Max, false);
      else
        combo.SelectItemByKey(-1L, false);
      combo.ItemSelected += (MyGuiControlCombobox.ItemSelectedDelegate) (() =>
      {
        MyFilterRange filter2 = this.SpaceFilters.GetFilter(MySpaceNumericOptionEnum.EnvionmentHostility);
        long selectedKey = combo.GetSelectedKey();
        if (selectedKey == -1L)
        {
          filter2.Active = false;
        }
        else
        {
          filter2.Active = true;
          filter2.Value = new SerializableRange()
          {
            Min = (float) selectedKey,
            Max = (float) selectedKey
          };
        }
      });
      this.Controls.Add((MyGuiControlBase) combo);
      this.m_currentPosition.X = currentPosition.X;
      this.m_currentPosition.Y += 0.04f + this.m_padding;
    }

    private void AddNumericRangeOption(MyStringId text, MySpaceNumericOptionEnum key)
    {
      MyFilterRange filter = this.SpaceFilters.GetFilter(key);
      if (filter == null)
        throw new ArgumentOutOfRangeException(nameof (key), (object) key, "Filter not found in dictionary!");
      this.AddNumericRangeOption(text, (Action<SerializableRange>) (r => filter.Value = r), filter.Value, filter.Active, (Action<MyGuiControlCheckbox>) (c => filter.Active = c.IsChecked), this.EnableAdvanced);
    }

    private MyGuiControlIndeterminateCheckbox[] AddCheckboxRow(
      MyStringId?[] text,
      MySpaceBoolOptionEnum[] keys,
      MyStringId[] tooltip,
      float maxLabelWidth = float.PositiveInfinity)
    {
      Action<MyGuiControlIndeterminateCheckbox>[] onClick = new Action<MyGuiControlIndeterminateCheckbox>[2];
      CheckStateEnum[] values = new CheckStateEnum[2];
      if (keys.Length != 0)
      {
        MyFilterBool filter = this.SpaceFilters.GetFilter(keys[0]);
        if (filter == null)
          throw new ArgumentOutOfRangeException(nameof (keys), (object) keys[0], "Filter not found in dictionary!");
        onClick[0] = (Action<MyGuiControlIndeterminateCheckbox>) (c => filter.CheckValue = c.State);
        values[0] = filter.CheckValue;
      }
      if (keys.Length > 1)
      {
        MyFilterBool filter = this.SpaceFilters.GetFilter(keys[1]);
        if (filter == null)
          throw new ArgumentOutOfRangeException(nameof (keys), (object) keys[1], "Filter not found in dictionary!");
        onClick[1] = (Action<MyGuiControlIndeterminateCheckbox>) (c => filter.CheckValue = c.State);
        values[1] = filter.CheckValue;
      }
      if (keys.Length > 2)
        throw new ArgumentOutOfRangeException();
      return this.AddIndeterminateDuo(text, onClick, new MyStringId?[2]
      {
        new MyStringId?(tooltip[0]),
        new MyStringId?(tooltip[1])
      }, values, (this.EnableAdvanced ? 1 : 0) != 0, maxLabelWidth);
    }
  }
}

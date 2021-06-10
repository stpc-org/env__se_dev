// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudBlockInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
  public class MyHudBlockInfo
  {
    public static readonly float EPSILON = 1f / 1000f;
    public bool ShowDetails;
    public List<MyHudBlockInfo.ComponentInfo> Components = new List<MyHudBlockInfo.ComponentInfo>(12);
    public string BlockName;
    private string m_contextHelp;
    private MyDefinitionId m_definitionId;
    public string[] BlockIcons;
    public float m_blockIntegrity;
    public float m_blockIntegrityChecked;
    public float CriticalIntegrity;
    public float OwnershipIntegrity;
    public bool ShowAvailable;
    public int CriticalComponentIndex = -1;
    public int MissingComponentIndex = -1;
    public int PCUCost;
    private long m_blockBuiltBy;
    public MyCubeSize GridSize;
    private MyHudBlockInfo.WhoWantsInfoDisplayed m_displayers;

    public string ContextHelp => this.m_contextHelp;

    public event Action<string> ContextHelpChanged;

    public MyDefinitionId DefinitionId
    {
      get => this.m_definitionId;
      set
      {
        this.m_definitionId = value;
        ++this.Version;
      }
    }

    public float BlockIntegrity
    {
      get => this.m_blockIntegrity;
      set
      {
        if ((double) Math.Abs(this.m_blockIntegrityChecked - value) > (double) MyHudBlockInfo.EPSILON)
        {
          this.m_blockIntegrityChecked = value;
          ++this.Version;
        }
        this.m_blockIntegrity = value;
      }
    }

    public long BlockBuiltBy
    {
      get => this.m_blockBuiltBy;
      set
      {
        if (this.m_blockBuiltBy == value)
          return;
        this.m_blockBuiltBy = value;
      }
    }

    public bool Visible => (uint) this.m_displayers > 0U;

    public void AddDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed displayer) => this.m_displayers |= displayer;

    public void RemoveDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed displayer) => this.m_displayers &= ~displayer;

    public void ChangeDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed displayer, bool display)
    {
      if (display)
        this.AddDisplayer(displayer);
      else
        this.RemoveDisplayer(displayer);
    }

    public void ClearDisplayers() => this.m_displayers = MyHudBlockInfo.WhoWantsInfoDisplayed.None;

    public int Version { get; private set; }

    public void SetContextHelp(MyDefinitionBase definition)
    {
      if (!string.IsNullOrEmpty(definition.DescriptionText))
      {
        if (string.IsNullOrEmpty(definition.DescriptionArgs))
        {
          this.m_contextHelp = definition.DescriptionText;
        }
        else
        {
          string[] strArray = definition.DescriptionArgs.Split(',');
          object[] objArray = new object[strArray.Length];
          for (int index = 0; index < strArray.Length; ++index)
            objArray[index] = MyIngameHelpObjective.GetHighlightedControl(MyStringId.GetOrCompute(strArray[index]));
          this.m_contextHelp = string.Format(definition.DescriptionText, objArray);
        }
      }
      else
        this.m_contextHelp = MyTexts.GetString(MySpaceTexts.Description_NotAvailable);
      Action<string> contextHelpChanged = this.ContextHelpChanged;
      if (contextHelpChanged == null)
        return;
      contextHelpChanged(this.m_contextHelp);
    }

    [Flags]
    public enum WhoWantsInfoDisplayed
    {
      None = 0,
      Tool = 1,
      CubeBuilder = 2,
      Cockpit = 4,
      Clipboard = 8,
    }

    public struct ComponentInfo
    {
      public MyDefinitionId DefinitionId;
      public string[] Icons;
      public string ComponentName;
      public int MountedCount;
      public int StockpileCount;
      public int TotalCount;
      public int AvailableAmount;

      public int InstalledCount => this.MountedCount + this.StockpileCount;

      public override string ToString() => string.Format("{0}/{1}/{2} {3}", (object) this.MountedCount, (object) this.StockpileCount, (object) this.TotalCount, (object) this.ComponentName);
    }
  }
}

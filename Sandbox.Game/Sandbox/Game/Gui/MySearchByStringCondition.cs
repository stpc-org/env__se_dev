// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MySearchByStringCondition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game;

namespace Sandbox.Game.Gui
{
  public class MySearchByStringCondition : IMySearchCondition
  {
    private string[] m_searchItems;
    private HashSet<MyCubeBlockDefinitionGroup> m_sortedBlocks = new HashSet<MyCubeBlockDefinitionGroup>();

    public string SearchName
    {
      set => this.m_searchItems = value.Split(' ');
    }

    public bool IsValid => this.m_searchItems != null;

    public void Clean()
    {
      this.m_searchItems = (string[]) null;
      this.CleanDefinitionGroups();
    }

    public bool MatchesCondition(string itemId)
    {
      foreach (string searchItem in this.m_searchItems)
      {
        if (!itemId.Contains(searchItem, StringComparison.OrdinalIgnoreCase))
          return false;
      }
      return true;
    }

    public bool MatchesCondition(MyDefinitionBase itemId) => itemId != null && itemId.DisplayNameText != null && this.MatchesCondition(itemId.DisplayNameText);

    public void AddDefinitionGroup(MyCubeBlockDefinitionGroup definitionGruop) => this.m_sortedBlocks.Add(definitionGruop);

    public HashSet<MyCubeBlockDefinitionGroup> GetSortedBlocks() => this.m_sortedBlocks;

    public void CleanDefinitionGroups() => this.m_sortedBlocks.Clear();
  }
}

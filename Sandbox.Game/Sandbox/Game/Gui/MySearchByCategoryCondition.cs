// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MySearchByCategoryCondition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System.Collections.Generic;
using VRage.Game;

namespace Sandbox.Game.Gui
{
  public class MySearchByCategoryCondition : IMySearchCondition
  {
    public List<MyGuiBlockCategoryDefinition> SelectedCategories;
    private MyGuiBlockCategoryDefinition m_lastCategory;
    private HashSet<MyCubeBlockDefinitionGroup> m_sortedBlocks = new HashSet<MyCubeBlockDefinitionGroup>();
    private Dictionary<string, List<MyCubeBlockDefinitionGroup>> m_blocksByCategories = new Dictionary<string, List<MyCubeBlockDefinitionGroup>>();

    public bool StrictSearch { get; set; }

    public bool MatchesCondition(string itemId) => this.IsItemInAnySelectedCategory(itemId);

    public bool MatchesCondition(MyDefinitionBase itemId) => this.IsItemInAnySelectedCategory(itemId.Id.ToString());

    public void AddDefinitionGroup(MyCubeBlockDefinitionGroup definitionGruop)
    {
      if (this.m_lastCategory == null)
        return;
      List<MyCubeBlockDefinitionGroup> blockDefinitionGroupList = (List<MyCubeBlockDefinitionGroup>) null;
      if (!this.m_blocksByCategories.TryGetValue(this.m_lastCategory.Name, out blockDefinitionGroupList))
      {
        blockDefinitionGroupList = new List<MyCubeBlockDefinitionGroup>();
        this.m_blocksByCategories.Add(this.m_lastCategory.Name, blockDefinitionGroupList);
      }
      blockDefinitionGroupList.Add(definitionGruop);
    }

    public HashSet<MyCubeBlockDefinitionGroup> GetSortedBlocks()
    {
      foreach (KeyValuePair<string, List<MyCubeBlockDefinitionGroup>> blocksByCategory in this.m_blocksByCategories)
      {
        foreach (MyCubeBlockDefinitionGroup blockDefinitionGroup in blocksByCategory.Value)
          this.m_sortedBlocks.Add(blockDefinitionGroup);
      }
      return this.m_sortedBlocks;
    }

    public void CleanDefinitionGroups()
    {
      this.m_sortedBlocks.Clear();
      this.m_blocksByCategories.Clear();
    }

    private bool IsItemInAnySelectedCategory(string itemId)
    {
      this.m_lastCategory = (MyGuiBlockCategoryDefinition) null;
      if (this.SelectedCategories == null)
        return true;
      foreach (MyGuiBlockCategoryDefinition selectedCategory in this.SelectedCategories)
      {
        if (selectedCategory.HasItem(itemId) || selectedCategory.ShowAnimations && itemId.Contains("AnimationDefinition"))
        {
          this.m_lastCategory = selectedCategory;
          return true;
        }
      }
      return false;
    }
  }
}

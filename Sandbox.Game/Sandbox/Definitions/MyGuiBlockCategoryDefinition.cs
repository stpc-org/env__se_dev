// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyGuiBlockCategoryDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GuiBlockCategoryDefinition), null)]
  public class MyGuiBlockCategoryDefinition : MyDefinitionBase
  {
    public string Name;
    public HashSet<string> ItemIds;
    public bool IsShipCategory;
    public bool IsBlockCategory = true;
    public bool SearchBlocks = true;
    public bool ShowAnimations;
    public bool ShowInCreative = true;
    public bool IsAnimationCategory;
    public bool IsToolCategory;
    public int ValidItems;
    public new bool Public = true;
    public bool StrictSearch;

    protected override void Init(MyObjectBuilder_DefinitionBase ob)
    {
      base.Init(ob);
      MyObjectBuilder_GuiBlockCategoryDefinition categoryDefinition = (MyObjectBuilder_GuiBlockCategoryDefinition) ob;
      this.Name = categoryDefinition.Name;
      this.ItemIds = new HashSet<string>((IEnumerable<string>) ((IEnumerable<string>) categoryDefinition.ItemIds).ToList<string>());
      this.IsBlockCategory = categoryDefinition.IsBlockCategory;
      this.IsShipCategory = categoryDefinition.IsShipCategory;
      this.SearchBlocks = categoryDefinition.SearchBlocks;
      this.ShowAnimations = categoryDefinition.ShowAnimations;
      this.ShowInCreative = categoryDefinition.ShowInCreative;
      this.Public = categoryDefinition.Public;
      this.IsAnimationCategory = categoryDefinition.IsAnimationCategory;
      this.IsToolCategory = categoryDefinition.IsToolCategory;
      this.StrictSearch = categoryDefinition.StrictSearch;
    }

    public bool HasItem(string itemId)
    {
      foreach (string itemId1 in this.ItemIds)
      {
        if (itemId.EndsWith(itemId1))
          return true;
      }
      return false;
    }

    private class Sandbox_Definitions_MyGuiBlockCategoryDefinition\u003C\u003EActor : IActivator, IActivator<MyGuiBlockCategoryDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGuiBlockCategoryDefinition();

      MyGuiBlockCategoryDefinition IActivator<MyGuiBlockCategoryDefinition>.CreateInstance() => new MyGuiBlockCategoryDefinition();
    }
  }
}

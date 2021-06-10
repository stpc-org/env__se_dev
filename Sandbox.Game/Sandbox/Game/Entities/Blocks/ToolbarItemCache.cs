// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.ToolbarItemCache
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Screens.Helpers;
using VRage.Game;
using VRage.Serialization;

namespace Sandbox.Game.Entities.Blocks
{
  public struct ToolbarItemCache
  {
    private MyToolbarItem m_cachedItem;
    private ToolbarItem m_item;

    public ToolbarItem Item
    {
      get => this.m_item;
      set
      {
        this.m_item = value;
        this.m_cachedItem = (MyToolbarItem) null;
      }
    }

    [NoSerialize]
    public MyToolbarItem CachedItem
    {
      get
      {
        if (this.m_cachedItem == null)
          this.m_cachedItem = ToolbarItem.ToItem(this.Item);
        return this.m_cachedItem;
      }
    }

    public MyObjectBuilder_ToolbarItem ToObjectBuilder() => this.m_cachedItem?.GetObjectBuilder();

    public void SetToToolbar(MyToolbar toolbar, int index)
    {
      MyToolbarItem cachedItem = this.m_cachedItem;
      if (cachedItem == null)
        return;
      toolbar.SetItemAtIndex(index, cachedItem);
    }

    public static implicit operator ToolbarItemCache(ToolbarItem item) => new ToolbarItemCache()
    {
      m_item = item
    };
  }
}

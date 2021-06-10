// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  public abstract class MyToolbarItem
  {
    public bool Enabled { get; private set; }

    public string[] Icons { get; private set; }

    public string SubIcon { get; private set; }

    public StringBuilder IconText { get; private set; }

    public StringBuilder DisplayName { get; private set; }

    public bool WantsToBeActivated { get; protected set; }

    public bool WantsToBeSelected { get; protected set; }

    public bool ActivateOnClick { get; protected set; }

    public MyToolbarItem()
    {
      this.Icons = new string[1]
      {
        MyGuiConstants.TEXTURE_ICON_FAKE.Texture
      };
      this.IconText = new StringBuilder();
      this.DisplayName = new StringBuilder();
    }

    public virtual void OnRemovedFromToolbar(MyToolbar toolbar)
    {
    }

    public virtual void OnAddedToToolbar(MyToolbar toolbar)
    {
    }

    public abstract bool Activate();

    public abstract bool Init(MyObjectBuilder_ToolbarItem data);

    public abstract MyObjectBuilder_ToolbarItem GetObjectBuilder();

    public abstract bool AllowedInToolbarType(MyToolbarType type);

    public abstract MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0);

    public MyToolbarItem.ChangeInfo SetEnabled(bool newEnabled)
    {
      if (newEnabled == this.Enabled)
        return MyToolbarItem.ChangeInfo.None;
      this.Enabled = newEnabled;
      return MyToolbarItem.ChangeInfo.Enabled;
    }

    public MyToolbarItem.ChangeInfo SetIcons(string[] newIcons)
    {
      if (newIcons == this.Icons)
        return MyToolbarItem.ChangeInfo.None;
      this.Icons = newIcons;
      return MyToolbarItem.ChangeInfo.Icon;
    }

    public MyToolbarItem.ChangeInfo SetSubIcon(string newSubIcon)
    {
      if (newSubIcon == this.SubIcon)
        return MyToolbarItem.ChangeInfo.None;
      this.SubIcon = newSubIcon;
      return MyToolbarItem.ChangeInfo.SubIcon;
    }

    public MyToolbarItem.ChangeInfo SetIconText(StringBuilder newIconText)
    {
      if (newIconText == null || this.IconText.CompareTo(newIconText) == 0)
        return MyToolbarItem.ChangeInfo.None;
      this.IconText.Clear();
      this.IconText.AppendStringBuilder(newIconText);
      return MyToolbarItem.ChangeInfo.IconText;
    }

    public MyToolbarItem.ChangeInfo ClearIconText()
    {
      if (this.IconText.Length == 0)
        return MyToolbarItem.ChangeInfo.None;
      this.IconText.Clear();
      return MyToolbarItem.ChangeInfo.IconText;
    }

    public MyToolbarItem.ChangeInfo SetDisplayName(string newDisplayName)
    {
      if (newDisplayName == null || this.DisplayName.CompareTo(newDisplayName) == 0)
        return MyToolbarItem.ChangeInfo.None;
      this.DisplayName.Clear();
      this.DisplayName.Append(newDisplayName);
      return MyToolbarItem.ChangeInfo.DisplayName;
    }

    public virtual void FillGridItem(MyGuiGridItem gridItem)
    {
      if (this.IconText.Length == 0)
        gridItem.ClearText(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
      else
        gridItem.AddText(this.IconText, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
    }

    public override bool Equals(object obj) => throw new InvalidOperationException("GetHashCode and Equals must be overridden");

    public override int GetHashCode() => throw new InvalidOperationException("GetHashCode and Equals must be overridden");

    [Flags]
    public enum ChangeInfo
    {
      None = 0,
      Enabled = 1,
      Icon = 2,
      SubIcon = 4,
      IconText = 8,
      DisplayName = 16, // 0x00000010
      All = DisplayName | IconText | SubIcon | Icon | Enabled, // 0x0000001F
    }
  }
}

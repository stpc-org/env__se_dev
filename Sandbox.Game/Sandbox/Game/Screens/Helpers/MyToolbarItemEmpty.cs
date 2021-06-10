// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyToolbarItemEmpty
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
  [MyToolbarItemDescriptor(typeof (MyObjectBuilder_ToolbarItemEmpty))]
  internal class MyToolbarItemEmpty : MyToolbarItem
  {
    public static MyToolbarItemEmpty Default = new MyToolbarItemEmpty();

    public MyToolbarItemEmpty()
    {
      int num = (int) this.SetEnabled(true);
      this.ActivateOnClick = false;
      this.WantsToBeSelected = true;
    }

    public override bool Activate() => false;

    public override bool Equals(object obj) => false;

    public override int GetHashCode() => -1;

    public override bool Init(MyObjectBuilder_ToolbarItem data) => true;

    public override MyObjectBuilder_ToolbarItem GetObjectBuilder() => (MyObjectBuilder_ToolbarItem) null;

    public override bool AllowedInToolbarType(MyToolbarType type) => true;

    public override MyToolbarItem.ChangeInfo Update(MyEntity owner, long playerID = 0) => MyToolbarItem.ChangeInfo.None;
  }
}

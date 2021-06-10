// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyItemComparer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;

namespace Sandbox.Game.Gui
{
  public class MyItemComparer : IComparer<MyGuiControlListbox.Item>
  {
    private Func<MyGuiControlListbox.Item, MyGuiControlListbox.Item, int> comparator;

    public MyItemComparer(
      Func<MyGuiControlListbox.Item, MyGuiControlListbox.Item, int> comp)
    {
      this.comparator = comp;
    }

    public int Compare(MyGuiControlListbox.Item x, MyGuiControlListbox.Item y) => this.comparator != null ? this.comparator(x, y) : 0;
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyItemComparer_Rew
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;

namespace Sandbox.Game.Gui
{
  public class MyItemComparer_Rew : IComparer<MyBlueprintItemInfo>
  {
    private Func<MyBlueprintItemInfo, MyBlueprintItemInfo, int> comparator;

    public MyItemComparer_Rew(
      Func<MyBlueprintItemInfo, MyBlueprintItemInfo, int> comp)
    {
      this.comparator = comp;
    }

    public int Compare(MyBlueprintItemInfo x, MyBlueprintItemInfo y) => this.comparator != null ? this.comparator(x, y) : 0;
  }
}

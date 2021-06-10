// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyTerminalComparer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using System.Text;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyTerminalComparer : IComparer<MyTerminalBlock>, IComparer<MyBlockGroup>
  {
    public static MyTerminalComparer Static = new MyTerminalComparer();

    public int Compare(MyTerminalBlock lhs, MyTerminalBlock rhs)
    {
      int num = (lhs.CustomName != null ? lhs.CustomName.ToString() : lhs.DefinitionDisplayNameText).CompareTo(rhs.CustomName != null ? rhs.CustomName.ToString() : rhs.DefinitionDisplayNameText);
      if (num != 0)
        return num;
      return lhs.NumberInGrid != rhs.NumberInGrid ? lhs.NumberInGrid.CompareTo(rhs.NumberInGrid) : 0;
    }

    public int Compare(MyBlockGroup x, MyBlockGroup y) => x.Name.CompareTo(y.Name);
  }
}

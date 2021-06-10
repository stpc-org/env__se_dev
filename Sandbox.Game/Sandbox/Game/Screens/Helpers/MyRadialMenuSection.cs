// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyRadialMenuSection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyRadialMenuSection
  {
    public List<MyRadialMenuItem> Items;
    public MyStringId Label;
    public bool IsEnabledCreative;
    public bool IsEnabledSurvival;

    public MyRadialMenuSection()
    {
    }

    public MyRadialMenuSection(List<MyRadialMenuItem> items, MyStringId label)
    {
      this.Items = items;
      this.Label = label;
    }

    public void Init(MyObjectBuilder_RadialMenuSection builder)
    {
      this.Label = builder.Label;
      this.Items = new List<MyRadialMenuItem>();
      foreach (MyObjectBuilder_RadialMenuItem data in builder.Items)
        this.Items.Add(MyRadialMenuItemFactory.CreateRadialMenuItem(data));
      this.IsEnabledCreative = builder.IsEnabledCreative;
      this.IsEnabledSurvival = builder.IsEnabledSurvival;
    }

    public void Postprocess() => this.Items.RemoveAll((Predicate<MyRadialMenuItem>) (x => !x.IsValid));
  }
}

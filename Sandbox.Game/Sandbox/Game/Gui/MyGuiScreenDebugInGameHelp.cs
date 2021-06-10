// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugInGameHelp
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Game", "In Game Help")]
  internal class MyGuiScreenDebugInGameHelp : MyGuiScreenDebugBase
  {
    private MyGuiControlListbox m_objectives;
    private MyStringHash m_objectiveToSet;
    private long m_nextTimeToSet;

    public MyGuiScreenDebugInGameHelp()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("Localization", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f * this.m_scale;
      this.AddLabel("Loading Screen Texts", Color.Yellow.ToVector4(), 1.2f);
      if (MySession.Static == null)
        return;
      this.m_objectives = this.AddListBox(0.37f);
      this.m_objectives.MultiSelect = false;
      this.m_objectives.VisibleRowsCount = 10;
      foreach (MyStringHash myStringHash in (IEnumerable<MyStringHash>) MySession.Static.GetComponent<MySessionComponentIngameHelp>().AvailableObjectives.OrderBy<MyStringHash, string>((Func<MyStringHash, string>) (x => x.String), (IComparer<string>) StringComparer.InvariantCulture))
        this.m_objectives.Items.Add(new MyGuiControlListbox.Item(new StringBuilder(myStringHash.String), myStringHash.String, userData: ((object) myStringHash)));
      this.AddButton("Set Current Objective", (Action<MyGuiControlButton>) (x =>
      {
        MyGuiControlListbox.Item lastSelected = this.m_objectives.GetLastSelected();
        if (lastSelected == null || !(lastSelected.UserData is MyStringHash userData))
          return;
        this.m_objectiveToSet = userData;
        this.m_nextTimeToSet = DateTime.Now.AddSeconds(1.0).Ticks;
      }));
    }

    public override bool Update(bool hasFocus)
    {
      if (this.m_nextTimeToSet < DateTime.Now.Ticks && this.m_objectiveToSet != MyStringHash.NullOrEmpty)
      {
        MySession.Static.GetComponent<MySessionComponentIngameHelp>().ForceObjective(this.m_objectiveToSet);
        this.m_objectiveToSet = MyStringHash.NullOrEmpty;
      }
      return base.Update(hasFocus);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugInGameHelp);
  }
}

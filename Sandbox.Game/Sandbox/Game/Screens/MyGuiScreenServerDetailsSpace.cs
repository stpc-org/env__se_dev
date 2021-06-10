// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenServerDetailsSpace
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRage;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenServerDetailsSpace : MyGuiScreenServerDetailsBase
  {
    public MyGuiScreenServerDetailsSpace(MyCachedServerItem server)
      : base(server)
    {
    }

    protected override void DrawSettings()
    {
      if (this.m_server.Rules != null)
      {
        string str;
        this.m_server.Rules.TryGetValue("SM", out str);
        if (!string.IsNullOrEmpty(str))
          this.AddLabel(MySpaceTexts.ServerDetails_ServerManagement, (object) str);
      }
      if (!string.IsNullOrEmpty(this.m_server.Description))
      {
        this.AddLabel(MyCommonTexts.Description, (object) null);
        this.m_currentPosition.Y += 0.008f;
        this.AddMultilineText(this.m_server.Description, 0.15f);
        this.m_currentPosition.Y += 0.008f;
      }
      MyGuiControlLabel myGuiControlLabel1 = this.AddLabel(MyCommonTexts.ServerDetails_WorldSettings, (object) null);
      SortedList<string, object> sortedList = this.LoadSessionSettings(VRage.Game.Game.SpaceEngineers);
      if (sortedList == null)
      {
        this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: MyTexts.GetString(MyCommonTexts.ServerDetails_SettingError), font: "Red"));
      }
      else
      {
        MyGuiControlParent parent = new MyGuiControlParent();
        MyGuiControlScrollablePanel controlScrollablePanel = new MyGuiControlScrollablePanel((MyGuiControlBase) parent);
        controlScrollablePanel.ScrollbarVEnabled = true;
        controlScrollablePanel.Position = this.m_currentPosition;
        controlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        controlScrollablePanel.Size = new Vector2(this.Size.Value.X - 0.112f, (float) ((double) this.Size.Value.Y / 2.0 - (double) this.m_currentPosition.Y - 0.144999995827675));
        controlScrollablePanel.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
        controlScrollablePanel.ScrolledAreaPadding = new MyGuiBorderThickness(0.005f);
        controlScrollablePanel.CanHaveFocus = true;
        controlScrollablePanel.CanFocusChildren = false;
        this.FocusedControl = (MyGuiControlBase) controlScrollablePanel;
        this.Controls.Add((MyGuiControlBase) controlScrollablePanel);
        Vector2 localPos = -controlScrollablePanel.Size / 2f;
        float y = 0.0f;
        foreach (KeyValuePair<string, object> keyValuePair in sortedList)
        {
          y += myGuiControlLabel1.Size.Y / 2f + this.m_padding;
          if (keyValuePair.Value is SerializableDictionary<string, short> serializableDictionary)
          {
            int count = serializableDictionary.Dictionary.Count;
            y += (myGuiControlLabel1.Size.Y / 2f + this.m_padding) * (float) count;
          }
        }
        localPos.Y = (float) (-(double) y / 2.0 + (double) myGuiControlLabel1.Size.Y / 2.0);
        parent.Size = new Vector2(controlScrollablePanel.Size.X, y);
        foreach (KeyValuePair<string, object> keyValuePair1 in sortedList)
        {
          object obj = keyValuePair1.Value;
          if (!(obj is SerializableDictionary<string, short>))
          {
            string text = string.Empty;
            if (obj is bool flag)
              text = flag ? MyTexts.GetString(MyCommonTexts.ControlMenuItemValue_On) : MyTexts.GetString(MyCommonTexts.ControlMenuItemValue_Off);
            else if (obj != null)
              text = obj.ToString();
            MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(localPos), text: (MyTexts.Get(MyStringId.GetOrCompute(keyValuePair1.Key)).ToString() + ":"));
            MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2?(new Vector2(controlScrollablePanel.Size.X / 2.5f, localPos.Y)), text: text, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
            localPos.Y += myGuiControlLabel2.Size.Y / 2f + this.m_padding;
            parent.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
            parent.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
            this.AddSeparator(parent, localPos);
          }
          else
          {
            Dictionary<string, short> dictionary = (obj as SerializableDictionary<string, short>).Dictionary;
            if (dictionary != null)
            {
              MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(localPos), text: (MyTexts.Get(MyStringId.GetOrCompute(keyValuePair1.Key)).ToString() + ":"));
              parent.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
              localPos.Y += myGuiControlLabel2.Size.Y / 2f + this.m_padding;
              this.AddSeparator(parent, localPos);
              foreach (KeyValuePair<string, short> keyValuePair2 in dictionary)
              {
                MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2?(localPos), text: ("     " + keyValuePair2.Key));
                MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(new Vector2?(new Vector2(controlScrollablePanel.Size.X / 2.5f, localPos.Y)), text: keyValuePair2.Value.ToString(), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
                parent.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
                parent.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
                localPos.Y += myGuiControlLabel3.Size.Y / 2f + this.m_padding;
                this.AddSeparator(parent, localPos);
              }
            }
          }
        }
      }
    }
  }
}

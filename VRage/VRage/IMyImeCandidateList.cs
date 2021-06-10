// Decompiled with JetBrains decompiler
// Type: VRage.IMyImeCandidateList
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace VRage
{
  public interface IMyImeCandidateList : IVRageGuiControl
  {
    Vector2 Position { get; set; }

    MyGuiDrawAlignEnum OriginAlign { get; set; }

    event Action<IMyImeCandidateList, int> ItemClicked;

    void Activate(bool autoPositionOnMouseTip, Vector2? offset = null);

    void Deactivate();

    void Clear();

    Vector2 GetListBoxSize();

    void AddItem(StringBuilder text, string tooltip = "", string icon = "", object userData = null);

    void CreateNewContextMenu();

    bool IsGuiControlEqual(IVRageGuiControl focusedControl);
  }
}

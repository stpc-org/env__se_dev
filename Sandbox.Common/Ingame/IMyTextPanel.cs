// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyTextPanel
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyTextPanel : IMyTextSurface, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    bool WritePublicTitle(string value, bool append = false);

    string GetPublicTitle();

    [Obsolete("LCD private text is deprecated")]
    bool WritePrivateText(string value, bool append = false);

    [Obsolete("LCD private text is deprecated")]
    string GetPrivateText();

    [Obsolete("LCD private text is deprecated")]
    bool WritePrivateTitle(string value, bool append = false);

    [Obsolete("LCD private text is deprecated")]
    string GetPrivateTitle();

    [Obsolete("LCD private text is deprecated")]
    void ShowPrivateTextOnScreen();

    [Obsolete("LCD public text is deprecated")]
    bool WritePublicText(string value, bool append = false);

    [Obsolete("LCD public text is deprecated")]
    string GetPublicText();

    [Obsolete("LCD public text is deprecated")]
    bool WritePublicText(StringBuilder value, bool append = false);

    [Obsolete("LCD public text is deprecated")]
    void ReadPublicText(StringBuilder buffer, bool append = false);

    [Obsolete("LCD public text is deprecated")]
    void ShowPublicTextOnScreen();

    [Obsolete("LCD public text is deprecated")]
    void ShowTextureOnScreen();

    [Obsolete("LCD public text is deprecated")]
    void SetShowOnScreen(ShowTextOnScreenFlag set);

    [Obsolete("LCD public text is deprecated")]
    ShowTextOnScreenFlag ShowOnScreen { get; }

    [Obsolete("LCD public text is deprecated")]
    bool ShowText { get; }
  }
}

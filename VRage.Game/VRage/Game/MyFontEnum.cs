// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyFontEnum
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game
{
  public struct MyFontEnum
  {
    public const string Debug = "Debug";
    public const string Red = "Red";
    public const string Green = "Green";
    public const string Blue = "Blue";
    public const string White = "White";
    public const string DarkBlue = "DarkBlue";
    public const string UrlNormal = "UrlNormal";
    public const string UrlHighlight = "UrlHighlight";
    public const string ErrorMessageBoxCaption = "ErrorMessageBoxCaption";
    public const string ErrorMessageBoxText = "ErrorMessageBoxText";
    public const string InfoMessageBoxCaption = "InfoMessageBoxCaption";
    public const string InfoMessageBoxText = "InfoMessageBoxText";
    public const string ScreenCaption = "ScreenCaption";
    public const string GameCredits = "GameCredits";
    public const string LoadingScreen = "LoadingScreen";
    public const string BuildInfo = "BuildInfo";
    public const string BuildInfoHighlight = "BuildInfoHighlight";
    private string m_value;

    public MyFontEnum(string value) => this.m_value = value;

    public override string ToString() => this.m_value;

    public static implicit operator MyFontEnum(string input) => new MyFontEnum(input);

    public static implicit operator string(MyFontEnum input) => input.m_value;
  }
}

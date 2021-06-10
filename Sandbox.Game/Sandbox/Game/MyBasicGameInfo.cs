// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyBasicGameInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Reflection;

namespace Sandbox.Game
{
  public struct MyBasicGameInfo
  {
    public int? GameVersion;
    public string GameName;
    public string GameNameSafe;
    public string ApplicationName;
    public string GameAcronym;
    public string MinimumRequirementsWeb;
    public string SplashScreenImage;
    public string AnalyticId;

    public bool CheckIsSetup()
    {
      bool flag1 = true;
      foreach (FieldInfo field in this.GetType().GetFields())
      {
        bool flag2 = field.GetValue((object) this) != null;
        flag1 &= flag2;
      }
      return flag1;
    }
  }
}

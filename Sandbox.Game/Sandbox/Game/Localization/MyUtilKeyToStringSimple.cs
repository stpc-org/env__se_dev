// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Localization.MyUtilKeyToStringSimple
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Input;

namespace Sandbox.Game.Localization
{
  internal class MyUtilKeyToStringSimple : MyUtilKeyToString
  {
    private string m_name;

    public override string Name => this.m_name;

    public MyUtilKeyToStringSimple(MyKeys key, string name)
      : base(key)
      => this.m_name = name;
  }
}

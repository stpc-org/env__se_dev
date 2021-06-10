// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Localization.MyUtilKeyToStringLocalized
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.Localization
{
  internal class MyUtilKeyToStringLocalized : MyUtilKeyToString
  {
    private MyStringId m_name;

    public override string Name => MyTexts.GetString(this.m_name);

    public MyUtilKeyToStringLocalized(MyKeys key, MyStringId name)
      : base(key)
      => this.m_name = name;
  }
}

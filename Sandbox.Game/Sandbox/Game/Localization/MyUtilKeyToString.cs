// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Localization.MyUtilKeyToString
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Input;

namespace Sandbox.Game.Localization
{
  internal abstract class MyUtilKeyToString
  {
    public MyKeys Key;

    public abstract string Name { get; }

    public MyUtilKeyToString(MyKeys key) => this.Key = key;
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyCreditsPerson
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Text;

namespace Sandbox.Game
{
  public class MyCreditsPerson
  {
    public StringBuilder Name;

    public MyCreditsPerson(string name) => this.Name = new StringBuilder(name);
  }
}

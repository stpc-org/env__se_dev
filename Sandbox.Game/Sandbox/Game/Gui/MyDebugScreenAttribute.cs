// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyDebugScreenAttribute
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.Gui
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public class MyDebugScreenAttribute : Attribute
  {
    public readonly string Group;
    public readonly string Name;
    public readonly MyDirectXSupport DirectXSupport;

    public MyDebugScreenAttribute(string group, string name, MyDirectXSupport directXSupport)
    {
      this.Group = group;
      this.Name = name;
      this.DirectXSupport = directXSupport;
    }

    public MyDebugScreenAttribute(string group, string name)
    {
      this.Group = group;
      this.Name = name;
      this.DirectXSupport = MyDirectXSupport.DX11;
    }
  }
}

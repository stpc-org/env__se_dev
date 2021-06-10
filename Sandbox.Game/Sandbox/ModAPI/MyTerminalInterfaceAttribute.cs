﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.MyTerminalInterfaceAttribute
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.ModAPI
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public class MyTerminalInterfaceAttribute : Attribute
  {
    public readonly Type[] LinkedTypes;

    public MyTerminalInterfaceAttribute(params Type[] linkedTypes) => this.LinkedTypes = linkedTypes;
  }
}

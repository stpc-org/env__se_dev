// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyGlobalEventHandler
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Reflection;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Game.World
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  [Obfuscation(Exclude = true, Feature = "cw symbol renaming")]
  public class MyGlobalEventHandler : Attribute
  {
    public MyDefinitionId EventDefinitionId;

    public MyGlobalEventHandler(Type objectBuilderType, string subtypeName) => this.EventDefinitionId = new MyDefinitionId((MyObjectBuilderType) objectBuilderType, subtypeName);
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.MyTextSurfaceScriptAttribute
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class MyTextSurfaceScriptAttribute : Attribute
  {
    public string Id;
    public string DisplayName;

    public MyTextSurfaceScriptAttribute(string id, string displayName)
    {
      this.Id = id;
      this.DisplayName = displayName;
    }
  }
}

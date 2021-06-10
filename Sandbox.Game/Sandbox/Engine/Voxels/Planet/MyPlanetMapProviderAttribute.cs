// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.Planet.MyPlanetMapProviderAttribute
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game.Common;

namespace Sandbox.Engine.Voxels.Planet
{
  public class MyPlanetMapProviderAttribute : MyFactoryTagAttribute
  {
    public MyPlanetMapProviderAttribute(Type objectBuilderType, bool mainBuilder = true)
      : base(objectBuilderType, mainBuilder)
    {
    }
  }
}

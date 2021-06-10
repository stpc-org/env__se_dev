// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentLight
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Network;

namespace Sandbox.Game.Components
{
  public class MyRenderComponentLight : MyRenderComponentCubeBlock
  {
    private class Sandbox_Game_Components_MyRenderComponentLight\u003C\u003EActor : IActivator, IActivator<MyRenderComponentLight>
    {
      object IActivator.CreateInstance() => (object) new MyRenderComponentLight();

      MyRenderComponentLight IActivator<MyRenderComponentLight>.CreateInstance() => new MyRenderComponentLight();
    }
  }
}

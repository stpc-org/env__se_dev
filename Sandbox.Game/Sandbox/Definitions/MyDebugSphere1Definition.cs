// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyDebugSphere1Definition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_DebugSphere1Definition), null)]
  public class MyDebugSphere1Definition : MyCubeBlockDefinition
  {
    private class Sandbox_Definitions_MyDebugSphere1Definition\u003C\u003EActor : IActivator, IActivator<MyDebugSphere1Definition>
    {
      object IActivator.CreateInstance() => (object) new MyDebugSphere1Definition();

      MyDebugSphere1Definition IActivator<MyDebugSphere1Definition>.CreateInstance() => new MyDebugSphere1Definition();
    }
  }
}

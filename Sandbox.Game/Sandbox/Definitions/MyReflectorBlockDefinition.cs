// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyReflectorBlockDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ReflectorBlockDefinition), null)]
  public class MyReflectorBlockDefinition : MyLightingBlockDefinition
  {
    public string ReflectorTexture;
    public string ReflectorConeMaterial;
    public float ReflectorThickness;
    public MyBounds RotationSpeedBounds;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ReflectorBlockDefinition reflectorBlockDefinition = (MyObjectBuilder_ReflectorBlockDefinition) builder;
      this.ReflectorTexture = reflectorBlockDefinition.ReflectorTexture;
      this.ReflectorConeMaterial = reflectorBlockDefinition.ReflectorConeMaterial;
      this.ReflectorThickness = reflectorBlockDefinition.ReflectorThickness;
      this.ReflectorConeDegrees = reflectorBlockDefinition.ReflectorConeDegrees;
      this.RotationSpeedBounds = (MyBounds) reflectorBlockDefinition.RotationSpeedBounds;
    }

    private class Sandbox_Definitions_MyReflectorBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyReflectorBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyReflectorBlockDefinition();

      MyReflectorBlockDefinition IActivator<MyReflectorBlockDefinition>.CreateInstance() => new MyReflectorBlockDefinition();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPrefabThrowerDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Audio;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_PrefabThrowerDefinition), null)]
  public class MyPrefabThrowerDefinition : MyDefinitionBase
  {
    public float? Mass;
    public float MaxSpeed;
    public float MinSpeed;
    public float PushTime;
    public string PrefabToThrow;
    public MyCueId ThrowSound;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_PrefabThrowerDefinition throwerDefinition = builder as MyObjectBuilder_PrefabThrowerDefinition;
      if (throwerDefinition.Mass.HasValue)
        this.Mass = throwerDefinition.Mass;
      this.MaxSpeed = throwerDefinition.MaxSpeed;
      this.MinSpeed = throwerDefinition.MinSpeed;
      this.PushTime = throwerDefinition.PushTime;
      this.PrefabToThrow = throwerDefinition.PrefabToThrow;
      this.ThrowSound = new MyCueId(MyStringHash.GetOrCompute(throwerDefinition.ThrowSound));
    }

    private class Sandbox_Definitions_MyPrefabThrowerDefinition\u003C\u003EActor : IActivator, IActivator<MyPrefabThrowerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPrefabThrowerDefinition();

      MyPrefabThrowerDefinition IActivator<MyPrefabThrowerDefinition>.CreateInstance() => new MyPrefabThrowerDefinition();
    }
  }
}

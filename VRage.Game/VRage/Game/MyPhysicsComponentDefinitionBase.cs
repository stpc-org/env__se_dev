// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPhysicsComponentDefinitionBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Xml.Serialization;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace VRage.Game
{
  [MyDefinitionType(typeof (MyObjectBuilder_PhysicsComponentDefinitionBase), null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyPhysicsComponentDefinitionBase : MyComponentDefinitionBase
  {
    public MyObjectBuilder_PhysicsComponentDefinitionBase.MyMassPropertiesComputationType MassPropertiesComputation;
    public RigidBodyFlag RigidBodyFlags;
    public string CollisionLayer;
    public float? LinearDamping;
    public float? AngularDamping;
    public bool ForceActivate;
    public MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags UpdateFlags;
    public bool Serialize;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_PhysicsComponentDefinitionBase componentDefinitionBase = builder as MyObjectBuilder_PhysicsComponentDefinitionBase;
      this.MassPropertiesComputation = componentDefinitionBase.MassPropertiesComputation;
      this.RigidBodyFlags = componentDefinitionBase.RigidBodyFlags;
      this.CollisionLayer = componentDefinitionBase.CollisionLayer;
      this.LinearDamping = componentDefinitionBase.LinearDamping;
      this.AngularDamping = componentDefinitionBase.AngularDamping;
      this.ForceActivate = componentDefinitionBase.ForceActivate;
      this.UpdateFlags = componentDefinitionBase.UpdateFlags;
      this.Serialize = componentDefinitionBase.Serialize;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_PhysicsComponentDefinitionBase objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_PhysicsComponentDefinitionBase;
      objectBuilder.MassPropertiesComputation = this.MassPropertiesComputation;
      objectBuilder.RigidBodyFlags = this.RigidBodyFlags;
      objectBuilder.CollisionLayer = this.CollisionLayer;
      objectBuilder.LinearDamping = this.LinearDamping;
      objectBuilder.AngularDamping = this.AngularDamping;
      objectBuilder.ForceActivate = this.ForceActivate;
      objectBuilder.UpdateFlags = this.UpdateFlags;
      objectBuilder.Serialize = this.Serialize;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class VRage_Game_MyPhysicsComponentDefinitionBase\u003C\u003EActor : IActivator, IActivator<MyPhysicsComponentDefinitionBase>
    {
      object IActivator.CreateInstance() => (object) new MyPhysicsComponentDefinitionBase();

      MyPhysicsComponentDefinitionBase IActivator<MyPhysicsComponentDefinitionBase>.CreateInstance() => new MyPhysicsComponentDefinitionBase();
    }
  }
}

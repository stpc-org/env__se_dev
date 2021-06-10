// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyComponentDefinitionBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Xml.Serialization;
using VRage.Game.Definitions;
using VRage.Network;

namespace VRage.Game
{
  [MyDefinitionType(typeof (MyObjectBuilder_ComponentDefinitionBase), null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyComponentDefinitionBase : MyDefinitionBase
  {
    protected override void Init(MyObjectBuilder_DefinitionBase builder) => base.Init(builder);

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder() => base.GetObjectBuilder();

    public override string ToString() => string.Format("ComponentDefinitionId={0}", (object) this.Id.TypeId);

    private class VRage_Game_MyComponentDefinitionBase\u003C\u003EActor : IActivator, IActivator<MyComponentDefinitionBase>
    {
      object IActivator.CreateInstance() => (object) new MyComponentDefinitionBase();

      MyComponentDefinitionBase IActivator<MyComponentDefinitionBase>.CreateInstance() => new MyComponentDefinitionBase();
    }
  }
}

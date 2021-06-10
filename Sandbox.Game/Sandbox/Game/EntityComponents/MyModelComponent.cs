// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyModelComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.EntityComponents
{
  [MyComponentType(typeof (MyModelComponent))]
  [MyComponentBuilder(typeof (MyObjectBuilder_ModelComponent), true)]
  public class MyModelComponent : MyEntityComponentBase
  {
    public static MyStringHash ModelChanged = MyStringHash.GetOrCompute(nameof (ModelChanged));

    public MyModelComponentDefinition Definition { get; private set; }

    public MyModel Model => this.Entity == null ? (MyModel) null : (this.Entity as MyEntity).Model;

    public MyModel ModelCollision => this.Entity == null ? (MyModel) null : (this.Entity as MyEntity).ModelCollision;

    public override string ComponentTypeDebugString => string.Format("Model Component {0}", this.Definition != null ? (object) this.Definition.Model : (object) "invalid");

    public override void Init(MyComponentDefinitionBase definition)
    {
      base.Init(definition);
      this.Definition = definition as MyModelComponentDefinition;
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.InitEntity();
      if (this.Definition == null)
        return;
      this.RaiseEntityEvent(MyModelComponent.ModelChanged, (MyEntityContainerEventExtensions.EntityEventParams) new MyEntityContainerEventExtensions.ModelChangedParams(this.Definition.Model, this.Definition.Size, this.Definition.Mass, this.Definition.Volume, this.Definition.DisplayNameText, this.Definition.Icons));
    }

    public void InitEntity()
    {
      if (this.Definition == null)
        return;
      MyEntity entity = this.Entity as MyEntity;
      entity.Init(new StringBuilder(this.Definition.DisplayNameText), this.Definition.Model, (MyEntity) null, new float?());
      entity.DisplayNameText = this.Definition.DisplayNameText;
    }

    private class Sandbox_Game_EntityComponents_MyModelComponent\u003C\u003EActor : IActivator, IActivator<MyModelComponent>
    {
      object IActivator.CreateInstance() => (object) new MyModelComponent();

      MyModelComponent IActivator<MyModelComponent>.CreateInstance() => new MyModelComponent();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyInventoryComponentDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_InventoryComponentDefinition), null)]
  public class MyInventoryComponentDefinition : MyComponentDefinitionBase
  {
    public float Volume;
    public float Mass;
    public bool RemoveEntityOnEmpty;
    public bool MultiplierEnabled;
    public int MaxItemCount;
    public MyInventoryConstraint InputConstraint;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_InventoryComponentDefinition componentDefinition = builder as MyObjectBuilder_InventoryComponentDefinition;
      this.Volume = componentDefinition.Volume;
      if (componentDefinition.Size.HasValue)
        this.Volume = ((Vector3) componentDefinition.Size.Value).Volume;
      this.Mass = componentDefinition.Mass;
      this.RemoveEntityOnEmpty = componentDefinition.RemoveEntityOnEmpty;
      this.MultiplierEnabled = componentDefinition.MultiplierEnabled;
      this.MaxItemCount = componentDefinition.MaxItemCount;
      if (componentDefinition.InputConstraint == null)
        return;
      this.InputConstraint = new MyInventoryConstraint(MyStringId.GetOrCompute(componentDefinition.InputConstraint.Description), componentDefinition.InputConstraint.Icon, componentDefinition.InputConstraint.IsWhitelist);
      foreach (SerializableDefinitionId entry in componentDefinition.InputConstraint.Entries)
      {
        if (string.IsNullOrEmpty(entry.SubtypeName))
          this.InputConstraint.AddObjectBuilderType(entry.TypeId);
        else
          this.InputConstraint.Add((MyDefinitionId) entry);
      }
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_InventoryComponentDefinition objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_InventoryComponentDefinition;
      objectBuilder.Volume = this.Volume;
      objectBuilder.Mass = this.Mass;
      objectBuilder.RemoveEntityOnEmpty = this.RemoveEntityOnEmpty;
      objectBuilder.MultiplierEnabled = this.MultiplierEnabled;
      objectBuilder.MaxItemCount = this.MaxItemCount;
      if (this.InputConstraint != null)
      {
        objectBuilder.InputConstraint = new MyObjectBuilder_InventoryComponentDefinition.InventoryConstraintDefinition()
        {
          IsWhitelist = this.InputConstraint.IsWhitelist,
          Icon = this.InputConstraint.Icon,
          Description = this.InputConstraint.Description
        };
        foreach (MyObjectBuilderType constrainedType in this.InputConstraint.ConstrainedTypes)
          objectBuilder.InputConstraint.Entries.Add((SerializableDefinitionId) new MyDefinitionId(constrainedType));
        foreach (MyDefinitionId constrainedId in this.InputConstraint.ConstrainedIds)
          objectBuilder.InputConstraint.Entries.Add((SerializableDefinitionId) constrainedId);
      }
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class Sandbox_Game_EntityComponents_MyInventoryComponentDefinition\u003C\u003EActor : IActivator, IActivator<MyInventoryComponentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyInventoryComponentDefinition();

      MyInventoryComponentDefinition IActivator<MyInventoryComponentDefinition>.CreateInstance() => new MyInventoryComponentDefinition();
    }
  }
}

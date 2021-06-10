// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyComponentGroupDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ComponentGroupDefinition), null)]
  public class MyComponentGroupDefinition : MyDefinitionBase
  {
    private MyObjectBuilder_ComponentGroupDefinition m_postprocessBuilder;
    private List<MyComponentDefinition> m_components;

    public bool IsValid => (uint) this.m_components.Count > 0U;

    public MyComponentGroupDefinition() => this.m_components = new List<MyComponentDefinition>();

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.m_postprocessBuilder = builder as MyObjectBuilder_ComponentGroupDefinition;
    }

    public new void Postprocess()
    {
      bool flag = true;
      int num = 0;
      foreach (MyObjectBuilder_ComponentGroupDefinition.Component component in this.m_postprocessBuilder.Components)
      {
        if (component.Amount > num)
          num = component.Amount;
      }
      for (int index = 0; index < num; ++index)
        this.m_components.Add((MyComponentDefinition) null);
      foreach (MyObjectBuilder_ComponentGroupDefinition.Component component in this.m_postprocessBuilder.Components)
      {
        MyComponentDefinition definition;
        MyDefinitionManager.Static.TryGetDefinition<MyComponentDefinition>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Component), component.SubtypeId), out definition);
        if (definition == null)
          flag = false;
        this.SetComponentDefinition(component.Amount, definition);
      }
      for (int index = 0; index < this.m_components.Count; ++index)
      {
        if (this.m_components[index] == null)
          flag = false;
      }
      if (flag)
        return;
      this.m_components.Clear();
    }

    public void SetComponentDefinition(int amount, MyComponentDefinition definition)
    {
      if (amount <= 0 || amount > this.m_components.Count)
        return;
      this.m_components[amount - 1] = definition;
    }

    public MyComponentDefinition GetComponentDefinition(int amount) => amount > 0 && amount <= this.m_components.Count ? this.m_components[amount - 1] : (MyComponentDefinition) null;

    public int GetComponentNumber() => this.m_components.Count;

    private class Sandbox_Definitions_MyComponentGroupDefinition\u003C\u003EActor : IActivator, IActivator<MyComponentGroupDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyComponentGroupDefinition();

      MyComponentGroupDefinition IActivator<MyComponentGroupDefinition>.CreateInstance() => new MyComponentGroupDefinition();
    }
  }
}

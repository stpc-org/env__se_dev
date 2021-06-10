// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPhysicalModelDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_PhysicalModelDefinition), typeof (MyPhysicalModelDefinition.Postprocessor))]
  public class MyPhysicalModelDefinition : MyDefinitionBase
  {
    public string Model;
    public MyPhysicalMaterialDefinition PhysicalMaterial;
    public float Mass;
    private string m_material;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_PhysicalModelDefinition physicalModelDefinition = builder as MyObjectBuilder_PhysicalModelDefinition;
      this.Model = physicalModelDefinition.Model;
      if (this.GetType() == typeof (MyCubeBlockDefinition) || this.GetType().IsSubclassOf(typeof (MyCubeBlockDefinition)))
        this.PhysicalMaterial = MyDestructionData.GetPhysicalMaterial(this, physicalModelDefinition.PhysicalMaterial);
      else
        this.m_material = physicalModelDefinition.PhysicalMaterial;
      this.Mass = physicalModelDefinition.Mass;
    }

    protected class Postprocessor : MyDefinitionPostprocessor
    {
      public override void AfterLoaded(ref MyDefinitionPostprocessor.Bundle definitions)
      {
      }

      public override void AfterPostprocess(
        MyDefinitionSet set,
        Dictionary<MyStringHash, MyDefinitionBase> definitions)
      {
        foreach (MyPhysicalModelDefinition modelDef in definitions.Values.Cast<MyPhysicalModelDefinition>())
          modelDef.PhysicalMaterial = MyDestructionData.GetPhysicalMaterial(modelDef, modelDef.m_material);
      }
    }

    private class Sandbox_Definitions_MyPhysicalModelDefinition\u003C\u003EActor : IActivator, IActivator<MyPhysicalModelDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPhysicalModelDefinition();

      MyPhysicalModelDefinition IActivator<MyPhysicalModelDefinition>.CreateInstance() => new MyPhysicalModelDefinition();
    }
  }
}

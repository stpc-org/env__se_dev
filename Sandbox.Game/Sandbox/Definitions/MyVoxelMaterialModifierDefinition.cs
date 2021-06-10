// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyVoxelMaterialModifierDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_VoxelMaterialModifierDefinition), typeof (MyVoxelMaterialModifierDefinition.Postprocessor))]
  public class MyVoxelMaterialModifierDefinition : MyDefinitionBase
  {
    public MyDiscreteSampler<VoxelMapChange> Options;
    private MyObjectBuilder_VoxelMaterialModifierDefinition m_ob;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.m_ob = (MyObjectBuilder_VoxelMaterialModifierDefinition) builder;
    }

    private class Postprocessor : MyDefinitionPostprocessor
    {
      public override void AfterLoaded(ref MyDefinitionPostprocessor.Bundle definitions)
      {
      }

      public override void AfterPostprocess(
        MyDefinitionSet set,
        Dictionary<MyStringHash, MyDefinitionBase> definitions)
      {
        foreach (MyVoxelMaterialModifierDefinition modifierDefinition in definitions.Values)
        {
          modifierDefinition.Options = new MyDiscreteSampler<VoxelMapChange>(((IEnumerable<MyVoxelMapModifierOption>) modifierDefinition.m_ob.Options).Select<MyVoxelMapModifierOption, VoxelMapChange>((Func<MyVoxelMapModifierOption, VoxelMapChange>) (x => new VoxelMapChange()
          {
            Changes = x.Changes == null ? (Dictionary<byte, byte>) null : ((IEnumerable<MyVoxelMapModifierChange>) x.Changes).ToDictionary<MyVoxelMapModifierChange, byte, byte>((Func<MyVoxelMapModifierChange, byte>) (y => MyDefinitionManager.Static.GetVoxelMaterialDefinition(y.From).Index), (Func<MyVoxelMapModifierChange, byte>) (y => MyDefinitionManager.Static.GetVoxelMaterialDefinition(y.To).Index))
          })), ((IEnumerable<MyVoxelMapModifierOption>) modifierDefinition.m_ob.Options).Select<MyVoxelMapModifierOption, float>((Func<MyVoxelMapModifierOption, float>) (x => x.Chance)));
          modifierDefinition.m_ob = (MyObjectBuilder_VoxelMaterialModifierDefinition) null;
        }
      }
    }

    private class Sandbox_Definitions_MyVoxelMaterialModifierDefinition\u003C\u003EActor : IActivator, IActivator<MyVoxelMaterialModifierDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyVoxelMaterialModifierDefinition();

      MyVoxelMaterialModifierDefinition IActivator<MyVoxelMaterialModifierDefinition>.CreateInstance() => new MyVoxelMaterialModifierDefinition();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPhysicalModelCollectionDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_PhysicalModelCollectionDefinition), null)]
  public class MyPhysicalModelCollectionDefinition : MyDefinitionBase
  {
    public MyDiscreteSampler<MyDefinitionId> Items;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_PhysicalModelCollectionDefinition collectionDefinition = builder as MyObjectBuilder_PhysicalModelCollectionDefinition;
      List<MyDefinitionId> values = new List<MyDefinitionId>();
      List<float> floatList = new List<float>();
      foreach (MyPhysicalModelItem physicalModelItem in collectionDefinition.Items)
      {
        MyDefinitionId myDefinitionId = new MyDefinitionId((MyObjectBuilderType) (Type) MyObjectBuilderType.ParseBackwardsCompatible(physicalModelItem.TypeId), physicalModelItem.SubtypeId);
        values.Add(myDefinitionId);
        floatList.Add(physicalModelItem.Weight);
      }
      this.Items = new MyDiscreteSampler<MyDefinitionId>(values, (IEnumerable<float>) floatList);
    }

    private class Sandbox_Definitions_MyPhysicalModelCollectionDefinition\u003C\u003EActor : IActivator, IActivator<MyPhysicalModelCollectionDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPhysicalModelCollectionDefinition();

      MyPhysicalModelCollectionDefinition IActivator<MyPhysicalModelCollectionDefinition>.CreateInstance() => new MyPhysicalModelCollectionDefinition();
    }
  }
}

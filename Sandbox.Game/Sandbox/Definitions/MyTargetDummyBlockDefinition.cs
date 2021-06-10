// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyTargetDummyBlockDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_TargetDummyBlockDefinition), null)]
  public class MyTargetDummyBlockDefinition : MyCubeBlockDefinition
  {
    public Dictionary<string, MyTargetDummyBlockDefinition.MyDummySubpartDescription> SubpartDefinitions = new Dictionary<string, MyTargetDummyBlockDefinition.MyDummySubpartDescription>();
    public MyInventoryConstraint InventoryConstraint;
    public float InventoryMaxVolume;
    public Vector3 InventorySize;
    public MyDefinitionId ConstructionItem;
    public int ConstructionItemAmount;
    public float MinRegenerationTimeInS;
    public float MaxRegenerationTimeInS;
    public string RegenerationEffectName;
    public string DestructionEffectName;
    public float RegenerationEffectMultiplier;
    public float DestructionEffectMultiplier;
    public MySoundPair RegenerationSound;
    public MySoundPair DestructionSound;
    public float MinFillFactor;
    public float MaxFillFactor;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_TargetDummyBlockDefinition dummyBlockDefinition))
        return;
      if (dummyBlockDefinition.DummySubpartNames != null && dummyBlockDefinition.DummySubpartCritical != null && (dummyBlockDefinition.DummySubpartHealth != null && dummyBlockDefinition.DummySubpartNames.Count == dummyBlockDefinition.DummySubpartCritical.Count) && dummyBlockDefinition.DummySubpartCritical.Count == dummyBlockDefinition.DummySubpartHealth.Count)
      {
        this.SubpartDefinitions.Clear();
        for (int index = 0; index < dummyBlockDefinition.DummySubpartNames.Count; ++index)
          this.SubpartDefinitions.Add(dummyBlockDefinition.DummySubpartNames[index], new MyTargetDummyBlockDefinition.MyDummySubpartDescription()
          {
            IsCritical = dummyBlockDefinition.DummySubpartCritical[index],
            Health = dummyBlockDefinition.DummySubpartHealth[index]
          });
      }
      else
        MyLog.Default.Error("Unequal TargetDummy subpart informations for: " + this.Id.SubtypeName);
      this.InventoryMaxVolume = dummyBlockDefinition.InventoryMaxVolume;
      this.InventorySize = dummyBlockDefinition.InventorySize;
      this.ConstructionItem = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Component), dummyBlockDefinition.ConstructionItemName);
      this.ConstructionItemAmount = dummyBlockDefinition.ConstructionItemAmount;
      this.InventoryConstraint = this.PrepareConstraint(MySpaceTexts.ToolTipItemFilter_GenericProductionBlockInput, this.ConstructionItem);
      this.MinRegenerationTimeInS = dummyBlockDefinition.MinRegenerationTimeInS;
      this.MaxRegenerationTimeInS = dummyBlockDefinition.MaxRegenerationTimeInS;
      this.RegenerationEffectName = dummyBlockDefinition.RegenerationEffectName;
      this.DestructionEffectName = dummyBlockDefinition.DestructionEffectName;
      this.RegenerationEffectMultiplier = dummyBlockDefinition.RegenerationEffectMultiplier;
      this.DestructionEffectMultiplier = dummyBlockDefinition.DestructionEffectMultiplier;
      this.MinFillFactor = dummyBlockDefinition.MinFillFactor;
      this.MaxFillFactor = dummyBlockDefinition.MaxFillFactor;
      this.RegenerationSound = new MySoundPair(dummyBlockDefinition.RegenerationSound);
      this.DestructionSound = new MySoundPair(dummyBlockDefinition.DestructionSound);
    }

    private MyInventoryConstraint PrepareConstraint(
      MyStringId descriptionId,
      MyDefinitionId itemId)
    {
      string icon = (string) null;
      MyInventoryConstraint inventoryConstraint = new MyInventoryConstraint(string.Format(MyTexts.GetString(descriptionId), (object) this.DisplayNameText), icon);
      inventoryConstraint.Add(itemId);
      return inventoryConstraint;
    }

    public struct MyDummySubpartDescription
    {
      public bool IsCritical;
      public float Health;
    }

    private class Sandbox_Definitions_MyTargetDummyBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyTargetDummyBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyTargetDummyBlockDefinition();

      MyTargetDummyBlockDefinition IActivator<MyTargetDummyBlockDefinition>.CreateInstance() => new MyTargetDummyBlockDefinition();
    }
  }
}

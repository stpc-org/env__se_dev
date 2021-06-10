// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyBlueprintDefinitionBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Definitions
{
  public abstract class MyBlueprintDefinitionBase : MyDefinitionBase
  {
    public MyBlueprintDefinitionBase.Item[] Prerequisites;
    public MyBlueprintDefinitionBase.Item[] Results;
    public string ProgressBarSoundCue;
    public float BaseProductionTimeInSeconds = 1f;
    public float OutputVolume;
    public bool Atomic;
    public bool IsPrimary;
    public int Priority;

    public MyObjectBuilderType InputItemType => this.Prerequisites[0].Id.TypeId;

    public new abstract void Postprocess();

    public bool PostprocessNeeded { get; protected set; }

    [Conditional("DEBUG")]
    private void VerifyInputItemType(MyObjectBuilderType inputType)
    {
      foreach (MyBlueprintDefinitionBase.Item prerequisite in this.Prerequisites)
      {
        ref MyBlueprintDefinitionBase.Item local = ref prerequisite;
      }
    }

    public override string ToString() => string.Format("(" + this.DisplayNameEnum.GetValueOrDefault(MyStringId.NullOrEmpty).String + "){{{0}}}->{{{1}}}", (object) string.Join<MyBlueprintDefinitionBase.Item>(" ", (IEnumerable<MyBlueprintDefinitionBase.Item>) this.Prerequisites), (object) string.Join<MyBlueprintDefinitionBase.Item>(" ", (IEnumerable<MyBlueprintDefinitionBase.Item>) this.Results));

    public abstract int GetBlueprints(
      List<MyBlueprintDefinitionBase.ProductionInfo> blueprints);

    public struct Item
    {
      public MyDefinitionId Id;
      public MyFixedPoint Amount;

      public override string ToString() => string.Format("{0}x {1}", (object) this.Amount, (object) this.Id);

      public static MyBlueprintDefinitionBase.Item FromObjectBuilder(
        BlueprintItem obItem)
      {
        return new MyBlueprintDefinitionBase.Item()
        {
          Id = (MyDefinitionId) obItem.Id,
          Amount = MyFixedPoint.DeserializeStringSafe(obItem.Amount)
        };
      }
    }

    public struct ProductionInfo
    {
      public MyBlueprintDefinitionBase Blueprint;
      public MyFixedPoint Amount;
    }
  }
}

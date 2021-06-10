// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyConsumableItemDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Input;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ConsumableItemDefinition), null)]
  public class MyConsumableItemDefinition : MyUsableItemDefinition
  {
    public List<MyConsumableItemDefinition.StatValue> Stats;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ConsumableItemDefinition consumableItemDefinition = builder as MyObjectBuilder_ConsumableItemDefinition;
      this.Stats = new List<MyConsumableItemDefinition.StatValue>();
      if (consumableItemDefinition.Stats == null)
        return;
      foreach (MyObjectBuilder_ConsumableItemDefinition.StatValue stat in consumableItemDefinition.Stats)
        this.Stats.Add(new MyConsumableItemDefinition.StatValue(stat.Name, stat.Value, stat.Time));
    }

    internal override string GetTooltipDisplayName(MyObjectBuilder_PhysicalObject content)
    {
      string empty = string.Empty;
      string str = !MyInput.Static.IsJoystickLastUsed ? MyTexts.GetString(MyCommonTexts.Consumable_InventoryItem_TTIP_Keyboard) : MyTexts.GetString(MyCommonTexts.Consumable_InventoryItem_TTIP_Gamepad);
      return base.GetTooltipDisplayName(content) + "\n" + str;
    }

    public struct StatValue
    {
      public string Name;
      public float Value;
      public float Time;

      public StatValue(string name, float value, float time)
      {
        this.Name = name;
        this.Value = value;
        this.Time = time;
      }
    }

    private class Sandbox_Definitions_MyConsumableItemDefinition\u003C\u003EActor : IActivator, IActivator<MyConsumableItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyConsumableItemDefinition();

      MyConsumableItemDefinition IActivator<MyConsumableItemDefinition>.CreateInstance() => new MyConsumableItemDefinition();
    }
  }
}

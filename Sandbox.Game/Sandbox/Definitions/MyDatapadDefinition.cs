// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyDatapadDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Input;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_DatapadDefinition), null)]
  public class MyDatapadDefinition : MyPhysicalItemDefinition
  {
    protected override void Init(MyObjectBuilder_DefinitionBase builder) => base.Init(builder);

    internal override string GetTooltipDisplayName(MyObjectBuilder_PhysicalObject content)
    {
      MyObjectBuilder_Datapad objectBuilderDatapad = content as MyObjectBuilder_Datapad;
      string empty = string.Empty;
      string str = !MyInput.Static.IsJoystickLastUsed ? MyTexts.GetString(MyCommonTexts.Datapad_InventoryItem_TTIP_Keyboard) : MyTexts.GetString(MyCommonTexts.Datapad_InventoryItem_TTIP_Gamepad);
      return string.IsNullOrEmpty(objectBuilderDatapad.Name) ? base.GetTooltipDisplayName(content) + "\n" + str : objectBuilderDatapad.Name + "\n" + str;
    }

    private class Sandbox_Definitions_MyDatapadDefinition\u003C\u003EActor : IActivator, IActivator<MyDatapadDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyDatapadDefinition();

      MyDatapadDefinition IActivator<MyDatapadDefinition>.CreateInstance() => new MyDatapadDefinition();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.SessionComponents.MySessionComponentSmartUpdaterDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace VRage.Game.Definitions.SessionComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_SessionComponentSmartUpdaterDefinition), null)]
  public class MySessionComponentSmartUpdaterDefinition : MySessionComponentDefinition
  {
    private class VRage_Game_Definitions_SessionComponents_MySessionComponentSmartUpdaterDefinition\u003C\u003EActor : IActivator, IActivator<MySessionComponentSmartUpdaterDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySessionComponentSmartUpdaterDefinition();

      MySessionComponentSmartUpdaterDefinition IActivator<MySessionComponentSmartUpdaterDefinition>.CreateInstance() => new MySessionComponentSmartUpdaterDefinition();
    }
  }
}

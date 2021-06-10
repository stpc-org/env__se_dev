// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.Session.MySessionComponentDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace VRage.Game.Components.Session
{
  [MyDefinitionType(typeof (MyObjectBuilder_SessionComponentDefinition), null)]
  public class MySessionComponentDefinition : MyDefinitionBase
  {
    private class VRage_Game_Components_Session_MySessionComponentDefinition\u003C\u003EActor : IActivator, IActivator<MySessionComponentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySessionComponentDefinition();

      MySessionComponentDefinition IActivator<MySessionComponentDefinition>.CreateInstance() => new MySessionComponentDefinition();
    }
  }
}

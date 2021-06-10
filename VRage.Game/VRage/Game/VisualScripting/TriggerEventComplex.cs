// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.TriggerEventComplex
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game.VisualScripting
{
  [VisualScriptingEvent(new bool[] {true, true, true}, new KeyTypeEnum[] {KeyTypeEnum.Trigger, KeyTypeEnum.EntityId, KeyTypeEnum.EntityName})]
  public delegate void TriggerEventComplex(string triggerName, long entityId, string entityName);
}

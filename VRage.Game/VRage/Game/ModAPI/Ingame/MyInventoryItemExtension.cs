// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.MyInventoryItemExtension
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game.ModAPI.Ingame
{
  public static class MyInventoryItemExtension
  {
    public static MyDefinitionId GetDefinitionId(this IMyInventoryItem self) => self.Content is MyObjectBuilder_PhysicalObject content ? content.GetObjectId() : new MyDefinitionId(self.Content.TypeId, self.Content.SubtypeId);
  }
}

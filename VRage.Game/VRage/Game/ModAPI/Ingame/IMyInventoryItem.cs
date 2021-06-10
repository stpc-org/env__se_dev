// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.IMyInventoryItem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.ObjectBuilders;

namespace VRage.Game.ModAPI.Ingame
{
  public interface IMyInventoryItem
  {
    MyFixedPoint Amount { get; set; }

    float Scale { get; set; }

    MyObjectBuilder_Base Content { get; set; }

    uint ItemId { get; set; }
  }
}

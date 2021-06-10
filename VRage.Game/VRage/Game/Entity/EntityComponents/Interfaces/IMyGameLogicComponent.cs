// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.EntityComponents.Interfaces.IMyGameLogicComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

namespace VRage.Game.Entity.EntityComponents.Interfaces
{
  public interface IMyGameLogicComponent
  {
    bool EntityUpdate { get; set; }

    void UpdateOnceBeforeFrame(bool entityUpdate);

    void UpdateBeforeSimulation(bool entityUpdate);

    void UpdateBeforeSimulation10(bool entityUpdate);

    void UpdateBeforeSimulation100(bool entityUpdate);

    void UpdateAfterSimulation(bool entityUpdate);

    void UpdateAfterSimulation10(bool entityUpdate);

    void UpdateAfterSimulation100(bool entityUpdate);

    void RegisterForUpdate();

    void UnregisterForUpdate();

    void Close();
  }
}

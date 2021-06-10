// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.IMyEnvironmentModuleProxy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;

namespace Sandbox.Game.WorldEnvironment
{
  public interface IMyEnvironmentModuleProxy
  {
    void Init(MyEnvironmentSector sector, List<int> items);

    void Close();

    void CommitLodChange(int lodBefore, int lodAfter);

    void CommitPhysicsChange(bool enabled);

    void OnItemChange(int index, short newModel);

    void OnItemChangeBatch(List<int> items, int offset, short newModel);

    void HandleSyncEvent(int item, object data, bool fromClient);

    void DebugDraw();
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.IMyEnvironmentModule
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage.ObjectBuilders;

namespace Sandbox.Game.WorldEnvironment
{
  public interface IMyEnvironmentModule
  {
    void ProcessItems(
      Dictionary<short, MyLodEnvironmentItemSet> items,
      int changedLodMin,
      int changedLodMax);

    void Init(MyLogicalEnvironmentSectorBase sector, MyObjectBuilder_Base ob);

    void Close();

    MyObjectBuilder_EnvironmentModuleBase GetObjectBuilder();

    void OnItemEnable(int item, bool enable);

    void HandleSyncEvent(int logicalItem, object data, bool fromClient);

    void DebugDraw();
  }
}

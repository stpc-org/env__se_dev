// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Modules.MyBotSpawningEnvironmentProxy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.AI;
using Sandbox.Game.GameSystems;
using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage.Game;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment.Modules
{
  public class MyBotSpawningEnvironmentProxy : IMyEnvironmentModuleProxy
  {
    private MyEnvironmentSector m_sector;
    protected readonly MyRandom m_random = new MyRandom();
    protected List<int> m_items;
    protected Queue<int> m_spawnQueue;

    public long SectorId => this.m_sector.SectorId;

    public void Init(MyEnvironmentSector sector, List<int> items)
    {
      this.m_sector = sector;
      this.m_items = items;
      this.m_spawnQueue = new Queue<int>();
      foreach (int num in this.m_items)
        this.m_spawnQueue.Enqueue(num);
    }

    public void Close() => this.m_spawnQueue.Clear();

    public void CommitLodChange(int lodBefore, int lodAfter)
    {
      if (lodAfter == 0)
        MyEnvironmentBotSpawningSystem.Static.RegisterBotSpawningProxy(this);
      else
        MyEnvironmentBotSpawningSystem.Static.UnregisterBotSpawningProxy(this);
    }

    public void CommitPhysicsChange(bool enabled)
    {
    }

    public void OnItemChange(int index, short newModel)
    {
    }

    public void OnItemChangeBatch(List<int> items, int offset, short newModel)
    {
    }

    public void HandleSyncEvent(int item, object data, bool fromClient)
    {
    }

    public void DebugDraw()
    {
    }

    public bool OnSpawnTick()
    {
      if (this.m_spawnQueue.Count == 0 || MyAIComponent.Static.GetAvailableUncontrolledBotsCount() < 1)
        return false;
      int count = this.m_spawnQueue.Count;
      int num = 0;
      while (num < count)
      {
        ++num;
        int index = this.m_spawnQueue.Dequeue();
        this.m_spawnQueue.Enqueue(index);
        if (this.m_sector.DataView.Items.Count >= index)
        {
          ItemInfo itemInfo = this.m_sector.DataView.Items[index];
          Vector3D vector3D = this.m_sector.SectorCenter + itemInfo.Position;
          if (MyEnvironmentBotSpawningSystem.Static.IsHumanPlayerWithinRange((Vector3) vector3D))
          {
            MyRuntimeEnvironmentItemInfo def;
            this.m_sector.Owner.GetDefinition((ushort) itemInfo.DefinitionIndex, out def);
            MyBotCollectionDefinition definition = MyDefinitionManager.Static.GetDefinition<MyBotCollectionDefinition>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_BotCollectionDefinition), def.Subtype));
            using (this.m_random.PushSeed(index.GetHashCode()))
            {
              MyAgentDefinition botDefinition = MyDefinitionManager.Static.GetBotDefinition(definition.Bots.Sample(this.m_random)) as MyAgentDefinition;
              MyAIComponent.Static.SpawnNewBot(botDefinition, itemInfo.Position + this.m_sector.SectorCenter, false);
            }
            return true;
          }
        }
      }
      return false;
    }
  }
}

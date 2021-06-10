// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.IMyBot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.AI.Logic;
using VRage.Game;

namespace Sandbox.Game.AI
{
  public interface IMyBot
  {
    void Init(MyObjectBuilder_Bot botBuilder);

    void InitActions(ActionCollection actionCollection);

    void InitLogic(MyBotLogic logic);

    void Cleanup();

    void Update();

    void DebugDraw();

    void Reset();

    bool IsValidForUpdate { get; }

    bool CreatedByPlayer { get; }

    MyObjectBuilder_Bot GetObjectBuilder();

    string BehaviorSubtypeName { get; }

    ActionCollection ActionCollection { get; }

    MyBotMemory BotMemory { get; }

    MyBotMemory LastBotMemory { get; set; }

    void ReturnToLastMemory();

    MyBotDefinition BotDefinition { get; }

    MyBotActionsBase BotActions { get; set; }

    MyBotLogic BotLogic { get; }
  }
}

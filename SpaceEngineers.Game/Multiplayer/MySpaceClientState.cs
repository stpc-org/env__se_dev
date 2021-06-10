// Decompiled with JetBrains decompiler
// Type: Multiplayer.MySpaceClientState
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using System;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Collections;

namespace Multiplayer
{
  internal class MySpaceClientState : MyClientState
  {
    private static MyClientState.MyContextKind GetContextByPage(MyTerminalPageEnum page)
    {
      switch (page)
      {
        case MyTerminalPageEnum.Inventory:
          return MyClientState.MyContextKind.Inventory;
        case MyTerminalPageEnum.ControlPanel:
          return MyClientState.MyContextKind.Terminal;
        case MyTerminalPageEnum.Production:
          return MyClientState.MyContextKind.Production;
        default:
          return MyClientState.MyContextKind.None;
      }
    }

    protected override void WriteInternal(BitStream stream, MyEntity controlledEntity)
    {
      MyClientState.MyContextKind contextByPage = MySpaceClientState.GetContextByPage(MyGuiScreenTerminal.GetCurrentScreen());
      stream.WriteInt32((int) contextByPage, 2);
      if (contextByPage == MyClientState.MyContextKind.None)
        return;
      long num = MyGuiScreenTerminal.InteractedEntity != null ? MyGuiScreenTerminal.InteractedEntity.EntityId : 0L;
      stream.WriteInt64(num);
    }

    protected override void ReadInternal(BitStream stream, MyEntity controlledEntity)
    {
      this.Context = (MyClientState.MyContextKind) stream.ReadInt32(2);
      if (this.Context != MyClientState.MyContextKind.None)
      {
        this.ContextEntity = MyEntities.GetEntityByIdOrDefault(stream.ReadInt64(), allowClosed: true);
        if (this.ContextEntity == null || !this.ContextEntity.GetTopMostParent((Type) null).MarkedForClose)
          return;
        this.ContextEntity = (MyEntity) null;
      }
      else
        this.ContextEntity = (MyEntity) null;
    }
  }
}

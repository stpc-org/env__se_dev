// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.IntergridCommunication.MyMessageListener
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.SessionComponents;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems.IntergridCommunication
{
  internal class MyMessageListener : IMyMessageProvider
  {
    private string m_callback;
    public bool m_hasPendingCallback;
    private Queue<MyIGCMessage> m_pendingMessages;
    private static Action<MyProgrammableBlock, string> m_invokeOverride;

    public MyIntergridCommunicationContext Context { get; private set; }

    public int MaxWaitingMessages => 25;

    public bool HasPendingMessage => this.m_pendingMessages != null && this.m_pendingMessages.Count > 0;

    public MyMessageListener(MyIntergridCommunicationContext context) => this.Context = context;

    public void EnqueueMessage(MyIGCMessage message)
    {
      if (this.m_pendingMessages == null)
        this.m_pendingMessages = new Queue<MyIGCMessage>();
      else if (this.m_pendingMessages.Count >= this.MaxWaitingMessages)
        this.m_pendingMessages.Dequeue();
      this.m_pendingMessages.Enqueue(message);
      this.RegisterForCallback();
      if (!MyDebugDrawSettings.DEBUG_DRAW_IGC)
        return;
      Vector3D to = this.Context.ProgrammableBlock.WorldMatrix.Translation;
      Vector3D from = MyEntities.GetEntityById(message.Source).WorldMatrix.Translation;
      Color color = this is IMyBroadcastListener ? Color.Blue : Color.Green;
      MyIGCSystemSessionComponent.Static.AddDebugDraw((Action) (() => MyRenderProxy.DebugDrawArrow3D(from, to, color)));
    }

    public void InvokeCallback()
    {
      this.UnregisterCallback();
      MyProgrammableBlock programmableBlock = this.Context.ProgrammableBlock;
      if (MyMessageListener.m_invokeOverride != null)
        MyMessageListener.m_invokeOverride(programmableBlock, this.m_callback);
      else
        programmableBlock.Run(this.m_callback, UpdateType.IGC);
    }

    public virtual MyIGCMessage AcceptMessage() => this.HasPendingMessage ? this.m_pendingMessages.Dequeue() : new MyIGCMessage();

    public virtual void SetMessageCallback(string argument) => this.m_callback = argument != null ? argument : throw new ArgumentNullException(nameof (argument));

    public void DisableMessageCallback()
    {
      this.UnregisterCallback();
      this.m_callback = (string) null;
    }

    private void RegisterForCallback()
    {
      if (this.m_callback == null || this.m_hasPendingCallback)
        return;
      this.m_hasPendingCallback = true;
      this.Context.RegisterForCallback(this);
    }

    private void UnregisterCallback()
    {
      if (!this.m_hasPendingCallback)
        return;
      this.m_hasPendingCallback = false;
      this.Context.UnregisterFromCallback(this);
    }
  }
}

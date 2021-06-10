// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.IntergridCommunication.BroadcastListener
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.ModAPI.Ingame;
using System;

namespace Sandbox.Game.GameSystems.IntergridCommunication
{
  internal class BroadcastListener : MyMessageListener, IMyBroadcastListener, IMyMessageProvider
  {
    public string Tag { get; private set; }

    public bool IsActive { get; set; }

    public BroadcastListener(MyIntergridCommunicationContext context, string tag)
      : base(context)
      => this.Tag = tag;

    public override void SetMessageCallback(string argument)
    {
      if (!this.IsActive)
        throw new Exception("Callbacks are not supported for disabled broadcast listeners!");
      base.SetMessageCallback(argument);
    }

    public override MyIGCMessage AcceptMessage()
    {
      MyIGCMessage myIgcMessage = base.AcceptMessage();
      if (this.IsActive || this.HasPendingMessage)
        return myIgcMessage;
      this.Context.DisposeBroadcastListener(this, false);
      return myIgcMessage;
    }
  }
}

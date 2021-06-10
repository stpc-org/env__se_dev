// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugCrashTests
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("VRage", "Crash tests")]
  internal class MyGuiScreenDebugCrashTests : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugCrashTests()
      : base()
      => this.RecreateControls(true);

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugCrashTests);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.13f);
      this.AddCaption("Crash tests", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.AddButton(new StringBuilder("Exception in update thread."), new Action<MyGuiControlButton>(this.UpdateThreadException));
      this.AddButton(new StringBuilder("Exception in render thread."), new Action<MyGuiControlButton>(this.RenderThreadException));
      this.AddButton(new StringBuilder("Exception in worker thread."), new Action<MyGuiControlButton>(this.WorkerThreadException));
      this.AddButton(new StringBuilder("Main thread invoked exception."), new Action<MyGuiControlButton>(this.MainThreadInvokedException));
      this.AddButton(new StringBuilder("Update thread out of memory."), new Action<MyGuiControlButton>(this.OutOfMemoryUpdateException));
      this.AddButton(new StringBuilder("Worker thread out of memory."), new Action<MyGuiControlButton>(this.OutOfMemoryWorkerException));
      this.AddButton(new StringBuilder("Immediate out of memory."), new Action<MyGuiControlButton>(this.ImmediteaOutOfMemoryException));
      this.AddButton(new StringBuilder("Havok access violation."), new Action<MyGuiControlButton>(this.HavokAccessViolationException));
      this.AddButton(new StringBuilder("Divide by zero."), new Action<MyGuiControlButton>(this.DivideByZero));
      this.AddButton(new StringBuilder("Assert."), (Action<MyGuiControlButton>) (x => {}));
      this.m_currentPosition.Y += 0.01f;
    }

    private void ServerCrash(MyGuiControlButton obj) => MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (s => new Action(MySession.OnCrash)));

    private void ImmediteaOutOfMemoryException(MyGuiControlButton obj) => throw new OutOfMemoryException();

    private void UpdateThreadException(MyGuiControlButton sender) => throw new InvalidOperationException("Forced exception");

    private void RenderThreadException(MyGuiControlButton sender) => MyRenderProxy.DebugCrashRenderThread();

    private void WorkerThreadException(MyGuiControlButton sender) => ThreadPool.QueueUserWorkItem(new WaitCallback(this.WorkerThreadCrasher));

    private void MainThreadInvokedException(MyGuiControlButton sender) => MySandboxGame.Static.Invoke(new Action(this.MainThreadCrasher), "DebugCrashTest");

    private void OutOfMemoryUpdateException(MyGuiControlButton sender) => this.Allocate();

    private void OutOfMemoryWorkerException(MyGuiControlButton sender) => ThreadPool.QueueUserWorkItem(new WaitCallback(this.Allocate));

    private void HavokAccessViolationException(MyGuiControlButton sender) => ThreadPool.QueueUserWorkItem(new WaitCallback(this.HavokAccessViolation));

    private void HavokAccessViolation(object state = null) => Console.WriteLine((object) new HkRigidBodyCinfo().LinearVelocity);

    private void Allocate(object state = null)
    {
      List<byte[]> numArrayList = new List<byte[]>();
      for (int index1 = 0; index1 < 10000000; ++index1)
      {
        byte[] numArray = new byte[1024000];
        for (int index2 = 0; index2 < numArray.Length; ++index2)
          numArray[index2] = (byte) (index2 ^ numArrayList.Count);
        numArrayList.Add(numArray);
      }
      Console.WriteLine(numArrayList.Count);
    }

    private void MainThreadCrasher() => throw new InvalidOperationException("Forced exception");

    private void WorkerThreadCrasher(object state)
    {
      Thread.Sleep(2000);
      throw new InvalidOperationException("Forced exception");
    }

    private void DivideByZero(MyGuiControlButton sender)
    {
      int num1 = 7;
      int num2 = 14;
      Console.WriteLine(num2 / (num2 - 2 * num1));
    }
  }
}

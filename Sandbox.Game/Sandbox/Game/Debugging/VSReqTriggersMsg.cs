// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSReqTriggersMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Runtime.InteropServices;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
  [ProtoContract]
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct VSReqTriggersMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_RTRG";

    private class Sandbox_Game_Debugging_VSReqTriggersMsg\u003C\u003EActor : IActivator, IActivator<VSReqTriggersMsg>
    {
      object IActivator.CreateInstance() => (object) new VSReqTriggersMsg();

      VSReqTriggersMsg IActivator<VSReqTriggersMsg>.CreateInstance() => new VSReqTriggersMsg();
    }
  }
}

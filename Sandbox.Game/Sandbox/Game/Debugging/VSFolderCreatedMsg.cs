// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSFolderCreatedMsg
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
  [ProtoContract]
  public struct VSFolderCreatedMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public string Path;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_FCRE";

    protected class Sandbox_Game_Debugging_VSFolderCreatedMsg\u003C\u003EPath\u003C\u003EAccessor : IMemberAccessor<VSFolderCreatedMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSFolderCreatedMsg owner, in string value) => owner.Path = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSFolderCreatedMsg owner, out string value) => value = owner.Path;
    }

    private class Sandbox_Game_Debugging_VSFolderCreatedMsg\u003C\u003EActor : IActivator, IActivator<VSFolderCreatedMsg>
    {
      object IActivator.CreateInstance() => (object) new VSFolderCreatedMsg();

      VSFolderCreatedMsg IActivator<VSFolderCreatedMsg>.CreateInstance() => new VSFolderCreatedMsg();
    }
  }
}

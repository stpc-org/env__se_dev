// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Debugging.VSFolderRenamedMsg
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
  public struct VSFolderRenamedMsg : MyExternalDebugStructures.IExternalDebugMsg
  {
    [ProtoMember(5)]
    public string OldPath;
    [ProtoMember(10)]
    public string NewPath;

    string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr() => "VS_FREN";

    protected class Sandbox_Game_Debugging_VSFolderRenamedMsg\u003C\u003EOldPath\u003C\u003EAccessor : IMemberAccessor<VSFolderRenamedMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSFolderRenamedMsg owner, in string value) => owner.OldPath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSFolderRenamedMsg owner, out string value) => value = owner.OldPath;
    }

    protected class Sandbox_Game_Debugging_VSFolderRenamedMsg\u003C\u003ENewPath\u003C\u003EAccessor : IMemberAccessor<VSFolderRenamedMsg, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref VSFolderRenamedMsg owner, in string value) => owner.NewPath = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref VSFolderRenamedMsg owner, out string value) => value = owner.NewPath;
    }

    private class Sandbox_Game_Debugging_VSFolderRenamedMsg\u003C\u003EActor : IActivator, IActivator<VSFolderRenamedMsg>
    {
      object IActivator.CreateInstance() => (object) new VSFolderRenamedMsg();

      VSFolderRenamedMsg IActivator<VSFolderRenamedMsg>.CreateInstance() => new VSFolderRenamedMsg();
    }
  }
}

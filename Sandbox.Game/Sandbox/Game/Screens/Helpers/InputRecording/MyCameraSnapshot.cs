// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.InputRecording.MyCameraSnapshot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Network;

namespace Sandbox.Game.Screens.Helpers.InputRecording
{
  [Obfuscation(Exclude = true, Feature = "cw symbol renaming")]
  [Serializable]
  public class MyCameraSnapshot
  {
    public MyPositionAndOrientation? CameraPosition { get; set; }

    public bool TakeScreenShot { get; set; }

    public int? LOD { get; set; }

    public MyTestingToolHelper.MyViewsEnum View { get; set; }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyCameraSnapshot\u003C\u003ECameraPosition\u003C\u003EAccessor : IMemberAccessor<MyCameraSnapshot, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyCameraSnapshot owner, in MyPositionAndOrientation? value) => owner.CameraPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyCameraSnapshot owner, out MyPositionAndOrientation? value) => value = owner.CameraPosition;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyCameraSnapshot\u003C\u003ETakeScreenShot\u003C\u003EAccessor : IMemberAccessor<MyCameraSnapshot, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyCameraSnapshot owner, in bool value) => owner.TakeScreenShot = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyCameraSnapshot owner, out bool value) => value = owner.TakeScreenShot;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyCameraSnapshot\u003C\u003ELOD\u003C\u003EAccessor : IMemberAccessor<MyCameraSnapshot, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyCameraSnapshot owner, in int? value) => owner.LOD = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyCameraSnapshot owner, out int? value) => value = owner.LOD;
    }

    protected class Sandbox_Game_Screens_Helpers_InputRecording_MyCameraSnapshot\u003C\u003EView\u003C\u003EAccessor : IMemberAccessor<MyCameraSnapshot, MyTestingToolHelper.MyViewsEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyCameraSnapshot owner, in MyTestingToolHelper.MyViewsEnum value) => owner.View = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyCameraSnapshot owner, out MyTestingToolHelper.MyViewsEnum value) => value = owner.View;
    }
  }
}

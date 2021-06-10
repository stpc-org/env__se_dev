// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyAdvancedDebugDraw
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Multiplayer;
using System;
using VRage.Input;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Utils
{
  [StaticEventOwner]
  internal class MyAdvancedDebugDraw
  {
    public static void DebugDrawLine3DSync(
      Vector3D start,
      Vector3D end,
      Color colorServer,
      Color colorClient,
      MyKeys? key = null)
    {
      if (Sync.IsServer)
        MyMultiplayer.RaiseStaticEvent<Vector3D, Vector3D, Color, MyKeys?>((Func<IMyEventOwner, Action<Vector3D, Vector3D, Color, MyKeys?>>) (x => new Action<Vector3D, Vector3D, Color, MyKeys?>(MyAdvancedDebugDraw.DebugDrawLine3DSyncInternal)), start, end, colorServer, key);
      else
        MyAdvancedDebugDraw.DebugDrawLine3DInternal(start, end, colorClient, key);
    }

    [Event(null, 36)]
    [Reliable]
    [Broadcast]
    protected static void DebugDrawLine3DSyncInternal(
      Vector3D start,
      Vector3D end,
      Color color,
      MyKeys? key)
    {
      MyAdvancedDebugDraw.DebugDrawLine3DInternal(start, end, color, key);
    }

    protected static void DebugDrawLine3DInternal(
      Vector3D start,
      Vector3D end,
      Color color,
      MyKeys? key)
    {
      if (key.HasValue && !MyInput.Static.IsKeyPress(key.Value))
        return;
      MyRenderProxy.DebugDrawLine3D(start, end, color, color, true, true);
    }

    protected sealed class DebugDrawLine3DSyncInternal\u003C\u003EVRageMath_Vector3D\u0023VRageMath_Vector3D\u0023VRageMath_Color\u0023System_Nullable`1\u003CVRage_Input_MyKeys\u003E : ICallSite<IMyEventOwner, Vector3D, Vector3D, Color, MyKeys?, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in Vector3D start,
        in Vector3D end,
        in Color color,
        in MyKeys? key,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAdvancedDebugDraw.DebugDrawLine3DSyncInternal(start, end, color, key);
      }
    }
  }
}

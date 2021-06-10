// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MySectorContactEvent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using VRage.Game.Entity;

namespace Sandbox.Game.WorldEnvironment
{
  public delegate void MySectorContactEvent(
    int itemId,
    MyEntity other,
    ref MyPhysics.MyContactPointEvent evt);
}

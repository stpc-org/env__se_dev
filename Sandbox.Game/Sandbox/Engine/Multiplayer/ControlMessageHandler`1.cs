// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.ControlMessageHandler`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Engine.Multiplayer
{
  public delegate void ControlMessageHandler<T>(ref T message, ulong sender) where T : struct;
}

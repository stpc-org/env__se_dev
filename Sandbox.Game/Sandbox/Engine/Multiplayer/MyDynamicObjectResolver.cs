// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyDynamicObjectResolver
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Engine.Multiplayer
{
  public class MyDynamicObjectResolver : IDynamicResolver
  {
    public void Serialize(BitStream stream, Type baseType, ref Type obj)
    {
      if (stream.Reading)
      {
        TypeId id = new TypeId(stream.ReadUInt32());
        obj = MyMultiplayer.Static.ReplicationLayer.GetType(id);
      }
      else
      {
        TypeId typeId = MyMultiplayer.Static.ReplicationLayer.GetTypeId(obj);
        stream.WriteUInt32((uint) typeId);
      }
    }
  }
}

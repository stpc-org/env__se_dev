// Decompiled with JetBrains decompiler
// Type: System.Extensions
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Network;

namespace System
{
  public static class Extensions
  {
    [ThreadStatic]
    private static List<IMyStateGroup> m_tmpStateGroupsPerThread;

    private static List<IMyStateGroup> m_tmpStateGroups
    {
      get
      {
        if (Extensions.m_tmpStateGroupsPerThread == null)
          Extensions.m_tmpStateGroupsPerThread = new List<IMyStateGroup>();
        return Extensions.m_tmpStateGroupsPerThread;
      }
    }

    public static NetworkId ReadNetworkId(this BitStream stream) => new NetworkId(stream.ReadUInt32Variant());

    public static TypeId ReadTypeId(this BitStream stream) => new TypeId(stream.ReadUInt32Variant());

    public static void WriteNetworkId(this BitStream stream, NetworkId networkId) => stream.WriteVariant(networkId.Value);

    public static void WriteTypeId(this BitStream stream, TypeId typeId) => stream.WriteVariant(typeId.Value);

    public static T FindStateGroup<T>(this IMyReplicable obj) where T : class, IMyStateGroup
    {
      try
      {
        if (obj == null)
          return default (T);
        obj.GetStateGroups(Extensions.m_tmpStateGroups);
        foreach (IMyStateGroup mTmpStateGroup in Extensions.m_tmpStateGroups)
        {
          if (mTmpStateGroup is T obj1)
            return obj1;
        }
        return default (T);
      }
      finally
      {
        Extensions.m_tmpStateGroups.Clear();
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerNetObject
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Serialization
{
  public static class MySerializerNetObject
  {
    private static INetObjectResolver m_netObjectResolver;

    public static INetObjectResolver NetObjectResolver => MySerializerNetObject.m_netObjectResolver;

    public static MySerializerNetObject.ResolverToken Using(
      INetObjectResolver netObjectResolver)
    {
      return new MySerializerNetObject.ResolverToken(netObjectResolver);
    }

    public struct ResolverToken : IDisposable
    {
      private INetObjectResolver m_previousResolver;

      public ResolverToken(INetObjectResolver newResolver)
      {
        this.m_previousResolver = MySerializerNetObject.m_netObjectResolver;
        MySerializerNetObject.m_netObjectResolver = newResolver;
      }

      public void Dispose()
      {
        MySerializerNetObject.m_netObjectResolver = this.m_previousResolver;
        this.m_previousResolver = (INetObjectResolver) null;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyEventContext
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Network
{
  public struct MyEventContext
  {
    [ThreadStatic]
    private static MyEventContext m_current;
    public readonly EndpointId Sender;
    public readonly MyClientStateBase ClientState;

    public static MyEventContext Current => MyEventContext.m_current;

    public static void ValidationFailed() => MyEventContext.m_current.HasValidationFailed = true;

    public bool IsLocallyInvoked { get; private set; }

    public bool HasValidationFailed { get; private set; }

    public bool IsValid { get; private set; }

    private MyEventContext(EndpointId sender, MyClientStateBase clientState, bool isInvokedLocally)
      : this()
    {
      this.Sender = sender;
      this.ClientState = clientState;
      this.IsLocallyInvoked = isInvokedLocally;
      this.HasValidationFailed = false;
      this.IsValid = true;
    }

    public static MyEventContext.Token Set(
      EndpointId endpoint,
      MyClientStateBase client,
      bool isInvokedLocally)
    {
      return new MyEventContext.Token(new MyEventContext(endpoint, client, isInvokedLocally));
    }

    public struct Token : IDisposable
    {
      private readonly MyEventContext m_oldContext;

      public Token(MyEventContext newContext)
      {
        this.m_oldContext = MyEventContext.m_current;
        MyEventContext.m_current = newContext;
      }

      void IDisposable.Dispose() => MyEventContext.m_current = this.m_oldContext;
    }
  }
}

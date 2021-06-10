// Decompiled with JetBrains decompiler
// Type: VRage.Library.Threading.SpinLockRef
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Threading
{
  public class SpinLockRef
  {
    private SpinLock m_spinLock;

    public SpinLockRef.Token Acquire() => new SpinLockRef.Token(this);

    public void Enter() => this.m_spinLock.Enter();

    public void Exit() => this.m_spinLock.Exit();

    public struct Token : IDisposable
    {
      private SpinLockRef m_lock;

      public Token(SpinLockRef spin)
      {
        this.m_lock = spin;
        this.m_lock.Enter();
      }

      public void Dispose() => this.m_lock.Exit();
    }
  }
}

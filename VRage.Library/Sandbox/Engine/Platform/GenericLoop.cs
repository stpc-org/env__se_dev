// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Platform.GenericLoop
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace Sandbox.Engine.Platform
{
  public class GenericLoop
  {
    private GenericLoop.VoidAction m_tickCallback;
    public bool IsDone;

    public virtual void Run(GenericLoop.VoidAction tickCallback)
    {
      this.m_tickCallback = tickCallback;
      while (!this.IsDone)
        this.m_tickCallback();
    }

    public delegate void VoidAction();
  }
}

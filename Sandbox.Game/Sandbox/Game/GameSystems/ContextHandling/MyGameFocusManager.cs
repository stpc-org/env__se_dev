// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.ContextHandling.MyGameFocusManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Game.GameSystems.ContextHandling
{
  public class MyGameFocusManager
  {
    private IMyFocusHolder m_currentFocusHolder;

    public void Register(IMyFocusHolder newFocusHolder)
    {
      if (this.m_currentFocusHolder != null && newFocusHolder != this.m_currentFocusHolder)
        this.m_currentFocusHolder.OnLostFocus();
      this.m_currentFocusHolder = newFocusHolder;
    }

    public void Unregister(IMyFocusHolder focusHolder)
    {
      if (this.m_currentFocusHolder != focusHolder)
        return;
      this.m_currentFocusHolder = (IMyFocusHolder) null;
    }

    public void Clear()
    {
      if (this.m_currentFocusHolder != null)
        this.m_currentFocusHolder.OnLostFocus();
      this.m_currentFocusHolder = (IMyFocusHolder) null;
    }
  }
}

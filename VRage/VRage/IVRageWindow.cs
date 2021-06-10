// Decompiled with JetBrains decompiler
// Type: VRage.IVRageWindow
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.IO;
using VRageMath;

namespace VRage
{
  public interface IVRageWindow
  {
    bool DrawEnabled { get; }

    bool IsActive { get; }

    Vector2I ClientSize { get; }

    event Action OnExit;

    void DoEvents();

    void Exit();

    bool UpdateRenderThread();

    void UpdateMainThread();

    void SetCursor(Stream stream);

    void AddMessageHandler(uint wm, ActionRef<MyMessage> action);

    void RemoveMessageHandler(uint wm, ActionRef<MyMessage> action);

    void ShowAndFocus();

    void Hide();
  }
}

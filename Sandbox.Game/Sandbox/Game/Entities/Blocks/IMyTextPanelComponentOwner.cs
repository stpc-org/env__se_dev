// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.IMyTextPanelComponentOwner
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Game.Entities.Blocks
{
  public interface IMyTextPanelComponentOwner
  {
    void OpenWindow(bool isEditable, bool sync, bool isPublic);

    MyTextPanelComponent PanelComponent { get; }

    bool IsTextPanelOpen { get; }
  }
}

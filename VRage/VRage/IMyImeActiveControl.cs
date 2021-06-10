// Decompiled with JetBrains decompiler
// Type: VRage.IMyImeActiveControl
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRage
{
  public interface IMyImeActiveControl
  {
    bool IsImeActive { get; set; }

    void DeactivateIme();

    void InsertChar(bool conpositionEnd, char character);

    void InsertCharMultiple(bool conpositionEnd, string chars);

    void KeypressBackspace(bool conpositionEnd);

    void KeypressBackspaceMultiple(bool conpositionEnd, int count);

    void KeypressDelete(bool conpositionEnd);

    void KeypressEnter(bool conpositionEnd);

    void KeypressRedo();

    void KeypressUndo();

    int GetMaxLength();

    int GetSelectionLength();

    int GetTextLength();

    Vector2 GetCornerPosition();

    Vector2 GetCarriagePosition(int shiftX);
  }
}

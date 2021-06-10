// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyTextSurface
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System.Collections.Generic;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyTextSurface
  {
    bool WriteText(string value, bool append = false);

    string GetText();

    bool WriteText(StringBuilder value, bool append = false);

    void ReadText(StringBuilder buffer, bool append = false);

    void AddImageToSelection(string id, bool checkExistence = false);

    void AddImagesToSelection(List<string> ids, bool checkExistence = false);

    void RemoveImageFromSelection(string id, bool removeDuplicates = false);

    void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false);

    void ClearImagesFromSelection();

    void GetSelectedImages(List<string> output);

    string CurrentlyShownImage { get; }

    float FontSize { get; set; }

    Color FontColor { get; set; }

    Color BackgroundColor { get; set; }

    byte BackgroundAlpha { get; set; }

    float ChangeInterval { get; set; }

    string Font { get; set; }

    void GetFonts(List<string> fonts);

    void GetSprites(List<string> sprites);

    TextAlignment Alignment { get; set; }

    void GetScripts(List<string> scripts);

    string Script { get; set; }

    ContentType ContentType { get; set; }

    Vector2 SurfaceSize { get; }

    Vector2 TextureSize { get; }

    MySpriteDrawFrame DrawFrame();

    bool PreserveAspectRatio { get; set; }

    float TextPadding { get; set; }

    Color ScriptBackgroundColor { get; set; }

    Color ScriptForegroundColor { get; set; }

    Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale);

    string Name { get; }

    string DisplayName { get; }
  }
}

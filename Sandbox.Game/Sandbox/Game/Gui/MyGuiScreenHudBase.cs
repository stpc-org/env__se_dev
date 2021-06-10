// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenHudBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Gui;
using VRage.Generics;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenHudBase : MyGuiScreenBase
  {
    private static readonly MyStringId ID_SQUARE = MyStringId.GetOrCompute("Square");
    protected string m_atlas;
    protected MyAtlasTextureCoordinate[] m_atlasCoords;
    protected float m_textScale;
    protected StringBuilder m_hudIndicatorText = new StringBuilder();
    protected StringBuilder m_helperSB = new StringBuilder();
    protected MyObjectsPoolSimple<MyHudText> m_texts;

    public string TextureAtlas => this.m_atlas;

    public MyGuiScreenHudBase()
      : base(new Vector2?(Vector2.Zero))
    {
      this.CanBeHidden = true;
      this.CanHideOthers = false;
      this.CanHaveFocus = false;
      this.m_drawEvenWithoutFocus = true;
      this.m_closeOnEsc = false;
      this.m_texts = new MyObjectsPoolSimple<MyHudText>(2000);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenHudBase);

    public override void LoadContent()
    {
      MyGuiScreenHudBase.LoadTextureAtlas(out this.m_atlas, out this.m_atlasCoords);
      base.LoadContent();
    }

    public static void LoadTextureAtlas(
      out string atlasFile,
      out MyAtlasTextureCoordinate[] atlasCoords)
    {
      MyTextureAtlasUtils.LoadTextureAtlas(MyEnumsToStrings.HudTextures, "Textures\\HUD\\", "Textures\\HUD\\HudAtlas.tai", out atlasFile, out atlasCoords);
    }

    public override void UnloadContent()
    {
      this.m_atlas = (string) null;
      this.m_atlasCoords = (MyAtlasTextureCoordinate[]) null;
      base.UnloadContent();
    }

    public static Vector2 ConvertHudToNormalizedGuiPosition(ref Vector2 hudPos)
    {
      Rectangle fullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
      Vector2 vector2_1 = new Vector2((float) fullscreenRectangle.Width, (float) fullscreenRectangle.Height);
      Vector2 vector2_2 = new Vector2((float) fullscreenRectangle.X, (float) fullscreenRectangle.Y);
      Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
      Vector2 vector2_3 = new Vector2((float) safeGuiRectangle.Width, (float) safeGuiRectangle.Height);
      Vector2 vector2_4 = new Vector2((float) safeGuiRectangle.X, (float) safeGuiRectangle.Y);
      return (hudPos * vector2_1 + vector2_2 - vector2_4) / vector2_3;
    }

    protected static Vector2 ConvertNormalizedGuiToHud(ref Vector2 normGuiPos)
    {
      Rectangle fullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
      Vector2 vector2_1 = new Vector2((float) fullscreenRectangle.Width, (float) fullscreenRectangle.Height);
      Vector2 vector2_2 = new Vector2((float) fullscreenRectangle.X, (float) fullscreenRectangle.Y);
      Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
      Vector2 vector2_3 = new Vector2((float) safeGuiRectangle.Width, (float) safeGuiRectangle.Height);
      Vector2 vector2_4 = new Vector2((float) safeGuiRectangle.X, (float) safeGuiRectangle.Y);
      return (normGuiPos * vector2_3 + vector2_4 - vector2_2) / vector2_1;
    }

    public static void HandleSelectedObjectHighlight(
      MyHudSelectedObject selection,
      MyHudObjectHighlightStyleData? data)
    {
      if (selection == null)
        return;
      if (selection.PreviousObject.Instance != null)
        MyGuiScreenHudBase.RemoveObjectHighlightInternal(ref selection.PreviousObject, true);
      switch (selection.State)
      {
        case MyHudSelectedObjectState.VisibleStateSet:
          if (!selection.Visible || selection.CurrentObject.Style != MyHudObjectHighlightStyle.OutlineHighlight && selection.CurrentObject.Style != MyHudObjectHighlightStyle.EdgeHighlight && selection.CurrentObject.Style != MyHudObjectHighlightStyle.DummyHighlight && (selection.CurrentObject.Instance == null || (int) selection.VisibleRenderID == (int) selection.CurrentObject.Instance.RenderObjectID))
            break;
          MyGuiScreenHudBase.DrawSelectedObjectHighlight(selection, data);
          break;
        case MyHudSelectedObjectState.MarkedForVisible:
          MyGuiScreenHudBase.DrawSelectedObjectHighlight(selection, data);
          break;
        case MyHudSelectedObjectState.MarkedForNotVisible:
          MyGuiScreenHudBase.RemoveObjectHighlight(selection);
          break;
      }
    }

    private static void DrawSelectedObjectHighlight(
      MyHudSelectedObject selection,
      MyHudObjectHighlightStyleData? data)
    {
      if (selection.InteractiveObject.RenderObjectID == uint.MaxValue)
        return;
      switch (selection.HighlightStyle)
      {
        case MyHudObjectHighlightStyle.None:
          return;
        case MyHudObjectHighlightStyle.DummyHighlight:
          MyGuiScreenHudBase.DrawSelectedObjectHighlightDummy(selection, data.Value.AtlasTexture, data.Value.TextureCoord);
          break;
        case MyHudObjectHighlightStyle.OutlineHighlight:
          if (selection.SectionNames != null && selection.SectionNames.Length == 0 && selection.SubpartIndices == null)
          {
            MyGuiScreenHudBase.DrawSelectedObjectHighlightDummy(selection, data.Value.AtlasTexture, data.Value.TextureCoord);
            break;
          }
          MyGuiScreenHudBase.DrawSelectedObjectHighlightOutline(selection);
          break;
        case MyHudObjectHighlightStyle.EdgeHighlight:
          MyGuiScreenHudBase.DrawSelectedObjectHighlightOutline(selection, true);
          break;
        default:
          throw new Exception("Unknown highlight style");
      }
      selection.Visible = true;
    }

    private static void RemoveObjectHighlight(MyHudSelectedObject selection)
    {
      MyGuiScreenHudBase.RemoveObjectHighlightInternal(ref selection.CurrentObject, false);
      selection.Visible = false;
    }

    private static void RemoveObjectHighlightInternal(
      ref MyHudSelectedObjectStatus status,
      bool reset)
    {
      switch (status.Style)
      {
        case MyHudObjectHighlightStyle.OutlineHighlight:
        case MyHudObjectHighlightStyle.EdgeHighlight:
          if (MySession.Static.GetComponent<MyHighlightSystem>() != null && !MySession.Static.GetComponent<MyHighlightSystem>().IsReserved(new MyHighlightSystem.ExclusiveHighlightIdentification(status.Instance.Owner.EntityId, status.SectionNames)) && status.Instance.RenderObjectID != uint.MaxValue)
          {
            MyRenderProxy.UpdateModelHighlight(status.Instance.RenderObjectID, status.SectionNames, status.SubpartIndices, new Color?(), instanceIndex: status.Instance.InstanceID);
            break;
          }
          break;
      }
      if (!reset)
        return;
      status.Reset();
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      MyHud.Crosshair?.RecreateControls(constructor);
    }

    public override bool Update(bool hasFocus)
    {
      int num = base.Update(hasFocus) ? 1 : 0;
      if (!MySandboxGame.Config.ShowCrosshairHUD)
        return num != 0;
      MyHud.Crosshair.Update();
      return num != 0;
    }

    public override bool Draw()
    {
      int num = base.Draw() ? 1 : 0;
      if (!MySandboxGame.Config.ShowCrosshairHUD)
        return num != 0;
      if (MyHud.CutsceneHud)
        return num != 0;
      MyHud.Crosshair.Draw(this.m_atlas, this.m_atlasCoords);
      return num != 0;
    }

    private static void DrawSelectedObjectHighlightOutline(
      MyHudSelectedObject selection,
      bool edgeHighlight = false)
    {
      Color color = selection.Color;
      if (edgeHighlight)
        color.A = (byte) 0;
      float highlightThickness = MySector.EnvironmentDefinition.ContourHighlightThickness;
      float highlightPulseInSeconds = MySector.EnvironmentDefinition.HighlightPulseInSeconds;
      MyHighlightSystem component = MySession.Static.GetComponent<MyHighlightSystem>();
      if ((component != null ? (!component.IsReserved(new MyHighlightSystem.ExclusiveHighlightIdentification(selection.InteractiveObject.Owner.EntityId, selection.SectionNames)) ? 1 : 0) : 0) == 0)
        return;
      MyRenderProxy.UpdateModelHighlight(selection.InteractiveObject.RenderObjectID, selection.SectionNames, selection.SubpartIndices, new Color?(color), highlightThickness, highlightPulseInSeconds, selection.InteractiveObject.InstanceID);
    }

    public static void DrawSelectedObjectHighlightDummy(
      MyHudSelectedObject selection,
      string atlasTexture,
      MyAtlasTextureCoordinate textureCoord)
    {
      Rectangle fullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
      Vector2 scale = new Vector2((float) fullscreenRectangle.Width, (float) fullscreenRectangle.Height);
      MatrixD worldMatrix = selection.InteractiveObject.ActivationMatrix * MySector.MainCamera.ViewMatrix * MySector.MainCamera.ProjectionMatrix;
      BoundingBoxD boundingBoxD = new BoundingBoxD(-Vector3D.Half, Vector3D.Half).TransformSlow(ref worldMatrix);
      Vector2 vector2_1 = new Vector2((float) boundingBoxD.Min.X, (float) boundingBoxD.Min.Y);
      Vector2 vector2_2 = new Vector2((float) boundingBoxD.Max.X, (float) boundingBoxD.Max.Y);
      Vector2 vector2_3 = vector2_1 - vector2_2;
      Vector2 pos1 = vector2_1 * 0.5f + 0.5f * Vector2.One;
      Vector2 pos2 = vector2_2 * 0.5f + 0.5f * Vector2.One;
      pos1.Y = 1f - pos1.Y;
      pos2.Y = 1f - pos2.Y;
      float textureScale = (float) Math.Pow((double) Math.Abs(vector2_3.X), 0.349999994039536) * 2.5f;
      if (selection.InteractiveObject.ShowOverlay)
      {
        BoundingBoxD localbox = new BoundingBoxD((Vector3D) new Vector3(-0.5f, -0.5f, -0.5f), (Vector3D) new Vector3(0.5f, 0.5f, 0.5f));
        Color color = Color.Gold * 0.4f;
        MatrixD activationMatrix = selection.InteractiveObject.ActivationMatrix;
        MatrixD worldToLocal = MatrixD.Invert(selection.InteractiveObject.WorldMatrix);
        MySimpleObjectDraw.DrawAttachedTransparentBox(ref activationMatrix, ref localbox, ref color, selection.InteractiveObject.RenderObjectID, ref worldToLocal, MySimpleObjectRasterizer.Solid, 0, 0.05f, new MyStringId?(MyGuiScreenHudBase.ID_SQUARE), onlyFrontFaces: true);
      }
      if (!MyFakes.ENABLE_USE_OBJECT_CORNERS)
        return;
      MyGuiScreenHudBase.DrawSelectionCorner(atlasTexture, selection, textureCoord, scale, pos1, -Vector2.UnitY, textureScale, false);
      MyGuiScreenHudBase.DrawSelectionCorner(atlasTexture, selection, textureCoord, scale, new Vector2(pos1.X, pos2.Y), Vector2.UnitX, textureScale, false);
      MyGuiScreenHudBase.DrawSelectionCorner(atlasTexture, selection, textureCoord, scale, new Vector2(pos2.X, pos1.Y), -Vector2.UnitX, textureScale, false);
      MyGuiScreenHudBase.DrawSelectionCorner(atlasTexture, selection, textureCoord, scale, pos2, Vector2.UnitY, textureScale, false);
    }

    public static void DrawSelectionCorner(
      string atlasTexture,
      MyHudSelectedObject selection,
      MyAtlasTextureCoordinate textureCoord,
      Vector2 scale,
      Vector2 pos,
      Vector2 rightVector,
      float textureScale,
      bool ignoreBounds)
    {
      if (MyVideoSettingsManager.IsTripleHead())
        pos.X *= 3f;
      MyRenderProxy.DrawSpriteAtlas(atlasTexture, pos, textureCoord.Offset, textureCoord.Size, rightVector, scale, selection.Color, selection.HalfSize / MyGuiManager.GetHudSize() * textureScale, ignoreBounds);
    }

    public MyHudText AllocateText() => this.m_texts.Allocate();

    public void DrawTexts()
    {
      if (this.m_texts.GetAllocatedCount() <= 0)
        return;
      for (int index = 0; index < this.m_texts.GetAllocatedCount(); ++index)
      {
        MyHudText allocatedItem = this.m_texts.GetAllocatedItem(index);
        if (allocatedItem.GetStringBuilder().Length != 0)
        {
          string font = allocatedItem.Font;
          allocatedItem.Position /= MyGuiManager.GetHudSize();
          Vector2 position = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref allocatedItem.Position);
          Vector2 textSize = MyGuiManager.MeasureString(font, allocatedItem.GetStringBuilder(), MyGuiSandbox.GetDefaultTextScaleWithLanguage()) * allocatedItem.Scale;
          position = MyUtils.GetCoordTopLeftFromAligned(position, textSize, allocatedItem.Alignement);
          MyGuiTextShadows.DrawShadow(ref position, ref textSize, fogAlphaMultiplier: ((float) allocatedItem.Color.A / (float) byte.MaxValue), alignment: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, ignoreBounds: true);
          MyGuiManager.DrawString(font, allocatedItem.GetStringBuilder().ToString(), position, allocatedItem.Scale, new Color?(allocatedItem.Color), ignoreBounds: true);
        }
      }
      this.m_texts.ClearAllAllocated();
    }

    public MyAtlasTextureCoordinate GetTextureCoord(
      MyHudTexturesEnum texture)
    {
      return this.m_atlasCoords[(int) texture];
    }
  }
}

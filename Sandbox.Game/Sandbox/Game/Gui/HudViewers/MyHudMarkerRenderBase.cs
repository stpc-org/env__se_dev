// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.HudViewers.MyHudMarkerRenderBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Gui;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI.HudViewers
{
  public class MyHudMarkerRenderBase
  {
    protected const double LS_METRES = 299792458.000137;
    protected const double LY_METRES = 9.460730473E+15;
    protected MyGuiScreenHudBase m_hudScreen;
    protected List<MyHudMarkerRenderBase.MyMarkerStyle> m_markerStyles;
    protected int[] m_markerStylesForBlocks;
    protected MyHudMarkerRenderBase.DistanceComparer m_distanceComparer = new MyHudMarkerRenderBase.DistanceComparer();

    public MyHudMarkerRenderBase(MyGuiScreenHudBase hudScreen)
    {
      this.m_hudScreen = hudScreen;
      this.m_markerStyles = new List<MyHudMarkerRenderBase.MyMarkerStyle>();
      int num1 = this.AllocateMarkerStyle("White", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_neutral, MyHudConstants.MARKER_COLOR_WHITE);
      int num2 = this.AllocateMarkerStyle("Red", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_enemy, MyHudConstants.MARKER_COLOR_WHITE);
      int num3 = this.AllocateMarkerStyle("DarkBlue", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_me, MyHudConstants.MARKER_COLOR_WHITE);
      int num4 = this.AllocateMarkerStyle("Green", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_friend, MyHudConstants.MARKER_COLOR_WHITE);
      this.m_markerStylesForBlocks = new int[MyUtils.GetMaxValueFromEnum<MyRelationsBetweenPlayerAndBlock>() + 1];
      this.m_markerStylesForBlocks[3] = num1;
      this.m_markerStylesForBlocks[4] = num2;
      this.m_markerStylesForBlocks[1] = num3;
      this.m_markerStylesForBlocks[2] = num4;
      this.m_markerStylesForBlocks[0] = num4;
    }

    public virtual void Update()
    {
    }

    public virtual void Draw()
    {
    }

    public int AllocateMarkerStyle(
      string font,
      MyHudTexturesEnum directionIcon,
      MyHudTexturesEnum targetIcon,
      Color color)
    {
      int count = this.m_markerStyles.Count;
      this.m_markerStyles.Add(new MyHudMarkerRenderBase.MyMarkerStyle(font, directionIcon, targetIcon, color));
      return count;
    }

    public void OverrideStyleForRelation(
      MyRelationsBetweenPlayerAndBlock relation,
      string font,
      MyHudTexturesEnum directionIcon,
      MyHudTexturesEnum targetIcon,
      Color color)
    {
      this.m_markerStyles[this.GetStyleForRelation(relation)] = new MyHudMarkerRenderBase.MyMarkerStyle(font, directionIcon, targetIcon, color);
    }

    public int GetStyleForRelation(MyRelationsBetweenPlayerAndBlock relation) => this.m_markerStylesForBlocks[(int) relation];

    public virtual void DrawLocationMarkers(MyHudLocationMarkers locationMarkers)
    {
    }

    protected void AddTexturedQuad(
      MyHudTexturesEnum texture,
      Vector2 position,
      Vector2 upVector,
      Color color,
      float halfWidth,
      float halfHeight)
    {
      Vector2 rightVector = new Vector2(-upVector.Y, upVector.X);
      MyAtlasTextureCoordinate textureCoord = this.m_hudScreen.GetTextureCoord(texture);
      Vector2 vector2 = new Vector2((float) MyGuiManager.GetSafeFullscreenRectangle().Width, (float) MyGuiManager.GetSafeFullscreenRectangle().Height);
      float x = vector2.X / MyGuiManager.GetHudSize().X;
      float y = vector2.Y / MyGuiManager.GetHudSize().Y;
      Vector2 position1 = position;
      if (MyVideoSettingsManager.IsTripleHead())
        ++position1.X;
      float num = vector2.Y / 1080f;
      halfWidth *= num;
      halfHeight *= num;
      MyRenderProxy.DrawSpriteAtlas(this.m_hudScreen.TextureAtlas, position1, textureCoord.Offset, textureCoord.Size, rightVector, new Vector2(x, y), color, new Vector2(halfWidth, halfHeight), true);
    }

    protected void AddTexturedQuad(
      string texture,
      Vector2 position,
      Vector2 upVector,
      Color color,
      float halfWidth,
      float halfHeight)
    {
      Vector2 vector2 = new Vector2((float) MyGuiManager.GetSafeFullscreenRectangle().Width, (float) MyGuiManager.GetSafeFullscreenRectangle().Height);
      float num1 = vector2.X / MyGuiManager.GetHudSize().X;
      float num2 = vector2.Y / MyGuiManager.GetHudSize().Y;
      if (MyVideoSettingsManager.IsTripleHead())
        ++position.X;
      position.X *= num1;
      position.Y *= num2;
      RectangleF destination = new RectangleF(position.X - halfWidth, position.Y - halfHeight, halfWidth * 2f, halfHeight * 2f);
      Rectangle? sourceRectangle = new Rectangle?();
      MyRenderProxy.DrawSprite(texture, ref destination, sourceRectangle, color, 0.0f, true, true);
    }

    public class MyMarkerStyle
    {
      public string Font { get; set; }

      public MyHudTexturesEnum TextureDirectionIndicator { get; set; }

      public MyHudTexturesEnum TextureTarget { get; set; }

      public Color Color { get; set; }

      public float TextureTargetRotationSpeed { get; set; }

      public float TextureTargetScale { get; set; }

      public MyMarkerStyle(
        string font,
        MyHudTexturesEnum textureDirectionIndicator,
        MyHudTexturesEnum textureTarget,
        Color color,
        float textureTargetRotationSpeed = 0.0f,
        float textureTargetScale = 1f)
      {
        this.Font = font;
        this.TextureDirectionIndicator = textureDirectionIndicator;
        this.TextureTarget = textureTarget;
        this.Color = color;
        this.TextureTargetRotationSpeed = textureTargetRotationSpeed;
        this.TextureTargetScale = textureTargetScale;
      }
    }

    public class DistanceComparer : IComparer<MyHudEntityParams>
    {
      public int Compare(MyHudEntityParams x, MyHudEntityParams y) => Vector3D.DistanceSquared(MySector.MainCamera.Position, y.Position).CompareTo(Vector3D.DistanceSquared(MySector.MainCamera.Position, x.Position));
    }
  }
}

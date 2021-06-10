// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyRenderComponentScreenAreas
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Components
{
  public class MyRenderComponentScreenAreas : MyRenderComponentCubeBlock
  {
    private readonly MyEntity m_entity;
    private readonly List<MyRenderComponentScreenAreas.PanelScreenArea> m_screenAreas = new List<MyRenderComponentScreenAreas.PanelScreenArea>();

    public MyRenderComponentScreenAreas(MyEntity entity) => this.m_entity = entity;

    public void UpdateModelProperties()
    {
      foreach (MyRenderComponentScreenAreas.PanelScreenArea screenArea in this.m_screenAreas)
      {
        foreach (uint renderObjectId in screenArea.RenderObjectIDs)
        {
          if (renderObjectId != uint.MaxValue)
            MyRenderProxy.UpdateModelProperties(renderObjectId, screenArea.Material, (RenderFlags) 0, (RenderFlags) 0, new Color?(), new float?());
        }
      }
    }

    public void ChangeTexture(int area, string path)
    {
      if (area >= this.m_screenAreas.Count)
        return;
      if (string.IsNullOrEmpty(path))
      {
        for (int index = 0; index < this.m_screenAreas[area].RenderObjectIDs.Length; ++index)
        {
          if (this.m_screenAreas[area].RenderObjectIDs[index] != uint.MaxValue)
          {
            MyRenderProxy.ChangeMaterialTexture(this.m_screenAreas[area].RenderObjectIDs[index], this.m_screenAreas[area].Material);
            if (!this.m_screenAreas[area].Material.Contains("TransparentScreenArea", StringComparison.Ordinal))
              MyRenderProxy.UpdateModelProperties(this.m_screenAreas[area].RenderObjectIDs[index], this.m_screenAreas[area].Material, (RenderFlags) 0, RenderFlags.Visible, new Color?(), new float?());
          }
        }
      }
      else
      {
        for (int index = 0; index < this.m_screenAreas[area].RenderObjectIDs.Length; ++index)
        {
          if (this.m_screenAreas[area].RenderObjectIDs[index] != uint.MaxValue)
          {
            MyRenderProxy.ChangeMaterialTexture(this.m_screenAreas[area].RenderObjectIDs[index], this.m_screenAreas[area].Material, path);
            if (!this.m_screenAreas[area].Material.Contains("TransparentScreenArea", StringComparison.Ordinal))
              MyRenderProxy.UpdateModelProperties(this.m_screenAreas[area].RenderObjectIDs[index], this.m_screenAreas[area].Material, RenderFlags.Visible, (RenderFlags) 0, new Color?(), new float?());
          }
        }
      }
    }

    public string GenerateOffscreenTextureName(long entityId, int area) => entityId != this.Entity.EntityId ? string.Format("LCDOffscreenTexture_{0}_{1}", (object) entityId, (object) this.m_screenAreas[area].Material) : this.m_screenAreas[area].OffscreenTextureName;

    internal static Vector2 CalcAspectFactor(Vector2I textureSize, Vector2 aspectRatio)
    {
      Vector2 vector2 = textureSize.X > textureSize.Y ? new Vector2(1f, (float) (textureSize.X / textureSize.Y)) : new Vector2((float) (textureSize.Y / textureSize.X), 1f);
      return aspectRatio * vector2;
    }

    internal static Vector2 CalcShift(Vector2I textureSize, Vector2 aspectFactor) => (Vector2) textureSize * (aspectFactor - Vector2.One) / 2f;

    public void RenderSpritesToTexture(
      int area,
      ListReader<MySprite> sprites,
      Vector2I textureSize,
      Vector2 aspectRatio,
      Color backgroundColor,
      byte backgroundAlpha)
    {
      string offscreenTextureName = this.GenerateOffscreenTextureName(this.m_entity.EntityId, area);
      Vector2 aspectFactor = MyRenderComponentScreenAreas.CalcAspectFactor(textureSize, aspectRatio);
      Vector2 vector2_1 = MyRenderComponentScreenAreas.CalcShift(textureSize, aspectFactor);
      bool flag = false;
      for (int index = 0; index < sprites.Count; ++index)
      {
        MySprite sprite = sprites[index];
        Vector2? nullable = sprite.Size;
        Vector2 vector2_2 = nullable ?? (Vector2) textureSize;
        nullable = sprite.Position;
        Vector2 vector2_3 = nullable ?? (Vector2) (textureSize / 2);
        Color color = sprite.Color ?? Color.White;
        Vector2 vector2_4 = vector2_3 + vector2_1;
        switch (sprite.Type)
        {
          case SpriteType.TEXTURE:
            MyLCDTextureDefinition definition1 = MyDefinitionManager.Static.GetDefinition<MyLCDTextureDefinition>(MyStringHash.GetOrCompute(sprite.Data));
            switch (definition1?.SpritePath ?? definition1?.TexturePath)
            {
              case null:
                continue;
              default:
                switch (sprite.Alignment)
                {
                  case TextAlignment.LEFT:
                    vector2_4 += new Vector2(vector2_2.X * 0.5f, 0.0f);
                    break;
                  case TextAlignment.RIGHT:
                    vector2_4 -= new Vector2(vector2_2.X * 0.5f, 0.0f);
                    break;
                }
                Vector2 rightVector = new Vector2(1f, 0.0f);
                if ((double) Math.Abs(sprite.RotationOrScale) > 9.99999974737875E-06)
                  rightVector = new Vector2((float) Math.Cos((double) sprite.RotationOrScale), (float) Math.Sin((double) sprite.RotationOrScale));
                MyRenderProxy.DrawSpriteAtlas(definition1.SpritePath ?? definition1.TexturePath, vector2_4, Vector2.Zero, Vector2.One, rightVector, Vector2.One, color, vector2_2 / 2f, false, offscreenTextureName);
                continue;
            }
          case SpriteType.TEXT:
            switch (sprite.Alignment)
            {
              case TextAlignment.RIGHT:
                vector2_4 -= new Vector2(vector2_2.X, 0.0f);
                break;
              case TextAlignment.CENTER:
                vector2_4 -= new Vector2(vector2_2.X * 0.5f, 0.0f);
                break;
            }
            MyFontDefinition definition2 = MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(MyStringHash.GetOrCompute(sprite.FontId));
            int textureWidthinPx = (int) Math.Round((double) vector2_2.X);
            MyRenderProxy.DrawStringAligned((int) (definition2 != null ? definition2.Id.SubtypeId : MyStringHash.GetOrCompute("Debug")), vector2_4, color, sprite.Data ?? string.Empty, sprite.RotationOrScale, float.PositiveInfinity, false, offscreenTextureName, textureWidthinPx, (MyRenderTextAlignmentEnum) sprite.Alignment);
            break;
          case SpriteType.CLIP_RECT:
            if (sprite.Position.HasValue && sprite.Size.HasValue)
            {
              if (flag)
                MyRenderProxy.SpriteScissorPop(offscreenTextureName);
              else
                flag = true;
              MyRenderProxy.SpriteScissorPush(new Rectangle((int) vector2_4.X, (int) vector2_4.Y, (int) vector2_2.X, (int) vector2_2.Y), offscreenTextureName);
              break;
            }
            if (flag)
            {
              MyRenderProxy.SpriteScissorPop(offscreenTextureName);
              flag = false;
              break;
            }
            break;
        }
      }
      if (flag)
        MyRenderProxy.SpriteScissorPop(offscreenTextureName);
      backgroundColor.A = backgroundAlpha;
      uint[] renderObjectIds = this.m_screenAreas[area].RenderObjectIDs;
      int index1 = 0;
      while (index1 < renderObjectIds.Length && renderObjectIds[index1] == uint.MaxValue)
        ++index1;
      if (index1 >= renderObjectIds.Length)
        return;
      MyRenderProxy.RenderOffscreenTexture(offscreenTextureName, new Vector2?(aspectFactor), new Color?(backgroundColor));
    }

    public override void AddRenderObjects()
    {
      base.AddRenderObjects();
      this.UpdateRenderAreas();
    }

    protected void UpdateRenderAreas()
    {
      for (int index1 = 0; index1 < this.RenderObjectIDs.Length; ++index1)
      {
        for (int index2 = 0; index2 < this.m_screenAreas.Count; ++index2)
          this.m_screenAreas[index2].RenderObjectIDs[index1] = this.RenderObjectIDs[index1];
      }
    }

    public override void ReleaseRenderObjectID(int index)
    {
      base.ReleaseRenderObjectID(index);
      for (int index1 = 0; index1 < this.m_screenAreas.Count; ++index1)
        this.m_screenAreas[index1].RenderObjectIDs[index] = uint.MaxValue;
    }

    public void AddScreenArea(uint[] renderObjectIDs, string materialName) => this.m_screenAreas.Add(new MyRenderComponentScreenAreas.PanelScreenArea()
    {
      RenderObjectIDs = ((IEnumerable<uint>) renderObjectIDs).ToArray<uint>(),
      Material = materialName,
      OffscreenTextureName = string.Format("LCDOffscreenTexture_{0}_{1}", (object) this.Entity.EntityId, (object) materialName)
    });

    public static int GetTextureByteCount(Vector2I textureSize) => (int) ((double) (textureSize.X * textureSize.Y) * 4.0 * 1.33333325386047);

    public void CreateTexture(int area, Vector2I textureSize) => MyRenderProxy.CreateGeneratedTexture(this.GenerateOffscreenTextureName(this.m_entity.EntityId, area), textureSize.X, textureSize.Y, immediatelyReady: false);

    public void ReleaseTexture(int area, bool useEmptyTexture = true)
    {
      if (useEmptyTexture)
        this.ChangeTexture(area, "EMPTY");
      MyRenderProxy.DestroyGeneratedTexture(this.GenerateOffscreenTextureName(this.m_entity.EntityId, area));
    }

    private class PanelScreenArea
    {
      public uint[] RenderObjectIDs;
      public string Material;
      public string OffscreenTextureName;
    }

    private class Sandbox_Game_Components_MyRenderComponentScreenAreas\u003C\u003EActor
    {
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyHudWeaponHitIndicator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyHudWeaponHitIndicator
  {
    private readonly int ANIMATION_DURATION = 15;
    private readonly int MAX_VISIBLE_TIME = 15;
    private readonly float ANIMATION_SCALE = 1.5f;
    private readonly Vector2 m_position = new Vector2(0.5f);
    private readonly Vector2 m_baseSize = new Vector2(1f, 1.333333f) * 0.025f;
    private float m_size;
    private Color m_color;
    private int m_visibleTime = 60;
    private bool m_isVisible = true;

    public MyGuiControlImage GuiControlImage { get; private set; }

    public MyHudWeaponHitIndicator()
    {
      MyGuiControlImage myGuiControlImage = new MyGuiControlImage(new Vector2?(this.m_position), new Vector2?(this.m_baseSize));
      myGuiControlImage.Visible = false;
      this.GuiControlImage = myGuiControlImage;
    }

    private void SetColor(Color hitColor)
    {
      this.m_color = hitColor;
      this.GuiControlImage.ColorMask = (Vector4) hitColor;
    }

    public void Hit(MySession.MyHitIndicatorTarget target)
    {
      switch (target)
      {
        case MySession.MyHitIndicatorTarget.Character:
          this.GuiControlImage.ColorMask = (Vector4) MySandboxGame.Config.HitIndicatorColorCharacter;
          this.GuiControlImage.BackgroundTexture = new MyGuiCompositeTexture()
          {
            Center = new MyGuiSizedTexture()
            {
              Texture = MySandboxGame.Config.HitIndicatorTextureCharacter
            }
          };
          break;
        case MySession.MyHitIndicatorTarget.Headshot:
          this.GuiControlImage.ColorMask = (Vector4) MySandboxGame.Config.HitIndicatorColorHeadshot;
          this.GuiControlImage.BackgroundTexture = new MyGuiCompositeTexture()
          {
            Center = new MyGuiSizedTexture()
            {
              Texture = MySandboxGame.Config.HitIndicatorTextureHeadshot
            }
          };
          break;
        case MySession.MyHitIndicatorTarget.Kill:
          this.GuiControlImage.ColorMask = (Vector4) MySandboxGame.Config.HitIndicatorColorKill;
          this.GuiControlImage.BackgroundTexture = new MyGuiCompositeTexture()
          {
            Center = new MyGuiSizedTexture()
            {
              Texture = MySandboxGame.Config.HitIndicatorTextureKill
            }
          };
          break;
        case MySession.MyHitIndicatorTarget.Grid:
          this.GuiControlImage.ColorMask = (Vector4) MySandboxGame.Config.HitIndicatorColorGrid;
          this.GuiControlImage.BackgroundTexture = new MyGuiCompositeTexture()
          {
            Center = new MyGuiSizedTexture()
            {
              Texture = MySandboxGame.Config.HitIndicatorTextureGrid
            }
          };
          break;
        case MySession.MyHitIndicatorTarget.Friend:
          this.GuiControlImage.ColorMask = (Vector4) MySandboxGame.Config.HitIndicatorColorFriend;
          this.GuiControlImage.BackgroundTexture = new MyGuiCompositeTexture()
          {
            Center = new MyGuiSizedTexture()
            {
              Texture = MySandboxGame.Config.HitIndicatorTextureFriend
            }
          };
          break;
      }
      this.m_visibleTime = 0;
    }

    public void Update()
    {
      bool flag = this.m_visibleTime < this.MAX_VISIBLE_TIME;
      this.GuiControlImage.Visible = flag;
      if (!flag)
        return;
      ++this.m_visibleTime;
      if (this.m_visibleTime < this.ANIMATION_DURATION)
      {
        float num = (float) (1.0 - (double) this.m_visibleTime / (double) this.ANIMATION_DURATION);
        this.m_size = (float) (1.0 + ((double) this.ANIMATION_SCALE - 1.0) * (double) num * (double) num);
      }
      this.GuiControlImage.Size = this.m_size * this.m_baseSize;
    }
  }
}

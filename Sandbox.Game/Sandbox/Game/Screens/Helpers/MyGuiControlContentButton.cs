// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlContentButton
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyGuiControlContentButton : MyGuiControlRadioButton
  {
    private readonly MyGuiControlLabel m_titleLabel;
    private MyGuiControlImage m_previewImage;
    private string m_previewImagePath;
    private readonly MyGuiControlContentButton.MyControlImages m_workshopSteamImages;
    private readonly MyGuiControlContentButton.MyControlImages m_workshopModioImages;
    private readonly MyGuiControlContentButton.MyControlImages m_localmodImages;
    private readonly MyGuiControlContentButton.MyControlImages m_cloudmodImages;
    public bool FocusHighlightAlsoSelects;
    private readonly List<MyGuiControlImage> m_dlcIcons;
    private string m_workshopServiceName;
    private readonly MyGuiCompositeTexture m_noThumbnailTexture = new MyGuiCompositeTexture("Textures\\GUI\\Icons\\Blueprints\\NoThumbnailFound.png");
    private readonly Color m_noThumbnailColor = new Color(94, 115, (int) sbyte.MaxValue);
    private MyBlueprintTypeEnum m_modType = MyBlueprintTypeEnum.DEFAULT;

    private MyGuiControlContentButton.MyControlImages? GetControlImages()
    {
      switch (this.m_modType)
      {
        case MyBlueprintTypeEnum.WORKSHOP:
          return new MyGuiControlContentButton.MyControlImages?(this.m_workshopServiceName == "mod.io" ? this.m_workshopModioImages : this.m_workshopSteamImages);
        case MyBlueprintTypeEnum.LOCAL:
          return new MyGuiControlContentButton.MyControlImages?(this.m_localmodImages);
        case MyBlueprintTypeEnum.CLOUD:
          return new MyGuiControlContentButton.MyControlImages?(this.m_cloudmodImages);
        default:
          return new MyGuiControlContentButton.MyControlImages?();
      }
    }

    private void SetIconType(MyBlueprintTypeEnum modType, string serviceName)
    {
      MyGuiControlContentButton.MyControlImages? controlImages = this.GetControlImages();
      if (controlImages?.Normal != null)
      {
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Normal);
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Highlight);
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Focus);
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Active);
      }
      this.m_workshopServiceName = serviceName;
      this.m_modType = modType;
      controlImages = this.GetControlImages();
      if (controlImages?.Normal == null)
        return;
      this.Elements.Add(this.HasHighlight ? (MyGuiControlBase) controlImages.Value.Highlight : (this.HasFocus ? (MyGuiControlBase) controlImages.Value.Focus : (this.Selected ? (MyGuiControlBase) controlImages.Value.Active : (MyGuiControlBase) controlImages.Value.Normal)));
    }

    public string Title => this.m_titleLabel.Text;

    public string PreviewImagePath => this.m_previewImagePath;

    public MyGuiControlContentButton(string title, string imagePath, string dlcIcon = null)
    {
      this.m_dlcIcons = new List<MyGuiControlImage>();
      this.VisualStyle = MyGuiControlRadioButtonStyleEnum.ScenarioButton;
      this.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel.Text = title;
      this.m_titleLabel = myGuiControlLabel;
      this.m_workshopSteamImages = this.CreateControlImages(MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM);
      this.m_workshopModioImages = this.CreateControlImages(MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_MOD_IO);
      this.m_localmodImages = this.CreateControlImages(MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL);
      this.m_cloudmodImages = this.CreateControlImages(MyGuiConstants.TEXTURE_ICON_MODS_CLOUD);
      this.m_previewImagePath = imagePath;
      if (!string.IsNullOrEmpty(dlcIcon))
        this.AddDlcIcon(dlcIcon);
      this.CreatePreview(imagePath);
      this.m_titleLabel.IsAutoScaleEnabled = true;
      this.m_titleLabel.IsAutoEllipsisEnabled = true;
      this.m_titleLabel.SetMaxWidth(this.m_previewImage.Size.X - 0.01f);
      this.Elements.Add((MyGuiControlBase) this.m_titleLabel);
      this.GamepadHelpTextId = MyCommonTexts.Gamepad_Help_ContentButton;
    }

    public void SetModType(MyBlueprintTypeEnum modType, string serviceName) => this.SetIconType(modType, serviceName);

    private MyGuiControlContentButton.MyControlImages CreateControlImages(
      MyGuiHighlightTexture textures)
    {
      MyGuiControlContentButton.MyControlImages myControlImages = new MyGuiControlContentButton.MyControlImages();
      ref MyGuiControlContentButton.MyControlImages local1 = ref myControlImages;
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage(textures: new string[1]
      {
        textures.Normal
      });
      myGuiControlImage1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      myGuiControlImage1.Size = textures.SizeGui;
      local1.Normal = myGuiControlImage1;
      ref MyGuiControlContentButton.MyControlImages local2 = ref myControlImages;
      MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(textures: new string[1]
      {
        textures.Highlight
      });
      myGuiControlImage2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      myGuiControlImage2.Size = textures.SizeGui;
      local2.Highlight = myGuiControlImage2;
      ref MyGuiControlContentButton.MyControlImages local3 = ref myControlImages;
      MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage(textures: new string[1]
      {
        textures.Focus
      });
      myGuiControlImage3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      myGuiControlImage3.Size = textures.SizeGui;
      local3.Focus = myGuiControlImage3;
      ref MyGuiControlContentButton.MyControlImages local4 = ref myControlImages;
      MyGuiControlImage myGuiControlImage4 = new MyGuiControlImage(textures: new string[1]
      {
        textures.Active
      });
      myGuiControlImage4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      myGuiControlImage4.Size = textures.SizeGui;
      local4.Active = myGuiControlImage4;
      return myControlImages;
    }

    protected override void RefreshInternals()
    {
      base.RefreshInternals();
      this.CheckBorder();
    }

    public void SetPreviewVisibility(bool visible)
    {
      this.m_previewImage.Visible = visible;
      Vector2 vector2_1 = new Vector2(242f, 128f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      if (visible)
      {
        this.m_previewImage.Size = vector2_1;
        this.m_previewImage.BorderEnabled = true;
        this.m_previewImage.BorderColor = new Vector4(0.23f, 0.27f, 0.3f, 1f);
        this.Size = new Vector2(this.m_previewImage.Size.X, this.m_titleLabel.Size.Y + this.m_previewImage.Size.Y);
        int num = 0;
        Vector2 vector2_2 = new Vector2(this.Size.X * 0.48f, (float) (-(double) this.Size.Y * 0.479999989271164) + this.m_titleLabel.Size.Y);
        foreach (MyGuiControlImage dlcIcon in this.m_dlcIcons)
        {
          dlcIcon.Visible = true;
          dlcIcon.Position = vector2_2 + new Vector2(0.0f, (float) num * (dlcIcon.Size.Y + 1f / 500f));
          ++num;
        }
      }
      else
      {
        this.m_previewImage.Size = new Vector2(0.0f, 0.0f);
        this.m_previewImage.BorderEnabled = true;
        this.m_previewImage.BorderColor = new Vector4(0.23f, 0.27f, 0.3f, 1f);
        this.Size = new Vector2(vector2_1.X, this.m_titleLabel.Size.Y + 1f / 500f);
      }
      foreach (MyGuiControlBase dlcIcon in this.m_dlcIcons)
        dlcIcon.Visible = visible;
    }

    public void CreatePreview(string path)
    {
      if (this.m_previewImage != null && this.Elements.Contains((MyGuiControlBase) this.m_previewImage))
        this.Elements.Remove((MyGuiControlBase) this.m_previewImage);
      this.m_previewImagePath = path;
      MyGuiControlImage myGuiControlImage = new MyGuiControlImage(textures: new string[1]
      {
        path
      });
      myGuiControlImage.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_previewImage = myGuiControlImage;
      if (!this.m_previewImage.IsAnyTextureValid())
      {
        this.m_previewImage.BackgroundTexture = this.m_noThumbnailTexture;
        this.m_previewImage.ColorMask = (Vector4) this.m_noThumbnailColor;
      }
      this.Elements.Add((MyGuiControlBase) this.m_previewImage);
      this.UpdatePositions();
      this.SetPreviewVisibility(true);
    }

    public void AddDlcIcon(string path)
    {
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage(textures: new string[1]
      {
        path
      });
      myGuiControlImage1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      myGuiControlImage1.Size = new Vector2(32f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      MyGuiControlImage myGuiControlImage2 = myGuiControlImage1;
      this.m_dlcIcons.Add(myGuiControlImage2);
      this.Elements.Add((MyGuiControlBase) myGuiControlImage2);
    }

    public void ClearDlcIcons()
    {
      if (this.m_dlcIcons == null || this.m_dlcIcons.Count == 0)
        return;
      foreach (MyGuiControlBase dlcIcon in this.m_dlcIcons)
        this.Elements.Remove(dlcIcon);
      this.m_dlcIcons.Clear();
    }

    protected override void OnSizeChanged()
    {
      base.OnSizeChanged();
      this.UpdatePositions();
    }

    private void UpdatePositions()
    {
      if (this.m_previewImage.Visible)
      {
        Vector2 vector2 = new Vector2(this.Size.X * -0.5f, this.Size.Y * -0.52f);
        this.m_titleLabel.Position = vector2 + new Vector2(3f / 1000f, 1f / 500f);
        this.m_previewImage.Position = vector2 + new Vector2(0.0f, this.m_titleLabel.Size.Y * 1f);
        Vector2 position = this.Size * 0.5f - new Vector2(1f / 1000f, 1f / 500f);
        this.UpdateControlImagesPositions(this.m_workshopSteamImages, position, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
        this.UpdateControlImagesPositions(this.m_workshopModioImages, position, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
        this.UpdateControlImagesPositions(this.m_localmodImages, position, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
        this.UpdateControlImagesPositions(this.m_cloudmodImages, position, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
        int num = 0;
        vector2 = new Vector2(this.Size.X * 0.48f, (float) (-(double) this.Size.Y * 0.5) + this.m_titleLabel.Size.Y);
        foreach (MyGuiControlImage dlcIcon in this.m_dlcIcons)
        {
          dlcIcon.Visible = true;
          dlcIcon.Position = vector2 + new Vector2(0.0f, (float) num * (dlcIcon.Size.Y + 1f / 500f));
          ++num;
        }
      }
      else
      {
        Vector2 vector2 = new Vector2(this.Size.X * -0.5f, this.Size.Y * -0.61f);
        this.m_titleLabel.Position = vector2 + new Vector2((float) (3.0 / 1000.0 + (double) this.m_workshopSteamImages.Normal.Size.X * 2.0 / 3.0), 1f / 500f);
        this.m_previewImage.Position = vector2 + new Vector2(0.0f, this.m_titleLabel.Size.Y * 1f);
        Vector2 position1 = vector2;
        this.UpdateControlImagesPositions(this.m_workshopSteamImages, position1, MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM.SizeGui * 2f / 3f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
        MyGuiControlContentButton.MyControlImages workshopModioImages = this.m_workshopModioImages;
        Vector2 position2 = position1;
        MyGuiHighlightTexture modsWorkshopSteam = MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM;
        Vector2 size1 = modsWorkshopSteam.SizeGui * 2f / 3f;
        this.UpdateControlImagesPositions(workshopModioImages, position2, size1, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
        MyGuiControlContentButton.MyControlImages localmodImages = this.m_localmodImages;
        Vector2 position3 = position1;
        modsWorkshopSteam = MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM;
        Vector2 size2 = modsWorkshopSteam.SizeGui * 2f / 3f;
        this.UpdateControlImagesPositions(localmodImages, position3, size2, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
        MyGuiControlContentButton.MyControlImages cloudmodImages = this.m_cloudmodImages;
        Vector2 position4 = position1;
        modsWorkshopSteam = MyGuiConstants.TEXTURE_ICON_MODS_WORKSHOP_STEAM;
        Vector2 size3 = modsWorkshopSteam.SizeGui * 2f / 3f;
        this.UpdateControlImagesPositions(cloudmodImages, position4, size3, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
        foreach (MyGuiControlBase dlcIcon in this.m_dlcIcons)
          dlcIcon.Visible = false;
      }
    }

    private void UpdateControlImagesPositions(
      MyGuiControlContentButton.MyControlImages images,
      Vector2 position,
      Vector2 size,
      MyGuiDrawAlignEnum align)
    {
      images.Normal.Position = position;
      images.Normal.OriginAlign = align;
      images.Normal.Size = size;
      images.Highlight.Position = position;
      images.Highlight.OriginAlign = align;
      images.Highlight.Size = size;
      images.Focus.Position = position;
      images.Focus.OriginAlign = align;
      images.Focus.Size = size;
      images.Active.Position = position;
      images.Active.OriginAlign = align;
      images.Active.Size = size;
    }

    protected override void OnHasHighlightChanged()
    {
      base.OnHasHighlightChanged();
      this.CheckBorder();
    }

    public override void OnFocusChanged(bool focus)
    {
      base.OnFocusChanged(focus);
      if (this.FocusHighlightAlsoSelects)
        this.Selected = true;
      this.CheckBorder();
    }

    private void SetControlImagesHighlight()
    {
      MyGuiControlContentButton.MyControlImages? controlImages = this.GetControlImages();
      if (!controlImages.HasValue)
        return;
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Normal))
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Normal);
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Focus))
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Focus);
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Active))
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Active);
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Highlight))
        return;
      this.Elements.Add((MyGuiControlBase) controlImages.Value.Highlight);
    }

    private void SetControlImagesNormal()
    {
      MyGuiControlContentButton.MyControlImages? controlImages = this.GetControlImages();
      if (!controlImages.HasValue)
        return;
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Focus))
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Focus);
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Active))
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Active);
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Highlight))
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Highlight);
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Normal))
        return;
      this.Elements.Add((MyGuiControlBase) controlImages.Value.Normal);
    }

    private void SetControlImagesFocus()
    {
      MyGuiControlContentButton.MyControlImages? controlImages = this.GetControlImages();
      if (!controlImages.HasValue)
        return;
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Normal))
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Normal);
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Active))
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Active);
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Highlight))
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Highlight);
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Focus))
        return;
      this.Elements.Add((MyGuiControlBase) controlImages.Value.Focus);
    }

    private void SetControlImagesActive()
    {
      MyGuiControlContentButton.MyControlImages? controlImages = this.GetControlImages();
      if (!controlImages.HasValue)
        return;
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Normal))
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Normal);
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Focus))
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Focus);
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Highlight))
        this.Elements.Remove((MyGuiControlBase) controlImages.Value.Highlight);
      if (this.Elements.Contains((MyGuiControlBase) controlImages.Value.Active))
        return;
      this.Elements.Add((MyGuiControlBase) controlImages.Value.Active);
    }

    private void CheckBorder()
    {
      if (this.HasHighlight)
      {
        this.BorderEnabled = true;
        this.BorderColor = MyGuiConstants.HIGHLIGHT_BACKGROUND_COLOR;
        this.BorderSize = 3;
        this.m_titleLabel.Font = "White";
        this.SetControlImagesHighlight();
      }
      else if (this.HasFocus)
      {
        this.BorderEnabled = true;
        this.BorderColor = MyGuiConstants.FOCUS_BACKGROUND_COLOR;
        this.BorderSize = 3;
        this.m_titleLabel.Font = "White";
        this.SetControlImagesFocus();
      }
      else if (this.Selected)
      {
        this.BorderEnabled = true;
        this.BorderColor = MyGuiConstants.ACTIVE_BACKGROUND_COLOR;
        this.BorderSize = 3;
        this.m_titleLabel.Font = "White";
        this.SetControlImagesActive();
      }
      else
      {
        this.BorderEnabled = false;
        this.BorderColor = new Vector4(0.23f, 0.27f, 0.3f, 1f);
        this.BorderSize = 1;
        if (this.m_titleLabel != null)
          this.m_titleLabel.Font = "Blue";
        this.SetControlImagesNormal();
      }
    }

    private struct MyControlImages
    {
      public MyGuiControlImage Normal;
      public MyGuiControlImage Highlight;
      public MyGuiControlImage Focus;
      public MyGuiControlImage Active;
    }
  }
}

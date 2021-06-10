// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlBloodOverlay
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyGuiControlBloodOverlay : MyGuiControlBase
  {
    public override void Update()
    {
      base.Update();
      MyGuiScreenBase topMostOwnerScreen = this.GetTopMostOwnerScreen();
      if (topMostOwnerScreen != MyGuiScreenHudSpace.Static)
        return;
      switch (topMostOwnerScreen.State)
      {
        case MyGuiScreenState.HIDING:
        case MyGuiScreenState.UNHIDING:
        case MyGuiScreenState.HIDDEN:
          this.Draw(1f, 1f);
          break;
      }
    }

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      base.Draw(transitionAlpha, backgroundTransitionAlpha);
      MySession mySession = MySession.Static;
      MyCharacter localCharacter = mySession.LocalCharacter;
      IMyControllableEntity controlledEntity = mySession.ControlledEntity;
      if (localCharacter == null || controlledEntity != localCharacter && (!(controlledEntity is MyCockpit myCockpit) || myCockpit.Pilot != localCharacter) && (!(controlledEntity is MyLargeTurretBase myLargeTurretBase) || myLargeTurretBase.Pilot != localCharacter))
        return;
      float hudBloodAlpha = localCharacter.Render.GetHUDBloodAlpha();
      if ((double) hudBloodAlpha <= 0.0)
        return;
      Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
      RectangleF destination = new RectangleF(0.0f, 0.0f, (float) fullscreenRectangle.Width, (float) fullscreenRectangle.Height);
      Rectangle? sourceRectangle = new Rectangle?();
      MyRenderProxy.DrawSprite("Textures\\Gui\\Blood.dds", ref destination, sourceRectangle, new Color(new Vector4(1f, 1f, 1f, hudBloodAlpha)), 0.0f, true, true);
    }

    public MyGuiControlBloodOverlay()
      : base()
    {
    }
  }
}

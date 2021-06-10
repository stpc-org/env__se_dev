// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenInitialLoading
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenInitialLoading : MyGuiScreenBase
  {
    private static MyGuiScreenInitialLoading m_instance;

    public static MyGuiScreenInitialLoading Instance
    {
      get
      {
        if (MyGuiScreenInitialLoading.m_instance == null)
          MyGuiScreenInitialLoading.m_instance = new MyGuiScreenInitialLoading();
        return MyGuiScreenInitialLoading.m_instance;
      }
    }

    public static bool CloseInstance()
    {
      if (MyGuiScreenInitialLoading.m_instance == null)
        return false;
      MyGuiScreenInitialLoading.m_instance.CloseScreen();
      MyGuiScreenInitialLoading.m_instance = (MyGuiScreenInitialLoading) null;
      return true;
    }

    public override string GetFriendlyName() => "Initial Loading screen";

    private MyGuiScreenInitialLoading()
      : base(new Vector2?(Vector2.Zero))
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      Vector2 normalizedCoord = new Vector2(1f, 0.9f);
      Vector2 normalizedSize = MyGuiManager.GetNormalizedSize(MyRenderProxy.GetTextureSize("Textures\\GUI\\screens\\screen_loading_wheel.dds"), 1f);
      MyGuiManager.DrawSpriteBatch("Textures\\GUI\\screens\\screen_loading_wheel.dds", normalizedCoord, 0.36f * normalizedSize, Color.White, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, waitTillLoaded: false, rotSpeed: 1f);
      MyGuiManager.DrawSpriteBatch("Textures\\GUI\\screens\\screen_loading_wheel.dds", normalizedCoord, 0.216f * normalizedSize, Color.White, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, waitTillLoaded: false, rotSpeed: -1f);
      MyGuiManager.DrawSpriteBatch("Textures\\GUI\\screens\\screen_loading_wheel.dds", normalizedCoord, 0.1296f * normalizedSize, Color.White, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, waitTillLoaded: false, rotSpeed: 1.5f);
    }
  }
}

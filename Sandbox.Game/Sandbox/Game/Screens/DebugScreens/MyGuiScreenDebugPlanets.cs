// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugPlanets
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Linq.Expressions;
using VRage;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
  internal class MyGuiScreenDebugPlanets : MyGuiScreenDebugBase
  {
    private float[] m_lodRanges;
    private static bool m_massive;
    private static MyGuiScreenDebugPlanets m_instance;

    public MyGuiScreenDebugPlanets()
      : base()
      => this.RecreateControls(true);

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugPlanets);

    private MyClipmapScaleEnum ScaleGroup => !MyGuiScreenDebugPlanets.m_massive ? MyClipmapScaleEnum.Normal : MyClipmapScaleEnum.Massive;

    private static bool Massive
    {
      get => MyGuiScreenDebugPlanets.m_massive;
      set
      {
        if (MyGuiScreenDebugPlanets.m_massive == value)
          return;
        MyGuiScreenDebugPlanets.m_instance.RecreateControls(false);
        MyGuiScreenDebugPlanets.m_massive = value;
      }
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.BackgroundColor = new Vector4?(new Vector4(1f, 1f, 1f, 0.5f));
      MyGuiScreenDebugPlanets.m_instance = this;
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.13f);
      this.AddCheckBox("Debug draw areas: ", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyDebugDrawSettings.DEBUG_DRAW_FLORA_BOXES)));
      this.AddCheckBox("Massive", (object) this, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyGuiScreenDebugPlanets.Massive)));
      this.m_lodRanges = new float[MyRenderConstants.RenderQualityProfile.LodClipmapRanges[(int) this.ScaleGroup].Length];
      for (int index = 0; index < this.m_lodRanges.Length; ++index)
      {
        int lod = index;
        this.AddSlider("LOD " + (object) index, this.m_lodRanges[index], 0.0f, index < 4 ? 1000f : (index < 7 ? 10000f : 300000f), (Action<MyGuiControlSlider>) (s => this.ChangeValue(s.Value, lod)));
      }
    }

    private void ChangeValue(float value, int lod)
    {
      this.m_lodRanges[lod] = value;
      float[][] lodClipmapRanges = MyRenderConstants.RenderQualityProfile.LodClipmapRanges;
      for (int index = 0; index < this.m_lodRanges.Length; ++index)
        lodClipmapRanges[(int) this.ScaleGroup][index] = this.m_lodRanges[index];
    }

    public override void HandleInput(bool receivedFocusInThisUpdate) => base.HandleInput(receivedFocusInThisUpdate);
  }
}

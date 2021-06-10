// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlVoxelHandSettings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.SessionComponents;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyGuiControlVoxelHandSettings : MyGuiControlParent
  {
    public MyToolbarItemVoxelHand Item { get; set; }

    public MyGuiControlVoxelHandSettings()
      : base(size: new Vector2?(new Vector2(0.263f, 0.4f)))
    {
    }

    public void UpdateControls()
    {
      IMyVoxelBrush myVoxelBrush = (IMyVoxelBrush) null;
      if (this.Item.Definition.Id.SubtypeName == "Box")
        myVoxelBrush = (IMyVoxelBrush) MyBrushBox.Static;
      else if (this.Item.Definition.Id.SubtypeName == "Capsule")
        myVoxelBrush = (IMyVoxelBrush) MyBrushCapsule.Static;
      else if (this.Item.Definition.Id.SubtypeName == "Ramp")
        myVoxelBrush = (IMyVoxelBrush) MyBrushRamp.Static;
      else if (this.Item.Definition.Id.SubtypeName == "Sphere")
        myVoxelBrush = (IMyVoxelBrush) MyBrushSphere.Static;
      else if (this.Item.Definition.Id.SubtypeName == "AutoLevel")
        myVoxelBrush = (IMyVoxelBrush) MyBrushAutoLevel.Static;
      if (myVoxelBrush == null)
        return;
      this.Elements.Clear();
      foreach (MyGuiControlBase guiControl in myVoxelBrush.GetGuiControls())
        this.Elements.Add(guiControl);
    }

    public override MyGuiControlBase HandleInput() => base.HandleInput() ?? this.HandleInputElements();

    internal void UpdateFromBrush(IMyVoxelBrush shape)
    {
      this.Elements.Clear();
      foreach (MyGuiControlBase guiControl in shape.GetGuiControls())
        this.Elements.Add(guiControl);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.CoordinateSystem.MyLocalCoordSys
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems.CoordinateSystem
{
  public class MyLocalCoordSys
  {
    private static readonly MyStringId ID_SQUARE = MyStringId.GetOrCompute("Square");
    private const float COLOR_ALPHA = 0.4f;
    private const int LOCAL_COORD_SIZE = 1000;
    private const float BBOX_BORDER_THICKNESS_MODIF = 0.0015f;
    private MyTransformD m_origin;
    private MyOrientedBoundingBoxD m_boundingBox;
    private Vector3D[] m_corners = new Vector3D[8];
    internal Color DebugColor;

    public MyTransformD Origin => this.m_origin;

    public long EntityCounter { get; set; }

    internal MyOrientedBoundingBoxD BoundingBox => this.m_boundingBox;

    public Color RenderColor { get; set; }

    public long Id { get; set; }

    public MyLocalCoordSys(int size = 1000)
    {
      this.m_origin = new MyTransformD(MatrixD.Identity);
      float num = (float) size / 2f;
      Vector3 vector3 = new Vector3(num, num, num);
      this.m_boundingBox = new MyOrientedBoundingBoxD(new BoundingBoxD((Vector3D) -vector3, (Vector3D) vector3), this.m_origin.TransformMatrix);
      this.m_boundingBox.GetCorners(this.m_corners, 0);
      this.RenderColor = this.GenerateRandomColor();
      this.DebugColor = this.GenerateDebugColor(this.RenderColor);
    }

    public MyLocalCoordSys(MyTransformD origin, int size = 1000)
    {
      this.m_origin = origin;
      Vector3 vector3 = new Vector3((float) (size / 2), (float) (size / 2), (float) (size / 2));
      this.m_boundingBox = new MyOrientedBoundingBoxD(new BoundingBoxD((Vector3D) -vector3, (Vector3D) vector3), this.m_origin.TransformMatrix);
      this.m_boundingBox.GetCorners(this.m_corners, 0);
      this.RenderColor = this.GenerateRandomColor();
      this.DebugColor = this.GenerateDebugColor(this.RenderColor);
    }

    private Color GenerateRandomColor()
    {
      double num1 = (double) MyRandom.Instance.Next(0, 100) / 100.0 * 0.400000005960464;
      float num2 = (float) ((double) MyRandom.Instance.Next(0, 100) / 100.0 * 0.400000005960464);
      float num3 = (float) ((double) MyRandom.Instance.Next(0, 100) / 100.0 * 0.400000005960464);
      double num4 = (double) num2;
      double num5 = (double) num3;
      return (Color) new Vector4((float) num1, (float) num4, (float) num5, 0.4f);
    }

    private Color GenerateDebugColor(Color original)
    {
      Vector3 hsv = new Color(original, 1f).ColorToHSV();
      hsv.Y = 0.8f;
      hsv.Z = 0.8f;
      return hsv.HSVtoColor();
    }

    public bool Contains(ref Vector3D vec) => this.m_boundingBox.Contains(ref vec);

    public void Draw()
    {
      MatrixD transformMatrix = this.Origin.TransformMatrix;
      Vector3D vector3D1 = Vector3D.One;
      Vector3D vector3D2 = Vector3D.Zero;
      for (int index = 0; index < 8; ++index)
      {
        Vector3D screen = MySector.MainCamera.WorldToScreen(ref this.m_corners[index]);
        vector3D1 = Vector3D.Min(vector3D1, screen);
        vector3D2 = Vector3D.Max(vector3D2, screen);
      }
      float lineWidth = 0.0015f / (float) MathHelper.Clamp((vector3D2 - vector3D1).Length(), 0.01, 1.0);
      Color color = MyFakes.ENABLE_DEBUG_DRAW_COORD_SYS ? this.DebugColor : this.RenderColor;
      BoundingBoxD localbox = new BoundingBoxD(-this.m_boundingBox.HalfExtent, this.m_boundingBox.HalfExtent);
      MySimpleObjectDraw.DrawTransparentBox(ref transformMatrix, ref localbox, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 1, lineWidth, new MyStringId?(MyLocalCoordSys.ID_SQUARE), new MyStringId?(MyLocalCoordSys.ID_SQUARE));
      if (!MyFakes.ENABLE_DEBUG_DRAW_COORD_SYS)
        return;
      MyRenderProxy.DebugDrawText3D(this.Origin.Position, string.Format("LCS Id:{0} Distance:{1:###.00}m", (object) this.Id, (object) (transformMatrix.Translation - MySector.MainCamera.Position).Length()), color, 1f, true);
      for (int index = -10; index < 11; ++index)
        MyRenderProxy.DebugDrawLine3D(this.Origin.Position + transformMatrix.Forward * 20.0 + transformMatrix.Right * ((double) index * 2.5), this.Origin.Position - transformMatrix.Forward * 20.0 + transformMatrix.Right * ((double) index * 2.5), color, color, false);
      for (int index = -10; index < 11; ++index)
        MyRenderProxy.DebugDrawLine3D(this.Origin.Position + transformMatrix.Right * 20.0 + transformMatrix.Forward * ((double) index * 2.5), this.Origin.Position - transformMatrix.Right * 20.0 + transformMatrix.Forward * ((double) index * 2.5), color, color, false);
    }
  }
}

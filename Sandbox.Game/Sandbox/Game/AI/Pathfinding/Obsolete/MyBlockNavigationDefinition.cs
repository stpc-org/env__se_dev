// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyBlockNavigationDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  [MyDefinitionType(typeof (MyObjectBuilder_BlockNavigationDefinition), null)]
  public class MyBlockNavigationDefinition : MyDefinitionBase
  {
    private static readonly StringBuilder m_tmpStringBuilder = new StringBuilder();
    private static readonly MyObjectBuilder_BlockNavigationDefinition m_tmpDefaultOb = new MyObjectBuilder_BlockNavigationDefinition();

    public MyGridNavigationMesh Mesh { get; private set; }

    public bool NoEntry { get; private set; }

    public MyBlockNavigationDefinition()
    {
      this.Mesh = (MyGridNavigationMesh) null;
      this.NoEntry = false;
    }

    public static MyObjectBuilder_BlockNavigationDefinition GetDefaultObjectBuilder(
      MyCubeBlockDefinition blockDefinition)
    {
      MyObjectBuilder_BlockNavigationDefinition tmpDefaultOb = MyBlockNavigationDefinition.m_tmpDefaultOb;
      MyBlockNavigationDefinition.m_tmpStringBuilder.Clear();
      MyBlockNavigationDefinition.m_tmpStringBuilder.Append("Default_");
      MyBlockNavigationDefinition.m_tmpStringBuilder.Append(blockDefinition.Size.X);
      MyBlockNavigationDefinition.m_tmpStringBuilder.Append("_");
      MyBlockNavigationDefinition.m_tmpStringBuilder.Append(blockDefinition.Size.Y);
      MyBlockNavigationDefinition.m_tmpStringBuilder.Append("_");
      MyBlockNavigationDefinition.m_tmpStringBuilder.Append(blockDefinition.Size.Z);
      tmpDefaultOb.Id = (SerializableDefinitionId) new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_BlockNavigationDefinition), MyBlockNavigationDefinition.m_tmpStringBuilder.ToString());
      tmpDefaultOb.Size = (SerializableVector3I) blockDefinition.Size;
      tmpDefaultOb.Center = (SerializableVector3I) blockDefinition.Center;
      return tmpDefaultOb;
    }

    public static void CreateDefaultTriangles(MyObjectBuilder_BlockNavigationDefinition ob)
    {
      Vector3I size = (Vector3I) ob.Size;
      Vector3I center = (Vector3I) ob.Center;
      int length = 4 * (size.X * size.Y + size.X * size.Z + size.Y * size.Z);
      ob.Triangles = new MyObjectBuilder_BlockNavigationDefinition.Triangle[length];
      int num1 = 0;
      Vector3 vector3_1 = size * 0.5f - (Vector3) center - Vector3.Half;
      for (int index1 = 0; index1 < 6; ++index1)
      {
        Base6Directions.Direction enumDirection = Base6Directions.EnumDirections[index1];
        Vector3 vector3_2 = vector3_1;
        Base6Directions.Direction direction1;
        Base6Directions.Direction direction2;
        Vector3 vector3_3;
        switch (enumDirection)
        {
          case Base6Directions.Direction.Backward:
            direction1 = Base6Directions.Direction.Right;
            direction2 = Base6Directions.Direction.Up;
            vector3_3 = vector3_2 + new Vector3(-0.5f, -0.5f, 0.5f) * size;
            break;
          case Base6Directions.Direction.Left:
            direction1 = Base6Directions.Direction.Backward;
            direction2 = Base6Directions.Direction.Up;
            vector3_3 = vector3_2 + new Vector3(-0.5f, -0.5f, -0.5f) * size;
            break;
          case Base6Directions.Direction.Right:
            direction1 = Base6Directions.Direction.Forward;
            direction2 = Base6Directions.Direction.Up;
            vector3_3 = vector3_2 + new Vector3(0.5f, -0.5f, 0.5f) * size;
            break;
          case Base6Directions.Direction.Up:
            direction1 = Base6Directions.Direction.Right;
            direction2 = Base6Directions.Direction.Forward;
            vector3_3 = vector3_2 + new Vector3(-0.5f, 0.5f, 0.5f) * size;
            break;
          case Base6Directions.Direction.Down:
            direction1 = Base6Directions.Direction.Right;
            direction2 = Base6Directions.Direction.Backward;
            vector3_3 = vector3_2 + new Vector3(-0.5f, -0.5f, -0.5f) * size;
            break;
          default:
            direction1 = Base6Directions.Direction.Left;
            direction2 = Base6Directions.Direction.Up;
            vector3_3 = vector3_2 + new Vector3(0.5f, -0.5f, -0.5f) * size;
            break;
        }
        Vector3 vector1 = Base6Directions.GetVector(direction1);
        Vector3 vector2 = Base6Directions.GetVector(direction2);
        int num2 = size.AxisValue(Base6Directions.GetAxis(direction2));
        int num3 = size.AxisValue(Base6Directions.GetAxis(direction1));
        for (int index2 = 0; index2 < num2; ++index2)
        {
          for (int index3 = 0; index3 < num3; ++index3)
          {
            MyObjectBuilder_BlockNavigationDefinition.Triangle triangle1 = new MyObjectBuilder_BlockNavigationDefinition.Triangle();
            triangle1.Points = new SerializableVector3[3];
            triangle1.Points[0] = (SerializableVector3) vector3_3;
            triangle1.Points[1] = (SerializableVector3) (vector3_3 + vector1);
            triangle1.Points[2] = (SerializableVector3) (vector3_3 + vector2);
            MyObjectBuilder_BlockNavigationDefinition.Triangle[] triangles1 = ob.Triangles;
            int index4 = num1;
            int num4 = index4 + 1;
            MyObjectBuilder_BlockNavigationDefinition.Triangle triangle2 = triangle1;
            triangles1[index4] = triangle2;
            MyObjectBuilder_BlockNavigationDefinition.Triangle triangle3 = new MyObjectBuilder_BlockNavigationDefinition.Triangle();
            triangle3.Points = new SerializableVector3[3];
            triangle3.Points[0] = (SerializableVector3) (vector3_3 + vector1);
            triangle3.Points[1] = (SerializableVector3) (vector3_3 + vector1 + vector2);
            triangle3.Points[2] = (SerializableVector3) (vector3_3 + vector2);
            MyObjectBuilder_BlockNavigationDefinition.Triangle[] triangles2 = ob.Triangles;
            int index5 = num4;
            num1 = index5 + 1;
            MyObjectBuilder_BlockNavigationDefinition.Triangle triangle4 = triangle3;
            triangles2[index5] = triangle4;
            vector3_3 += vector1;
          }
          vector3_3 = vector3_3 - vector1 * (float) num3 + vector2;
        }
      }
    }

    protected override void Init(MyObjectBuilder_DefinitionBase ob)
    {
      base.Init(ob);
      MyObjectBuilder_BlockNavigationDefinition navigationDefinition = ob as MyObjectBuilder_BlockNavigationDefinition;
      if (ob == null)
        return;
      if (navigationDefinition.NoEntry || navigationDefinition.Triangles == null)
      {
        this.NoEntry = true;
      }
      else
      {
        this.NoEntry = false;
        MyGridNavigationMesh gridNavigationMesh = new MyGridNavigationMesh((MyCubeGrid) null, (MyNavmeshCoordinator) null, navigationDefinition.Triangles.Length);
        Vector3I max = (Vector3I) navigationDefinition.Size - Vector3I.One - (Vector3I) navigationDefinition.Center;
        Vector3I min = -(Vector3I) navigationDefinition.Center;
        foreach (MyObjectBuilder_BlockNavigationDefinition.Triangle triangle in navigationDefinition.Triangles)
        {
          Vector3 point1 = (Vector3) triangle.Points[0];
          Vector3 point2 = (Vector3) triangle.Points[1];
          Vector3 point3 = (Vector3) triangle.Points[2];
          MyNavigationTriangle tri = gridNavigationMesh.AddTriangle(ref point1, ref point2, ref point3);
          Vector3 vector3_1 = (point1 + point2 + point3) / 3f;
          Vector3 vector3_2 = (vector3_1 - point1) * 0.0001f;
          Vector3 vector3_3 = (vector3_1 - point2) * 0.0001f;
          Vector3 vector3_4 = (vector3_1 - point3) * 0.0001f;
          Vector3I result1 = Vector3I.Round(point1 + vector3_2);
          Vector3I result2 = Vector3I.Round(point2 + vector3_3);
          Vector3I result3 = Vector3I.Round(point3 + vector3_4);
          Vector3I.Clamp(ref result1, ref min, ref max, out result1);
          Vector3I.Clamp(ref result2, ref min, ref max, out result2);
          Vector3I.Clamp(ref result3, ref min, ref max, out result3);
          Vector3I result4;
          Vector3I.Min(ref result1, ref result2, out result4);
          Vector3I.Min(ref result4, ref result3, out result4);
          Vector3I result5;
          Vector3I.Max(ref result1, ref result2, out result5);
          Vector3I.Max(ref result5, ref result3, out result5);
          Vector3I next = result4;
          Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref result4, ref result5);
          while (vector3IRangeIterator.IsValid())
          {
            gridNavigationMesh.RegisterTriangle(tri, ref next);
            vector3IRangeIterator.GetNext(out next);
          }
        }
        this.Mesh = gridNavigationMesh;
      }
    }

    private struct SizeAndCenter
    {
      private readonly Vector3I m_size;
      private readonly Vector3I m_center;

      public SizeAndCenter(Vector3I size, Vector3I center)
      {
        this.m_size = size;
        this.m_center = center;
      }

      private bool Equals(MyBlockNavigationDefinition.SizeAndCenter other) => other.m_size == this.m_size && other.m_center == this.m_center;

      public override bool Equals(object obj) => obj != null && !(obj.GetType() != typeof (MyBlockNavigationDefinition.SizeAndCenter)) && this.Equals((MyBlockNavigationDefinition.SizeAndCenter) obj);

      public override int GetHashCode()
      {
        Vector3I vector3I = this.m_size;
        int num = vector3I.GetHashCode() * 1610612741;
        vector3I = this.m_center;
        int hashCode = vector3I.GetHashCode();
        return num + hashCode;
      }
    }

    private class Sandbox_Game_AI_Pathfinding_Obsolete_MyBlockNavigationDefinition\u003C\u003EActor : IActivator, IActivator<MyBlockNavigationDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyBlockNavigationDefinition();

      MyBlockNavigationDefinition IActivator<MyBlockNavigationDefinition>.CreateInstance() => new MyBlockNavigationDefinition();
    }
  }
}

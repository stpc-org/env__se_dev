// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyFractureComponentBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Game.EntityComponents
{
  public abstract class MyFractureComponentBase : MyEntityComponentBase
  {
    protected readonly List<HkdShapeInstanceInfo> m_tmpChildren = new List<HkdShapeInstanceInfo>();
    protected readonly List<HkdShapeInstanceInfo> m_tmpShapeInfos = new List<HkdShapeInstanceInfo>();
    protected readonly List<MyObjectBuilder_FractureComponentBase.FracturedShape> m_tmpShapeList = new List<MyObjectBuilder_FractureComponentBase.FracturedShape>();
    public HkdBreakableShape Shape;

    public abstract MyPhysicalModelDefinition PhysicalModelDefinition { get; }

    public override bool IsSerialized() => true;

    public override string ComponentTypeDebugString => "Fracture";

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      MyRenderComponentFracturedPiece componentFracturedPiece = new MyRenderComponentFracturedPiece();
      if (this.Entity.Render.ModelStorage != null)
        componentFracturedPiece.ModelStorage = this.Entity.Render.ModelStorage;
      this.Entity.Render.UpdateRenderObject(false);
      MyPersistentEntityFlags2 persistentFlags = this.Entity.Render.PersistentFlags;
      Vector3 colorMaskHsv = this.Entity.Render.ColorMaskHsv;
      Dictionary<string, MyTextureChange> textureChanges = this.Entity.Render.TextureChanges;
      bool metalnessColorable = this.Entity.Render.MetalnessColorable;
      this.Entity.Render = (MyRenderComponentBase) componentFracturedPiece;
      this.Entity.Render.NeedsDraw = true;
      this.Entity.Render.PersistentFlags |= persistentFlags | MyPersistentEntityFlags2.CastShadows;
      this.Entity.Render.ColorMaskHsv = colorMaskHsv;
      this.Entity.Render.TextureChanges = textureChanges;
      this.Entity.Render.MetalnessColorable = metalnessColorable;
      this.Entity.Render.EnableColorMaskHsv = false;
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      if (!this.Shape.IsValid())
        return;
      this.Shape.RemoveReference();
    }

    public virtual bool RemoveChildShapes(IEnumerable<string> shapeNames)
    {
      this.m_tmpShapeList.Clear();
      this.GetCurrentFracturedShapeList(this.m_tmpShapeList, shapeNames);
      this.RecreateShape(this.m_tmpShapeList);
      this.m_tmpShapeList.Clear();
      return false;
    }

    protected void GetCurrentFracturedShapeList(
      List<MyObjectBuilder_FractureComponentBase.FracturedShape> shapeList,
      IEnumerable<string> excludeShapeNames = null)
    {
      MyFractureComponentBase.GetCurrentFracturedShapeList(this.Shape, shapeList, excludeShapeNames);
    }

    private static bool GetCurrentFracturedShapeList(
      HkdBreakableShape breakableShape,
      List<MyObjectBuilder_FractureComponentBase.FracturedShape> shapeList,
      IEnumerable<string> excludeShapeNames = null)
    {
      if (!breakableShape.IsValid())
        return false;
      string name = breakableShape.Name;
      bool flag1 = string.IsNullOrEmpty(name);
      if (excludeShapeNames != null && !flag1)
      {
        foreach (string excludeShapeName in excludeShapeNames)
        {
          if (name == excludeShapeName)
            return false;
        }
      }
      if (breakableShape.GetChildrenCount() > 0)
      {
        List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
        breakableShape.GetChildren(list);
        bool flag2 = true;
        foreach (HkdShapeInstanceInfo shapeInstanceInfo in list)
          flag2 &= MyFractureComponentBase.GetCurrentFracturedShapeList(shapeInstanceInfo.Shape, shapeList, excludeShapeNames);
        if (!flag1 & flag2)
        {
          foreach (HkdShapeInstanceInfo shapeInstanceInfo in list)
          {
            HkdShapeInstanceInfo inst = shapeInstanceInfo;
            if (inst.Shape.IsValid())
              shapeList.RemoveAll((Predicate<MyObjectBuilder_FractureComponentBase.FracturedShape>) (s => s.Name == inst.ShapeName));
          }
          shapeList.Add(new MyObjectBuilder_FractureComponentBase.FracturedShape()
          {
            Name = name,
            Fixed = breakableShape.IsFixed()
          });
        }
        return flag2;
      }
      if (flag1)
        return false;
      shapeList.Add(new MyObjectBuilder_FractureComponentBase.FracturedShape()
      {
        Name = name,
        Fixed = breakableShape.IsFixed()
      });
      return true;
    }

    protected abstract void RecreateShape(
      List<MyObjectBuilder_FractureComponentBase.FracturedShape> shapeList);

    protected void SerializeInternal(MyObjectBuilder_FractureComponentBase ob)
    {
      if (string.IsNullOrEmpty(this.Shape.Name) || this.Shape.IsCompound() || this.Shape.GetChildrenCount() > 0)
      {
        this.Shape.GetChildren(this.m_tmpChildren);
        foreach (HkdShapeInstanceInfo tmpChild in this.m_tmpChildren)
        {
          MyObjectBuilder_FractureComponentBase.FracturedShape fracturedShape = new MyObjectBuilder_FractureComponentBase.FracturedShape()
          {
            Name = tmpChild.ShapeName,
            Fixed = MyDestructionHelper.IsFixed(tmpChild.Shape)
          };
          ob.Shapes.Add(fracturedShape);
        }
        this.m_tmpChildren.Clear();
      }
      else
        ob.Shapes.Add(new MyObjectBuilder_FractureComponentBase.FracturedShape()
        {
          Name = this.Shape.Name
        });
    }

    public virtual void SetShape(HkdBreakableShape shape, bool compound)
    {
      if (this.Shape.IsValid())
        this.Shape.RemoveReference();
      this.Shape = shape;
      if (!(this.Entity.Render is MyRenderComponentFracturedPiece render))
        return;
      render.ClearModels();
      if (compound)
      {
        shape.GetChildren(this.m_tmpChildren);
        foreach (HkdShapeInstanceInfo tmpChild in this.m_tmpChildren)
        {
          if (tmpChild.IsValid())
            render.AddPiece(tmpChild.ShapeName, (MatrixD) ref Matrix.Identity);
        }
        this.m_tmpChildren.Clear();
      }
      else
        render.AddPiece(shape.Name, (MatrixD) ref Matrix.Identity);
      render.UpdateRenderObject(true);
    }

    public struct Info
    {
      public MyEntity Entity;
      public HkdBreakableShape Shape;
      public bool Compound;
    }
  }
}

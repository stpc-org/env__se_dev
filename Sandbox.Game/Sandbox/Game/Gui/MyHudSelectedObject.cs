// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudSelectedObject
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRageMath;
using VRageRender.Models;

namespace Sandbox.Game.Gui
{
  public class MyHudSelectedObject
  {
    [ThreadStatic]
    private static List<string> m_tmpSectionNames = new List<string>();
    [ThreadStatic]
    private static List<uint> m_tmpSubpartIds = new List<uint>();
    private bool m_highlightAttributeDirty;
    private bool m_visible;
    private uint m_visibleRenderID = uint.MaxValue;
    private string m_highlightAttribute;
    internal MyHudSelectedObjectStatus CurrentObject;
    internal MyHudSelectedObjectStatus PreviousObject;
    private Vector2 m_halfSize = Vector2.One * 0.02f;
    private Color m_color = MyHudConstants.HUD_COLOR_LIGHT;
    private MyHudObjectHighlightStyle m_style;

    internal MyHudSelectedObjectState State { get; private set; }

    public string HighlightAttribute
    {
      get => this.m_highlightAttribute;
      internal set
      {
        if (this.m_highlightAttribute == value)
          return;
        this.CheckForTransition();
        this.m_highlightAttribute = value;
        this.CurrentObject.SectionNames = new string[0];
        this.CurrentObject.SubpartIndices = (uint[]) null;
        if (value == null)
          return;
        this.m_highlightAttributeDirty = true;
      }
    }

    public MyHudObjectHighlightStyle HighlightStyle
    {
      get => this.m_style;
      set
      {
        if (this.m_style == value)
          return;
        this.CheckForTransition();
        this.m_style = value;
      }
    }

    public Vector2 HalfSize
    {
      get => this.m_halfSize;
      set
      {
        if (this.m_halfSize == value)
          return;
        this.CheckForTransition();
        this.m_halfSize = value;
      }
    }

    public Color Color
    {
      get => this.m_color;
      set
      {
        if (this.m_color == value)
          return;
        this.CheckForTransition();
        this.m_color = value;
      }
    }

    public bool Visible
    {
      get => this.m_visible;
      internal set
      {
        this.m_visibleRenderID = !value ? uint.MaxValue : this.CurrentObject.Instance.RenderObjectID;
        this.CurrentObject.Style = !value ? MyHudObjectHighlightStyle.None : this.m_style;
        this.m_visible = value;
        this.State = MyHudSelectedObjectState.VisibleStateSet;
      }
    }

    public uint VisibleRenderID => this.m_visibleRenderID;

    public IMyUseObject InteractiveObject => this.CurrentObject.Instance;

    internal uint[] SubpartIndices
    {
      get
      {
        this.ComputeHighlightIndices();
        return this.CurrentObject.SubpartIndices;
      }
    }

    internal string[] SectionNames
    {
      get
      {
        this.ComputeHighlightIndices();
        return this.CurrentObject.SectionNames;
      }
    }

    public void Clean()
    {
      this.CurrentObject = new MyHudSelectedObjectStatus();
      this.PreviousObject = new MyHudSelectedObjectStatus();
    }

    private void ComputeHighlightIndices()
    {
      if (!this.m_highlightAttributeDirty)
        return;
      if (this.m_highlightAttribute == null)
      {
        this.m_highlightAttributeDirty = false;
      }
      else
      {
        MyHudSelectedObject.m_tmpSectionNames.Clear();
        MyHudSelectedObject.m_tmpSubpartIds.Clear();
        string[] strArray = this.m_highlightAttribute.Split(";"[0]);
        MyModel model = this.CurrentObject.Instance.Owner.Render.GetModel();
        bool flag = true;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string str = strArray[index];
          if (str.StartsWith("subpart_"))
          {
            MyEntitySubpart subpart;
            flag = this.CurrentObject.Instance.Owner.TryGetSubpart(str.Substring("subpart_".Length), out subpart);
            if (flag)
            {
              uint renderObjectId = subpart.Render.GetRenderObjectID();
              if (renderObjectId != uint.MaxValue)
                MyHudSelectedObject.m_tmpSubpartIds.Add(renderObjectId);
            }
            else
              break;
          }
          else if (str.StartsWith("subblock_"))
          {
            if (this.CurrentObject.Instance.Owner is MyCubeBlock owner)
            {
              string name = str.Substring("subblock_".Length);
              MySlimBlock block;
              flag = owner.TryGetSubBlock(name, out block);
              if (flag)
              {
                uint renderObjectId = block.FatBlock.Render.GetRenderObjectID();
                if (renderObjectId != uint.MaxValue)
                  MyHudSelectedObject.m_tmpSubpartIds.Add(renderObjectId);
              }
              else
                break;
            }
            else
              break;
          }
          else
          {
            MyMeshSection section;
            flag = model.TryGetMeshSection(strArray[index], out section);
            if (flag)
              MyHudSelectedObject.m_tmpSectionNames.Add(section.Name);
            else
              break;
          }
        }
        if (flag)
        {
          this.CurrentObject.SectionNames = MyHudSelectedObject.m_tmpSectionNames.ToArray();
          if (MyHudSelectedObject.m_tmpSubpartIds.Count != 0)
            this.CurrentObject.SubpartIndices = MyHudSelectedObject.m_tmpSubpartIds.ToArray();
        }
        else
        {
          this.CurrentObject.SectionNames = new string[0];
          this.CurrentObject.SubpartIndices = (uint[]) null;
        }
        this.m_highlightAttributeDirty = false;
      }
    }

    internal void Highlight(IMyUseObject obj)
    {
      if (this.SetObjectInternal(obj))
        return;
      if (this.m_visible)
      {
        if (this.State != MyHudSelectedObjectState.MarkedForNotVisible)
          return;
        this.State = MyHudSelectedObjectState.VisibleStateSet;
      }
      else
        this.State = MyHudSelectedObjectState.MarkedForVisible;
    }

    internal void RemoveHighlight()
    {
      if (this.m_visible)
      {
        this.State = MyHudSelectedObjectState.MarkedForNotVisible;
      }
      else
      {
        if (this.State != MyHudSelectedObjectState.MarkedForVisible)
          return;
        this.State = MyHudSelectedObjectState.VisibleStateSet;
      }
    }

    internal void ResetCurrent()
    {
      this.CurrentObject.Reset();
      this.m_highlightAttributeDirty = true;
    }

    private bool SetObjectInternal(IMyUseObject obj)
    {
      if (this.CurrentObject.Instance == obj)
        return false;
      int num = this.CheckForTransition() ? 1 : 0;
      this.ResetCurrent();
      this.CurrentObject.Instance = obj;
      return num != 0;
    }

    private bool CheckForTransition()
    {
      if (this.CurrentObject.Instance == null || !this.m_visible)
        return false;
      if (this.PreviousObject.Instance != null)
        return true;
      this.DoTransition();
      return true;
    }

    private void DoTransition()
    {
      this.PreviousObject = this.CurrentObject;
      this.State = MyHudSelectedObjectState.MarkedForVisible;
    }
  }
}

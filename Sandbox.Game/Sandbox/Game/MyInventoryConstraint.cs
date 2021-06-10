// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyInventoryConstraint
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game
{
  public class MyInventoryConstraint
  {
    public string Icon;
    public bool m_useDefaultIcon;
    public readonly string Description;
    private HashSet<MyDefinitionId> m_constrainedIds;
    private HashSet<MyObjectBuilderType> m_constrainedTypes;

    public bool IsWhitelist { get; set; }

    public HashSetReader<MyDefinitionId> ConstrainedIds => (HashSetReader<MyDefinitionId>) this.m_constrainedIds;

    public HashSetReader<MyObjectBuilderType> ConstrainedTypes => (HashSetReader<MyObjectBuilderType>) this.m_constrainedTypes;

    public MyInventoryConstraint(MyStringId description, string icon = null, bool whitelist = true)
    {
      this.Icon = icon;
      this.m_useDefaultIcon = icon == null;
      this.Description = MyTexts.GetString(description);
      this.m_constrainedIds = new HashSet<MyDefinitionId>();
      this.m_constrainedTypes = new HashSet<MyObjectBuilderType>();
      this.IsWhitelist = whitelist;
    }

    public MyInventoryConstraint(string description, string icon = null, bool whitelist = true)
    {
      this.Icon = icon;
      this.m_useDefaultIcon = icon == null;
      this.Description = description;
      this.m_constrainedIds = new HashSet<MyDefinitionId>();
      this.m_constrainedTypes = new HashSet<MyObjectBuilderType>();
      this.IsWhitelist = whitelist;
    }

    public MyInventoryConstraint Add(MyDefinitionId id)
    {
      this.m_constrainedIds.Add(id);
      this.UpdateIcon();
      return this;
    }

    public MyInventoryConstraint Remove(MyDefinitionId id)
    {
      this.m_constrainedIds.Remove(id);
      this.UpdateIcon();
      return this;
    }

    public MyInventoryConstraint AddObjectBuilderType(MyObjectBuilderType type)
    {
      this.m_constrainedTypes.Add(type);
      this.UpdateIcon();
      return this;
    }

    public MyInventoryConstraint RemoveObjectBuilderType(
      MyObjectBuilderType type)
    {
      this.m_constrainedTypes.Remove(type);
      this.UpdateIcon();
      return this;
    }

    public void Clear()
    {
      this.m_constrainedIds.Clear();
      this.m_constrainedTypes.Clear();
      this.UpdateIcon();
    }

    public bool Check(MyDefinitionId checkedId) => this.IsWhitelist ? this.m_constrainedTypes.Contains(checkedId.TypeId) || this.m_constrainedIds.Contains(checkedId) : !this.m_constrainedTypes.Contains(checkedId.TypeId) && !this.m_constrainedIds.Contains(checkedId);

    public void UpdateIcon()
    {
      if (!this.m_useDefaultIcon)
        return;
      if (this.m_constrainedIds.Count == 0 && this.m_constrainedTypes.Count == 1)
      {
        MyObjectBuilderType objectBuilderType = this.m_constrainedTypes.First<MyObjectBuilderType>();
        if (objectBuilderType == typeof (MyObjectBuilder_Ore))
          this.Icon = MyGuiConstants.TEXTURE_ICON_FILTER_ORE;
        else if (objectBuilderType == typeof (MyObjectBuilder_Ingot))
        {
          this.Icon = MyGuiConstants.TEXTURE_ICON_FILTER_INGOT;
        }
        else
        {
          if (!(objectBuilderType == typeof (MyObjectBuilder_Component)))
            return;
          this.Icon = MyGuiConstants.TEXTURE_ICON_FILTER_COMPONENT;
        }
      }
      else if (this.m_constrainedIds.Count == 1 && this.m_constrainedTypes.Count == 0)
      {
        if (!(this.m_constrainedIds.First<MyDefinitionId>() == new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Ingot), "Uranium")))
          return;
        this.Icon = MyGuiConstants.TEXTURE_ICON_FILTER_URANIUM;
      }
      else
        this.Icon = (string) null;
    }
  }
}

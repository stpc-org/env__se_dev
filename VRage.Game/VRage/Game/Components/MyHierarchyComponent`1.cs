// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyHierarchyComponent`1
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Network;
using VRageMath;

namespace VRage.Game.Components
{
  public class MyHierarchyComponent<TYPE> : MyHierarchyComponentBase
  {
    public Action<BoundingBoxD, List<TYPE>> QueryAABBImpl;
    public Action<BoundingSphereD, List<TYPE>> QuerySphereImpl;
    public Action<LineD, List<MyLineSegmentOverlapResult<TYPE>>> QueryLineImpl;

    public void QueryAABB(ref BoundingBoxD aabb, List<TYPE> result)
    {
      if (this.Entity == null || this.Entity.MarkedForClose || this.QueryAABBImpl == null)
        return;
      this.QueryAABBImpl(aabb, result);
    }

    public void QuerySphere(ref BoundingSphereD sphere, List<TYPE> result)
    {
      if (this.Entity.MarkedForClose || this.QuerySphereImpl == null)
        return;
      this.QuerySphereImpl(sphere, result);
    }

    public void QueryLine(ref LineD line, List<MyLineSegmentOverlapResult<TYPE>> result)
    {
      if (this.Entity.MarkedForClose || this.QueryLineImpl == null)
        return;
      this.QueryLineImpl(line, result);
    }

    private class VRage_Game_Components_MyHierarchyComponent`1\u003C\u003EActor : IActivator, IActivator<MyHierarchyComponent<TYPE>>
    {
      object IActivator.CreateInstance() => (object) new MyHierarchyComponent<TYPE>();

      MyHierarchyComponent<TYPE> IActivator<MyHierarchyComponent<TYPE>>.CreateInstance() => new MyHierarchyComponent<TYPE>();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: VRage.Groups.MyGroupsBase`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;

namespace VRage.Groups
{
  public abstract class MyGroupsBase<TNode> where TNode : class
  {
    public abstract void AddNode(TNode nodeToAdd);

    public abstract void RemoveNode(TNode nodeToRemove);

    public abstract void CreateLink(long linkId, TNode parentNode, TNode childNode);

    public abstract bool BreakLink(long linkId, TNode parentNode, TNode childNode = null);

    public abstract bool LinkExists(long linkId, TNode parentNode, TNode childNode = null);

    public abstract List<TNode> GetGroupNodes(TNode nodeInGroup);

    public abstract void GetGroupNodes(TNode nodeInGroup, List<TNode> result);

    public abstract bool HasSameGroup(TNode nodeA, TNode nodeB);
  }
}

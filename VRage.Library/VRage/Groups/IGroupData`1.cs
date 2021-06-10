// Decompiled with JetBrains decompiler
// Type: VRage.Groups.IGroupData`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Groups
{
  public interface IGroupData<TNode> where TNode : class
  {
    void OnCreate<TGroupData>(MyGroups<TNode, TGroupData>.Group group) where TGroupData : IGroupData<TNode>, new();

    void OnRelease();

    void OnNodeAdded(TNode entity);

    void OnNodeRemoved(TNode entity);
  }
}

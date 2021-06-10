// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyBlockGroupData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Groups;

namespace Sandbox.Game.Entities.Cube
{
  public class MyBlockGroupData : IGroupData<MySlimBlock>
  {
    public void OnRelease()
    {
    }

    public void OnNodeAdded(MySlimBlock entity)
    {
    }

    public void OnNodeRemoved(MySlimBlock entity)
    {
    }

    public void OnCreate<TGroupData>(MyGroups<MySlimBlock, TGroupData>.Group group) where TGroupData : IGroupData<MySlimBlock>, new()
    {
    }
  }
}

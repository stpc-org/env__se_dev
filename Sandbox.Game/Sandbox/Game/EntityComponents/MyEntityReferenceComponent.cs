// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyEntityReferenceComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Components;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
  public class MyEntityReferenceComponent : MyEntityComponentBase
  {
    private int m_references;

    public override string ComponentTypeDebugString => "ReferenceCount";

    public void Ref() => ++this.m_references;

    public bool Unref()
    {
      --this.m_references;
      if (this.m_references > 0)
        return false;
      this.Entity.Close();
      return true;
    }

    private class Sandbox_Game_EntityComponents_MyEntityReferenceComponent\u003C\u003EActor : IActivator, IActivator<MyEntityReferenceComponent>
    {
      object IActivator.CreateInstance() => (object) new MyEntityReferenceComponent();

      MyEntityReferenceComponent IActivator<MyEntityReferenceComponent>.CreateInstance() => new MyEntityReferenceComponent();
    }
  }
}

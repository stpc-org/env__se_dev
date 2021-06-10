// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MyStateMachineTransition
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;
using VRage.Library.Utils;
using VRage.Utils;

namespace VRage.Generics
{
  public class MyStateMachineTransition
  {
    public MyStringId Name = MyStringId.NullOrEmpty;
    public MyStateMachineNode TargetNode;
    public List<IMyCondition> Conditions = new List<IMyCondition>();
    public int? Priority;

    public int Id { get; private set; }

    public virtual bool Evaluate()
    {
      for (int index = 0; index < this.Conditions.Count; ++index)
      {
        if (!this.Conditions[index].Evaluate())
          return false;
      }
      return true;
    }

    public void _SetId(int newId) => this.Id = newId;

    public override string ToString() => this.TargetNode != null ? "transition -> " + this.TargetNode.Name : "transition -> (null)";
  }
}

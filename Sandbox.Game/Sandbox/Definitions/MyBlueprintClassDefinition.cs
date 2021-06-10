// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyBlueprintClassDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_BlueprintClassDefinition), null)]
  public class MyBlueprintClassDefinition : MyDefinitionBase, IEnumerable<MyBlueprintDefinitionBase>, IEnumerable
  {
    public string HighlightIcon;
    public string FocusIcon;
    public string InputConstraintIcon;
    public string OutputConstraintIcon;
    public string ProgressBarSoundCue;
    private SortedSet<MyBlueprintDefinitionBase> m_blueprints;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_BlueprintClassDefinition blueprintClassDefinition = builder as MyObjectBuilder_BlueprintClassDefinition;
      this.HighlightIcon = blueprintClassDefinition.HighlightIcon;
      this.FocusIcon = blueprintClassDefinition.FocusIcon;
      this.InputConstraintIcon = blueprintClassDefinition.InputConstraintIcon;
      this.OutputConstraintIcon = blueprintClassDefinition.OutputConstraintIcon;
      this.ProgressBarSoundCue = blueprintClassDefinition.ProgressBarSoundCue;
      this.m_blueprints = new SortedSet<MyBlueprintDefinitionBase>((IComparer<MyBlueprintDefinitionBase>) MyBlueprintClassDefinition.PrioritizedSubtypeComparer.Static);
    }

    public void AddBlueprint(MyBlueprintDefinitionBase blueprint)
    {
      if (this.m_blueprints.Contains(blueprint))
        return;
      this.m_blueprints.Add(blueprint);
    }

    public void ClearBlueprints() => this.m_blueprints.Clear();

    public bool ContainsBlueprint(MyBlueprintDefinitionBase blueprint) => this.m_blueprints.Contains(blueprint);

    public IEnumerator<MyBlueprintDefinitionBase> GetEnumerator() => (IEnumerator<MyBlueprintDefinitionBase>) this.m_blueprints.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.m_blueprints.GetEnumerator();

    private class SubtypeComparer : IComparer<MyBlueprintDefinitionBase>
    {
      public static MyBlueprintClassDefinition.SubtypeComparer Static = new MyBlueprintClassDefinition.SubtypeComparer();

      public int Compare(MyBlueprintDefinitionBase x, MyBlueprintDefinitionBase y) => x.Id.SubtypeName.CompareTo(y.Id.SubtypeName);
    }

    private class PrioritizedSubtypeComparer : IComparer<MyBlueprintDefinitionBase>
    {
      public static MyBlueprintClassDefinition.PrioritizedSubtypeComparer Static = new MyBlueprintClassDefinition.PrioritizedSubtypeComparer();

      public int Compare(MyBlueprintDefinitionBase x, MyBlueprintDefinitionBase y)
      {
        if (x == null)
          return y != null ? 1 : 0;
        if (y == null || x.Priority > y.Priority)
          return -1;
        return x.Priority < y.Priority ? 1 : x.Id.SubtypeName.CompareTo(y.Id.SubtypeName);
      }
    }

    private class Sandbox_Definitions_MyBlueprintClassDefinition\u003C\u003EActor : IActivator, IActivator<MyBlueprintClassDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyBlueprintClassDefinition();

      MyBlueprintClassDefinition IActivator<MyBlueprintClassDefinition>.CreateInstance() => new MyBlueprintClassDefinition();
    }
  }
}

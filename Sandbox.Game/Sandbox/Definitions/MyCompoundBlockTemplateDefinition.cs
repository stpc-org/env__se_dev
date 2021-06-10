// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyCompoundBlockTemplateDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_CompoundBlockTemplateDefinition), null)]
  public class MyCompoundBlockTemplateDefinition : MyDefinitionBase
  {
    public MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding[] Bindings;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_CompoundBlockTemplateDefinition templateDefinition = builder as MyObjectBuilder_CompoundBlockTemplateDefinition;
      if (templateDefinition.Bindings != null)
      {
        this.Bindings = new MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding[templateDefinition.Bindings.Length];
        for (int index1 = 0; index1 < templateDefinition.Bindings.Length; ++index1)
        {
          MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding compoundBlockBinding = new MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding();
          compoundBlockBinding.BuildType = MyStringId.GetOrCompute(templateDefinition.Bindings[index1].BuildType != null ? templateDefinition.Bindings[index1].BuildType.ToLower() : (string) null);
          compoundBlockBinding.Multiple = templateDefinition.Bindings[index1].Multiple;
          if (templateDefinition.Bindings[index1].RotationBinds != null && templateDefinition.Bindings[index1].RotationBinds.Length != 0)
          {
            compoundBlockBinding.RotationBinds = new MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding[templateDefinition.Bindings[index1].RotationBinds.Length];
            for (int index2 = 0; index2 < templateDefinition.Bindings[index1].RotationBinds.Length; ++index2)
            {
              if (templateDefinition.Bindings[index1].RotationBinds[index2].Rotations != null && templateDefinition.Bindings[index1].RotationBinds[index2].Rotations.Length != 0)
              {
                compoundBlockBinding.RotationBinds[index2] = new MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding();
                compoundBlockBinding.RotationBinds[index2].BuildTypeReference = MyStringId.GetOrCompute(templateDefinition.Bindings[index1].RotationBinds[index2].BuildTypeReference != null ? templateDefinition.Bindings[index1].RotationBinds[index2].BuildTypeReference.ToLower() : (string) null);
                compoundBlockBinding.RotationBinds[index2].Rotations = new MyBlockOrientation[templateDefinition.Bindings[index1].RotationBinds[index2].Rotations.Length];
                for (int index3 = 0; index3 < templateDefinition.Bindings[index1].RotationBinds[index2].Rotations.Length; ++index3)
                  compoundBlockBinding.RotationBinds[index2].Rotations[index3] = (MyBlockOrientation) templateDefinition.Bindings[index1].RotationBinds[index2].Rotations[index3];
              }
            }
          }
          this.Bindings[index1] = compoundBlockBinding;
        }
      }
      else
        this.Bindings = (MyCompoundBlockTemplateDefinition.MyCompoundBlockBinding[]) null;
    }

    public class MyCompoundBlockRotationBinding
    {
      public MyStringId BuildTypeReference;
      public MyBlockOrientation[] Rotations;
    }

    public class MyCompoundBlockBinding
    {
      public MyStringId BuildType;
      public bool Multiple;
      public MyCompoundBlockTemplateDefinition.MyCompoundBlockRotationBinding[] RotationBinds;
    }

    private class Sandbox_Definitions_MyCompoundBlockTemplateDefinition\u003C\u003EActor : IActivator, IActivator<MyCompoundBlockTemplateDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyCompoundBlockTemplateDefinition();

      MyCompoundBlockTemplateDefinition IActivator<MyCompoundBlockTemplateDefinition>.CreateInstance() => new MyCompoundBlockTemplateDefinition();
    }
  }
}

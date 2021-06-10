// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_ProgressBarCompositeTexture
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [MyObjectBuilderDefinition(null, null)]
  public class MyObjectBuilder_ProgressBarCompositeTexture : MyObjectBuilder_CompositeTexture
  {
    public MyStringHash ProgressCenter;
    public MyStringHash ProgressLeft;
    public MyStringHash ProgressRight;
    public MyStringHash ProgressOverlay;

    public override bool IsValid() => base.IsValid() || this.ProgressCenter != MyStringHash.NullOrEmpty || (this.ProgressLeft != MyStringHash.NullOrEmpty || this.ProgressRight != MyStringHash.NullOrEmpty) || this.ProgressOverlay != MyStringHash.NullOrEmpty;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003EProgressCenter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        owner.ProgressCenter = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        value = owner.ProgressCenter;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003EProgressLeft\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        owner.ProgressLeft = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        value = owner.ProgressLeft;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003EProgressRight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        owner.ProgressRight = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        value = owner.ProgressRight;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003EProgressOverlay\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        owner.ProgressOverlay = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        value = owner.ProgressOverlay;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003ELeftTop\u003C\u003EAccessor : MyObjectBuilder_CompositeTexture.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ELeftTop\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CompositeTexture&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CompositeTexture&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003ELeftCenter\u003C\u003EAccessor : MyObjectBuilder_CompositeTexture.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ELeftCenter\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CompositeTexture&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CompositeTexture&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003ELeftBottom\u003C\u003EAccessor : MyObjectBuilder_CompositeTexture.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ELeftBottom\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CompositeTexture&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CompositeTexture&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003ECenterTop\u003C\u003EAccessor : MyObjectBuilder_CompositeTexture.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ECenterTop\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CompositeTexture&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CompositeTexture&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003ECenter\u003C\u003EAccessor : MyObjectBuilder_CompositeTexture.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ECenter\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CompositeTexture&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CompositeTexture&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003ECenterBottom\u003C\u003EAccessor : MyObjectBuilder_CompositeTexture.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ECenterBottom\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CompositeTexture&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CompositeTexture&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003ERightTop\u003C\u003EAccessor : MyObjectBuilder_CompositeTexture.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ERightTop\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CompositeTexture&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CompositeTexture&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003ERightCenter\u003C\u003EAccessor : MyObjectBuilder_CompositeTexture.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ERightCenter\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CompositeTexture&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CompositeTexture&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003ERightBottom\u003C\u003EAccessor : MyObjectBuilder_CompositeTexture.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CompositeTexture\u003C\u003ERightBottom\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CompositeTexture&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CompositeTexture&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarCompositeTexture, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarCompositeTexture owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarCompositeTexture\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ProgressBarCompositeTexture>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ProgressBarCompositeTexture();

      MyObjectBuilder_ProgressBarCompositeTexture IActivator<MyObjectBuilder_ProgressBarCompositeTexture>.CreateInstance() => new MyObjectBuilder_ProgressBarCompositeTexture();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_CrosshairStyle
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_CrosshairStyle : MyObjectBuilder_StatControls
  {
    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle\u003C\u003EApplyHudScale\u003C\u003EAccessor : MyObjectBuilder_StatControls.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003EApplyHudScale\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CrosshairStyle owner, in bool value) => this.Set((MyObjectBuilder_StatControls&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CrosshairStyle owner, out bool value) => this.Get((MyObjectBuilder_StatControls&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_StatControls.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CrosshairStyle owner, in Vector2 value) => this.Set((MyObjectBuilder_StatControls&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CrosshairStyle owner, out Vector2 value) => this.Get((MyObjectBuilder_StatControls&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle\u003C\u003EOriginAlign\u003C\u003EAccessor : MyObjectBuilder_StatControls.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003EOriginAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CrosshairStyle owner, in MyGuiDrawAlignEnum value) => this.Set((MyObjectBuilder_StatControls&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CrosshairStyle owner,
        out MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatControls&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor : MyObjectBuilder_StatControls.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003EVisibleCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CrosshairStyle owner, in ConditionBase value) => this.Set((MyObjectBuilder_StatControls&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CrosshairStyle owner, out ConditionBase value) => this.Get((MyObjectBuilder_StatControls&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle\u003C\u003EStatStyles\u003C\u003EAccessor : MyObjectBuilder_StatControls.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatControls\u003C\u003EStatStyles\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, MyObjectBuilder_StatVisualStyle[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CrosshairStyle owner,
        in MyObjectBuilder_StatVisualStyle[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatControls&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CrosshairStyle owner,
        out MyObjectBuilder_StatVisualStyle[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatControls&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CrosshairStyle owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CrosshairStyle owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CrosshairStyle owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CrosshairStyle owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CrosshairStyle owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CrosshairStyle owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CrosshairStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CrosshairStyle owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CrosshairStyle owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CrosshairStyle\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CrosshairStyle>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CrosshairStyle();

      MyObjectBuilder_CrosshairStyle IActivator<MyObjectBuilder_CrosshairStyle>.CreateInstance() => new MyObjectBuilder_CrosshairStyle();
    }
  }
}

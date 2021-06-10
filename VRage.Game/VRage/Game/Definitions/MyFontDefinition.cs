// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.MyFontDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_FontDefinition), null)]
  public class MyFontDefinition : MyDefinitionBase
  {
    private static Dictionary<MyStringHash, MyFont> m_fontsById = new Dictionary<MyStringHash, MyFont>();
    private MyObjectBuilder_FontDefinition m_ob;
    private List<MyObjectBuilder_FontData> m_currentResources;

    public bool IsValid => this.m_ob != null;

    public string CompatibilityPath => this.m_ob.Path;

    public Color? ColorMask => this.m_ob.ColorMask;

    public bool Default => this.m_ob.Default;

    public IEnumerable<MyObjectBuilder_FontData> Resources => (IEnumerable<MyObjectBuilder_FontData>) this.m_currentResources;

    public void UseLanguage(string language)
    {
      MyObjectBuilder_FontDefinition.LanguageResources languageResources = this.m_ob.LanguageSpecificDefinitions.FirstOrDefault<MyObjectBuilder_FontDefinition.LanguageResources>((Func<MyObjectBuilder_FontDefinition.LanguageResources, bool>) (x => x.Language == language));
      this.m_currentResources = languageResources != null ? languageResources.Resources : this.m_ob.Resources;
      MyFontDefinition.SortBySize(this.m_currentResources);
      MyFontDefinition.m_fontsById[this.Id.SubtypeId] = !string.IsNullOrEmpty(this.CompatibilityPath) ? new MyFont(this.CompatibilityPath) : new MyFont(this.Resources.FirstOrDefault<MyObjectBuilder_FontData>()?.Path);
    }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.m_ob = builder as MyObjectBuilder_FontDefinition;
      this.m_currentResources = this.m_ob.Resources;
      MyFontDefinition.SortBySize(this.m_currentResources);
      MyFontDefinition.m_fontsById[this.Id.SubtypeId] = !string.IsNullOrEmpty(this.CompatibilityPath) ? new MyFont(this.CompatibilityPath) : new MyFont(this.Resources.FirstOrDefault<MyObjectBuilder_FontData>()?.Path);
    }

    private static void SortBySize(List<MyObjectBuilder_FontData> resources) => resources?.Sort((Comparison<MyObjectBuilder_FontData>) ((dataX, dataY) => dataX.Size.CompareTo(dataY.Size)));

    public static MyFont GetFont(MyStringHash fontId) => MyFontDefinition.m_fontsById[fontId];

    public static Vector2 MeasureStringRaw(
      string font,
      StringBuilder text,
      float scale,
      bool useMyRenderGuiConstants = true)
    {
      MyFont myFont;
      return MyFontDefinition.m_fontsById.TryGetValue(MyStringHash.GetOrCompute(font), out myFont) ? myFont.MeasureString(text, scale, useMyRenderGuiConstants) : Vector2.Zero;
    }

    public static Vector2 MeasureStringRaw(
      string font,
      string text,
      float scale,
      bool useMyRenderGuiConstants = true)
    {
      MyFont myFont;
      return MyFontDefinition.m_fontsById.TryGetValue(MyStringHash.GetOrCompute(font), out myFont) ? myFont.MeasureString(text, scale, useMyRenderGuiConstants) : Vector2.Zero;
    }

    private class VRage_Game_Definitions_MyFontDefinition\u003C\u003EActor : IActivator, IActivator<MyFontDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyFontDefinition();

      MyFontDefinition IActivator<MyFontDefinition>.CreateInstance() => new MyFontDefinition();
    }
  }
}

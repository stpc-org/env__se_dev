// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.SessionComponents.MyContainerDropSystemDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Network;
using VRageMath;

namespace VRage.Game.Definitions.SessionComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_ContainerDropSystemDefinition), null)]
  public class MyContainerDropSystemDefinition : MySessionComponentDefinition
  {
    [Obsolete]
    public float PersonalContainerRatio;
    [Obsolete]
    public int ContainerDropTime;
    public float PersonalContainerDistMin;
    public float PersonalContainerDistMax;
    public float CompetetiveContainerDistMin;
    public float CompetetiveContainerDistMax;
    public int CompetetiveContainerGPSTimeOut;
    public int CompetetiveContainerGridTimeOut;
    public int PersonalContainerGridTimeOut;
    public Color CompetetiveContainerGPSColorFree;
    public Color CompetetiveContainerGPSColorClaimed;
    public Color PersonalContainerGPSColor;
    public string ContainerAudioCue;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_ContainerDropSystemDefinition systemDefinition = (MyObjectBuilder_ContainerDropSystemDefinition) builder;
      this.PersonalContainerRatio = 0.9f;
      this.ContainerDropTime = (int) Math.Max(systemDefinition.ContainerDropTime * 60f, 1f);
      this.PersonalContainerDistMin = systemDefinition.PersonalContainerDistMin * 1000f;
      this.PersonalContainerDistMax = systemDefinition.PersonalContainerDistMax * 1000f;
      this.CompetetiveContainerDistMin = systemDefinition.CompetetiveContainerDistMin * 1000f;
      this.CompetetiveContainerDistMax = systemDefinition.CompetetiveContainerDistMax * 1000f;
      this.CompetetiveContainerGPSTimeOut = (int) systemDefinition.CompetetiveContainerGPSTimeOut * 60;
      this.CompetetiveContainerGridTimeOut = (int) systemDefinition.CompetetiveContainerGridTimeOut * 60;
      this.PersonalContainerGridTimeOut = (int) systemDefinition.PersonalContainerGridTimeOut * 60;
      this.CompetetiveContainerGPSColorFree = new Color(systemDefinition.CompetetiveContainerGPSColorFree.R, systemDefinition.CompetetiveContainerGPSColorFree.G, systemDefinition.CompetetiveContainerGPSColorFree.B);
      this.CompetetiveContainerGPSColorClaimed = new Color(systemDefinition.CompetetiveContainerGPSColorClaimed.R, systemDefinition.CompetetiveContainerGPSColorClaimed.G, systemDefinition.CompetetiveContainerGPSColorClaimed.B);
      this.PersonalContainerGPSColor = new Color(systemDefinition.PersonalContainerGPSColor.R, systemDefinition.PersonalContainerGPSColor.G, systemDefinition.PersonalContainerGPSColor.B);
      this.ContainerAudioCue = systemDefinition.ContainerAudioCue;
    }

    private class VRage_Game_Definitions_SessionComponents_MyContainerDropSystemDefinition\u003C\u003EActor : IActivator, IActivator<MyContainerDropSystemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyContainerDropSystemDefinition();

      MyContainerDropSystemDefinition IActivator<MyContainerDropSystemDefinition>.CreateInstance() => new MyContainerDropSystemDefinition();
    }
  }
}

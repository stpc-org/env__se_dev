// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyMedicalRoomDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_MedicalRoomDefinition), null)]
  public class MyMedicalRoomDefinition : MyCubeBlockDefinition
  {
    public string ResourceSinkGroup;
    public string IdleSound;
    public string ProgressSound;
    public string RespawnSuitName;
    public HashSet<string> CustomWardrobeNames;
    public bool RespawnAllowed;
    public bool HealingAllowed;
    public bool RefuelAllowed;
    public bool SuitChangeAllowed;
    public bool CustomWardrobesEnabled;
    public bool ForceSuitChangeOnRespawn;
    public bool SpawnWithoutOxygenEnabled;
    public Vector3D WardrobeCharacterOffset;
    public float WardrobeCharacterOffsetLength;
    public List<ScreenArea> ScreenAreas;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_MedicalRoomDefinition medicalRoomDefinition = builder as MyObjectBuilder_MedicalRoomDefinition;
      this.ResourceSinkGroup = medicalRoomDefinition.ResourceSinkGroup;
      this.IdleSound = medicalRoomDefinition.IdleSound;
      this.ProgressSound = medicalRoomDefinition.ProgressSound;
      this.RespawnSuitName = medicalRoomDefinition.RespawnSuitName;
      this.RespawnAllowed = medicalRoomDefinition.RespawnAllowed;
      this.HealingAllowed = medicalRoomDefinition.HealingAllowed;
      this.RefuelAllowed = medicalRoomDefinition.RefuelAllowed;
      this.SuitChangeAllowed = medicalRoomDefinition.SuitChangeAllowed;
      this.CustomWardrobesEnabled = medicalRoomDefinition.CustomWardrobesEnabled;
      this.ForceSuitChangeOnRespawn = medicalRoomDefinition.ForceSuitChangeOnRespawn;
      this.SpawnWithoutOxygenEnabled = medicalRoomDefinition.SpawnWithoutOxygenEnabled;
      this.WardrobeCharacterOffset = medicalRoomDefinition.WardrobeCharacterOffset;
      this.WardrobeCharacterOffsetLength = (float) this.WardrobeCharacterOffset.Length();
      this.CustomWardrobeNames = medicalRoomDefinition.CustomWardRobeNames != null ? new HashSet<string>((IEnumerable<string>) medicalRoomDefinition.CustomWardRobeNames) : new HashSet<string>();
      this.ScreenAreas = medicalRoomDefinition.ScreenAreas != null ? medicalRoomDefinition.ScreenAreas.ToList<ScreenArea>() : (List<ScreenArea>) null;
    }

    private class Sandbox_Definitions_MyMedicalRoomDefinition\u003C\u003EActor : IActivator, IActivator<MyMedicalRoomDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyMedicalRoomDefinition();

      MyMedicalRoomDefinition IActivator<MyMedicalRoomDefinition>.CreateInstance() => new MyMedicalRoomDefinition();
    }
  }
}

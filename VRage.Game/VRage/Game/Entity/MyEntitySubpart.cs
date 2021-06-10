// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.MyEntitySubpart
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.IO;
using VRage.Network;
using VRageMath;
using VRageRender.Import;

namespace VRage.Game.Entity
{
  public class MyEntitySubpart : MyEntity
  {
    public static bool GetSubpartFromDummy(
      string modelPath,
      string dummyName,
      MyModelDummy dummy,
      ref MyEntitySubpart.Data outData)
    {
      if (!dummyName.Contains("subpart_") || !dummy.CustomData.ContainsKey("file"))
        return false;
      string str = Path.Combine(Path.GetDirectoryName(modelPath), (string) dummy.CustomData["file"]) + ".mwm";
      outData = new MyEntitySubpart.Data()
      {
        Name = dummyName.Substring("subpart_".Length),
        File = str,
        InitialTransform = Matrix.Normalize(dummy.Matrix)
      };
      return true;
    }

    public MyEntitySubpart() => this.Save = false;

    public struct Data
    {
      public string Name;
      public string File;
      public Matrix InitialTransform;
    }

    private class VRage_Game_Entity_MyEntitySubpart\u003C\u003EActor : IActivator, IActivator<MyEntitySubpart>
    {
      object IActivator.CreateInstance() => (object) new MyEntitySubpart();

      MyEntitySubpart IActivator<MyEntitySubpart>.CreateInstance() => new MyEntitySubpart();
    }
  }
}

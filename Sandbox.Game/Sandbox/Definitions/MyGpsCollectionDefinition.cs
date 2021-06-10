// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyGpsCollectionDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GpsCollectionDefinition), null)]
  public class MyGpsCollectionDefinition : MyDefinitionBase
  {
    public List<MyGpsCollectionDefinition.MyGpsCoordinate> Positions;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_GpsCollectionDefinition collectionDefinition = builder as MyObjectBuilder_GpsCollectionDefinition;
      this.Positions = new List<MyGpsCollectionDefinition.MyGpsCoordinate>();
      if (collectionDefinition.Positions == null || collectionDefinition.Positions.Length == 0)
        return;
      StringBuilder name = new StringBuilder();
      Vector3D zero = Vector3D.Zero;
      StringBuilder additionalData = new StringBuilder();
      foreach (string position in collectionDefinition.Positions)
      {
        if (MyGpsCollection.ParseOneGPSExtended(position, name, ref zero, additionalData))
        {
          MyGpsCollectionDefinition.MyGpsCoordinate myGpsCoordinate = new MyGpsCollectionDefinition.MyGpsCoordinate()
          {
            Name = name.ToString(),
            Coords = zero
          };
          string str1 = additionalData.ToString();
          if (!string.IsNullOrWhiteSpace(str1))
          {
            string[] strArray = str1.Split(':');
            for (int index = 0; index < strArray.Length / 2; ++index)
            {
              string str2 = strArray[2 * index];
              string str3 = strArray[2 * index + 1];
              if (!string.IsNullOrWhiteSpace(str2) && !string.IsNullOrWhiteSpace(str3))
              {
                if (myGpsCoordinate.Actions == null)
                  myGpsCoordinate.Actions = new List<MyGpsCollectionDefinition.MyGpsAction>();
                myGpsCoordinate.Actions.Add(new MyGpsCollectionDefinition.MyGpsAction()
                {
                  BlockName = str2,
                  ActionId = str3
                });
              }
            }
          }
          this.Positions.Add(myGpsCoordinate);
        }
      }
    }

    public struct MyGpsAction
    {
      public string BlockName;
      public string ActionId;
    }

    public struct MyGpsCoordinate
    {
      public string Name;
      public Vector3D Coords;
      public List<MyGpsCollectionDefinition.MyGpsAction> Actions;
    }

    private class Sandbox_Definitions_MyGpsCollectionDefinition\u003C\u003EActor : IActivator, IActivator<MyGpsCollectionDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGpsCollectionDefinition();

      MyGpsCollectionDefinition IActivator<MyGpsCollectionDefinition>.CreateInstance() => new MyGpsCollectionDefinition();
    }
  }
}

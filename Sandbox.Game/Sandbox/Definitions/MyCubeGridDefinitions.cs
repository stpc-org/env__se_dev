// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyCubeGridDefinitions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [PreloadRequired]
  public static class MyCubeGridDefinitions
  {
    public static readonly Dictionary<MyStringId, Dictionary<Vector3I, MyTileDefinition>> TileGridOrientations = new Dictionary<MyStringId, Dictionary<Vector3I, MyTileDefinition>>()
    {
      {
        MyStringId.GetOrCompute("Square"),
        new Dictionary<Vector3I, MyTileDefinition>()
        {
          {
            Vector3I.Up,
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
              Normal = Vector3.Up,
              FullQuad = true
            }
          },
          {
            Vector3I.Forward,
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
              Normal = Vector3.Forward,
              FullQuad = true
            }
          },
          {
            Vector3I.Backward,
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
              Normal = Vector3.Backward,
              FullQuad = true
            }
          },
          {
            Vector3I.Down,
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
              Normal = Vector3.Down,
              FullQuad = true
            }
          },
          {
            Vector3I.Right,
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Right),
              Normal = Vector3.Right,
              FullQuad = true
            }
          },
          {
            Vector3I.Left,
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
              Normal = Vector3.Left,
              FullQuad = true
            }
          }
        }
      },
      {
        MyStringId.GetOrCompute("Slope"),
        new Dictionary<Vector3I, MyTileDefinition>()
        {
          {
            new Vector3I(0, 1, 1),
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.Identity,
              Normal = Vector3.Normalize(new Vector3(0.0f, 1f, 1f)),
              IsEmpty = true
            }
          },
          {
            new Vector3I(0, 1, -1),
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Up),
              Normal = Vector3.Normalize(new Vector3(0.0f, 1f, -1f)),
              IsEmpty = true
            }
          },
          {
            new Vector3I(-1, 1, 0),
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
              Normal = Vector3.Normalize(new Vector3(1f, 1f, 0.0f)),
              IsEmpty = true
            }
          },
          {
            new Vector3I(1, 1, 0),
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Up),
              Normal = Vector3.Normalize(new Vector3(-1f, 1f, 0.0f)),
              IsEmpty = true
            }
          },
          {
            new Vector3I(0, -1, 1),
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Down),
              Normal = Vector3.Normalize(new Vector3(0.0f, 1f, 1f)),
              IsEmpty = true
            }
          },
          {
            new Vector3I(0, -1, -1),
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Down),
              Normal = Vector3.Normalize(new Vector3(0.0f, 1f, -1f)),
              IsEmpty = true
            }
          },
          {
            new Vector3I(-1, -1, 0),
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Down),
              Normal = Vector3.Normalize(new Vector3(1f, 1f, 0.0f)),
              IsEmpty = true
            }
          },
          {
            new Vector3I(1, -1, 0),
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
              Normal = Vector3.Normalize(new Vector3(-1f, 1f, 0.0f)),
              IsEmpty = true
            }
          },
          {
            new Vector3I(-1, 0, -1),
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Forward),
              Normal = Vector3.Normalize(new Vector3(0.0f, 1f, 1f)),
              IsEmpty = true
            }
          },
          {
            new Vector3I(-1, 0, 1),
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Backward),
              Normal = Vector3.Normalize(new Vector3(0.0f, 1f, -1f)),
              IsEmpty = true
            }
          },
          {
            new Vector3I(1, 0, -1),
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Forward),
              Normal = Vector3.Normalize(new Vector3(1f, 1f, 0.0f)),
              IsEmpty = true
            }
          },
          {
            new Vector3I(1, 0, 1),
            new MyTileDefinition()
            {
              LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Backward),
              Normal = Vector3.Normalize(new Vector3(-1f, 1f, 0.0f)),
              IsEmpty = true
            }
          }
        }
      }
    };
    public static readonly Dictionary<Vector3I, MyEdgeOrientationInfo> EdgeOrientations = new Dictionary<Vector3I, MyEdgeOrientationInfo>((IEqualityComparer<Vector3I>) new Vector3INormalEqualityComparer())
    {
      {
        new Vector3I(0, 0, 1),
        new MyEdgeOrientationInfo(Matrix.Identity, MyCubeEdgeType.Horizontal)
      },
      {
        new Vector3I(0, 0, -1),
        new MyEdgeOrientationInfo(Matrix.Identity, MyCubeEdgeType.Horizontal)
      },
      {
        new Vector3I(1, 0, 0),
        new MyEdgeOrientationInfo(Matrix.CreateRotationY(1.570796f), MyCubeEdgeType.Horizontal)
      },
      {
        new Vector3I(-1, 0, 0),
        new MyEdgeOrientationInfo(Matrix.CreateRotationY(1.570796f), MyCubeEdgeType.Horizontal)
      },
      {
        new Vector3I(0, 1, 0),
        new MyEdgeOrientationInfo(Matrix.CreateRotationX(1.570796f), MyCubeEdgeType.Vertical)
      },
      {
        new Vector3I(0, -1, 0),
        new MyEdgeOrientationInfo(Matrix.CreateRotationX(1.570796f), MyCubeEdgeType.Vertical)
      },
      {
        new Vector3I(-1, 0, -1),
        new MyEdgeOrientationInfo(Matrix.CreateRotationZ(1.570796f), MyCubeEdgeType.Horizontal_Diagonal)
      },
      {
        new Vector3I(1, 0, 1),
        new MyEdgeOrientationInfo(Matrix.CreateRotationZ(1.570796f), MyCubeEdgeType.Horizontal_Diagonal)
      },
      {
        new Vector3I(-1, 0, 1),
        new MyEdgeOrientationInfo(Matrix.CreateRotationZ(-1.570796f), MyCubeEdgeType.Horizontal_Diagonal)
      },
      {
        new Vector3I(1, 0, -1),
        new MyEdgeOrientationInfo(Matrix.CreateRotationZ(-1.570796f), MyCubeEdgeType.Horizontal_Diagonal)
      },
      {
        new Vector3I(0, 1, -1),
        new MyEdgeOrientationInfo(Matrix.Identity, MyCubeEdgeType.Vertical_Diagonal)
      },
      {
        new Vector3I(0, -1, 1),
        new MyEdgeOrientationInfo(Matrix.Identity, MyCubeEdgeType.Vertical_Diagonal)
      },
      {
        new Vector3I(-1, -1, 0),
        new MyEdgeOrientationInfo(Matrix.CreateRotationY(-1.570796f), MyCubeEdgeType.Vertical_Diagonal)
      },
      {
        new Vector3I(0, -1, -1),
        new MyEdgeOrientationInfo(Matrix.CreateRotationX(1.570796f), MyCubeEdgeType.Vertical_Diagonal)
      },
      {
        new Vector3I(1, -1, 0),
        new MyEdgeOrientationInfo(Matrix.CreateRotationY(1.570796f), MyCubeEdgeType.Vertical_Diagonal)
      },
      {
        new Vector3I(-1, 1, 0),
        new MyEdgeOrientationInfo(Matrix.CreateRotationY(1.570796f), MyCubeEdgeType.Vertical_Diagonal)
      },
      {
        new Vector3I(1, 1, 0),
        new MyEdgeOrientationInfo(Matrix.CreateRotationY(-1.570796f), MyCubeEdgeType.Vertical_Diagonal)
      },
      {
        new Vector3I(0, 1, 1),
        new MyEdgeOrientationInfo(Matrix.CreateRotationX(1.570796f), MyCubeEdgeType.Vertical_Diagonal)
      }
    };
    private static MyCubeGridDefinitions.TableEntry[] m_tileTable = new MyCubeGridDefinitions.TableEntry[Enum.GetNames(typeof (MyCubeTopology)).Length];
    private static MatrixI[] m_allPossible90rotations;
    private static MatrixI[][] m_uniqueTopologyRotationTable;

    static MyCubeGridDefinitions()
    {
      MyCubeGridDefinitions.TableEntry[] tileTable1 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry1 = new MyCubeGridDefinitions.TableEntry();
      tableEntry1.RotationOptions = MyRotationOptionsEnum.None;
      tableEntry1.Tiles = new MyTileDefinition[6]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
          Normal = Vector3.Up,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
          Normal = Vector3.Forward,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
          Normal = Vector3.Backward,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
          Normal = Vector3.Down,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Right),
          Normal = Vector3.Right,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
          Normal = Vector3.Left,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        }
      };
      tableEntry1.Edges = new MyEdgeDefinition[12]
      {
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, 1f, -1f),
          Point1 = new Vector3(1f, 1f, -1f),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, 1f, -1f),
          Point1 = new Vector3(-1f, 1f, 1f),
          Side0 = 0,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, 1f, -1f),
          Point1 = new Vector3(1f, 1f, 1f),
          Side0 = 0,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, 1f, 1f),
          Point1 = new Vector3(1f, 1f, 1f),
          Side0 = 0,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 3,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 3,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, -1f),
          Point1 = new Vector3(1f, -1f, 1f),
          Side0 = 3,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(1f, -1f, 1f),
          Side0 = 3,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, 1f, -1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 1,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, 1f, -1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 1,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, 1f, 1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 5,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, 1f, 1f),
          Point1 = new Vector3(1f, -1f, 1f),
          Side0 = 4,
          Side1 = 2
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry2 = tableEntry1;
      tileTable1[0] = tableEntry2;
      MyCubeGridDefinitions.TableEntry[] tileTable2 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry3 = new MyCubeGridDefinitions.TableEntry();
      tableEntry3.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry3.Tiles = new MyTileDefinition[5]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(0.0f, 1f, 1f)),
          IsEmpty = true,
          Id = MyStringId.GetOrCompute("Slope")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Left,
          Up = new Vector3(0.0f, -1f, -1f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, -1f, -1f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(3.141593f),
          Normal = Vector3.Down,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
          Normal = Vector3.Forward,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        }
      };
      tableEntry3.Edges = new MyEdgeDefinition[9]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 0,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 0,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, 1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 0,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 3,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 1,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 2,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 1,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 2,
          Side1 = 4
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry4 = tableEntry3;
      tileTable2[1] = tableEntry4;
      MyCubeGridDefinitions.TableEntry[] tileTable3 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry5 = new MyCubeGridDefinitions.TableEntry();
      tableEntry5.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry5.Tiles = new MyTileDefinition[4]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationY(-1.570796f),
          Normal = Vector3.Forward,
          Up = new Vector3(1f, -1f, 0.0f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, -1f, -1f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(-1f, 1f, 1f)),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(1.570796f),
          Normal = Vector3.Down,
          Up = new Vector3(1f, 0.0f, -1f)
        }
      };
      tableEntry5.Edges = new MyEdgeDefinition[6]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 0,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 0,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 2,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 1,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 2,
          Side1 = 3
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry6 = tableEntry5;
      tileTable3[2] = tableEntry6;
      MyCubeGridDefinitions.TableEntry[] tileTable4 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry7 = new MyCubeGridDefinitions.TableEntry();
      tableEntry7.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry7.Tiles = new MyTileDefinition[7]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(1f, -1f, -1f)),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(3.141593f),
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, 1f, 1f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(3.141593f) * Matrix.CreateRotationY(-1.570796f),
          Normal = Vector3.Forward,
          Up = new Vector3(-1f, 1f, 0.0f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(-1.570796f) * Matrix.CreateRotationX(3.141593f),
          Normal = Vector3.Down,
          Up = new Vector3(-1f, 0.0f, 1f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Up,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(1.570796f),
          Normal = Vector3.Left,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(1.570796f),
          Normal = Vector3.Backward,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        }
      };
      tableEntry7.Edges = new MyEdgeDefinition[12]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 2,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 2,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 2,
          Side1 = 0
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, 1),
          Side0 = 4,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, 1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 6,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, 1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 0,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 6,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 5,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 5,
          Side1 = 6
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(-1, 1, -1),
          Side0 = 5,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(1, 1, 1),
          Side0 = 4,
          Side1 = 6
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry8 = tableEntry7;
      tileTable4[3] = tableEntry8;
      MyCubeGridDefinitions.TableEntry[] tileTable5 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry9 = new MyCubeGridDefinitions.TableEntry();
      tableEntry9.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry9.Tiles = new MyTileDefinition[6]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Right),
          Normal = Vector3.Right,
          FullQuad = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
          Normal = Vector3.Up,
          FullQuad = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
          Normal = Vector3.Forward,
          FullQuad = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Left),
          Normal = Vector3.Left,
          FullQuad = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
          Normal = Vector3.Down,
          FullQuad = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
          Normal = Vector3.Backward,
          FullQuad = true
        }
      };
      tableEntry9.Edges = new MyEdgeDefinition[12]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, 1, 1),
          Side0 = 0,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, 1),
          Side0 = 0,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(1, 1, 1),
          Side0 = 0,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 3,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 3,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 3,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, 1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 3,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 1,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 1,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 5,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, 1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 4,
          Side1 = 2
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry10 = tableEntry9;
      tileTable5[4] = tableEntry10;
      MyCubeGridDefinitions.TableEntry[] tileTable6 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry11 = new MyCubeGridDefinitions.TableEntry();
      tableEntry11.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry11.Tiles = new MyTileDefinition[6]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Right, Vector3.Up),
          Normal = Vector3.Up,
          FullQuad = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
          Normal = Vector3.Forward,
          FullQuad = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
          Normal = Vector3.Backward,
          FullQuad = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
          Normal = Vector3.Down,
          FullQuad = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Right),
          Normal = Vector3.Right,
          FullQuad = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
          Normal = Vector3.Left,
          FullQuad = true
        }
      };
      tableEntry11.Edges = new MyEdgeDefinition[12]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, 1, 1),
          Side0 = 0,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, 1),
          Side0 = 0,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(1, 1, 1),
          Side0 = 0,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 3,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 3,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 3,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, 1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 3,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 1,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 1,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 5,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, 1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 4,
          Side1 = 2
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry12 = tableEntry11;
      tileTable6[5] = tableEntry12;
      MyCubeGridDefinitions.TableEntry[] tileTable7 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry13 = new MyCubeGridDefinitions.TableEntry();
      tableEntry13.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry13.Tiles = new MyTileDefinition[5]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(0.0f, 1f, 1f)),
          IsEmpty = true,
          Id = MyStringId.GetOrCompute("Slope")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Left,
          Up = new Vector3(0.0f, -1f, -1f),
          IsRounded = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, -1f, -1f),
          IsRounded = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(3.141593f),
          Normal = Vector3.Down,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
          Normal = Vector3.Forward,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        }
      };
      tableEntry13.Edges = new MyEdgeDefinition[7]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 0,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, 1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 0,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 3,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 1,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 2,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 1,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 2,
          Side1 = 4
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry14 = tableEntry13;
      tileTable7[6] = tableEntry14;
      MyCubeGridDefinitions.TableEntry[] tileTable8 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry15 = new MyCubeGridDefinitions.TableEntry();
      tableEntry15.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry15.Tiles = new MyTileDefinition[4]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationY(-1.570796f),
          Normal = Vector3.Forward,
          Up = new Vector3(1f, -1f, 0.0f),
          IsRounded = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, -1f, -1f),
          IsRounded = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(-1f, 1f, 1f)),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(1.570796f),
          Normal = Vector3.Down,
          Up = new Vector3(1f, 0.0f, -1f),
          IsRounded = true
        }
      };
      tableEntry15.Edges = new MyEdgeDefinition[3]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 0,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 1,
          Side1 = 3
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry16 = tableEntry15;
      tileTable8[7] = tableEntry16;
      MyCubeGridDefinitions.TableEntry[] tileTable9 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry17 = new MyCubeGridDefinitions.TableEntry();
      tableEntry17.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry17.Tiles = new MyTileDefinition[7]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(-1.570796f) * Matrix.CreateRotationX(3.141593f),
          Normal = Vector3.Down,
          Up = new Vector3(-1f, 0.0f, 1f),
          IsRounded = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(3.141593f),
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, 1f, 1f),
          IsRounded = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(3.141593f) * Matrix.CreateRotationY(-1.570796f),
          Normal = Vector3.Forward,
          Up = new Vector3(-1f, 1f, 0.0f),
          IsRounded = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Up,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(1.570796f),
          Normal = Vector3.Left,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(1.570796f),
          Normal = Vector3.Backward,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(1f, -1f, -1f)),
          IsEmpty = true
        }
      };
      tableEntry17.Edges = new MyEdgeDefinition[9]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 2,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 2,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, 1),
          Side0 = 3,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, 1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 5,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 5,
          Side1 = 0
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 4,
          Side1 = 0
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 4,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(-1, 1, -1),
          Side0 = 4,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(1, 1, 1),
          Side0 = 3,
          Side1 = 5
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry18 = tableEntry17;
      tileTable9[8] = tableEntry18;
      MyCubeGridDefinitions.TableEntry[] tileTable10 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry19 = new MyCubeGridDefinitions.TableEntry();
      tableEntry19.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry19.Tiles = new MyTileDefinition[5]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(0.0f, 1f, 1f)),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Left,
          Up = new Vector3(0.0f, -1f, -1f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, -1f, -1f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(3.141593f),
          Normal = Vector3.Down,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
          Normal = Vector3.Forward,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        }
      };
      tableEntry19.Edges = new MyEdgeDefinition[9]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 0,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 0,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, 1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 0,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 3,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 1,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 2,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 1,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 2,
          Side1 = 4
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry20 = tableEntry19;
      tileTable10[9] = tableEntry20;
      MyCubeGridDefinitions.TableEntry[] tileTable11 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry21 = new MyCubeGridDefinitions.TableEntry();
      tableEntry21.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry21.Tiles = new MyTileDefinition[4]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationY(-1.570796f),
          Normal = Vector3.Forward,
          Up = new Vector3(1f, -1f, 0.0f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, -1f, -1f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(-1f, 1f, 1f)),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(1.570796f),
          Normal = Vector3.Down,
          Up = new Vector3(1f, 0.0f, -1f)
        }
      };
      tableEntry21.Edges = new MyEdgeDefinition[6]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 0,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 0,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 2,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 1,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 2,
          Side1 = 3
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry22 = tableEntry21;
      tileTable11[10] = tableEntry22;
      MyCubeGridDefinitions.TableEntry[] tileTable12 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry23 = new MyCubeGridDefinitions.TableEntry();
      tableEntry23.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry23.Tiles = new MyTileDefinition[6]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up),
          Normal = Vector3.Normalize(new Vector3(0.0f, 2f, 1f)),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
          Normal = Vector3.Forward,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
          Normal = Vector3.Backward
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Left, Vector3.Down),
          Normal = Vector3.Down,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Backward, Vector3.Right),
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, -2f, -1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
          Normal = Vector3.Left,
          Up = new Vector3(0.0f, -2f, -1f),
          DontOffsetTexture = true
        }
      };
      tableEntry23.Edges = new MyEdgeDefinition[7]
      {
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, 1f, -1f),
          Point1 = new Vector3(1f, 1f, -1f),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 3,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 3,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 3,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, 1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 3,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 1,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 1,
          Side1 = 4
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry24 = tableEntry23;
      tileTable12[11] = tableEntry24;
      MyCubeGridDefinitions.TableEntry[] tileTable13 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry25 = new MyCubeGridDefinitions.TableEntry();
      tableEntry25.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry25.Tiles = new MyTileDefinition[5]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(0.0f, 2f, 1f)),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Left,
          Up = new Vector3(0.0f, -2f, -1f),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, -2f, -1f),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(3.141593f),
          Normal = Vector3.Down,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
          Normal = Vector3.Forward
        }
      };
      tableEntry25.Edges = new MyEdgeDefinition[4]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, 1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 0,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 3,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 1,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 2,
          Side1 = 3
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry26 = tableEntry25;
      tileTable13[12] = tableEntry26;
      MyCubeGridDefinitions.TableEntry[] tileTable14 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry27 = new MyCubeGridDefinitions.TableEntry();
      tableEntry27.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry27.Tiles = new MyTileDefinition[5]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(2f, 1f, 1f)),
          IsEmpty = true,
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Left,
          Up = new Vector3(0.0f, 1f, 1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, -1f, 1f),
          IsEmpty = true,
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(3.141593f),
          Normal = Vector3.Down,
          Up = new Vector3(-2f, 0.0f, 1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
          Normal = Vector3.Forward,
          Up = new Vector3(-2f, 1f, 0.0f),
          DontOffsetTexture = true
        }
      };
      tableEntry27.Edges = new MyEdgeDefinition[4]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 3,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 1,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 1,
          Side1 = 4
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry28 = tableEntry27;
      tileTable14[13] = tableEntry28;
      MyCubeGridDefinitions.TableEntry[] tileTable15 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry29 = new MyCubeGridDefinitions.TableEntry();
      tableEntry29.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry29.Tiles = new MyTileDefinition[4]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationY(-1.570796f),
          Normal = Vector3.Forward,
          Up = new Vector3(1f, -2f, 0.0f),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, -2f, -1f),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(-1f, 2f, 1f)),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(1.570796f),
          Normal = Vector3.Down,
          Up = new Vector3(1f, 0.0f, -1f),
          IsEmpty = true
        }
      };
      tableEntry29.Edges = new MyEdgeDefinition[1]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 1,
          Side1 = 3
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry30 = tableEntry29;
      tileTable15[14] = tableEntry30;
      MyCubeGridDefinitions.TableEntry[] tileTable16 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry31 = new MyCubeGridDefinitions.TableEntry();
      tableEntry31.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry31.Tiles = new MyTileDefinition[7]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(2f, -2f, -1f)),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(3.141593f),
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, -1f, 2f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(3.141593f) * Matrix.CreateRotationY(-1.570796f),
          Normal = Vector3.Forward,
          Up = new Vector3(2f, 0.0f, -1f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(-1.570796f) * Matrix.CreateRotationX(3.141593f),
          Normal = Vector3.Down,
          Up = new Vector3(1f, 0.0f, 2f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Up,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(1.570796f),
          Normal = Vector3.Left,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(1.570796f),
          Normal = Vector3.Backward,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        }
      };
      tableEntry31.Edges = new MyEdgeDefinition[9]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 2,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 2,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, 1),
          Side0 = 4,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, 1),
          Point1 = (Vector3) new Vector3I(1, -1, 1),
          Side0 = 6,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 6,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 5,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 5,
          Side1 = 6
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(-1, 1, -1),
          Side0 = 5,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(1, 1, 1),
          Side0 = 4,
          Side1 = 6
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry32 = tableEntry31;
      tileTable16[15] = tableEntry32;
      MyCubeGridDefinitions.TableEntry[] tileTable17 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry33 = new MyCubeGridDefinitions.TableEntry();
      tableEntry33.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry33.Tiles = new MyTileDefinition[7]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(2f, -2f, -1f)),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(3.141593f),
          Normal = Vector3.Right,
          Up = new Vector3(0.0f, 1f, 1f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(3.141593f) * Matrix.CreateRotationY(-1.570796f),
          Normal = Vector3.Forward,
          Up = new Vector3(0.0f, -2f, -1f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(-1.570796f) * Matrix.CreateRotationX(3.141593f),
          Normal = Vector3.Down,
          Up = new Vector3(2f, 0.0f, -1f)
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Up,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(1.570796f),
          Normal = Vector3.Left,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(1.570796f),
          Normal = Vector3.Backward
        }
      };
      tableEntry33.Edges = new MyEdgeDefinition[7]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 2,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 2,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, 1),
          Side0 = 4,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, -1),
          Side0 = 5,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(-1, -1, 1),
          Side0 = 5,
          Side1 = 6
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(-1, 1, -1),
          Side0 = 5,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, 1),
          Point1 = (Vector3) new Vector3I(1, 1, 1),
          Side0 = 4,
          Side1 = 6
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry34 = tableEntry33;
      tileTable17[16] = tableEntry34;
      MyCubeGridDefinitions.TableEntry[] tileTable18 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry35 = new MyCubeGridDefinitions.TableEntry();
      tableEntry35.RotationOptions = MyRotationOptionsEnum.Horizontal;
      tableEntry35.Tiles = new MyTileDefinition[6]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Right),
          Normal = Vector3.Right,
          FullQuad = false
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Backward),
          Normal = Vector3.Backward,
          FullQuad = false,
          IsEmpty = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up),
          Normal = Vector3.Up,
          FullQuad = false
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Left),
          Normal = Vector3.Left,
          FullQuad = false
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Down, Vector3.Forward),
          Normal = Vector3.Forward,
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Down),
          Normal = Vector3.Down,
          FullQuad = false
        }
      };
      tableEntry35.Edges = new MyEdgeDefinition[4]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 4,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(-1, 1, -1),
          Side0 = 4,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 4,
          Side1 = 0
        },
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, 1, -1),
          Point1 = (Vector3) new Vector3I(1, 1, -1),
          Side0 = 4,
          Side1 = 2
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry36 = tableEntry35;
      tileTable18[17] = tableEntry36;
      MyCubeGridDefinitions.TableEntry[] tileTable19 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry37 = new MyCubeGridDefinitions.TableEntry();
      tableEntry37.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry37.Tiles = new MyTileDefinition[5]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(-1.570796f),
          Normal = Vector3.Forward,
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Normalize(new Vector3(0.0f, 1f, 1f)),
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = Vector3.Left,
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationZ(3.141593f),
          Normal = Vector3.Down,
          IsEmpty = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateRotationX(-1.570796f) * Matrix.CreateRotationY(3.141593f),
          Normal = Vector3.Right,
          IsEmpty = true
        }
      };
      tableEntry37.Edges = new MyEdgeDefinition[1]
      {
        new MyEdgeDefinition()
        {
          Point0 = (Vector3) new Vector3I(-1, -1, -1),
          Point1 = (Vector3) new Vector3I(1, -1, -1),
          Side0 = 3,
          Side1 = 0
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry38 = tableEntry37;
      tileTable19[18] = tableEntry38;
      float angle1 = 2.094395f;
      float angle2 = 4.188791f;
      MyCubeGridDefinitions.TableEntry[] tileTable20 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry39 = new MyCubeGridDefinitions.TableEntry();
      tableEntry39.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry39.Tiles = new MyTileDefinition[5]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(-1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 1f, 0.0f), 1.570796f),
          Normal = new Vector3(0.0f, 0.0f, -1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.0, 0.70711, 0.70711),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.70711, 0.70711, 0.0),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), 3.141593f),
          Normal = new Vector3(0.0f, -1f, 0.0f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        }
      };
      tableEntry39.Edges = new MyEdgeDefinition[7]
      {
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(-1f, 1f, -1f),
          Side0 = 1,
          Side1 = 0
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, -1f),
          Point1 = new Vector3(-1f, 1f, -1f),
          Side0 = 1,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(-1f, 1f, -1f),
          Side0 = 0,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, 1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 4,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, 1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 4,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, -1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 4,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 4,
          Side1 = 0
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry40 = tableEntry39;
      tileTable20[27] = tableEntry40;
      MyCubeGridDefinitions.TableEntry[] tileTable21 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry41 = new MyCubeGridDefinitions.TableEntry();
      tableEntry41.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry41.Tiles = new MyTileDefinition[7]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.70711, 0.70711, 0.0),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.0, 0.70711, 0.70711),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 1f, 0.0f), 1.570796f),
          Normal = new Vector3(0.0f, 0.0f, 1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), 1.570796f),
          Normal = new Vector3(-1f, 0.0f, 0.0f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, 0.57735), angle1),
          Normal = new Vector3(0.0f, 0.0f, -1f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.707107, 0.0, 0.707107), 3.141593f),
          Normal = new Vector3(0.0f, -1f, 0.0f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        }
      };
      tableEntry41.Edges = new MyEdgeDefinition[9]
      {
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, -1f),
          Point1 = new Vector3(1f, 1f, -1f),
          Side0 = 3,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(-1f, 1f, 1f),
          Side0 = 4,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(1f, -1f, 1f),
          Side0 = 6,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 6,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 6,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, 1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 6,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(-1f, 1f, -1f),
          Side0 = 5,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, 1f, 1f),
          Point1 = new Vector3(-1f, 1f, -1f),
          Side0 = 0,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, 1f, -1f),
          Point1 = new Vector3(1f, 1f, -1f),
          Side0 = 1,
          Side1 = 5
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry42 = tableEntry41;
      tileTable21[28] = tableEntry42;
      MyCubeGridDefinitions.TableEntry[] tileTable22 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry43 = new MyCubeGridDefinitions.TableEntry();
      tableEntry43.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry43.Tiles = new MyTileDefinition[5]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, -0.57735), angle2),
          Normal = new Vector3(0.0f, -1f, 0.0f),
          IsRounded = true,
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.70711, 0.0, 0.70711),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.0f, 1f, 0.0f),
          IsRounded = true,
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-1f, 0.0f, 0.0f), 1.570796f),
          Normal = new Vector3(0.0f, 0.0f, -1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, 0.57735, 0.57735), angle1),
          Normal = new Vector3(-1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        }
      };
      tableEntry43.Edges = new MyEdgeDefinition[3]
      {
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 0,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, -1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 0,
          Side1 = 3
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry44 = tableEntry43;
      tileTable22[26] = tableEntry44;
      MyCubeGridDefinitions.TableEntry[] tileTable23 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry45 = new MyCubeGridDefinitions.TableEntry();
      tableEntry45.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry45.Tiles = new MyTileDefinition[4]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, -1f, 0.0f), 3.141593f),
          Normal = new Vector3(1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, 0.57735, 0.57735), angle1),
          Normal = new Vector3(0.0f, 0.0f, 1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, 0.57735), angle1),
          Normal = new Vector3(0.0f, -1f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(-0.57735, 0.57735, -0.57735),
          DontOffsetTexture = true
        }
      };
      tableEntry45.Edges = new MyEdgeDefinition[0];
      MyCubeGridDefinitions.TableEntry tableEntry46 = tableEntry45;
      tileTable23[20] = tableEntry46;
      MyCubeGridDefinitions.TableEntry[] tileTable24 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry47 = new MyCubeGridDefinitions.TableEntry();
      tableEntry47.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry47.Tiles = new MyTileDefinition[7]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(-0.57735, 0.57735, -0.57735),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, -1f, 0.0f), 3.141593f),
          Normal = new Vector3(0.0f, 0.0f, -1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, 0.57735, 0.57735), angle1),
          Normal = new Vector3(0.0f, 1f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 1f, 0.0f), 1.570796f),
          Normal = new Vector3(-1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), 3.141593f),
          Normal = new Vector3(0.0f, -1f, 0.0f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, -1f), 1.570796f),
          Normal = new Vector3(1f, 0.0f, 0.0f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.57735, -0.57735, -0.57735), angle1),
          Normal = new Vector3(0.0f, 0.0f, 1f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        }
      };
      tableEntry47.Edges = new MyEdgeDefinition[9]
      {
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, -1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 4,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, 1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 4,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, 1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 4,
          Side1 = 6
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 4,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, 1f, -1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 5,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, 1f, 1f),
          Point1 = new Vector3(1f, -1f, 1f),
          Side0 = 5,
          Side1 = 6
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, 1f, 1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 6,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, 1f, 1f),
          Point1 = new Vector3(-1f, 1f, 1f),
          Side0 = 2,
          Side1 = 6
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, 1f, 1f),
          Point1 = new Vector3(1f, 1f, -1f),
          Side0 = 2,
          Side1 = 5
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry48 = tableEntry47;
      tileTable24[21] = tableEntry48;
      MyCubeGridDefinitions.TableEntry[] tileTable25 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry49 = new MyCubeGridDefinitions.TableEntry();
      tableEntry49.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry49.Tiles = new MyTileDefinition[5]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, -0.57735), angle2),
          Normal = new Vector3(0.0f, -1f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.70711, 0.0, 0.70711),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.40825, 0.8165, 0.40825),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), 1.570796f),
          Normal = new Vector3(-1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, 0.57735), angle1),
          Normal = new Vector3(0.0f, 0.0f, -1f),
          DontOffsetTexture = true
        }
      };
      tableEntry49.Edges = new MyEdgeDefinition[4]
      {
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(-1f, 1f, -1f),
          Side0 = 4,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 0,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 0,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, -1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 0,
          Side1 = 1
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry50 = tableEntry49;
      tileTable25[29] = tableEntry50;
      MyCubeGridDefinitions.TableEntry[] tileTable26 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry51 = new MyCubeGridDefinitions.TableEntry();
      tableEntry51.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry51.Tiles = new MyTileDefinition[7]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.0f, 1f, 0.0f),
          IsRounded = true,
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.40825, 0.8165, 0.40825),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.0f, 0.0f, 1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, -1f, 0.0f), 3.141593f),
          Normal = new Vector3(1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), 3.141593f),
          Normal = new Vector3(0.0f, -1f, 0.0f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-1f, 0.0f, 0.0f), 1.570796f),
          Normal = new Vector3(0.0f, 0.0f, -1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, 0.57735, 0.57735), angle1),
          Normal = new Vector3(-1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        }
      };
      tableEntry51.Edges = new MyEdgeDefinition[4]
      {
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, 1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 4,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, -1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 4,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 4,
          Side1 = 6
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, 1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 4,
          Side1 = 2
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry52 = tableEntry51;
      tileTable26[25] = tableEntry52;
      MyCubeGridDefinitions.TableEntry[] tileTable27 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry53 = new MyCubeGridDefinitions.TableEntry();
      tableEntry53.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry53.Tiles = new MyTileDefinition[7]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.70711, 0.70711, 0.0),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.0f, 0.0f, 1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.0f, 0.0f, -1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), 3.141593f),
          Normal = new Vector3(0.0f, -1f, 0.0f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), 1.570796f),
          Normal = new Vector3(-1f, 0.0f, 0.0f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 1f, 0.0f), 1.570796f),
          Normal = new Vector3(0.0f, 1f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, -0.57735), angle1),
          Normal = new Vector3(1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        }
      };
      tableEntry53.Edges = new MyEdgeDefinition[7]
      {
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(-1f, 1f, 1f),
          Side0 = 4,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(-1f, 1f, -1f),
          Side0 = 4,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, 1f, 1f),
          Point1 = new Vector3(-1f, 1f, -1f),
          Side0 = 4,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, 1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 3,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 3,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, -1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 3,
          Side1 = 2
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, 1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 3,
          Side1 = 6
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry54 = tableEntry53;
      tileTable27[19] = tableEntry54;
      MyCubeGridDefinitions.TableEntry[] tileTable28 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry55 = new MyCubeGridDefinitions.TableEntry();
      tableEntry55.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry55.Tiles = new MyTileDefinition[6]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.0f, 0.0f, 1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, -1f, 0.0f), 3.141593f),
          Normal = new Vector3(1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.40825, 0.8165, 0.40825),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), 3.141593f),
          Normal = new Vector3(0.0f, -1f, 0.0f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), 1.570796f),
          Normal = new Vector3(-1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, 0.57735), angle1),
          Normal = new Vector3(0.0f, 0.0f, -1f),
          DontOffsetTexture = true
        }
      };
      tableEntry55.Edges = new MyEdgeDefinition[5]
      {
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, 1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 3,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, -1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 3,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 3,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, 1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 3,
          Side1 = 0
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(-1f, 1f, -1f),
          Side0 = 5,
          Side1 = 4
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry56 = tableEntry55;
      tileTable28[24] = tableEntry56;
      MyCubeGridDefinitions.TableEntry[] tileTable29 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry57 = new MyCubeGridDefinitions.TableEntry();
      tableEntry57.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry57.Tiles = new MyTileDefinition[7]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.57735, -0.57735, -0.57735), angle1),
          Normal = new Vector3(0.0f, 1f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, -1f, 0.0f), 1.570796f),
          Normal = new Vector3(-0.40825, 0.8165, 0.40825),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-1f, 0.0f, 0.0f), 3.141593f),
          Normal = new Vector3(0.0f, -1f, 0.0f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-1f, 0.0f, 0.0f), 1.570796f),
          Normal = new Vector3(0.0f, 0.0f, -1f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, -0.57735), angle1),
          Normal = new Vector3(1f, 0.0f, 0.0f),
          FullQuad = true,
          Id = MyStringId.GetOrCompute("Square")
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(0.0f, 0.0f, 1f), 1.570796f),
          Normal = new Vector3(-1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.57735, -0.57735, -0.57735), angle2),
          Normal = new Vector3(0.0f, 0.0f, 1f),
          DontOffsetTexture = true
        }
      };
      tableEntry57.Edges = new MyEdgeDefinition[9]
      {
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, 1f, 1f),
          Point1 = new Vector3(1f, -1f, 1f),
          Side0 = 4,
          Side1 = 6
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, 1f, -1f),
          Point1 = new Vector3(1f, 1f, 1f),
          Side0 = 4,
          Side1 = 0
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(-1f, 1f, -1f),
          Side0 = 5,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, 1f, -1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 3,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, 1f, -1f),
          Point1 = new Vector3(1f, 1f, -1f),
          Side0 = 3,
          Side1 = 0
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(-1f, -1f, 1f),
          Side0 = 2,
          Side1 = 5
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(1f, -1f, 1f),
          Side0 = 2,
          Side1 = 6
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(1f, -1f, -1f),
          Point1 = new Vector3(1f, -1f, 1f),
          Side0 = 2,
          Side1 = 4
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(1f, -1f, -1f),
          Side0 = 2,
          Side1 = 3
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry58 = tableEntry57;
      tileTable29[23] = tableEntry58;
      MyCubeGridDefinitions.TableEntry[] tileTable30 = MyCubeGridDefinitions.m_tileTable;
      MyCubeGridDefinitions.TableEntry tableEntry59 = new MyCubeGridDefinitions.TableEntry();
      tableEntry59.RotationOptions = MyRotationOptionsEnum.Both;
      tableEntry59.Tiles = new MyTileDefinition[4]
      {
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.CreateFromAxisAngle(new Vector3(-0.707107, -0.707107, 0.0), 3.141593f),
          Normal = new Vector3(0.0f, -1f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.0f, 0.0f, 1f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(-1f, 0.0f, 0.0f),
          DontOffsetTexture = true
        },
        new MyTileDefinition()
        {
          LocalMatrix = Matrix.Identity,
          Normal = new Vector3(0.40825, 0.8165, -0.40825),
          DontOffsetTexture = true
        }
      };
      tableEntry59.Edges = new MyEdgeDefinition[3]
      {
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, -1f),
          Point1 = new Vector3(1f, -1f, 1f),
          Side0 = 0,
          Side1 = 3
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(1f, -1f, 1f),
          Side0 = 0,
          Side1 = 1
        },
        new MyEdgeDefinition()
        {
          Point0 = new Vector3(-1f, -1f, 1f),
          Point1 = new Vector3(-1f, -1f, -1f),
          Side0 = 0,
          Side1 = 2
        }
      };
      MyCubeGridDefinitions.TableEntry tableEntry60 = tableEntry59;
      tileTable30[22] = tableEntry60;
      MyCubeGridDefinitions.InitTopologyUniqueRotationsMatrices();
    }

    public static MyTileDefinition[] GetCubeTiles(MyCubeBlockDefinition block) => block.CubeDefinition == null ? (MyTileDefinition[]) null : MyCubeGridDefinitions.m_tileTable[(int) block.CubeDefinition.CubeTopology].Tiles;

    public static MyCubeGridDefinitions.TableEntry GetTopologyInfo(
      MyCubeTopology topology)
    {
      return MyCubeGridDefinitions.m_tileTable[(int) topology];
    }

    public static MyRotationOptionsEnum GetCubeRotationOptions(
      MyCubeBlockDefinition block)
    {
      return block.CubeDefinition == null ? MyRotationOptionsEnum.Both : MyCubeGridDefinitions.m_tileTable[(int) block.CubeDefinition.CubeTopology].RotationOptions;
    }

    public static void GetRotatedBlockSize(
      MyCubeBlockDefinition block,
      ref Matrix rotation,
      out Vector3I size)
    {
      Vector3I.TransformNormal(ref block.Size, ref rotation, out size);
    }

    private static void InitTopologyUniqueRotationsMatrices()
    {
      MyCubeGridDefinitions.m_allPossible90rotations = new MatrixI[24]
      {
        new MatrixI(Base6Directions.Direction.Forward, Base6Directions.Direction.Up),
        new MatrixI(Base6Directions.Direction.Down, Base6Directions.Direction.Forward),
        new MatrixI(Base6Directions.Direction.Backward, Base6Directions.Direction.Down),
        new MatrixI(Base6Directions.Direction.Up, Base6Directions.Direction.Backward),
        new MatrixI(Base6Directions.Direction.Forward, Base6Directions.Direction.Right),
        new MatrixI(Base6Directions.Direction.Down, Base6Directions.Direction.Right),
        new MatrixI(Base6Directions.Direction.Backward, Base6Directions.Direction.Right),
        new MatrixI(Base6Directions.Direction.Up, Base6Directions.Direction.Right),
        new MatrixI(Base6Directions.Direction.Forward, Base6Directions.Direction.Down),
        new MatrixI(Base6Directions.Direction.Up, Base6Directions.Direction.Forward),
        new MatrixI(Base6Directions.Direction.Backward, Base6Directions.Direction.Up),
        new MatrixI(Base6Directions.Direction.Down, Base6Directions.Direction.Backward),
        new MatrixI(Base6Directions.Direction.Forward, Base6Directions.Direction.Left),
        new MatrixI(Base6Directions.Direction.Up, Base6Directions.Direction.Left),
        new MatrixI(Base6Directions.Direction.Backward, Base6Directions.Direction.Left),
        new MatrixI(Base6Directions.Direction.Down, Base6Directions.Direction.Left),
        new MatrixI(Base6Directions.Direction.Left, Base6Directions.Direction.Up),
        new MatrixI(Base6Directions.Direction.Left, Base6Directions.Direction.Backward),
        new MatrixI(Base6Directions.Direction.Left, Base6Directions.Direction.Down),
        new MatrixI(Base6Directions.Direction.Left, Base6Directions.Direction.Forward),
        new MatrixI(Base6Directions.Direction.Right, Base6Directions.Direction.Down),
        new MatrixI(Base6Directions.Direction.Right, Base6Directions.Direction.Backward),
        new MatrixI(Base6Directions.Direction.Right, Base6Directions.Direction.Up),
        new MatrixI(Base6Directions.Direction.Right, Base6Directions.Direction.Forward)
      };
      MyCubeGridDefinitions.m_uniqueTopologyRotationTable = new MatrixI[Enum.GetValues(typeof (MyCubeTopology)).Length][];
      MyCubeGridDefinitions.m_uniqueTopologyRotationTable[0] = (MatrixI[]) null;
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.Slope, 0);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.Corner, 2);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.InvCorner, 0);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.StandaloneBox, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.RoundedSlope, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.RoundSlope, 0);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.RoundCorner, 2);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.RoundInvCorner, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.RotatedSlope, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.RotatedCorner, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.HalfBox, 1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.Slope2Base, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.Slope2Tip, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.Corner2Base, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.Corner2Tip, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.InvCorner2Base, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.InvCorner2Tip, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.HalfSlopeBox, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.HalfSlopeInverted, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.HalfSlopeCorner, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.SlopedCornerTip, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.SlopedCornerBase, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.SlopedCorner, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.HalfSlopedCornerBase, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.HalfCorner, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.CornerSquare, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.CornerSquareInverted, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.HalfSlopedCorner, -1);
      MyCubeGridDefinitions.FillRotationsForTopology(MyCubeTopology.HalfSlopeCornerInverted, -1);
    }

    private static void FillRotationsForTopology(MyCubeTopology topology, int mainTile)
    {
      Vector3[] vector3Array = new Vector3[MyCubeGridDefinitions.m_allPossible90rotations.Length];
      MyCubeGridDefinitions.m_uniqueTopologyRotationTable[(int) topology] = new MatrixI[MyCubeGridDefinitions.m_allPossible90rotations.Length];
      for (int index1 = 0; index1 < MyCubeGridDefinitions.m_allPossible90rotations.Length; ++index1)
      {
        int index2 = -1;
        if (mainTile != -1)
        {
          Vector3 result;
          Vector3.TransformNormal(ref MyCubeGridDefinitions.m_tileTable[(int) topology].Tiles[mainTile].Normal, ref MyCubeGridDefinitions.m_allPossible90rotations[index1], out result);
          vector3Array[index1] = result;
          for (int index3 = 0; index3 < index1; ++index3)
          {
            if ((double) Vector3.Dot(vector3Array[index3], result) > 0.980000019073486)
            {
              index2 = index3;
              break;
            }
          }
        }
        MyCubeGridDefinitions.m_uniqueTopologyRotationTable[(int) topology][index1] = index2 == -1 ? MyCubeGridDefinitions.m_allPossible90rotations[index1] : MyCubeGridDefinitions.m_uniqueTopologyRotationTable[(int) topology][index2];
      }
    }

    public static MyBlockOrientation GetTopologyUniqueOrientation(
      MyCubeTopology myCubeTopology,
      MyBlockOrientation orientation)
    {
      if (MyCubeGridDefinitions.m_uniqueTopologyRotationTable[(int) myCubeTopology] == null)
        return MyBlockOrientation.Identity;
      for (int index = 0; index < MyCubeGridDefinitions.m_allPossible90rotations.Length; ++index)
      {
        MatrixI possible90rotation = MyCubeGridDefinitions.m_allPossible90rotations[index];
        if (possible90rotation.Forward == orientation.Forward && possible90rotation.Up == orientation.Up)
          return MyCubeGridDefinitions.m_uniqueTopologyRotationTable[(int) myCubeTopology][index].GetBlockOrientation();
      }
      return MyBlockOrientation.Identity;
    }

    public static MatrixI[] AllPossible90rotations => MyCubeGridDefinitions.m_allPossible90rotations;

    public class TableEntry
    {
      public MyRotationOptionsEnum RotationOptions;
      public MyTileDefinition[] Tiles;
      public MyEdgeDefinition[] Edges;
    }
  }
}

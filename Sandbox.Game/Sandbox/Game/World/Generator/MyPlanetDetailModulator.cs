// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyPlanetDetailModulator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using System.Collections.Generic;
using VRage.Game;
using VRage.Noise;

namespace Sandbox.Game.World.Generator
{
  internal class MyPlanetDetailModulator : IMyModule
  {
    private MyPlanetGeneratorDefinition m_planetDefinition;
    private MyPlanetMaterialProvider m_oreDeposit;
    private float m_radius;
    private Dictionary<byte, MyPlanetDetailModulator.MyModulatorData> m_modulators = new Dictionary<byte, MyPlanetDetailModulator.MyModulatorData>();

    public MyPlanetDetailModulator(
      MyPlanetGeneratorDefinition planetDefinition,
      MyPlanetMaterialProvider oreDeposit,
      int seed,
      float radius)
    {
      this.m_planetDefinition = planetDefinition;
      this.m_oreDeposit = oreDeposit;
      this.m_radius = radius;
      foreach (MyPlanetDistortionDefinition distortionDefinition in this.m_planetDefinition.DistortionTable)
      {
        MyModuleFast myModuleFast = (MyModuleFast) null;
        float num = distortionDefinition.Frequency * (radius / 6f);
        string type = distortionDefinition.Type;
        if (!(type == "Billow"))
        {
          if (!(type == "RidgedMultifractal"))
          {
            if (!(type == "Perlin"))
            {
              if (type == "Simplex")
              {
                MySimplexFast mySimplexFast = new MySimplexFast();
                mySimplexFast.Seed = seed;
                mySimplexFast.Frequency = (double) num;
                myModuleFast = (MyModuleFast) mySimplexFast;
              }
            }
            else
            {
              int seed1 = seed;
              double frequency = (double) num;
              myModuleFast = (MyModuleFast) new MyPerlinFast(MyNoiseQuality.High, distortionDefinition.LayerCount, seed1, frequency);
            }
          }
          else
          {
            int seed1 = seed;
            double frequency = (double) num;
            myModuleFast = (MyModuleFast) new MyRidgedMultifractalFast(MyNoiseQuality.High, distortionDefinition.LayerCount, seed1, frequency);
          }
        }
        else
        {
          int seed1 = seed;
          double frequency = (double) num;
          myModuleFast = (MyModuleFast) new MyBillowFast(MyNoiseQuality.High, distortionDefinition.LayerCount, seed1, frequency);
        }
        if (myModuleFast != null)
          this.m_modulators.Add(distortionDefinition.Value, new MyPlanetDetailModulator.MyModulatorData()
          {
            Height = distortionDefinition.Height,
            Modulator = myModuleFast
          });
      }
    }

    public double GetValue(double x) => 0.0;

    public double GetValue(double x, double y) => 0.0;

    public double GetValue(double x, double y, double z) => 0.0;

    private struct MyModulatorData
    {
      public float Height;
      public MyModuleFast Modulator;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyParticleEffectsIDEnum
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game
{
  [Obsolete]
  public enum MyParticleEffectsIDEnum
  {
    None = -1, // 0xFFFFFFFF
    Dummy = 0,
    Explosion_Large = 3,
    Explosion_Huge = 4,
    Explosion_Asteroid = 6,
    Explosion_Medium = 7,
    Prefab_LeakingFire_x2 = 8,
    Prefab_LeakingBiohazard = 11, // 0x0000000B
    Prefab_LeakingBiohazard2 = 12, // 0x0000000C
    Prefab_LeakingSmoke = 14, // 0x0000000E
    Prefab_Fire_Field = 15, // 0x0000000F
    Grid_Deformation = 21, // 0x00000015
    Grid_Destruction = 22, // 0x00000016
    Smoke_HandDrillDust = 23, // 0x00000017
    CollisionSparksLargeDistant = 24, // 0x00000018
    CollisionSparksLargeClose = 25, // 0x00000019
    CollisionSparksHandDrill = 26, // 0x0000001A
    Welder = 27, // 0x0000001B
    AngleGrinder = 28, // 0x0000001C
    Hit_BasicAmmoSmall = 29, // 0x0000001D
    MaterialHit_DestructibleSmall = 30, // 0x0000001E
    MaterialHit_IndestructibleSmall = 31, // 0x0000001F
    MaterialHit_MetalSmall = 32, // 0x00000020
    Explosion_Warhead_15 = 33, // 0x00000021
    Explosion_Warhead_02 = 34, // 0x00000022
    Explosion_Warhead_30 = 35, // 0x00000023
    Explosion_Warhead_50 = 36, // 0x00000024
    WelderSecondary = 37, // 0x00000025
    Smoke_Construction = 38, // 0x00000026
    Smoke_HandDrillDustStones = 41, // 0x00000029
    MeteorParticle = 42, // 0x0000002A
    MeteorAsteroidCollision = 43, // 0x0000002B
    MeteorParticleAfterHit = 44, // 0x0000002C
    Smoke_Collector = 45, // 0x0000002D
    Prefab_DestructionSmoke = 47, // 0x0000002F
    FireTorch = 48, // 0x00000030
    ChipOff_Gravel = 49, // 0x00000031
    ChipOff_Wood = 50, // 0x00000032
    Collision_Meteor = 51, // 0x00000033
    DestructionTree = 60, // 0x0000003C
    MeteorTrail_Smoke = 100, // 0x00000064
    MeteorTrail_FireAndSmoke = 101, // 0x00000065
    Damage_Sparks = 200, // 0x000000C8
    Damage_Smoke = 201, // 0x000000C9
    Damage_SmokeDirectionalA = 202, // 0x000000CA
    Damage_SmokeDirectionalB = 203, // 0x000000CB
    Damage_SmokeDirectionalC = 204, // 0x000000CC
    Damage_SmokeBiochem = 205, // 0x000000CD
    Damage_Radioactive = 210, // 0x000000D2
    Damage_Gravitons = 211, // 0x000000D3
    Damage_Mechanical = 212, // 0x000000D4
    Damage_WeapExpl = 213, // 0x000000D5
    Damage_Electrical = 214, // 0x000000D6
    Prefab_LeakingSteamWhite = 300, // 0x0000012C
    Prefab_LeakingSteamGrey = 301, // 0x0000012D
    Prefab_LeakingSteamBlack = 302, // 0x0000012E
    Prefab_DustyArea = 303, // 0x0000012F
    Prefab_EMP_Storm = 304, // 0x00000130
    Prefab_LeakingElectricity = 305, // 0x00000131
    Prefab_LeakingFire = 306, // 0x00000132
    UniversalLauncher_DecoyFlare = 400, // 0x00000190
    UniversalLauncher_IlluminatingShell = 401, // 0x00000191
    UniversalLauncher_SmokeBomb = 402, // 0x00000192
    MaterialHit_MetalSparks = 445, // 0x000001BD
    Drill_Laser = 450, // 0x000001C2
    Drill_Saw = 451, // 0x000001C3
    Drill_Nuclear_Original = 452, // 0x000001C4
    Drill_Thermal = 453, // 0x000001C5
    Drill_Nuclear = 454, // 0x000001C6
    Drill_Pressure_Charge = 455, // 0x000001C7
    Drill_Pressure_Fire = 456, // 0x000001C8
    Drill_Pressure_Impact = 457, // 0x000001C9
    Drill_Pressure_Impact_Metal = 458, // 0x000001CA
    Smoke_Autocannon = 500, // 0x000001F4
    Smoke_CannonShot = 501, // 0x000001F5
    Smoke_Missile = 502, // 0x000001F6
    Smoke_MissileStart = 503, // 0x000001F7
    Smoke_LargeGunShot = 504, // 0x000001F8
    Smoke_SmallGunShot = 505, // 0x000001F9
    Smoke_DrillDust = 506, // 0x000001FA
    Harvester_Harvesting = 550, // 0x00000226
    Harvester_Finished = 551, // 0x00000227
    Explosion_Ammo = 600, // 0x00000258
    Explosion_Blaster = 601, // 0x00000259
    Explosion_Smallship = 604, // 0x0000025C
    Explosion_Bomb = 605, // 0x0000025D
    Explosion_SmallPrefab = 607, // 0x0000025F
    Explosion_Missile_Close = 616, // 0x00000268
    Explosion_Plasma = 630, // 0x00000276
    Explosion_Nuclear = 640, // 0x00000280
    MaterialExplosion_Destructible = 650, // 0x0000028A
    Explosion_Missile = 666, // 0x0000029A
    Explosion_BioChem = 667, // 0x0000029B
    Explosion_EMP = 669, // 0x0000029D
    Hit_ExplosiveAmmo = 700, // 0x000002BC
    Hit_ChemicalAmmo = 701, // 0x000002BD
    Hit_HighSpeedAmmo = 702, // 0x000002BE
    Hit_PiercingAmmo = 703, // 0x000002BF
    Hit_BasicAmmo = 704, // 0x000002C0
    Hit_AutocannonBasicAmmo = 705, // 0x000002C1
    Hit_AutocannonChemicalAmmo = 706, // 0x000002C2
    Hit_AutocannonHighSpeedAmmo = 707, // 0x000002C3
    Hit_AutocannonPiercingAmmo = 708, // 0x000002C4
    Hit_AutocannonExplosiveAmmo = 709, // 0x000002C5
    Hit_EMPAmmo = 710, // 0x000002C6
    Hit_AutocannonEMPAmmo = 711, // 0x000002C7
    Collision_Smoke = 800, // 0x00000320
    Collision_Sparks = 801, // 0x00000321
    DestructionSmokeLarge = 802, // 0x00000322
    DestructionHit = 803, // 0x00000323
    EngineThrust = 900, // 0x00000384
    Trail_Shotgun = 950, // 0x000003B6
    Explosion_Meteor = 951, // 0x000003B7
  }
}

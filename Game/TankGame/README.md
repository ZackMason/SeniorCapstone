# Gravitank Gauntlet

# [Play The Game!](https://cerealcode.itch.io/untitled-tank-game)

## Table Of Contents
- [Build](#build)
- [Tank](#tank)
- [Weapons](#weapons)
- [Destruction](#destruction)
- [AI](#ai)
- [Singletons](#singletons)

## Build
The game is built using only Unity 2021.3.16f1 with the Universal Render Pipeline, and python 3.7 with pytorch for machine learning. 

## Tank
- [Controller](Assets/GameObjects/Tank/HoverTankController.cs)
- [Engine](Assets/GameObjects/Tank/Engine/HoverEngine.cs)
- [Faction](Assets/Scripts/Faction.cs)

## Weapons
- [IWeapon](Assets/GameObjects/Tank/Weapons/IWeapon.cs)
- [HEWeapon](Assets/GameObjects/Tank/Weapons/HEWeapon.cs)
- [HEProjectile](Assets/GameObjects/Tank/Weapons/HEProjectile.cs)

## Destruction
- [Destructible](Assets/GameObjects/Buildings/Destructible.cs)
- [DeletePrec](Assets/GameObjects/Buildings/DeletePerc.cs)
- [Health](Assets/Scripts/Health.cs)
- [TimedDelete](Assets/Scripts/TimedDelete.cs)

## AI

### Broadphase
- [TargetFinder](Assets/GameObjects/Enemies/Tanks/TargetFinder.cs)
- [CombatPositionFinder](Assets/GameObjects/Enemies/Tanks/CombatPositionFinder.cs)

### [ITankBrain](Assets/GameObjects/Tank/ITankBrain.cs)
- [PlayerTankBrain](Assets/GameObjects/Player/PlayerTankBrain.cs)
- [EnemyTankBrain](Assets/GameObjects/Enemies/Tanks/EnemyTankBrain.cs)
- [MLTankBrain](Assets/GameObjects/Enemies/Tanks/MLTankBrain.cs)

### [Turret](Assets/GameObjects/Enemies/Turrets/Turret.cs)

## Singletons
- [Globals](Assets/Singletons/Globals.cs)
- [SoundManager](Assets/Singletons/SoundManager.cs)
- [LevelManager](Assets/Singletons/LevelManager.cs)
- [ExplosionManager](Assets/Singletons/ExplosionManager.cs)
- [ParticleManager](Assets/Singletons/ParticleManager.cs)
- [RespawnManager](Assets/Singletons/RespawnManager.cs)
- [UnitManager](Assets/Singletons/UnitManager.cs)

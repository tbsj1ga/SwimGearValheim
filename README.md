# SwimGear

**English** · [Русский](README-RU.md)

A Valheim mod: your weapons and tools stay in your hands while swimming, and you can equip
in the water — at no cost to the frame rate. Client-side only: other players and the server
need nothing.

Version history is in `CHANGELOG.md`.

## How it works

The game asks `Character.IsSwimming()` in two places to take your gear away in the water:

- `Humanoid.UpdateEquipment`, every frame, hides the hand items of a swimming player;
- `Humanoid.EquipItem` refuses to equip while the player is swimming.

There, and only there, and only for players, SwimGear makes the answer "not swimming". Both
methods mark the character they run for while they run (a prefix sets it, a finalizer puts
back what was there, so nesting and exceptions are safe), and the `IsSwimming` prefix compares
one reference. Everywhere else — movement, stamina, attacks, dodging, creatures' AI — the game
sees the truth.

**Why another mod for this.** UseEquipmentInWater and WieldEquipmentWhileSwimming find "only
there" by building a `System.Diagnostics.StackTrace` and joining the names of eight frames into
a string on every `IsSwimming` call whose character is in the water — for every creature, not
only the player; the player check comes after the stack trace. In a base by the sea that was
about 2,000 calls a second and ~24 ms of every second of play (HostProfiler), plus several
kilobytes of garbage per call for the collector.

**What stays vanilla in the water.** `ItemTypes` lists kinds of items (`ItemDrop.ItemData.
ItemType` names), `Items` single prefabs. With such an item in either hand, `UpdateEquipment`
is left alone and the game puts both hands away, as it always did; such an item cannot be
equipped in the water.

## Compatibility

Built against **Valheim 1.0.20** (network version 41), **BepInEx 5.4.23.5**
(BepInExPack_Valheim 5.4.2351). Do not run it together with UseEquipmentInWater,
WieldEquipmentWhileSwimming or another mod doing the same.

## Who needs it

| Who | What |
|---|---|
| A player who wants their gear in the water | installs it |
| Other players, the host, a dedicated server | nothing |

## Bugs and feedback

https://github.com/tbsj1ga/SwimGearValheim/issues

## Installation

Through r2modman / Thunderstore, or put `build/SwimGear.dll` into

```
%AppData%\r2modmanPlus-local\Valheim\profiles\Valheim\BepInEx\plugins\SwimGear\
```

(or `build.ps1 -Install`).

## Settings

`BepInEx/config/j1ga.swimgear.cfg`, or in-game with ConfigurationManager (F1). Changes apply at
once.

| Section | Key | Default | What |
|---|---|---|---|
| 01 General | `Enabled` | `true` | off: the game takes your gear away in the water as usual |
| 02 Not in water | `ItemTypes` | empty | item kinds that stay vanilla in the water, comma separated |
| 02 Not in water | `Items` | empty | single items that stay vanilla, by prefab name |

## Building

```
powershell -ExecutionPolicy Bypass -File .\build.ps1            # build and check references
powershell -ExecutionPolicy Bypass -File .\build.ps1 -Install   # ... and copy into plugins
powershell -ExecutionPolicy Bypass -File .\build.ps1 -Package   # ... and make the Thunderstore zip
powershell -ExecutionPolicy Bypass -File .\build.ps1 -Nexus     # ... zip, and the Nexus Mods kit
```

The compiler is `csc.exe` from the .NET Framework (C# 5); references come straight from the
game folder and the r2modman profile's `BepInEx\core`. After the build, `check-refs.ps1` checks
every reference and Harmony patch target against the game.

## Repository

Branch `main` on GitHub: https://github.com/tbsj1ga/SwimGearValheim.

## More mods by j1gA

| | Mod |
|---|---|
| [![LivingMap](https://raw.githubusercontent.com/tbsj1ga/LivingMapValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/LivingMap/) | **[LivingMap](https://thunderstore.io/c/valheim/p/j1gA/LivingMap/)** — Your buildings, roads and cleared forest on the map and the minimap — and a detailed map when you zoom in. |
| [![StationSpeed](https://raw.githubusercontent.com/tbsj1ga/StationSpeedValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/StationSpeed/) | **[StationSpeed](https://thunderstore.io/c/valheim/p/j1gA/StationSpeed/)** — Faster smelters, kilns, fermenters and crops — consistent even for players without the mod. |
| [![WeaponArts](https://raw.githubusercontent.com/tbsj1ga/WeaponArtsValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/WeaponArts/) | **[WeaponArts](https://thunderstore.io/c/valheim/p/j1gA/WeaponArts/)** — One key, one active ability per weapon: stagger, taunt, heals, berserk, crits. |
| [![ExtendedBosses](https://raw.githubusercontent.com/tbsj1ga/ExtendedBossesValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/ExtendedBosses/) | **[ExtendedBosses](https://thunderstore.io/c/valheim/p/j1gA/ExtendedBosses/)** — Raid-style boss fights: phases, adds, nests, shields, marks — built from vanilla parts. |
| [![HostOwner](https://raw.githubusercontent.com/tbsj1ga/HostOwnerValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/HostOwner/) | **[HostOwner](https://thunderstore.io/c/valheim/p/j1gA/HostOwner/)** — The host takes ownership of stations and bosses near it, so its mods work for everyone. |
| [![HudLayout](https://raw.githubusercontent.com/tbsj1ga/HudLayoutValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/HudLayout/) | **[HudLayout](https://thunderstore.io/c/valheim/p/j1gA/HudLayout/)** — Move, resize and restyle your HUD with the mouse: bars, food, hotbar, minimap, even other mods' HUD. |
| [![HealthPulse](https://raw.githubusercontent.com/tbsj1ga/HealthPulseValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/HealthPulse/) | **[HealthPulse](https://thunderstore.io/c/valheim/p/j1gA/HealthPulse/)** — Low health warning: the screen edges pulse red like a heartbeat, faster as your health drops. |
| [![ChestReach](https://raw.githubusercontent.com/tbsj1ga/ChestReachValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/ChestReach/) | **[ChestReach](https://thunderstore.io/c/valheim/p/j1gA/ChestReach/)** — Craft, build, refuel and cook from the chests around you — no lag, players without the mod can join. |

## Credits and AI assistance

The idea comes from UseEquipmentInWater by LVH-IT; SwimGear is written from scratch and shares
no code with it. This mod was developed with the help of an AI assistant (Claude by
Anthropic); the design decisions, in-game testing and releases are the author's.

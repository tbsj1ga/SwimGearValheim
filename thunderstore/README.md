# SwimGear

**Keep your weapons and tools in your hands while swimming.** Fight serpents from the water
with a spear, swim to a shipwreck with your pickaxe out, cross a river with your shield up —
the game no longer puts everything away the moment you start swimming, and you can equip in
the water.

**No cost to your FPS.** The usual way to do this asks the call stack, as text, every time
the game checks whether anyone is swimming — thousands of times a second for every creature in
the water, measured at ~24 ms of every second of play plus the garbage it leaves for the
collector. SwimGear marks the two moments the game takes your gear away and answers there
only: one reference compared per check.

- Only your gear changes. Movement, stamina, attacks and dodging in the water, and every
  creature, stay as in vanilla.
- Choose what stays vanilla in the water: item kinds (`Bow`, `Shield`, `Torch`, `Tool`,
  `TwoHandedWeapon`...) or single items by prefab name.
- **Client-side.** Install it if you want it; other players, the host and the server need
  nothing.
- Replaces UseEquipmentInWater and WieldEquipmentWhileSwimming: do not run it together with
  them.

## Settings

`BepInEx/config/j1ga.swimgear.cfg`, or in-game with ConfigurationManager (F1):

- `Enabled` — on / off.
- `ItemTypes` — kinds of items that stay vanilla in the water, comma separated:
  `OneHandedWeapon, TwoHandedWeapon, TwoHandedWeaponLeft, Bow, Shield, Torch, Tool`.
- `Items` — single items that stay vanilla, by prefab name, e.g. `FishingRod`.

## Compatibility

Built against Valheim 1.0.20, BepInEx 5.4.23.5 (BepInExPack_Valheim 5.4.2351).

## Bugs and feedback

[GitHub issues](https://github.com/tbsj1ga/SwimGearValheim/issues) - please include
`BepInEx/LogOutput.log`.

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

Source, full documentation and the changelog: https://github.com/tbsj1ga/SwimGearValheim

*The idea comes from UseEquipmentInWater (LVH-IT); SwimGear is written from scratch.
Developed with the help of an AI assistant (Claude by Anthropic); the design decisions,
verification against the game code and in-game testing are the author's.*

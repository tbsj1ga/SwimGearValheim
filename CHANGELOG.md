# Changelog

**English** · [Русский](CHANGELOG-RU.md)

The version is set in one place — `SwimGearPlugin.Version` in `src/SwimGearPlugin.cs`.

## 0.1.0

- First release. Weapons and tools stay in your hands while swimming and can be equipped in the
  water; movement, stamina, attacks and creatures stay vanilla.
- No stack trace per swim check: the two places that take your gear away mark the character
  while they run, the swim check compares one reference.
- `ItemTypes` and `Items`: what stays vanilla in the water.

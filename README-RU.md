# SwimGear

[English](README.md) · **Русский**

Мод для Valheim: оружие и инструменты остаются в руках, когда вы плывёте, а в воде можно
экипироваться — без цены для FPS. Только на клиенте: другим игрокам и серверу ничего не нужно.

История версий — в `CHANGELOG-RU.md`.

## Как это работает

Игра спрашивает `Character.IsSwimming()` в двух местах, чтобы отобрать снаряжение в воде:

- `Humanoid.UpdateEquipment` каждый кадр убирает предметы из рук плывущего игрока;
- `Humanoid.EquipItem` не даёт экипироваться, пока игрок плывёт.

Там, только там и только для игроков SwimGear отвечает «не плывёт». Оба метода, пока работают,
помечают персонажа, для которого идут (префикс ставит пометку, финализатор возвращает прежнюю
— вложенные вызовы и исключения безопасны), а префикс `IsSwimming` сравнивает одну ссылку.
Везде остальном — движение, выносливость, атаки, перекаты, ИИ существ — игра видит правду.

**Зачем ещё один мод.** UseEquipmentInWater и WieldEquipmentWhileSwimming находят «только
там», строя `System.Diagnostics.StackTrace` и склеивая имена восьми кадров стека в строку при
каждом вызове `IsSwimming` для персонажа в воде — для каждого существа, не только игрока;
проверка на игрока стоит после стека. На базе у моря это около 2000 вызовов в секунду и ~24 мс
каждой секунды игры (HostProfiler), плюс несколько килобайт мусора на вызов для сборщика.

**Что остаётся ванильным в воде.** `ItemTypes` — виды предметов (имена
`ItemDrop.ItemData.ItemType`), `Items` — отдельные префабы. С таким предметом в любой руке
`UpdateEquipment` не трогается, и игра убирает обе руки, как всегда; такой предмет нельзя
надеть в воде.

## Совместимость

Собран под **Valheim 1.0.20** (network version 41), **BepInEx 5.4.23.5**
(BepInExPack_Valheim 5.4.2351). Не вместе с UseEquipmentInWater, WieldEquipmentWhileSwimming
и другими модами, делающими то же самое.

## Кому что нужно

| Кто | Что |
|---|---|
| Игрок, которому нужно снаряжение в воде | ставит мод |
| Другие игроки, хост, выделенный сервер | ничего |

## Ошибки и пожелания

https://github.com/tbsj1ga/SwimGearValheim/issues

## Установка

Через r2modman / Thunderstore, или положите `build/SwimGear.dll` в

```
%AppData%\r2modmanPlus-local\Valheim\profiles\Valheim\BepInEx\plugins\SwimGear\
```

(или `build.ps1 -Install`).

## Настройки

`BepInEx/config/j1ga.swimgear.cfg` или в игре через ConfigurationManager (F1). Изменения
применяются сразу.

| Раздел | Ключ | По умолчанию | Что |
|---|---|---|---|
| 01 General | `Enabled` | `true` | выключено: игра отбирает снаряжение в воде как обычно |
| 02 Not in water | `ItemTypes` | пусто | виды предметов, которые в воде остаются ванильными, через запятую |
| 02 Not in water | `Items` | пусто | отдельные предметы по имени префаба |

## Сборка

```
powershell -ExecutionPolicy Bypass -File .\build.ps1            # сборка и проверка ссылок
powershell -ExecutionPolicy Bypass -File .\build.ps1 -Install   # ... и копия в plugins
powershell -ExecutionPolicy Bypass -File .\build.ps1 -Package   # ... и zip для Thunderstore
powershell -ExecutionPolicy Bypass -File .\build.ps1 -Nexus     # ... zip и комплект для Nexus Mods
```

## Репозиторий

Ветка `main` на GitHub: https://github.com/tbsj1ga/SwimGearValheim.

## Другие моды j1gA

| | Мод |
|---|---|
| [![LivingMap](https://raw.githubusercontent.com/tbsj1ga/LivingMapValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/LivingMap/) | **[LivingMap](https://thunderstore.io/c/valheim/p/j1gA/LivingMap/)** — ваши постройки, дороги и вырубки на карте и миникарте, и подробная карта при приближении. |
| [![StationSpeed](https://raw.githubusercontent.com/tbsj1ga/StationSpeedValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/StationSpeed/) | **[StationSpeed](https://thunderstore.io/c/valheim/p/j1gA/StationSpeed/)** — быстрее плавильни, печи, ферментеры и посадки, одинаково и для игроков без мода. |
| [![WeaponArts](https://raw.githubusercontent.com/tbsj1ga/WeaponArtsValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/WeaponArts/) | **[WeaponArts](https://thunderstore.io/c/valheim/p/j1gA/WeaponArts/)** — одна клавиша, одна активная способность на оружие: оглушение, агро, лечение, берсерк, криты. |
| [![ExtendedBosses](https://raw.githubusercontent.com/tbsj1ga/ExtendedBossesValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/ExtendedBosses/) | **[ExtendedBosses](https://thunderstore.io/c/valheim/p/j1gA/ExtendedBosses/)** — рейдовые бои с боссами: фазы, прислужники, гнёзда, щиты, метки — из ванильных частей. |
| [![HostOwner](https://raw.githubusercontent.com/tbsj1ga/HostOwnerValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/HostOwner/) | **[HostOwner](https://thunderstore.io/c/valheim/p/j1gA/HostOwner/)** — хост забирает себе станции и боссов рядом, чтобы его моды работали для всех. |
| [![HudLayout](https://raw.githubusercontent.com/tbsj1ga/HudLayoutValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/HudLayout/) | **[HudLayout](https://thunderstore.io/c/valheim/p/j1gA/HudLayout/)** — двигайте, масштабируйте и перекрашивайте HUD мышью: полосы, еда, хотбар, миникарта, даже HUD других модов. |
| [![HealthPulse](https://raw.githubusercontent.com/tbsj1ga/HealthPulseValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/HealthPulse/) | **[HealthPulse](https://thunderstore.io/c/valheim/p/j1gA/HealthPulse/)** — при низком здоровье края экрана пульсируют красным, как сердце. |
| [![ChestReach](https://raw.githubusercontent.com/tbsj1ga/ChestReachValheim/main/docs/media/icon-128.png)](https://thunderstore.io/c/valheim/p/j1gA/ChestReach/) | **[ChestReach](https://thunderstore.io/c/valheim/p/j1gA/ChestReach/)** — крафт, стройка, заправка и готовка из сундуков вокруг — без тормозов, игроки без мода могут зайти. |

## Благодарности и помощь ИИ

Идея — из UseEquipmentInWater от LVH-IT; SwimGear написан с нуля и не содержит его кода. Мод
разработан с помощью ИИ-ассистента (Claude от Anthropic); решения по дизайну, проверка в игре
и релизы — за автором.

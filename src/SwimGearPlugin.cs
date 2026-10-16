using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace SwimGear
{
    // Keep weapons and tools in your hands while swimming.
    //
    // The game asks Character.IsSwimming() in two places to take your gear away in the water:
    // Humanoid.UpdateEquipment hides the hand items of a swimming player every frame, and
    // Humanoid.EquipItem refuses to equip while swimming. Only there, and only for players,
    // the answer becomes "not swimming"; everywhere else - movement, stamina, attacks, dodging,
    // the creatures' AI - the game sees the truth.
    //
    // Other mods for this find "only there" by building a StackTrace and its method names as
    // text on every IsSwimming call of every creature in the water - thousands of calls a
    // second, measured at ~24 ms per second of play plus the garbage it makes. Here the two
    // methods mark the character they run for while they run, and IsSwimming compares one
    // reference.
    [BepInPlugin(Guid, Name, Version)]
    public class SwimGearPlugin : BaseUnityPlugin
    {
        public const string Guid = "j1ga.swimgear";
        public const string Name = "SwimGear";
        public const string Version = "0.1.0";

        // The character whose UpdateEquipment / EquipItem is running right now: its IsSwimming
        // says false. Main thread only, like the game's own code here.
        private static Character _inside;

        private static ConfigEntry<bool> _enabled;
        private static ConfigEntry<string> _deniedTypes;
        private static ConfigEntry<string> _deniedItems;
        private static readonly HashSet<ItemDrop.ItemData.ItemType> _types = new HashSet<ItemDrop.ItemData.ItemType>();
        private static readonly HashSet<string> _items = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private Harmony _harmony;

        private void Awake()
        {
            _enabled = Config.Bind("01 General", "Enabled", true, "Off: the game takes your gear away in the water as usual.");
            _deniedTypes = Config.Bind("02 Not in water", "ItemTypes", "",
                "Kinds of items that stay vanilla in the water (put away, cannot be equipped), comma separated: " +
                "OneHandedWeapon, TwoHandedWeapon, TwoHandedWeaponLeft, Bow, Shield, Torch, Tool. Empty: everything can be used.");
            _deniedItems = Config.Bind("02 Not in water", "Items", "",
                "Single items that stay vanilla in the water, by prefab name, comma separated. Example: Tankard,FishingRod");
            _deniedTypes.SettingChanged += delegate { ReadLists(); };
            _deniedItems.SettingChanged += delegate { ReadLists(); };
            ReadLists();

            _harmony = new Harmony(Guid);
            _harmony.PatchAll(typeof(SwimGearPlugin).Assembly);
            Logger.LogInfo(Name + " " + Version + " loaded.");
        }

        private void OnDestroy()
        {
            if (_harmony != null) _harmony.UnpatchSelf();
        }

        private void ReadLists()
        {
            _types.Clear();
            _items.Clear();
            foreach (string raw in (_deniedTypes.Value ?? "").Split(',', ';'))
            {
                string s = raw.Trim();
                if (s.Length == 0) continue;
                try { _types.Add((ItemDrop.ItemData.ItemType)Enum.Parse(typeof(ItemDrop.ItemData.ItemType), s, true)); }
                catch (Exception) { Logger.LogWarning("Unknown item type '" + s + "' in ItemTypes; ignored."); }
            }
            foreach (string raw in (_deniedItems.Value ?? "").Split(',', ';'))
            {
                string s = raw.Trim();
                if (s.Length > 0) _items.Add(s);
            }
        }

        private static bool Denied(ItemDrop.ItemData item)
        {
            if (item == null || item.m_shared == null) return false;
            if (_types.Count > 0 && _types.Contains(item.m_shared.m_itemType)) return true;
            if (_items.Count > 0 && item.m_dropPrefab != null && _items.Contains(item.m_dropPrefab.name)) return true;
            return false;
        }

        // ------------------------------------------------------------------
        // Harmony
        // ------------------------------------------------------------------
        [HarmonyPatch(typeof(Character), "IsSwimming")]
        private static class Character_IsSwimming_Patch
        {
            private static bool Prefix(Character __instance, ref bool __result)
            {
                if (_inside == null || !ReferenceEquals(_inside, __instance)) return true;
                __result = false;
                return false;
            }
        }

        // Every frame for every humanoid; hides the hand items of a swimming player. A denied
        // item in either hand: vanilla, both hands are put away as the game does.
        [HarmonyPatch(typeof(Humanoid), "UpdateEquipment")]
        private static class Humanoid_UpdateEquipment_Patch
        {
            private static void Prefix(Humanoid __instance, ItemDrop.ItemData ___m_rightItem, ItemDrop.ItemData ___m_leftItem, out Character __state)
            {
                __state = _inside;
                if (!_enabled.Value || !__instance.IsPlayer()) return;
                if (Denied(___m_rightItem) || Denied(___m_leftItem)) return;
                _inside = __instance;
            }

            private static Exception Finalizer(Exception __exception, Character __state)
            {
                _inside = __state;
                return __exception;
            }
        }

        [HarmonyPatch(typeof(Humanoid), "EquipItem")]
        private static class Humanoid_EquipItem_Patch
        {
            private static void Prefix(Humanoid __instance, ItemDrop.ItemData item, out Character __state)
            {
                __state = _inside;
                if (!_enabled.Value || !__instance.IsPlayer() || Denied(item)) return;
                _inside = __instance;
            }

            private static Exception Finalizer(Exception __exception, Character __state)
            {
                _inside = __state;
                return __exception;
            }
        }
    }
}

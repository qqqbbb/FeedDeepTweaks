using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeedDeepTweaks
{
    internal class Config
    {

        public static ConfigEntry<bool> controllerRumble;
        public static ConfigEntry<bool> removeLightFromShop;
        public static ConfigEntry<bool> goldOnlyShop;
        public static ConfigEntry<bool> shopIsFree;
        public static ConfigEntry<bool> noSoldOutItemsInShopAfterReroll;

        public static void Bind()
        {
            controllerRumble = Main.config.Bind("", "Controller rumble", true);
            removeLightFromShop = Main.config.Bind("", "Remove lantern and glowstick from shop in Blocked episode", false);
            goldOnlyShop = Main.config.Bind("", "Shop accepts only gold. Orbs you collect will convert to gold.", false);
            shopIsFree = Main.config.Bind("", "Everything in shop is free.", false);
            noSoldOutItemsInShopAfterReroll = Main.config.Bind("", "No sold out items in shop after reroll in Blocked episode.", false);

        }


    }
}

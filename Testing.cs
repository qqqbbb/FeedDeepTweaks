using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace FeedDeepTweaks
{
    internal class Testing
    {
        //[HarmonyPatch(typeof(Diver), "Update")]
        class Diver_Update_Patch
        {
            public static void Postfix(Diver __instance)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    //Util.Message("last Selected " + Diver.instance.m_lastSelectedTowedGrabable.);
                }
            }
        }

        //[HarmonyPatch(typeof(CompassNeedle), "Update")]
        class CompassNeedle_Update_Patch
        {
            public static void Postfix(CompassNeedle __instance)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    Util.Message("CompassTarget " + __instance.m_target.ToString() + " tunnels " + __instance.m_tunnels.Count);
                }
            }
        }

        static HashSet<ShopPurchaseSlot> slots = new HashSet<ShopPurchaseSlot>();

        //[HarmonyPatch(typeof(ShopPurchaseSlot), "IsItemStillAvailable")]
        class CompassNeedle_IsItemStillAvailable_Patch
        {
            public static void Postfix(ShopPurchaseSlot __instance, bool __result)
            {
                if (__instance.name == "Reroll")
                    return;

                if (!slots.Contains(__instance))
                { // MysteryChest Ammo Scanner
                    //Util.Message("IsItemStillAvailable " + __instance.name);
                    Main.logger.LogMessage("IsItemStillAvailable " + __instance.name + " " + __result + " itemsToAward.Count " + __instance.m_itemsToAward.Count);
                    slots.Add(__instance);
                    if (!__result)
                    {
                        Util.Message("IsItemStillAvailable " + __instance.name + " itemsToAward.Count " + __instance.m_itemsToAward.Count);

                    }
                }
            }
        }


    }
}

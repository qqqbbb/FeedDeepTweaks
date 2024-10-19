using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FeedDeepTweaks
{
    internal class Patches
    {
        //static bool chestCantBeSoldOutInShop = true;

        [HarmonyPatch(typeof(Gamepad), "SetMotorSpeeds")]
        class Gamepad_SetMotorSpeeds_Patch
        {
            public static bool Prefix(Gamepad __instance)
            {
                //AddDebug(" Targeting GetTarget  " + result.name);
                //Main.logger.LogMessage("Gamepad SetMotorSpeeds ");
                return Config.controllerRumble.Value;
            }
        }


        [HarmonyPatch(typeof(ShopPanel))]
        class ShopPanel_Patch
        {
            [HarmonyPatch("RerollShop"), HarmonyPrefix]
            public static bool RerollShopPrefix(ShopPanel __instance, int roll, bool resetItems)
            {
                //Main.logger.LogMessage(" RerollShop.roll " + roll + " resetItems " + resetItems);
                if (resetItems)
                {
                    ++__instance.m_rerolls;
                    foreach (ShopPurchaseSlot allSlot in __instance.m_allSlots)
                    {
                        allSlot.SetSinglePurchase(false);
                        allSlot.UpdateInfo();
                    }
                }
                else
                {
                    foreach (ShopPurchaseSlot allSlot in __instance.m_allSlots)
                        allSlot.SyncButtonIndex();
                }
                UnityEngine.Random.InitState(roll);
                List<ShopPurchaseSlot> list1 = new List<ShopPurchaseSlot>();
                for (int index = 0; index < __instance.m_forceShowReRolls.Length; ++index)
                {
                    if (__instance.m_forceShowReRolls[index] == __instance.m_rerolls)
                        list1.Add(__instance.m_forceShowSlots[index]);
                }
                //Main.logger.LogMessage(" RerollShop.list1 Count " + list1.Count);
                List<ShopPurchaseSlot> allSlots = new List<ShopPurchaseSlot>();
                foreach (ShopPurchaseSlot slot in __instance.m_allSlots)
                {
                    allSlots.Add(slot);
                    //Main.logger.LogMessage(" RerollShop.allSlot " + allSlot.name + " " + allSlot.GetCurrentItem().m_id);
                }
                allSlots.Shuffle();
                foreach (ShopPurchaseSlot slot in allSlots)
                {
                    slot.gameObject.SetActive(false);
                    Item item = slot.GetCurrentItem();
                    if (Config.noSoldOutItemsInShopAfterReroll.Value && resetItems && !slot.IsItemStillAvailable())
                    {
                        continue;
                        Main.logger.LogMessage("! IsItemStillAvailable " + item.m_id + " itemsToAward.Count " + slot.m_itemsToAward.Count);
                        Util.Message("! IsItemStillAvailable " + item.m_id + " itemsToAward.Count " + slot.m_itemsToAward.Count);
                    }
                    if (Config.removeLightFromShop.Value)
                    {
                        if (item.m_id == "GlowStick" || item.m_id == "Fixed Light")
                            continue;
                    }
                    if (Config.goldOnlyShop.Value)
                    {
                        if (item.m_id == "Orb")
                            continue;
                    }
                    //if (chestCantBeSoldOutInShop && item.m_id == "MysteryChest")
                    //{
                    //}
                    if (!list1.Contains(slot) && list1.Count <= __instance.m_slotsToShow)
                        list1.Add(slot);
                }
                list1.Shuffle();

                for (int index = 0; index < __instance.m_slotsToShow; ++index)
                {
                    ShopPurchaseSlot slot = list1[index];
                    //Main.logger.LogMessage(" RerollShop " + slot.name + ", " + slot.GetCurrentItem().m_id);
                    slot.gameObject.SetActive(true);
                    slot.transform.SetSiblingIndex(index);
                    slot.UpdateInfo();
                }
                __instance.m_defaultShopSlot = list1[0];
                SaveManager.instance.m_shopData.m_shopRollSeed = roll;
                SaveManager.instance.m_shopData.m_rerolls = __instance.m_rerolls;
                SaveManager.instance.SaveLevelData();
                if (__instance.m_rerollButton != null)
                    return false;

                EventSystem.current.SetSelectedGameObject(__instance.m_defaultShopSlot.gameObject);
                return false;
            }

            [HarmonyPatch("RePriceShop"), HarmonyPostfix]
            public static void RePriceShopPostfix(ShopPanel __instance)
            {
                if (Config.goldOnlyShop.Value)
                {
                    foreach (ShopPurchaseSlot slot in __instance.m_allSlots)
                        slot.m_isOrbPrices = false;
                }
            }

            [HarmonyPatch("Start"), HarmonyPostfix]
            public static void StartPostfix(ShopPanel __instance)
            {
                if (Config.goldOnlyShop.Value)
                {
                    foreach (ShopPurchaseSlot slot in __instance.GetComponentsInChildren<ShopPurchaseSlot>()) // key in Blocked
                        slot.m_isOrbPrices = false;

                    foreach (ShopPurchaseSlot slot in __instance.m_allSlots)
                        slot.m_isOrbPrices = false;
                }
            }
        }

        [HarmonyPatch(typeof(ShopPurchaseSlot), "GetCurrentCost")]
        class ShopPurchaseSlot_GetCurrentCost_Patch
        {
            public static void Postfix(ShopPurchaseSlot __instance, ref int __result)
            {
                if (Config.shopIsFree.Value)
                    __result = 0;
            }
        }

        [HarmonyPatch(typeof(Shop), "ChangeOrbBalance")]
        class Shop_ChangeOrbBalance_Patch
        {
            public static bool Prefix(Shop __instance, int change)
            {
                if (Config.goldOnlyShop.Value)
                {
                    //Util.Message(" ChangeOrbBalance " + change);
                    __instance.ChangeGoldBalance(change);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(IntroLogo), "Start")]
        class IntroLogo_Start_Patch
        {
            public static bool Prefix(IntroLogo __instance)
            {
                __instance.m_material.SetFloat("_Dissolve", __instance.curr);
                SceneManager.LoadScene("Menu");
                return false;
            }
        }

        [HarmonyPatch(typeof(MenuManager), "OpenSequence")]
        class MenuManager_OpenSequence_Patch
        {
            public static bool Prefix(MenuManager __instance)
            {
                return false;
            }
        }



    }
}

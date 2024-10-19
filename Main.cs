using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FeedDeepTweaks
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Main : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "qqqbbb.feedDeep.tweaks";
        public const string PLUGIN_NAME = "FeedDeepTweaks";
        public const string PLUGIN_VERSION = "1.0.0";

        public static ConfigFile config;
        public static ManualLogSource logger;

        private void Awake()
        {
            Harmony harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();
            config = this.Config;
            FeedDeepTweaks.Config.Bind();
            logger = Logger;
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
        }




    }
}

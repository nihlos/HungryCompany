using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HungryCompany
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class HungryCompany : BaseUnityPlugin
    {
        private const string modGUID = "nihl.HungryCompany";
        private const string modName = "Hungry Company";
        private const string modVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        internal ManualLogSource MLS;

        private static HungryCompany instance;
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            MLS = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            MLS.LogInfo("Hungry Company Loaded!");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), modGUID);
        }
    }
    [HarmonyPatch(typeof(DepositItemsDesk))]
    internal class CompanyPatch 
    {
        [HarmonyPatch("AnimationGrabPlayer")]
        [HarmonyPrefix]
        static void KillMorePeople(ref DepositItemsDesk __instance, int monsterAnimationID, int playerID)
        {
            __instance.currentMood.maxPlayersToKillBeforeSatisfied = 99;
        }

        [HarmonyPatch("CollisionDetect")]
        [HarmonyPostfix]
        static void CollisionPatch(ref DepositItemsDesk __instance, int monsterAnimationID)
        {
            __instance.monsterAnimations[monsterAnimationID].animatorCollidedOnClient = false;
        }

        [HarmonyPatch("CheckAnimationGrabPlayerServerRpc")]
        [HarmonyPostfix]
        static void ServerPatch(ref DepositItemsDesk __instance, int monsterAnimationID, int playerID)
        {
            __instance.monsterAnimations[monsterAnimationID].animatorCollidedOnClient = false;
        }

        [HarmonyPatch("ConfirmAnimationGrabPlayerClientRpc")]
        [HarmonyPostfix]
        static void ClientPatch(ref DepositItemsDesk __instance, int monsterAnimationID, int playerID)
        {
            __instance.monsterAnimations[monsterAnimationID].animatorCollidedOnClient = false;
        }
    }
}

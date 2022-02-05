using ExitGames.Client.Photon;
using FragLabs.Audio.Codecs;
using Harmony;
using MelonLoader;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;
using VRC;
using BestHTTP;

namespace AutoModeration.Client
{
    public class Patch
    {
        public Patch(Type PatchClass, Type YourClass, string Method, string ReplaceMethod, BindingFlags stat = BindingFlags.Static, BindingFlags pub = BindingFlags.NonPublic)
        {
            MelonLogger.Msg("Attempting to patch " + ReplaceMethod);
            Patch.HInstance.Patch(AccessTools.Method(PatchClass, Method, null, null), this.GetPatch(YourClass, ReplaceMethod, stat, pub), null, null);
            MelonLogger.Msg("Successfully patched " + ReplaceMethod);
        }
        public static readonly HarmonyInstance HInstance = HarmonyInstance.Create("AutoModeration");
        private HarmonyMethod GetPatch(Type YourClass, string MethodName, BindingFlags stat, BindingFlags pub)
        {
            return new HarmonyMethod(YourClass.GetMethod(MethodName, stat | pub));
        }

        public static void InitPatches() {
            MethodInfo[] array = (from m in typeof(NetworkManager).GetMethods()
                                  where m.Name.Contains("Method_Public_Void_Player_") && !m.Name.Contains("PDM")
                                  select m).ToArray<MethodInfo>();
            new Patch(typeof(NetworkManager), typeof(Patch), array[0].Name, "OnPlayerJoined", BindingFlags.Static, BindingFlags.NonPublic);
        }


        private static bool OnPlayerJoined(VRC.Player __0)
        {
            if (Load.modActive)
            {
                if (RoomManager.field_Private_Static_RoomManager_0.prop_IWorldInstance_0.prop_WorldInstanceAccessType_0 != VRC.DataModel.WorldInstanceAccessType.Public)
                {
                    if (VRCPlayer.field_Internal_Static_VRCPlayer_0.field_Private_VRCPlayerApi_0.isInstanceOwner)
                    {
                        if (__0.field_Private_APIUser_0.id != APIUser.CurrentUser.id)
                        {
                            if (Load.whitelistEnabled)
                            {
                                Kick.CheckIfWhiteListed(__0);
                            }
                            else if (Load.blacklistEnabled)
                            {
                                Kick.CheckIfBlackListed(__0);
                            }
                            else if (Load.friendsOnlyAllowed)
                            {
                                Kick.CheckIfFriendListed(__0);
                            }
                            else if (Load.TrustBasedEnabled)
                            {
                                Kick.CheckTrustLevel(__0);
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
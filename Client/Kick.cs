using ExitGames.Client.Photon;
using MelonLoader;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRC.Core;

namespace AutoModeration.Client
{
    internal class Kick
    {
        public static void KickUser(VRC.Player player)
        {
            try
            {
                var worldID = RoomManager.field_Internal_Static_ApiWorldInstance_0.id.ToString().Split(':')[0];
                ApiModeration.SendModeration(player.field_Private_APIUser_0.id, ApiModeration.ModerationType.Kick, "The instance owner, has kicked you out of the room.", ApiModeration.ModerationTimeRange.OneHour, worldID, RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceId,
                    new Action<ApiModelContainer<ApiModeration>>(content =>
                    {
                        foreach (var item in content.ResponseDictionary)
                        {
                            var sex = Serialization.FromIL2CPPToManaged<string>(item.Value);
                            Dictionary<System.Byte, System.Object> keyValuePairs = new Dictionary<System.Byte, System.Object>();
                            keyValuePairs.Add(3, sex);
                            keyValuePairs.Add(0, (byte)1);
                            keyValuePairs.Add(1, $"{player.field_Private_APIUser_0.id}");
                            OpRaiseEvent(33, keyValuePairs,
                            new RaiseEventOptions
                            {
                                field_Public_ReceiverGroup_0 = ReceiverGroup.Others,
                                field_Public_EventCaching_0 = EventCaching.DoNotCache,
                            }, default(SendOptions));
                            global::MelonLoader.MelonLogger.Msg($"Successfully kicked {player.field_Private_APIUser_0.displayName} !");
                        }
                    }), null);
            }
            catch (System.Exception e)
            {
                global::MelonLoader.MelonLogger.Msg(e);
            }
        }

        public static void OpRaiseEvent(byte code, object customObject, RaiseEventOptions RaiseEventOptions, SendOptions sendOptions)
        {
            Il2CppSystem.Object Object = Serialization.FromManagedToIL2CPP<Il2CppSystem.Object>(customObject);
            OpRaiseEvent(code, Object, RaiseEventOptions, sendOptions);
        }

        public static void OpRaiseEvent(byte code, Il2CppSystem.Object customObject, RaiseEventOptions RaiseEventOptions, SendOptions sendOptions)
           => PhotonNetwork.Method_Public_Static_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0
            (code,
             customObject,
             RaiseEventOptions,
             sendOptions);

        internal static void CheckIfWhiteListed(VRC.Player player)
        {
            string[] whiteListedUsers = File.ReadAllLines(FileManager._whiteListPath);
            if (!whiteListedUsers.Contains(player.field_Private_APIUser_0.id))
            {
                Kick.KickUser(player);
                MelonLogger.Msg(ConsoleColor.Red, $"{player.field_Private_APIUser_0.displayName} was not whitelisted and got kicked! User will be able to rejoin in 1 hour!");
            }
            else
            {
                MelonLogger.Msg(ConsoleColor.Green, $"{player.field_Private_APIUser_0.displayName} was whitelisted! User was able to join the lobby successfully!");
            }
        }

        internal static void CheckIfBlackListed(VRC.Player player)
        {
            string[] blackListedUsers = File.ReadAllLines(FileManager._BlackListPath);
            if (blackListedUsers.Contains(player.field_Private_APIUser_0.id))
            {
                Kick.KickUser(player);
                MelonLogger.Msg(ConsoleColor.Red, $"{player.field_Private_APIUser_0.displayName} was blacklisted and got kicked! User will be able to rejoin in 1 hour!");
            }
            else
            {
                MelonLogger.Msg(ConsoleColor.Green, $"{player.field_Private_APIUser_0.displayName} was not on the blacklist and was able to join!");
            }
        }

        internal static void CheckIfFriendListed(VRC.Player player)
        {
            if (!APIUser.IsFriendsWith(player.prop_APIUser_0.id))
            {
                Kick.KickUser(player);
                MelonLogger.Msg(ConsoleColor.Red, $"{player.field_Private_APIUser_0.displayName} was not your friend and got kicked! User will be able to rejoin in 1 hour!");
            }
            else
            {
                MelonLogger.Msg(ConsoleColor.Green, $"{player.field_Private_APIUser_0.displayName} is your friend and was able to join!");
            }
        }

        public static string GetRank(string[] tags)
        {
            string rank = string.Empty;

            if (tags.Contains("system_trust_veteran"))
            {
                rank += "Trusted User";
            }
            else if (tags.Contains("system_trust_trusted"))
            {
                rank += "Known User";
            }
            else if (tags.Contains("system_trust_known"))
            {
                rank += "User";
            }
            else if (tags.Contains("system_trust_basic"))
            {
                rank += "New User";
            }
            else
            {
                rank += "Visitor";
            }
            return rank;
        }

        internal static void CheckTrustLevel(VRC.Player player)
        {
            var rank = GetRank(player.field_Private_APIUser_0.tags.ToArray());
            if (!Load.visitorsAllowed && rank == "Visitor")
            {
                KickUser(player);
                MelonLogger.Msg(ConsoleColor.Red, $"{player.field_Private_APIUser_0.displayName} is {rank} rank and got kicked! User will be able to rejoin in 1 hour!");
            }
            else if (!Load.newusersAllowed && rank == "New User")
            {
                KickUser(player);
                MelonLogger.Msg(ConsoleColor.Red, $"{player.field_Private_APIUser_0.displayName} is {rank} rank and got kicked! User will be able to rejoin in 1 hour!");
            }
            else if (!Load.usersAllowed && rank == "User")
            {
                KickUser(player);
                MelonLogger.Msg(ConsoleColor.Red, $"{player.field_Private_APIUser_0.displayName} is {rank} rank and got kicked! User will be able to rejoin in 1 hour!");
            }
            else if (!Load.knownusersAllowed && rank == "Known User")
            {
                KickUser(player);
                MelonLogger.Msg(ConsoleColor.Red, $"{player.field_Private_APIUser_0.displayName} is {rank} rank and got kicked! User will be able to rejoin in 1 hour!");
            }
            else if (!Load.trustedusersAllowed && rank == "Trusted User")
            {
                KickUser(player);
                MelonLogger.Msg(ConsoleColor.Red, $"{player.field_Private_APIUser_0.displayName} is {rank} rank and got kicked! User will be able to rejoin in 1 hour!");
            }
            else
            {
                MelonLogger.Msg(ConsoleColor.Green, $"{player.field_Private_APIUser_0.displayName} is {rank} and was allowed to join!");
            }
        }
    }
}
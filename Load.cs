using KiraiMod.WingAPI;
using KiraiMod.WingAPI.RawUI;
using MelonLoader;
using System;
using System.Collections;
using UnityEngine;
using static KiraiMod.WingAPI.Wing;

namespace AutoModeration
{
    internal class Load : MelonMod
    {
        public static bool whitelistEnabled = false;
        public static bool blacklistEnabled = false;
        public static bool TrustBasedEnabled = false;
        public static bool visitorsAllowed = false;
        public static bool newusersAllowed = false;
        public static bool usersAllowed = false;
        public static bool knownusersAllowed = false;
        public static bool trustedusersAllowed = false;
        public static bool friendsOnlyAllowed = false;
        public static bool modActive = false;
        private static bool hasInitialized = false;

        public static Action<BaseWing> OnWingInit = new Action<BaseWing>(_ =>
              {
                  WingPage MainPage = Left.CreatePage("Kick-Mod");
                  WingToggle ModActivated = MainPage.CreateToggle("ON/OFF", 0, UnityEngine.Color.green, UnityEngine.Color.red, false, new System.Action<bool>(state => modActive = state));
                  WingToggle Whitelist = MainPage.CreateToggle("White-List", 1, UnityEngine.Color.green, UnityEngine.Color.red, false, new System.Action<bool>(state => whitelistEnabled = state));
                  WingToggle Blacklist = MainPage.CreateToggle("Black-List", 2, UnityEngine.Color.green, UnityEngine.Color.red, false, new System.Action<bool>(state => blacklistEnabled = state));
                  WingToggle Friends = MainPage.CreateToggle("Friends-Only", 3, UnityEngine.Color.green, UnityEngine.Color.red, false, new System.Action<bool>(state => friendsOnlyAllowed = state));
                  WingToggle TrustLevel = MainPage.CreateToggle("Trust-Based", 4, UnityEngine.Color.green, UnityEngine.Color.red, false, new System.Action<bool>(state => TrustBasedEnabled = state));
                  WingPage TrustLevelPage = MainPage.CreateNestedPage("Trust-Levels", 5);

                  WingToggle Visitors = TrustLevelPage.CreateToggle("Visitors", 0, UnityEngine.Color.green, UnityEngine.Color.red, false, new System.Action<bool>(state => visitorsAllowed = state));
                  WingToggle NewUsers = TrustLevelPage.CreateToggle("New-Users", 1, UnityEngine.Color.green, UnityEngine.Color.red, false, new System.Action<bool>(state => newusersAllowed = state));
                  WingToggle Users = TrustLevelPage.CreateToggle("User", 2, UnityEngine.Color.green, UnityEngine.Color.red, false, new System.Action<bool>(state => usersAllowed = state));
                  WingToggle KnownUsers = TrustLevelPage.CreateToggle("Known-Users", 3, UnityEngine.Color.green, UnityEngine.Color.red, false, new System.Action<bool>(state => knownusersAllowed = state));
                  WingToggle TrustedUsers = TrustLevelPage.CreateToggle("Trusted-Users", 4, UnityEngine.Color.green, UnityEngine.Color.red, false, new System.Action<bool>(state => trustedusersAllowed = state));
              });

        public override void OnApplicationStart()
        {
            Initialize();
            Client.FileManager.DoFileStuff();
            Client.Patch.InitPatches();
        }

        public static void Initialize()
        {
            if (hasInitialized) return;
            hasInitialized = true;

            MelonCoroutines.Start(FindUI());
        }

        private static IEnumerator FindUI()
        {
            while ((Wing.Misc.UserInterface = GameObject.Find("UserInterface")?.transform) is null)
                yield return null;

            while ((Wing.Misc.QuickMenu = Wing.Misc.UserInterface.Find("Canvas_QuickMenu(Clone)")) is null)
                yield return null;

            Left.Setup(Wing.Misc.QuickMenu.Find("Container/Window/Wing_Left"));
            Right.Setup(Wing.Misc.QuickMenu.Find("Container/Window/Wing_Right"));

            Left.WingOpen.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(new Action(() => Init_L()));
            Right.WingOpen.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(new Action(() => Init_R()));
        }

        private static Action Init_L = new Action(() =>
              {
                  Init_L = new Action(() => { });
                  MelonLogger.Msg("Creating Left Wing UI");
                  OnWingInit(Left);
              });

        private static Action Init_R = new Action(() =>
              {
                  Init_R = new Action(() => { });
                  MelonLogger.Msg("Creating Right Wing UI");
                  OnWingInit(Right);
              });
    }
}
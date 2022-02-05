using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoModeration.Client
{
    internal class FileManager
    {
        private static string _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "Moderation");
        public static string _whiteListPath = Path.Combine(_rootPath, "WhiteList.txt");
        public static string _BlackListPath = Path.Combine(_rootPath, "BlackList.txt");
        public static void DoFileStuff() {

            Directory.CreateDirectory(_rootPath);

            if (!File.Exists(_whiteListPath))
            {
                File.CreateText(_whiteListPath);
                global::MelonLoader.MelonLogger.Msg(ConsoleColor.DarkYellow, "Whitelist not found! Generating one at: " + _whiteListPath + "!..");
            }
            else
            {
                global::MelonLoader.MelonLogger.Msg(ConsoleColor.Green, $"Whitelist found!.. White-listed user count: {File.ReadAllLines(_whiteListPath).Length}");
            }

            if (!File.Exists(_BlackListPath))
            {
                File.CreateText(_BlackListPath);
                global::MelonLoader.MelonLogger.Msg(ConsoleColor.DarkYellow, "Blacklist not found! Generating one at: " + _BlackListPath + "!..");
            }
            else
            {
                global::MelonLoader.MelonLogger.Msg(ConsoleColor.Green, $"Blacklist found!.. Black-listed user count: {File.ReadAllLines(_BlackListPath).Length}");
            }
        }
    }
}


using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

[assembly: MelonInfo(typeof(AstralBypass_Updater.AstralBypass_Updater), "AstralBypass_Updater", "1.0.0", "Viper")]
[assembly: MelonColor(ConsoleColor.DarkMagenta)]

namespace AstralBypass_Updater
{
    public partial class AstralBypass_Updater : MelonPlugin
    {
        public override void OnApplicationEarlyStart()
        {

            string latestVersion = GetLatestVersion();
            Update(latestVersion);

        }
        private string GetLatestVersion()
        {
            string githubResponse;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/Astrum-Project/AstralBypass/releases/latest");
                request.Method = "GET";
                request.KeepAlive = true;
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = $"AstralBypass_Updater/1.0.0";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader requestReader = new StreamReader(response.GetResponseStream()))
                {
                    githubResponse = requestReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error("Failed to fetch latest plugin version info from github:\n" + e);
                return null;
            }

            JObject obj = JsonConvert.DeserializeObject(githubResponse) as JObject;

            return obj.GetValue("tag_name")?.ToString();
        }
        private void Update(string version)
        {
            byte[] data;
            using (WebClient wc = new WebClient())
            {
                data = wc.DownloadData($"https://github.com/Astrum-Project/AstralBypass/releases/download/{version}/AstralBypass.dll");
            }

            File.WriteAllBytes(MelonHandler.PluginsDirectory + "/AstralBypass.dll", data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBLXAuthy
{
    public class RBLXAuth
    {

        public static string user;
        public static string pastebin;
        public static string id;


        /// <summary>
        /// Initialize RBLXAuthy
        /// </summary>
        public static void init()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Roblox\RobloxStudioBrowser\roblox.com", false))
                {
                    string cookie = key.GetValue(".ROBLOSECURITY").ToString();
                    cookie = cookie.Substring(46).Trim('>');
                    cookie = ".ROBLOSECURITY=" + cookie;
                    WebClient wc = new WebClient();
                    wc.Headers["Cookie"] = cookie;
                    string robloxinfodata = wc.DownloadString("https://users.roblox.com/v1/users/authenticated");
                    dynamic JSON = JObject.Parse(robloxinfodata);
                    user = JSON.name;
                    id = JSON.id;
                }
                
                
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        /// <summary>
        /// Get Roblox Avatar Link
        /// </summary>
        /// <returns>The link to the users avatar </returns>
        public static string get_avatar()
        {
            WebClient wc = new WebClient();
            string jsonpfp = wc.DownloadString($"https://thumbnails.roblox.com/v1/users/avatar-headshot?userIds={id}&size=150x150&format=Png&isCircular=true");
            dynamic Data = JObject.Parse(jsonpfp);
            
            return Data["data"][0]["imageUrl"].ToString();
        }
        /// <summary>
        /// Get Roblox Username
        /// </summary>
        /// <returns>Username of client.</returns>
        public static string get_username()
        {
            return user;
        }

        /// <summary>
        /// Checks if a user is whitelisted.
        /// </summary>
        /// <returns>Returns true or false</returns>
        public static bool IsWhitelisted()
        {
            WebClient wc = new WebClient();
            string authenticated = wc.DownloadString(pastebin);
            if(authenticated.Contains(user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gives Users HWID
        /// </summary>
        /// <returns>Returns HWID</returns>

        public static string get_hwid()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
        }
    }
}

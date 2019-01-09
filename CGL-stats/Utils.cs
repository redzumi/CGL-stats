using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CGL_stats
{
    class Utils
    {

        public static string API_AUTH_URL = "https://csgolounge.com/v1/";
        public static string API_MATCHES_URL = "https://csgolounge.com/v1/matches/";

        public static string getPage(string url, string method = "GET", string sessionToken = null)
        {
            string content = String.Empty;

            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = method;

                if (sessionToken != null)
                {
                    // API works only with auth, idk for what
                    request.Headers.Add("Authorization", "JWT " + sessionToken);
                }

                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();

                using (StreamReader sr = new StreamReader(data))
                    content = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return content;
        }

        public static Session getSession()
        {
            string response = Utils.getPage(API_AUTH_URL, "POST");
            Authentication auth = JsonConvert.DeserializeObject<Authentication>(response);
            return auth.session;
        }

        public static CGLMatches getCGLMatches(string sessionToken)
        {
            string response = Utils.getPage(API_MATCHES_URL, "GET", sessionToken);
            CGLMatches matches = JsonConvert.DeserializeObject<CGLMatches>(response);
            return matches;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL_stats
{
    class Worker
    {
        public void init()
        {
            Session session = Utils.getSession();
            CGLMatches cglMatches = Utils.getCGLMatches(session.token); // TODO: CSGL, lol

            List<Match> matches = getMatchesFromCGL(cglMatches);

            // Utils.log("Handled " + matches.Count + " matches.");
            // Utils.log("Done.");
        }

        public List<Match> getMatchesFromCGL(CGLMatches cglMatches)
        {
            return null;
        }

        // public List<Match> getMatches()
        // {
        //     List<Match> matches = new List<Match>();

        //     string jsonResponse = Utils.getPage(API_MATCHES);
        //     APIResponse response = JsonConvert.DeserializeObject<APIResponse>(jsonResponse);

        //     return matches;
        // }

    }
}

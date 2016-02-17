using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGL_stats
{
    class Worker
    {

        public string rawHtml = String.Empty;

        public void init()
        {
            List<MatchInfo> matches = handleMatches(getMatches());

            Utils.log("Handled " + matches.Count + " matches.");

            foreach (MatchInfo match in matches)
            {
                Utils.writeMatch(match);
            }

            Utils.log("Remaking stats info.");

            reStat();

            Utils.log("Done.");
        }


        public void reStat()
        {
            List<string> betsLines = Utils.getLines(Settings.betsFile);

            int total = 0;
            int win = 0;
            int lose = 0;

            string newBets = String.Empty;

            foreach(string line in betsLines)
            {
                if (line.Contains("(win)"))
                    total++;
                if (line.Contains("+++"))
                    win++;
                if (line.Contains("---"))
                    lose++;
            }

            string newLine = String.Empty;
            foreach (string line in betsLines)
            {
                newLine = line;

                if (newLine.Contains("total: "))
                    newLine = "total: " + total;
                if (newLine.Contains("win: "))
                    newLine = "win: " + win;
                if (newLine.Contains("lose: "))
                    newLine = "lose: " + lose;

                if(newBets.Length > 1)
                    newBets = newBets + Environment.NewLine + newLine;
                else
                    newBets = line;
            }

            Utils.writeToFile(Settings.betsFile, newBets, true);
        }


        public List<MatchInfo> handleMatches(List<string> htmlMatches)
        {
            Utils.log("Getted " + htmlMatches.Count + " matches.");

            List<MatchInfo> matches = new List<MatchInfo>();

            foreach(string matchHtml in htmlMatches) {
                if(matchHtml.Contains("<span class=\"format\">") && !matchHtml.Contains("match notavailable")) //if match exist and not played
                {
                    MatchInfo matchInfo = new MatchInfo();

                    HtmlAgilityPack.HtmlDocument rawMatch = new HtmlAgilityPack.HtmlDocument();
                    rawMatch.LoadHtml(matchHtml);

                    //map info
                    string maps = rawMatch.DocumentNode.SelectNodes("//span[@class='format']")[0].InnerText;
                    matchInfo.mapsCount = maps;

                    //team info
                    var teams = rawMatch.DocumentNode.SelectNodes("//div[@class='teamtext']");
                    matchInfo.firstTeam = Utils.getBetween(teams[0].InnerHtml, "<b>", "</b>");
                    matchInfo.firstPercent = Utils.getBetween(teams[0].InnerHtml, "<i>", "</i>").Replace("%", "");
                    matchInfo.secondTeam = Utils.getBetween(teams[1].InnerHtml, "<b>", "</b>");
                    matchInfo.secondPercent = Utils.getBetween(teams[1].InnerHtml, "<i>", "</i>").Replace("%", "");

                    //link - <a href="match?m=1234">
                    var id = rawMatch.DocumentNode.Descendants("a").Where(d =>
                        d.Attributes.Contains("href") && d.Attributes["href"].Value.Contains("match?m=")); //match link
                    matchInfo.matchId = id.ToList()[0].GetAttributeValue("href", "0");

                    matches.Add(matchInfo);
                }
            }

            return matches;
        }

        public List<string> getMatches()
        {
            List<string> matches = new List<string>();

            rawHtml = Utils.getPage(Settings.csgoUrl);

            HtmlAgilityPack.HtmlDocument rawDoc = new HtmlAgilityPack.HtmlDocument();
            rawDoc.LoadHtml(rawHtml);

            foreach (HtmlNode rawMatch in rawDoc.DocumentNode.SelectNodes("//div[@class='matchmain']"))
            {
                matches.Add(rawMatch.InnerHtml);
            }

            return matches;
        }

    }
}

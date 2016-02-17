using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CGL_stats
{
    class Utils
    {

        public static string getPage(string url)
        {
            string content = String.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();

                Stream data = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(data))
                    content = sr.ReadToEnd();
            }
            catch(Exception ex)
            {
                log("ERROR::" + ex.Message);
            }
            return content;
        }

        public static List<string> getLines(string fileName)
        {
            List<string> lines = new List<string>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                while (true)
                {
                    string line = reader.ReadLine();

                    if (line == null)
                        break;

                    lines.Add(line);
                }
            }

            return lines;
        }

        public static string readFile(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        public static string getBetween(string value, string a, string b)
        {
            int posA = value.IndexOf(a);
            int posB = value.LastIndexOf(b);
            if (posA == -1) return "";
            if (posB == -1) return "";
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB) return "";
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        public static bool matchAlreadyWrited(string matchId)
        {
            string bets = readFile(Settings.betsFile);

            if (bets.Contains(matchId))
                return true;
            else
                return false;
        }

        public static string getMatchLine(MatchInfo match)
        {
            int totalLenght = 100;

            int firstPercPos = 55;
            int secondPercPos = 115;

            string firstPart = "http://csgolounge.com/" + match.matchId + "  " + match.mapsCount.Replace("BO", "") + "  " + match.firstTeam;
            firstPart = firstPart + getSpaces(firstPercPos - firstPart.Length) + match.firstPercent;

            string secondPart = match.secondTeam;
            secondPart = firstPart + getSpaces(totalLenght - firstPart.Length - 2) + "- " + secondPart;


            secondPart = secondPart + getSpaces(secondPercPos - secondPart.Length) +  " " + match.secondPercent;

            return secondPart;
        }

        public static string getSpaces(int count)
        {
            string spaces = String.Empty;
            for (int i = 0; i <= count; i++)
                spaces = spaces + " ";
            return spaces;
        }

        public static void writeMatch(MatchInfo match)
        {
            if (matchAlreadyWrited(match.matchId))
                return;
            string matchLine = getMatchLine(match);

            writeToFile(Settings.betsFile, Environment.NewLine + matchLine, false);
        }

        public static void writeToFile(string fileName, string lines, bool overwrite)
        {
            StreamWriter log;

            if (!File.Exists(fileName))
            {
                log = new StreamWriter(fileName);
                log.WriteLine(lines);
                log.Close();
            }
            else
                if(overwrite)
                    File.WriteAllText(fileName, lines);
                else
                    File.AppendAllText(fileName, lines);
        }

        public static void log(string message)
        {
            Console.WriteLine(message);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace BasketballPlayers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Paste the path to the .json file: ");
            var pathJson = Console.ReadLine();

            Console.Write("Type the name of the .json file: ");
            var jsonFileName = Console.ReadLine();

            var jsonString = File.ReadAllText($@"{pathJson}\{jsonFileName}");
            var playersFromJson = JsonConvert.DeserializeObject<Player[]>(jsonString);
            var output = new HashSet<QualifiedPlayer>();

            Console.Write("Type the maximum number of years the player has played in the league in order to qualify: ");
            var maxYearsToQualify = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            Console.Write("The minimum rating the player should have in order to qualify: ");
            var minRatingToQualify = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            Console.Write("Paste the path to the CSV (comma separated value) file that will be generated: ");
            var pathCsv = Console.ReadLine();

            var currentYear = DateTime.Now.Year;

            foreach (var playerInfo in playersFromJson)
            {
                var yearsPlaying = currentYear - playerInfo.PlayingSince;
                if (playerInfo.Rating >= minRatingToQualify && yearsPlaying <= maxYearsToQualify)
                {
                    var eachPlayer = new QualifiedPlayer
                    {
                        Name = playerInfo.Name,
                        Rating = playerInfo.Rating
                    };
                    output.Add(eachPlayer);
                }
            }

            var csv = new StringBuilder();
            csv.AppendLine("Name,Rating");
            foreach (var qPlayer in output.OrderByDescending(r => r.Rating))
            {
                var name = qPlayer.Name;
                var rating = qPlayer.Rating.ToString();
                var newLine = $"{name}, {rating}";
                csv.AppendLine(newLine);
            }
            File.WriteAllText(pathCsv + "\\qualified.csv", csv.ToString());
        }
    }
}
using System;
using System.IO;

namespace VerifyIP
{
    class MainClass
    {
		private static string fileIn = "input.csv";
		private static string fileOut = "out.csv";
		private static string ipIgnore = null;
        public static void Main(string[] args)
        {
			for (var i = 0; i < args.Length; i++)
			{
			    switch(args[i])
				{
					case "-i":
					case "--input":
						fileIn = args[++i]; break;
					case "-o":
					case "--output":
						fileOut = args[++i]; break;
					case "-ip":
						ipIgnore = args[++i]; break;
					default:
						Default(); return;

				}
			}

			if (!File.Exists(fileIn))
			{
				Console.WriteLine($"File \"{fileIn}\" not exists");
				return;
			}

			var a = new Verify(fileIn, fileOut, ipIgnore);
         }

        private static void Default()
		{
			var msg = @"
Application to check IPs from a list of urls.
- Bruno Fagundes (fagundes.bl@gmail.com).

verifyip [params] [file].

Params:
-i,  --input     Input file with the urls in csv. Default: input.csv.
-o, --output     Output file. Default: output.csv 
-ip              Ignore the IP . If the url contain this ip, it not add output file.
";
			Console.WriteLine(msg);
		}
    }
}

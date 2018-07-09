using System;
using System.IO;

namespace VerifyIP
{
    class MainClass
    {
		private static string fileIn = "input.csv";
		private static string fileOut = "out.csv";
		private static string fileError = "error.csv";      
		private static string ipIgnore = "193.120.40.66;5.149.175.107;144.2.243.138";


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
					case "-e":
					case "--error":
						fileError = args[++i]; break;
					default:
						Default(); return;

				}
			}


            //Checking if files exists:
			if (!File.Exists(fileIn))
			{
				Verify.WriteList(fileError, $"The file \"{fileIn}\" not exist");
				Console.WriteLine($"File \"{fileIn}\" not exists");
				return;
			}

			try
            {
				if (File.Exists(fileError))
                    File.Delete(fileError);
				
				if (File.Exists(fileOut))
					File.Delete(fileOut);
            }
            catch (Exception ex)
            {
				Verify.WriteList(fileError, ex.Message);
				Console.WriteLine(ex.Message);
                
                return;
            }

            Verify.Start(fileIn, fileOut, ipIgnore);
			Console.WriteLine("Process finish!");
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
-e, --error      Error file. Default: error.csv
-ip              Ignore the IPs (separed with semicolon) . 
                 If the url contain this ip, it not add output file.
                 Default: 193.120.40.66;5.149.175.107;144.2.243.138
";
			Console.WriteLine(msg);
		}
    }
}

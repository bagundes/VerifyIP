using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Linq;

namespace VerifyIP
{
    public class Verify
    {
        public static void Start(string input, string output, string ip, int timeout = 3 * 1000)
		{
			var urls = ListUrls(input);
			var ips = ip.Split(';');
			var c = 0F;

            foreach(var url in urls)
			{
				Double perc = Math.Round((++c / urls.Count) * 100);

				if (String.IsNullOrEmpty(url))
                    continue;
				            
				Console.WriteLine($"{perc}% - {url}");
               
                var message = String.Empty;
                var ipUrl = String.Empty;

                try
                {
					ipUrl = PingAddress(url,timeout);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
                finally
                {
					if(!ips.Contains(ipUrl))
					    WriteList(output, url, ipUrl, message);
                }
			}
        }

		/// <summary>
        /// Writes in out list.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="ip">Ip.</param>
        /// <param name="message">Message.</param>
		public static void WriteList(string output, string url, string ip, string message)
        {
			string path = @output;
            string appendText = $"{url};{ip};{message}\n";

            if (!File.Exists(path))
            {
                // Create a file to write to.
                string createText = $"URL;IP;Message\n";
                File.WriteAllText(path, createText);
            }

            File.AppendAllText(path, appendText);
        }

        /// <summary>
        /// Writes the list.
        /// </summary>
        /// <param name="output">Output.</param>
        /// <param name="message">Message.</param>
        public static void WriteList(string output, string message)
		{
			if (!File.Exists(output))
            {
				File.WriteAllText(output, message);
            }

			File.AppendAllText(output, message);
		}


		/// <summary>
        /// Lists the urls.
        /// </summary>
        /// <returns>The urls.</returns>
        /// <param name="csv">csv file.</param>
        public static List<string> ListUrls(string csv)
        {
            List<string> list = new List<string>();

            using (var reader = new StreamReader(csv))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        var values = line.Split(';');
                        if (values.Length > 0)
                            list.AddRange(values);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Inform the IP address
        /// </summary>
        /// <returns>The address.</returns>
        /// <param name="url">URL.</param>
        /// <param name="timeout">Timeout. (default 3 seconds)</param>
        public static string PingAddress(string url, int timeout = 3 * 1000)
        {
            var ip = String.Empty;
            var pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;
         
			try
			{
				var reply = pingSender.Send(url, timeout);

                switch(reply.Status)
				{
					case IPStatus.Success:
						ip = reply.Address.ToString();
						Console.WriteLine( " -> {0}", reply.Address.ToString()); break;	
					case IPStatus.TtlExpired:
						throw new Exception("Not possible connect the domain");
					case IPStatus.TimeExceeded:
						throw new Exception("Timeout");
					default:
						throw new Exception($"Other error acurred. Status {reply.Status.ToString()}");
				}
			}catch(SocketException se)
			{
				throw new Exception(se.Message);
			}
            
            return ip;
        }
    }
}

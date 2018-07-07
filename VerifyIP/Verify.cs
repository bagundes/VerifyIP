using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace VerifyIP
{
    public class Verify
    {
        public Verify(string input, string output, string ip, int timeout = 3 * 1000)
        {
			var urls = ListUrls(input);
			ip = ip ?? ".";

            foreach(var url in urls)
			{
				if (String.IsNullOrEmpty(url))
					continue;
				
				Console.WriteLine($"Test {url}");
                var message = String.Empty;
                var ipUrl = String.Empty;
                try
                {
					ipUrl = PingAddress(url,timeout);
                    Console.Write($" - {url}");
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
                finally
                {
					if(ip != ipUrl)
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
		private void WriteList(string output, string url, string ip, string message)
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
        /// Lists the urls.
        /// </summary>
        /// <returns>The urls.</returns>
        /// <param name="csv">csv file.</param>
        private List<string> ListUrls(string csv)
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
        private string PingAddress(string url, int timeout = 3 * 1000)
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
				if (reply.Status == IPStatus.Success)
				{
					ip = reply.Address.ToString();
					Console.WriteLine("{0} -> {1}",url, reply.Address.ToString());
                    //Console.WriteLine("Address: {0}", reply.Address.ToString());
					//Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
					//Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
					//Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
					//Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
					//Console.WriteLine("\n");
				}
			}catch(SocketException se)
			{
				throw new Exception(se.Message);
			}


            return ip;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
//--created by Paul Zänker

namespace Async_Await
{
    class Program : CalculatePi
    {
        public static new Stopwatch stopWatch = new Stopwatch();
        const string url = "https://www.angio.net/pi/digits/50.txt";

        static async Task<double> CalcPiAsync()
        {
            Task<double> t = new Task<double>(() => {
                CalculatePi cp = new CalculatePi();
                return cp.calculatePi();
            });

            t.Start();
            double result = await t;
            return result;
        }

        static async Task<double> ReadPiAsync()
        {
            Task<double> t = new Task<double>(() => {
                ReadFromNetworkAsync(url);
                return 0;
            });

            t.Start();
            double result = await t;
            return result;
        }

        private static async void ReadFromNetworkAsync(string url)
        {
            var client = new WebClient();

            stopWatch.Start();
            string result = await client.DownloadStringTaskAsync(url);
            Console.WriteLine("{0}", result);
            Console.WriteLine(">> Pi heruntergeladen in " +stopWatch.ElapsedMilliseconds + " ms\n");
            stopWatch.Stop();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Pi wird gelesen ...");
            Console.WriteLine("Pi wird berechnet ...\n");
            Task<double> t_calc = CalcPiAsync();
            Task<double> t_read = ReadPiAsync();
            Task.WaitAll(new Task[] { t_calc, t_read });

            Console.WriteLine("Differenz zwischen den Werten = {0}\n", t_read.Result - t_calc.Result);
        }
    }
}

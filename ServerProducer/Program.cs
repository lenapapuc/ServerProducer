using System;
using System.Threading;

namespace DiningHallPr
{
    class Program
    {
        
        private static bool _keepRunning = true;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                Program._keepRunning = false;
            };

            Console.WriteLine("Starting HTTP listener...");
            var httpServer = new Server();
            
            Threads threadGeneration = new Threads();
            
            foreach (var thread in  threadGeneration.ExtractThreads())
            {
                thread.Start();
            }
            foreach (var thread in threadGeneration.GenerateThreads())
            {
                thread.Start();
            }
            
            httpServer.Start();

            while (Program._keepRunning)
            {
                
            }
            
            httpServer.Stop();
            foreach (var thread in  threadGeneration.ExtractThreads())
            {
                thread.Abort();
            }
            foreach (var thread in threadGeneration.GenerateThreads())
            {
                thread.Abort();
            }
            Console.WriteLine("Exiting gracefully...");
        }
    }
}
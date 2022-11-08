using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiningHallPr
{
    public class Threads
    {
        private static Mutex mut = new Mutex();
        public static Queue<string> newQueue = new Queue<string>();
        public void Generation()
        {
            while (true)
            {
                mut.WaitOne();
               
                string str = RandomString.RandomStrings(6);
                newQueue.Enqueue(str);
                mut.ReleaseMutex();
                Thread.Sleep(5000);
            }
        }
        public void Extractor()
        {
          
           //Thread.Sleep(1000);
           string str = string.Empty;
            
            while (true)
            {
                mut.WaitOne();
                if (newQueue.Count > 0)
                {
                    str = newQueue.Dequeue();
                 
                }
                mut.ReleaseMutex();
                using var client = new HttpClient();
                //var json = JsonConvert.SerializeObject(str);
                if (str != null)
                {
                    var data = new StringContent(str, Encoding.UTF8, "application/json");
                    client.PostAsync("http://localhost:8100/dinninghall", data);
                }

                Thread.Sleep(1000);
                
            }
        }
        
        public List<Thread> GenerateThreads()
        {
            List<Thread> list = new List<Thread>();
            int thread_generators = 7;
            for (int i = 0; i < thread_generators; i++)
            {
                
                Thread thread = new Thread(Generation);
                list.Add(thread);
                
            }
            return list;
        }

        public List<Thread> ExtractThreads()
        {
            int thread_extractors = 3;
            List<Thread> list = new List<Thread>();
            for (int i = 0; i < thread_extractors; i++)
            {
                Thread thread = new Thread(Extractor);
                list.Add(thread);
            }
            return list;
        }
    }
}
using System;
using System.Threading;
using Coflnet;
using Coflnet.Server;

namespace wow
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            FileController.dataPaht += "/wow";
            Console.WriteLine(FileController.dataPaht);

            var server = CoflnetSocket.socketServer;
            server.AddWebSocketService<WoWProxy>("/wow",()=>{
                var s = new WoWProxy();
                return s;
            });

            ServerCore.Init(new SourceReference(1,0));
            Console.WriteLine("just sleeping a bit");

            for (long i = 0; i < long.MaxValue; i++)
            {
                Console.Write("a");
                Thread.Sleep(1000);
            }
        }
    }
}

namespace UnityEngine
{
    public class Debug
    {
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }

        public static void Log(int i)
        {
            Log(i.ToString());
        }
    }
}

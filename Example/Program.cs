using System;

namespace ExampleCode
{
    class Program
    {
        static TtnLib.TtnClient client;

        static void Main(string[] args)
        {
            client = new TtnLib.TtnClient("lorawan_atmosphere_managment_algebra", "ttn-account-v2.OH-eGEPy5gt9BTnEetm9WTgEnxoCSwW_d3FIk9P7K2c");
            try
            {
                client.ConnectAsync().Wait();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message); ;
            }
           
            if (client.Connected)
            {
                client.SubscribeToChanelAsync("smt32").Wait();
            }

            client.OnMessageReceived += A_OnMessageReceived;

            try
            {
                client.SendMessageAsync("smt32", "AA", 5, true).Wait();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            
            Console.ReadKey();
        }

        private static void A_OnMessageReceived(string message)
        {
            Console.WriteLine(message);
        }
    }
}

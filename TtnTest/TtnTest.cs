using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using TtnLib;
namespace TtnTest
{
    [TestClass]
    public class TtnTest
    {
        [TestMethod]
        public async Task TestConnection()
        {
            TtnClient client = new TtnClient("lorawan_atmosphere_managment_algebra", "ttn-account-v2.OH-eGEPy5gt9BTnEetm9WTgEnxoCSwW_d3FIk9P7K2c", "eu");
            try
            {
                await client.ConnectAsync();
            }
            catch (System.Exception)
            {

                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task TestSendMessage()
        {
            TtnClient client = new TtnClient("lorawan_atmosphere_managment_algebra", "ttn-account-v2.OH-eGEPy5gt9BTnEetm9WTgEnxoCSwW_d3FIk9P7K2c", "eu");
            try
            {
                await client.ConnectAsync();
            }
            catch (System.Exception)
            {

                Assert.Fail();
            }
           
            try
            {
                await client.SendMessageAsync(deviceName: "smt32", message: "test",5,false);
            }
            catch (System.Exception)
            {

                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task TestReciveMessageAsync()
        {
            int counter = 0;
            TtnClient client = new TtnClient("lorawan_atmosphere_managment_algebra", "ttn-account-v2.OH-eGEPy5gt9BTnEetm9WTgEnxoCSwW_d3FIk9P7K2c", "eu");
            await client.ConnectAsync();
            if (client.Connected)
            {
                await client.SubscribeToChanelAsync("smt32");
            }

            client.OnMessageReceived += delegate (string s) { counter++; };

            //We expect answer in period of 60s
            Thread.Sleep(60000);
            Assert.AreNotEqual(0, counter);
        }
    }
}

using MQTTnet.AspNetCore;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using MQTTnet;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Text.Encodings;

namespace TtnLib
{
    public class TtnClient
    {
        #region Fields
        private IMqttClient _client;
        private IMqttClientOptions _clientOptions;
        private string _aplicationName;
        private string _accessKey;
        private string _region;
        private const string _broker_url = ".thethings.network";
        public bool Connected { get { return _client.IsConnected; } }
        #endregion

        #region Event definition
        public delegate void ConnectedEventHandler(object sender, MqttClientConnectedEventArgs args);
        public event ConnectedEventHandler OnConnected;
        public delegate void DisconnectedEventHandler(object sender, MqttClientDisconnectedEventArgs args);
        public event DisconnectedEventHandler OnDisconnected;
        public delegate void MessageReceivedEventHandler(string message);
        public event MessageReceivedEventHandler OnMessageReceived;
        #endregion

        #region Constructor
        public TtnClient(string aplicationName, string accessKey, string region = "eu")
        {
            _aplicationName = aplicationName;
            _accessKey = accessKey;
            _region = region;

            _clientOptions = new MqttClientOptionsBuilder()
                                .WithTcpServer($"{region}{_broker_url}")
                                .WithTls()
                                .WithCredentials(_aplicationName, _accessKey)
                                .Build();

         
             _client = new MqttFactory().CreateMqttClient();
            
             _client.Connected += MqttClient_Connected;
             _client.Disconnected += MqttClient_Disconnected;
             _client.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;  
        }
        #endregion

        #region Events
        private void MqttClient_Disconnected(object sender, MqttClientDisconnectedEventArgs e)
            => OnDisconnected?.Invoke(sender, e);

        private void MqttClient_Connected(object sender, MqttClientConnectedEventArgs e)
            => OnConnected?.Invoke(sender, e);

        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
            => OnMessageReceived?.Invoke(e.ApplicationMessage.ConvertPayloadToString());
        #endregion

        #region Methods
        public async Task ConnectAsync()
        {
            try
            {
                await _client.ConnectAsync(_clientOptions);
            }
            catch (Exception)
            {

                throw;
            }
        }
            
        public async Task<IList<MqttSubscribeResult>> SubscribeToChanelAsync(string deviceName)
            => await _client.SubscribeAsync($"{_aplicationName}/devices/{deviceName}/up");

        public async Task UnsubscribeFromChanelAsync(string deviceName)
            => await _client.UnsubscribeAsync($"{_aplicationName}/devices/{deviceName}/up");

        public async Task SendMessageAsync(string deviceName, string message, int port = 1, bool confirmed = false)
        {
            var mqttMessage = new MqttApplicationMessageBuilder()
                        .WithTopic($"{_aplicationName}/devices/{deviceName}/down")
                        .WithPayload(JsonConvert.SerializeObject(
                            new Message() {
                                confirmed = confirmed,
                                payload_raw =Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(message)),
                                port = port
                            }))
                        .Build();
            try
            {
                await _client.PublishAsync(mqttMessage);
            }
            catch (Exception)
            {

                throw;
            }
        } 
        #endregion
    }
}


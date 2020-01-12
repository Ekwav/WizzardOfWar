using Coflnet;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using System.Text;
using System;
using Coflnet.Serializer;

namespace wow
{
    public class ProxyMessageData
    {
        public string type;

        public string data;

        public T GetAs<T>()
        {
            return JsonConvert.DeserializeObject<T>(
                Encoding.UTF8.GetString(
                    Convert.FromBase64String(data)));
        }

        public static ProxyMessageData Create<T>(string type, T data)
        {
            return new ProxyMessageData(){
                type=type,
                data = JsonConvert.SerializeObject(data)
            };
        }

        public ServerMessageData ToMessageData(SourceReference target,SourceReference sender,IClientConnection connection)
        {

            var value =  new ProxyServerMessageData()
            {
                message = Encoding.UTF8.GetBytes(data),
                type = type,
                sId = sender,
                rId = target,
                Connection = connection
            };

            

            return value;
        }
    }

    public class ProxyServerMessageData : ServerMessageData
    {
        public override T GetAs<T>()
        {
            return JsonConvert.DeserializeObject<T>(
                Encoding.UTF8.GetString(
                    Convert.FromBase64String(Data)));
        }
    }

    public class JsonNetEncoder : ISerializer
    {
        public T Deserialize<T>(byte[] args)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize<T>(T target)
        {
            
            return  Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(target));
        }
    }

    public class WoWProxy : CoflnetWebsocketServer
    {
        public SourceReference currentTarget = new SourceReference(1,0);
        public SourceReference senderIdentity = SourceReference.NextLocalId;

        protected override void OnOpen()
        {
            this.Encoder = new JsonNetEncoder();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            // try to parse and execute the command sent
            try
            {
                UnityEngine.Debug.Log(e.Data);
                var proxyData = JsonConvert.DeserializeObject<ProxyMessageData>(e.Data);
                ServerMessageData messageData = proxyData.ToMessageData(currentTarget,senderIdentity,this);
                
                ReferenceManager.Instance.ExecuteForReference(messageData);
            }
            catch (CoflnetException ex)
            {
                var data = JsonConvert.SerializeObject(new CoflnetExceptionTransmit(ex));
                Send(data);
                Track.instance.Error(ex.Message, e.Data, ex.StackTrace);
            }    
        }

        public void SendBack(ProxyMessageData data)
        {
            Send(JsonConvert.SerializeObject(data));
        }

        public override void SendBack(MessageData data)
        {
            // ignore standard send
            Console.WriteLine("Stopped " + data.type);
        }

    }
}
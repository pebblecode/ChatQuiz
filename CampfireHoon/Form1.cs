using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CampfireHoon
{
    public partial class Form1 : Form
    {
        private readonly CampfireRoom _room;

        public Form1()
        {
            InitializeComponent();
            AccountConfig config = new AccountConfig { AccountName = "pebbleit", AuthToken = "fee994663c634db07ea450f0b1de0cdbbc583d61" };
            _room = new CampfireRoom(config, 536178, true);
            _room.DataEmitted += PrintLine;
        }

        private void PrintLine(ChatMessage message)
        {
            this.textBox1.AppendText(message.ToString());
            this.textBox1.SelectionStart = this.textBox1.Text.Length - 1;
            this.textBox1.SelectionLength = 0;
            this.textBox1.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _room.Join();
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                _room.Say(textBox2.Text);
                textBox2.Text = string.Empty;
            }
        }
    }

    public class ChatMessage
    {
        public string Body { get; set; }
        public string Username { get; set; }
        public override string ToString()
        {
            return string.Format("{0}: {1}\r\n", Username, Body);
        }
    }

    //{"room_id":1,"created_at":"2009-12-01 23:44:40","body":"hello","id":1, "user_id":1,"type":"TextMessage","starred":"true"}
    internal class ChatMessageDTO
    {
        public int room_id { get; set; }
        public DateTime created_at { get; set; }
        public string body { get; set; }
        public int id { get; set; }
        public int user_id { get; set; }
        public string type { get; set; }
        
    }

    internal class UserInfo
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class AccountConfig
    {
        public string AccountName { get; set; }
        public string AuthToken { get; set; }
    }

    public delegate void ChatMessageEventHandler(ChatMessage message);

    public class CampfireRoom
    {
        private readonly AccountConfig _config;
        private readonly int _roomId;
        private readonly bool _useSsl;
        private readonly Dictionary<int, UserInfo> _userMap = new Dictionary<int, UserInfo>();

        public event ChatMessageEventHandler DataEmitted;

        public CampfireRoom(AccountConfig config, int roomId, bool useSsl)
        {
            _config = config;
            _roomId = roomId;
            _useSsl = useSsl;
        }

        public async void Say(string message)
        {
            string url = string.Format("{0}{1}.campfirenow.com/room/{2}/speak.xml", Scheme, _config.AccountName, _roomId);
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/xml";
            request.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", EncodedAuthToken);
            request.Accept = "application/xml";

            using (var sw = new XmlTextWriter(await request.GetRequestStreamAsync(), Encoding.UTF8))
            {
                sw.WriteStartDocument();
                sw.WriteStartElement("message");
                sw.WriteElementString("type", "TextMessage");
                sw.WriteElementString("body", message);
                sw.WriteEndElement();
                sw.WriteEndDocument();
            }

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse) await request.GetResponseAsync();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                SystemMessage(string.Format("Failed to say '{0}': {1}", message, ex.Message));
            }
        }
        
        public void Join()
        {
            string url = string.Format("{0}{1}.campfirenow.com/room/{2}/join.xml", Scheme, _config.AccountName, _roomId);
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/xml";
            request.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", EncodedAuthToken);
            request.Accept = "application/xml";
            
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }

            LogHttpWebResponse("Room Join", response);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Task.Run(() => PreloadUsers());
                StartStreaming();
            }
        }

        private void PreloadUsers()
        {
            string url = string.Format("{0}{1}.campfirenow.com/room/{2}.json", Scheme, _config.AccountName, _roomId);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "application/json";
            request.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", EncodedAuthToken);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                JObject userDto = JObject.Parse(reader.ReadToEnd());
                JObject body = (JObject)userDto["room"];
                JArray users = (JArray)body["users"];
                foreach (JObject user in users)
                {
                    UserInfo userInfo = new UserInfo { id = (int)user["id"], name = user["name"].ToString() };
                    _userMap.Add(userInfo.id, userInfo);
                }
            }
        }

        private async void StartStreaming()
        {
            string url = string.Format("{0}streaming.campfirenow.com/room/{1}/live.json", Scheme, _roomId);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "application/json";
            request.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", EncodedAuthToken);

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse) request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }

            LogHttpWebResponse("Streaming", response);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                
                while (true)
                {
                    string line = await reader.ReadLineAsync();
                    ChatMessageDTO chatMessage = await ParseChatMessage(line);
                    if (chatMessage != null)
                    {
                        //Ensure we have the details for this user
                        GetUserInfo(chatMessage.user_id);
                        DataEmitted(FormatUserChatMessage(chatMessage));
                    }
                }
            }
        }

        private ChatMessage FormatUserChatMessage(ChatMessageDTO chatDto)
        {
            if (_userMap.ContainsKey(chatDto.user_id))
            {
                UserInfo user = _userMap[chatDto.user_id];
                return new ChatMessage { Username = user.name, Body = chatDto.body };
            }
            return new ChatMessage { Username = chatDto.user_id.ToString(), Body = chatDto.body };
        }

        private void GetUserInfo(int userId)
        {
            if (_userMap.ContainsKey(userId))
            {
                return;
            }

            Task.Run(
                () =>
                {
                    string url = string.Format("{0}{1}.campfirenow.com/users/{2}.json", Scheme, _config.AccountName, userId);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    request.Accept = "application/json";
                    request.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", EncodedAuthToken);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JObject userDto = JObject.Parse(reader.ReadToEnd());
                        JObject body = (JObject)userDto["user"];
                        UserInfo user = new UserInfo { id = userId, name = body["name"].ToString() };
                        _userMap.Add(userId, user);
                    }
                });
        }

        private async Task<ChatMessageDTO> ParseChatMessage(string line)
        {
            try
            {
                return await JsonConvert.DeserializeObjectAsync<ChatMessageDTO>(line.Trim());
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void LogHttpWebResponse(string action,  HttpWebResponse response)
        {
            SystemMessage(string.Format("{0}: {1}", action, response.StatusDescription));
        }
        private void SystemMessage(string message)
        {
            DataEmitted(new ChatMessage { Body = message, Username = "SYSTEM" });
        }

        private string EncodedAuthToken
        {
            get
            {
                var bytesToEncode = Encoding.ASCII.GetBytes(_config.AuthToken + ":X");
                return Convert.ToBase64String(bytesToEncode);
            }
        }

        private string Scheme
        {
            get
            {
                if (_useSsl)
                {
                    return "HTTPS://";
                }
                return "HTTP://";
            }
        }
    }
}

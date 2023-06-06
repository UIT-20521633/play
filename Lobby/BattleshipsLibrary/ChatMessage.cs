using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BattleshipsLibrary
{
    // Tin nhắn trò chuyện, tên người gửi
    [ProtoContract]
    public class ChatMessage
    {
        [ProtoMember(1)]
        public string PlayerName { get; set; }
        [ProtoMember(2)]
        public string Message { get; set; }

        public ChatMessage()
        {
            
        }

        public ChatMessage(string playerName, string message)
        {
            PlayerName = playerName;
            Message = message;
        }

        public override string ToString()
        {
            return $"{PlayerName}: {Message}";
        }
    }
}

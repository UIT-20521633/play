using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BattleshipsLibrary
{
    // Bắt đầu trò chơi, id trò chơi

    [ProtoContract]
    public class GameStartInfo
    {
        [ProtoMember(1)]
        public bool IsStarting { get; set; }
        [ProtoMember(2)]        
        public int GameId { get; set; }

        public GameStartInfo()
        {
            
        }

        public GameStartInfo(bool isStarting, int gameId)
        {
            IsStarting = isStarting;
            GameId = gameId;
        }
    }
}

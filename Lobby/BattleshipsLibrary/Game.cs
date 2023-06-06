using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet;

namespace BattleshipsLibrary
{
    public class Game
    {
        // Client trong game
        public Client Client1 { get; set; }
        public Client Client2 { get; set; }

        // Vị trí tàu
        public List<GridPosition> Client1ShipPositions;
        public List<GridPosition> Client2ShipPositions;

        // Các biến khác
        public bool IsStarted { get; set; }
        public int Id { get; set; }

        private bool _reset;

        public Game()
        {
            
        }

        // Bắt đầu trò chơi
        public void StartGame(int id)
        {
            Id = id;
            IsStarted = true;

            GameStartInfo client1Info = new GameStartInfo(true, id);
            GameStartInfo client2Info = new GameStartInfo(false, id);

            // Gửi thông tin bắt đầu cho client
            NetworkComms.SendObject("GameStartInfo", Client1.Ip, Client1.Port, client1Info);
            NetworkComms.SendObject("GameStartInfo", Client2.Ip, Client2.Port, client2Info);
        }

        // Kiểm tra xem client có trong game không
        public bool HasClient(Client client)
        {
            return (Client1 == client || Client2 == client);
        }

        // Tấn công client 1
        public void FireOnClient1(GridPosition position)
        {
            // tấn công tàu của client
            if (Client1ShipPositions.Any(x => x.Equals(position)))
            {
                // Tìm vị trí đánh
                GridPosition pos = Client1ShipPositions.Find(x => x.Equals(position));
                // Cập nhật thuộc tính khi truy cập
                pos.IsHit = true;

                //Gửi vị trí cho cả hai client + đến lượt họ
                NetworkComms.SendObject("GamePositionUpdateInfo", Client1.Ip, Client1.Port, new GamePositionUpdateInfo(pos, UpdateType.PlayerGrid, false));
                NetworkComms.SendObject("GamePositionUpdateInfo", Client2.Ip, Client2.Port, new GamePositionUpdateInfo(pos, UpdateType.EnemyGrid, true));
            }
            else
            {
                //Tìm một vị trí bị bỏ lỡ
                GridPosition pos = new GridPosition(position.X, position.Y);
                pos.IsMissed = true;

                NetworkComms.SendObject("GamePositionUpdateInfo", Client1.Ip, Client1.Port, new GamePositionUpdateInfo(pos, UpdateType.PlayerGrid, true));
                NetworkComms.SendObject("GamePositionUpdateInfo", Client2.Ip, Client2.Port, new GamePositionUpdateInfo(pos, UpdateType.EnemyGrid, false));
            }

            //Nếu tất cả các vị trí đều bị đánh
            if (Client1ShipPositions.All(x => x.IsHit))
            {
                Console.WriteLine($"(Hra #{Id}) {Client2.Name} vyhrál!");
                NetworkComms.SendObject("EndGameInfo", Client2.Ip, Client2.Port, true); //True - Výhra
                NetworkComms.SendObject("EndGameInfo", Client1.Ip, Client1.Port, false); //False - Prohra

                _reset = true;
            }
        }

        //Tấn công client 2
        public void FireOnClient2(GridPosition position)
        {
            if (Client2ShipPositions.Any(x => x.Equals(position)))
            {
                GridPosition pos = Client2ShipPositions.Find(x => x.Equals(position));
                pos.IsHit = true;

                NetworkComms.SendObject("GamePositionUpdateInfo", Client2.Ip, Client2.Port, new GamePositionUpdateInfo(pos, UpdateType.PlayerGrid, false));
                NetworkComms.SendObject("GamePositionUpdateInfo", Client1.Ip, Client1.Port, new GamePositionUpdateInfo(pos, UpdateType.EnemyGrid, true));
            }
            else
            {
                GridPosition pos = new GridPosition(position.X, position.Y);
                pos.IsMissed = true;

                NetworkComms.SendObject("GamePositionUpdateInfo", Client2.Ip, Client2.Port, new GamePositionUpdateInfo(pos, UpdateType.PlayerGrid, true));
                NetworkComms.SendObject("GamePositionUpdateInfo", Client1.Ip, Client1.Port, new GamePositionUpdateInfo(pos, UpdateType.EnemyGrid, false));
            }

            if (Client2ShipPositions.All(x => x.IsHit))
            {
                Console.WriteLine($"(Hra #{Id}) {Client1.Name} vyhrál!");
                NetworkComms.SendObject("EndGameInfo", Client1.Ip, Client1.Port, true);
                NetworkComms.SendObject("EndGameInfo", Client2.Ip, Client2.Port, false);

                _reset = true;
            }
        }

        public void ResetIfEnd()
        {
            if (!_reset) return;
            Client1 = null;
            Client2 = null;
        }
    }
}

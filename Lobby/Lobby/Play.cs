using BattleshipsLibrary;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet;
using RemoteProcedureCalls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lobby
{
    public partial class Play : Form
    {
        public Play()
        {
            InitializeComponent();
        }

        private void Play_Load(object sender, EventArgs e)
        {

        }
        // Di chuyển trở lại sảnh - đại biểu
        private delegate void GoToLobbyFormDelegate();
        // Di chuyển trở lại sảnh
        private void GoToLobbyForm()
        {
            _closeFromCode = true;
            Close();
            Bảng_lựa_chọn.Show();
        }

        // Đặt lượt của ai - đại biểu
        private delegate void NextTurnDelegate(bool isMyTurn);

        //Thiết lập lượt của ai
        private void NextTurn(bool isMyTurn)
        {
            if (isMyTurn)
            {
                button5.Enabled = true;
                richTextBox1.Text = "Đến lượt bạn!";
            }
            else
            {
                button5.Enabled = false;
                _selectedPosition = null;
                richTextBox1.Text = "Chờ đợi đối thủ...";
            }
        }

        #endregion

        #region Delegate pointery packetů

        //Bắt đầu trò chơi
        private void GameStartInfoDelegatePointer(PacketHeader packetheader, Connection connection, GameStartInfo info)
        {
            _gameId = info.GameId;

            if (info.IsStarting)
            {
                MessageBox.Show("Trò chơi đã bắt đầu, đến lượt bạn!");
                Invoke(new NextTurnDelegate(NextTurn), true);
            }
            else
            {
                MessageBox.Show("Trò chơi đã bắt đầu, đối thủ bắn trước!");
            }
        }

        // Cập nhật sau khi kích hoạt
        private void GamePositionUpdateInfoDelegatePointer(PacketHeader packetheader, Connection connection, GamePositionUpdateInfo info)
        {
            if (info.UpdateType == UpdateType.PlayerGrid)
            {
                GridTile tile = (GridTile)tlpPlayerGrid.GetControlFromPosition(info.GridPosition.X, info.GridPosition.Y);
                int posIndex = _occupiedPositions.FindIndex(x => x.Equals(info.GridPosition));
                if (posIndex >= 0) _occupiedPositions[posIndex] = info.GridPosition;
                tile.GridPosition = info.GridPosition;
                if (tile.GridPosition.IsHit) tile.SetOnFire();
                tile.Invalidate();
            }

            if (info.UpdateType == UpdateType.EnemyGrid)
            {
                GridTile tile = (GridTile)tlpEnemyGrid.GetControlFromPosition(info.GridPosition.X, info.GridPosition.Y);
                tile.GridPosition = info.GridPosition;
                tile.MouseDown -= EnemyGrid_Click;
                tile.Invalidate();
            }

            Invoke(new NextTurnDelegate(NextTurn), info.IsOnTurn);
        }

        //Trò chơi kết thúc
        private void EndGameInfoDelegatePointer(PacketHeader packetheader, Connection connection, bool isWinner)
        {
            MessageBox.Show(isWinner ? "Bạn đã thắng!" : "Bạn đã thua!");
            Invoke(new GoToLobbyFormDelegate(GoToLobbyForm));
        }

        // Ngắt kết nối đối thủ
        private void DisconnectDelegatePointer(PacketHeader packetheader, Connection connection, bool incomingobject)
        {
            MessageBox.Show("Đối phương đã ngắt kết nối!");
            Invoke(new GoToLobbyFormDelegate(GoToLobbyForm));
        }

        #endregion

        #region Form Eventy

        //Đóng cửa sổ
        private void Play_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_closeFromCode) return;
            NetworkComms.SendObject("Disconnect", ServerIp, ServerPort, EnemyName);
        }


        //Chọn tàu
        private void SelectShip(ShipType type)
        {
            _selectedShipType = type;
            rtbInfo.Text = $"Được chọn: {Enums.GetDescription(_selectedShipType)}\nBấm vào vị trí đầu tiên";
        }



        // Cập nhật nhãn khi khởi động
        private void Play_Shown(object sender, EventArgs e)
        {
            lblMyWaters.Text += $" ({PlayerName})";
            lblEnemyWaters.Text += $" ({EnemyName})";
            Text = $"Trận đánh (#{_gameId})";
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        // Đang gửi vị trí tàu của tôi đến máy chủ, sẵn sàng chơi
        private void button6_Click_1(object sender, EventArgs e)
        {
            GameStartRequest gsr = new GameStartRequest(_occupiedPositions, EnemyIp, EnemyPort);
            NetworkComms.SendObject("GameStartRequest", ServerIp, ServerPort, gsr);
            button6.Enabled = false;
            AcceptButton = button5;
        }

        //Lựa chọn tàu bằng các nút
        private void button8_Click(object sender, EventArgs e) => SelectShip(ShipType.Destroyer);

        private void button1_Click(object sender, EventArgs e) => SelectShip(ShipType.Submarine);

        private void button2_Click(object sender, EventArgs e) => SelectShip(ShipType.Cruiser);

        private void button3_Click(object sender, EventArgs e) => SelectShip(ShipType.Battleship);

        private void button4_Click(object sender, EventArgs e) => SelectShip(ShipType.Carrier);
        // Tấn công
        private void button5_Click(object sender, EventArgs e)
        {
            if (_selectedPosition != null)
            {
                GameFireInfo gfi = new GameFireInfo(_gameId, _selectedPosition);
                NetworkComms.SendObject("GameFireInfo", ServerIp, ServerPort, gfi);

                NextTurn(false);
            }
            else
            {
                MessageBox.Show("Đầu tiên bạn phải chọn một vị trí!");
            }
        }
    }
}

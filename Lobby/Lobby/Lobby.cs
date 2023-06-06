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
    public partial class Lobby : Form
    {
        public Lobby()
        {
            InitializeComponent();
        }

        private void btnPhong1_Click(object sender, EventArgs e)
        {
            //Bật form Chơi có background Phòng 1
            Bảng_lựa_chọn m = new Bảng_lựa_chọn();
            m.ShowDialog();
        }

        private void btnPhong2_Click(object sender, EventArgs e)
        {
            //Bật form Chơi có background Phòng 2
            Play m = new Play();
            m.ShowDialog();
        }

        private void btnPhong3_Click(object sender, EventArgs e)
        {
            //Bật form Chơi có background Phòng 3
        }

        private void btnPhong4_Click(object sender, EventArgs e)
        {
            //Bật form Chơi có background Phòng 4
        }

        private void btnPhong5_Click(object sender, EventArgs e)
        {
            //Bật form Chơi có background Phòng 5
        }
        private void Nguoi1Room1()
        {

        }
        private void Nguoi2Room1()
        {

        }
        
    }
}

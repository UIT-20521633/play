using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lobby.Properties;
using BattleshipsLibrary;

namespace Lobby
{
   
    static class Drawing
    {
        //Lớp vẽ trên Ô lưới, vẽ dấu, đánh và phút chọn của mục tiêu
        private static Pen _pen = new Pen(Color.Red, 4);
        private static int _offset = 5;

        // Vẽ Tam giác
        public static void DrawSelected(GridTile gridTile, Graphics graphics)
        {
            Point[] points =
            {
                new Point(gridTile.Width / 2, _offset),
                new Point(_offset, gridTile.Height - _offset),
                new Point(gridTile.Width - _offset, gridTile.Height - _offset)
            };

            graphics.DrawClosedCurve(_pen, points, 0.3f, FillMode.Winding);
        }

        // Vẽ X
        public static void DrawHit(GridTile gridTile, Graphics graphics)
        {
            Point upperLeft = new Point(0 + _offset, 0 + _offset);
            Point upperRight = new Point(gridTile.Width - _offset, 0 + _offset);
            Point lowerLeft = new Point(0 + _offset, gridTile.Height - _offset);
            Point lowerRight = new Point(gridTile.Width - _offset, gridTile.Height - _offset);

            graphics.DrawLine(_pen, upperLeft, lowerRight);
            graphics.DrawLine(_pen, upperRight, lowerLeft);
        }

        //Vẽ O
        public static void DrawMiss(GridTile gridTile, Graphics graphics)
        {
            Rectangle r = new Rectangle(0 + _offset, 0 + _offset, gridTile.Width - _offset*2, gridTile.Height - _offset*2);

            graphics.DrawEllipse(_pen, r);
        }

        //Lửa
        public static void DrawFire(GridTile gridTile, Graphics graphics)
        {
            graphics.DrawImage(Resources.Fire, new Point(0,0));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lobby
{
    public static class Utils
    {
        // Xoay hình ảnh và sau đó quay lại
        public static Image RotateImage(Bitmap img, RotateFlipType rotateFlipType)
        {
            img.RotateFlip(rotateFlipType);
            return img;
        }
    }
}

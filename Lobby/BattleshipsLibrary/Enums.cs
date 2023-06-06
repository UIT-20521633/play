using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsLibrary
{
    public class Enums
    {
        // Trả về mô tả enum, thay vì tên
        //https://www.codingame.com/playgrounds/2487/c---how-to-display-friendly-names-for-enumerations
        public static string GetDescription(Enum enumName)
        {
            Type enumNameType = enumName.GetType();
            MemberInfo[] memberInfo = enumNameType.GetMember(enumName.ToString());
            if ((memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs.Count() > 0))
                {
                    return ((DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return enumName.ToString();
        }
    }

    //loại tàu
    public enum ShipType
    {
        [Description("Không có gì được chọn!")]
        None = 0,

        [Description("Kẻ huỷ diệt")]
        Destroyer = 1,

        [Description("tàu ngầm")]
        Submarine = 2,

        [Description("tàu tuần dương")]
        Cruiser = 3,

        [Description("Tàu chiến")]
        Battleship = 4,

        [Description("Tàu sân bay")]
        Carrier = 5
    }

    // gạch kết cấu
    public enum TileType
    {
        Water,
        ShipCenterHorizontal,
        ShipCenterVertical,
        ShipEndLeft,
        ShipEndRight,
        ShipEndUp,
        ShipEndDown,
        ShipSolo
    }

    // Kiểu đặt tàu
    public enum PlacementType
    {
        Solo,
        Horizontal,
        Vertical,
        Invalid,
        Occupied
    }

    // Cập nhật lưới người chơi hoặc kẻ thù (chữ thập, vòng tròn)
    public enum UpdateType
    {
        PlayerGrid,
        EnemyGrid
    }

    // Máy chủ phản hồi yêu cầu kết nối
    public enum ResponseType
    {
        Accepted,
        Rejected
    }
}

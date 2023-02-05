using System.Windows.Media;

namespace Monopoly.UI
{
    public static class Constants
    {
        public static readonly Color BoardColor = (Color)ColorConverter.ConvertFromString("#e9e6db");

        public const int BoardRows = 11;
        public const int BoardColumns = 11;
        public const int BoardCellWidth = 160;
        public const int BoardCellHeight = 160;
        public const int DefaultBoardCellRowSpan = 1;
        public const int DefaultBoardCellColumnSpan = 1;
        public const double BoardCellBorderThickness = 2;
        public const double StripeThickness = 40;
        public const double PlayerSize = 25;
    }
}

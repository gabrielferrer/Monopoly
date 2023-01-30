using System.Windows;
using System.Windows.Media;

namespace Monopoly.VM
{
    class BoardCellDto
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public int RowSpan { get; set; }

        public int ColumnSpan { get; set; }

        public Spaces.Space Space { get; set; }

        public Thickness Border { get; set; }

        public UI.BoardCellOrientation Orientation { get; set; }

        public Color? StripeColor { get; set; }
    }
}

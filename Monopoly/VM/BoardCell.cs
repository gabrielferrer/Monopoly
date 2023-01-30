using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Monopoly.VM
{
    class BoardCell : ViewModelBase
    {
        public BoardCell(BoardCellDto boardCellDto)
        {
            Text = new ObservableCollection<string>(boardCellDto.Space.Text.Select(x => string.Copy(x)).ToArray());
            Row = boardCellDto.Row;
            Column = boardCellDto.Column;
            RowSpan = boardCellDto.RowSpan;
            ColumnSpan = boardCellDto.ColumnSpan;
            Border = boardCellDto.Border;
            Orientation = boardCellDto.Orientation;
            StripeColor = boardCellDto.StripeColor.HasValue ? new SolidColorBrush(boardCellDto.StripeColor.Value) : null;
        }

        public ObservableCollection<string> Text { get; }

        public double Width => UI.Constants.BoardCellWidth;

        public double Height => UI.Constants.BoardCellHeight;

        public double StripeThickness => UI.Constants.StripeThickness;

        public int Row { get; }

        public int Column { get; }

        public int RowSpan { get; }

        public int ColumnSpan { get; }

        public Thickness Border { get; }

        public UI.BoardCellOrientation Orientation { get; }

        public Brush StripeColor { get; }
    }
}

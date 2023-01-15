using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Monopoly.VM
{
    class BoardCell : ViewModelBase
    {
        private int row = 0;
        private int column = 0;
        private int rowSpan = UI.Constants.DefaultBoardCellRowSpan;
        private int columnSpan = UI.Constants.DefaultBoardCellColumnSpan;
        private Thickness border;

        public BoardCell(Spaces.Space space)
        {
            Text = new ObservableCollection<string>(space.Text.Select(x => string.Copy(x)).ToArray());
        }

        public ObservableCollection<string> Text { get; }

        public double Width => UI.Constants.BoardCellWidth;

        public double Height => UI.Constants.BoardCellHeight;

        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                if (row == value) return;
                row = value;
                OnPropertyChanged(nameof(Row));
            }
        }

        public int Column
        {
            get
            {
                return column;
            }
            set
            {
                if (column == value) return;
                column = value;
                OnPropertyChanged(nameof(Column));
            }
        }

        public int RowSpan
        {
            get
            {
                return rowSpan;
            }
            set
            {
                if (rowSpan == value) return;
                rowSpan = value;
                OnPropertyChanged(nameof(RowSpan));
            }
        }

        public int ColumnSpan
        {
            get
            {
                return columnSpan;
            }
            set
            {
                if (columnSpan == value) return;
                columnSpan = value;
                OnPropertyChanged(nameof(ColumnSpan));
            }
        }

        public Thickness Border
        {
            get
            {
                return border;
            }
            set
            {
                if (border == value) return;
                border = value;
                OnPropertyChanged(nameof(Border));
            }
        }
    }
}

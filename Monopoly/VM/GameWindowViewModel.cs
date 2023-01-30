using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Monopoly.VM
{
    class GameWindowViewModel : ViewModelBase
    {
        #region Fields

        private Game game;
        private double[] rowDefinitions;
        private double[] columnDefinitions;

        #endregion

        #region Constructors

        public GameWindowViewModel()
        {
            rowDefinitions = new double[UI.Constants.BoardRows];
            columnDefinitions = new double[UI.Constants.BoardColumns];
            BoardCells = new ObservableCollection<BoardCell>();

            for (int row = 0; row < UI.Constants.BoardRows; row++) rowDefinitions[row] = double.NaN;
            for (int column = 0; column < UI.Constants.BoardColumns; column++) columnDefinitions[column] = double.NaN;

            game = new Game();

            int index = 0;

            CreateBoardBottomRow(ref index);
            CreateBoardLeftRow(ref index);
            CreateBoardTopRow(ref index);
            CreateBoardRightRow(ref index);

#if DEBUG
            //using (var stream = new System.IO.StreamWriter(@"C:\Users\Gabriel\Desktop\log.txt"))
            //{
            //game.Log(stream);
            //}
#endif
        }

        private void CreateBoardRightRow(ref int index)
        {
            for (int row = 1, column = 10; row < 10; row++, index++)
            {
                var boardCellDto = new BoardCellDto
                {
                    Row = row,
                    Column = column,
                    RowSpan = UI.Constants.DefaultBoardCellRowSpan,
                    ColumnSpan = UI.Constants.DefaultBoardCellColumnSpan,
                    Space = game.Board.Spaces.ElementAt(index),
                    Border = CreateCellBorder(row, column),
                    Orientation = UI.BoardCellOrientation.East,
                    StripeColor = GetStripeColor(index)
                };

                BoardCells.Add(new BoardCell(boardCellDto));
            }
        }

        private void CreateBoardTopRow(ref int index)
        {
            for (int row = 0, column = 1; column <= 10; column++, index++)
            {
                var boardCellDto = new BoardCellDto
                {
                    Row = row,
                    Column = column,
                    RowSpan = UI.Constants.DefaultBoardCellRowSpan,
                    ColumnSpan = UI.Constants.DefaultBoardCellColumnSpan,
                    Space = game.Board.Spaces.ElementAt(index),
                    Border = CreateCellBorder(row, column),
                    Orientation = column == 10 ? UI.BoardCellOrientation.NorthEast : UI.BoardCellOrientation.North,
                    StripeColor = GetStripeColor(index)
                };

                BoardCells.Add(new BoardCell(boardCellDto));
            }
        }

        private void CreateBoardLeftRow(ref int index)
        {
            for (int row = 9, column = 0; row >= 0; row--, index++)
            {
                var boardCellDto = new BoardCellDto
                {
                    Row = row,
                    Column = column,
                    RowSpan = UI.Constants.DefaultBoardCellRowSpan,
                    ColumnSpan = UI.Constants.DefaultBoardCellColumnSpan,
                    Space = game.Board.Spaces.ElementAt(index),
                    Border = CreateCellBorder(row, column),
                    Orientation = row == 0 ? UI.BoardCellOrientation.NorthWest : UI.BoardCellOrientation.West,
                    StripeColor = GetStripeColor(index)
                };

                BoardCells.Add(new BoardCell(boardCellDto));
            }
        }

        private void CreateBoardBottomRow(ref int index)
        {
            for (int row = 10, column = 10; column >= 0; column--, index++)
            {
                var boardCellDto = new BoardCellDto
                {
                    Row = row,
                    Column = column,
                    RowSpan = UI.Constants.DefaultBoardCellRowSpan,
                    ColumnSpan = UI.Constants.DefaultBoardCellColumnSpan,
                    Space = game.Board.Spaces.ElementAt(index),
                    Border = CreateCellBorder(row, column),
                    Orientation = column == 0 ? UI.BoardCellOrientation.SouthWest : column == 10 ? UI.BoardCellOrientation.SouthEast : UI.BoardCellOrientation.South,
                    StripeColor = GetStripeColor(index)
                };

                BoardCells.Add(new BoardCell(boardCellDto));
            }
        }

        private Thickness CreateCellBorder(int row, int column)
        {
            if (row > 0 && row < 9 && (column == 0 || column == 10))
            {
                return new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness);
            }

            if (row > 0 && row == 9 && (column == 0 || column == 10))
            {
                return new Thickness(UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness, 0);
            }

            if (column > 0 && column < 9 && (row == 0 || row == 10))
            {
                return new Thickness(0, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness, UI.Constants.BoardCellBorderThickness);
            }

            if (column > 0 && column == 9 && (row == 0 || row == 10))
            {
                return new Thickness(0, UI.Constants.BoardCellBorderThickness, 0, UI.Constants.BoardCellBorderThickness);
            }

            // All corners.
            return new Thickness(UI.Constants.BoardCellBorderThickness);
        }

        public Color? GetStripeColor(int index)
        {
            switch (index)
            {
                case 1: case 3: return UI.PropertyColors.FirstGroup;
                case 6: case 8: case 9: return UI.PropertyColors.SecondGroup;
                case 11: case 13: case 14: return UI.PropertyColors.ThirdGroup;
                case 16: case 18: case 19: return UI.PropertyColors.FourthGroup;
                case 21: case 23: case 24: return UI.PropertyColors.FifthGroup;
                case 26: case 27: case 29: return UI.PropertyColors.SixthGroup;
                case 31: case 32: case 34: return UI.PropertyColors.SeventhGroup;
                case 37: case 39: return UI.PropertyColors.EighthGroup;
            }

            return null;
        }

        #endregion

        #region Properties

        public Brush WindowColor => new SolidColorBrush(UI.Constants.BoardColor);

        public double WindowWidth => columnDefinitions.Length * UI.Constants.BoardCellWidth + (columnDefinitions.Length + 1 * UI.Constants.BoardCellBorderThickness);

        public double WindowHeight => rowDefinitions.Length * UI.Constants.BoardCellHeight + (rowDefinitions.Length + 1 * UI.Constants.BoardCellBorderThickness);

        public ObservableCollection<double> RowDefinitions => new ObservableCollection<double>(rowDefinitions);

        public ObservableCollection<double> ColumnDefinitions => new ObservableCollection<double>(columnDefinitions);

        public int Rows => rowDefinitions.Length;

        public int Columns => columnDefinitions.Length;

        public ObservableCollection<BoardCell> BoardCells { get; }

        #endregion
    }
}

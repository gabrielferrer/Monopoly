using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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

            for (int row = 0; row < UI.Constants.BoardRows; row++) rowDefinitions[row] = UI.Constants.BoardCellHeight;
            for (int column = 0; column < UI.Constants.BoardColumns; column++) columnDefinitions[column] = UI.Constants.BoardCellWidth;

            game = new Game();

            int index = 0;

            // Board bottom row.
            for (int row = 10, column = 10; column >= 0; column--, index++)
            {
                BoardCells.Add(new BoardCell(game.Board.Spaces.ElementAt(index)) { Row = row, Column = column, Border = CreateCellBorder(row, column) });
            }

            // Board left column.
            for (int row = 9, column = 0; row >= 0; row--, index++)
            {
                BoardCells.Add(new BoardCell(game.Board.Spaces.ElementAt(index)) { Row = row, Column = column, Border = CreateCellBorder(row, column) });
            }

            // Board top row.
            for (int row = 0, column = 1; column < UI.Constants.BoardColumns; column++, index++)
            {
                BoardCells.Add(new BoardCell(game.Board.Spaces.ElementAt(index)) { Row = row, Column = column, Border = CreateCellBorder(row, column) });
            }

            // Board right column.
            for (int row = 1, column = 10; row + 1 < UI.Constants.BoardRows; row++, index++)
            {
                BoardCells.Add(new BoardCell(game.Board.Spaces.ElementAt(index)) { Row = row, Column = column, Border = CreateCellBorder(row, column) });
            }

#if DEBUG
            //using (var stream = new System.IO.StreamWriter(@"C:\Users\Gabriel\Desktop\log.txt"))
            //{
            //game.Log(stream);
            //}
#endif
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

        #endregion

        #region Properties

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

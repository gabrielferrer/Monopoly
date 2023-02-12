using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Monopoly.VM.Spaces
{
    public abstract class Space : ViewModelBase
    {
        public Space(SpaceDto spaceDto)
        {
            Visitors = new ObservableCollection<Player>();
            Row = spaceDto.Row;
            Column = spaceDto.Column;
            RowSpan = spaceDto.RowSpan;
            ColumnSpan = spaceDto.ColumnSpan;
            Border = spaceDto.Border;
            Orientation = spaceDto.Orientation;
            StripeColor = spaceDto.StripeColor.HasValue ? new SolidColorBrush(spaceDto.StripeColor.Value) : null;
        }

        public void Clear()
        {
            Visitors.Clear();
        }

        internal void SetVisitors(List<Player> visitors)
        {
            Visitors.Clear();
            foreach (var visitor in visitors) Visitors.Add(visitor);
        }

#if DEBUG
        public void Log(System.IO.StreamWriter stream)
        {
            stream.WriteLine(nameof(Space));
            foreach (var line in Text) stream.WriteLine($"{line}");
            foreach (var visitor in Visitors) visitor.Log(stream);
            stream.WriteLine("--------------------");
        }
#endif
        public ObservableCollection<Player> Visitors { get; }

        public abstract IEnumerable<string> Text { get; }

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

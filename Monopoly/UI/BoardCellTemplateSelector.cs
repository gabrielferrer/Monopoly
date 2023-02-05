using System.Windows;
using System.Windows.Controls;

namespace Monopoly.UI
{
    public class BoardCellTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CornerCellTemplate { get; set; }

        public DataTemplate EastCellTemplate { get; set; }

        public DataTemplate NorthCellTemplate { get; set; }

        public DataTemplate SouthCellTemplate { get; set; }

        public DataTemplate WestCellTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is VM.Spaces.Space space)) return null;

            if (space.Orientation == BoardCellOrientation.West) return WestCellTemplate;

            if (space.Orientation == BoardCellOrientation.North) return NorthCellTemplate;

            if (space.Orientation == BoardCellOrientation.East) return EastCellTemplate;

            if (space.Orientation == BoardCellOrientation.South) return SouthCellTemplate;

            return CornerCellTemplate;
        }
    }
}

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Monopoly.UI
{
    public class GridExtensions
    {
        #region Constants

        private static readonly IList<double> DefaultValue = new List<double>();

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty RowDefinitionsProperty = DependencyProperty.RegisterAttached(
            "RowDefinitions",
            typeof(IList<double>),
            typeof(GridExtensions),
            new PropertyMetadata(DefaultValue, RowDefinitionsChanged)
            );

        public static readonly DependencyProperty ColumnDefinitionsProperty = DependencyProperty.RegisterAttached(
            "ColumnDefinitions",
            typeof(IList<double>),
            typeof(GridExtensions),
            new PropertyMetadata(DefaultValue, ColumnDefinitionsChanged)
            );

        #endregion

        #region Infrastructure

        private static RowDefinition CreateRowDefinition(GridLength gridLength)
        {
            return new RowDefinition
            {
                Height = gridLength,
                MinHeight = Constants.MinimumBoardCellHeight,
                MaxHeight = Constants.MaximumBoardCellHeight
            };
        }

        private static ColumnDefinition CreateColumnDefinition(GridLength gridLength)
        {
            return new ColumnDefinition
            {
                Width = gridLength,
                MinWidth = Constants.MinimumBoardCellWidth,
                MaxWidth = Constants.MaximumBoardCellWidth
            };
        }

        public static IList<double> GetRowDefinitions(DependencyObject obj)
        {
            return (IList<double>)obj.GetValue(RowDefinitionsProperty);
        }

        public static void SetRowDefinitions(DependencyObject obj, IList<double> value)
        {
            obj.SetValue(RowDefinitionsProperty, value);
        }

        public static void RowDefinitionsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid grid && e.NewValue is IList<double> rowDefinitions))
            {
                return;
            }

            grid.RowDefinitions.Clear();

            for (int index = 0; index < rowDefinitions.Count; index++)
            {
                var rowDefinition = double.IsNaN(rowDefinitions[index])
                    ? CreateRowDefinition(GridLength.Auto)
                    : CreateRowDefinition(new GridLength(rowDefinitions[index]));

                grid.RowDefinitions.Add(rowDefinition);
            }
        }

        public static IList<double> GetColumnDefinitions(DependencyObject obj)
        {
            return (IList<double>)obj.GetValue(ColumnDefinitionsProperty);
        }

        public static void SetColumnDefinitions(DependencyObject obj, IList<double> value)
        {
            obj.SetValue(ColumnDefinitionsProperty, value);
        }

        public static void ColumnDefinitionsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid grid && e.NewValue is IList<double> columnDefinitions))
            {
                return;
            }

            grid.ColumnDefinitions.Clear();

            for (int index = 0; index < columnDefinitions.Count; index++)
            {
                var columnDefinition = double.IsNaN(columnDefinitions[index])
                    ? CreateColumnDefinition(GridLength.Auto)
                    : CreateColumnDefinition(new GridLength(columnDefinitions[index]));

                grid.ColumnDefinitions.Add(columnDefinition);
            }
        }

        #endregion
    }
}

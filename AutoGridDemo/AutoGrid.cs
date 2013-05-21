using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AutoGridDemo
{
    /// <summary>
    /// A grid control that automatically lays out children so you don't need to specify grid.row and grid.column
    /// You can specify row and or column definitions as normal or using a string like "auto,auto,10*,50,10*".
    /// If no column definitions are given you can set NumColumns and that many will be auto-created.
    /// You can also set default row height and column width.
    /// Children are then inserted into cells one row at a time from left to right. New rows are created as required.
    /// Handles colspan automatically but at the moment will re-use a cell that has a row span covering it.
    /// Use something like an empty <Rectangle/> as a spacer.
    /// </summary>
    public class AutoGrid : Grid
    {
        public AutoGrid()
        {
            Loaded += OnLoaded;
        }

        private bool _hasLoaded;

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!_hasLoaded && !String.IsNullOrWhiteSpace(Rows))
            {
                var heights = GridLengthParser.FromString(Rows);
                foreach (var height in heights)
                {
                    RowDefinitions.Add(new RowDefinition { Height = height });
                }
            }

            if (!_hasLoaded && !String.IsNullOrWhiteSpace(Columns))
            {
                var widths = GridLengthParser.FromString(Columns);
                foreach (var width in widths)
                {
                    ColumnDefinitions.Add(new ColumnDefinition { Width = width });
                }
            }

            _hasLoaded = true;

            for (int i = ColumnDefinitions.Count; i < NumColumns; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition { Width = ColumnWidth });
            }

            if (RowDefinitions.Count == 0)
            {
                RowDefinitions.Add(new RowDefinition { Height = RowHeight });
            }

            var currentColumn = 0;
            var currentRow = 0;
            var addRow = false;
            var columnCount = ColumnDefinitions.Count == 0 ? 1 : ColumnDefinitions.Count; //if we have no col defs we have one col.

            foreach (var child in Children)
            {
                if (!GetAutoplace((UIElement)child)) return;
                if (addRow)
                {
                    currentRow = GetNextRowAndAddIfRequired(currentRow);
                    currentColumn = 0;
                    addRow = false;
                }
                var cellsLeft = columnCount - currentColumn;
                var childRowSpan = GetColumnSpan((FrameworkElement)child);
                if (cellsLeft < childRowSpan) //can't fit this row
                {
                    currentRow = GetNextRowAndAddIfRequired(currentRow);
                    currentColumn = 0;
                }
                SetColumn((FrameworkElement)child, currentColumn);
                SetRow((FrameworkElement)child, currentRow);
                currentColumn += childRowSpan;
                if (currentColumn >= columnCount)
                {
                    addRow = true;
                }
            }

            if (StretchLastRow)
            {
                RowDefinitions.Last().Height = new GridLength(1, GridUnitType.Star);
            }
        }

        private int GetNextRowAndAddIfRequired(int currentRow)
        {
            if (RowDefinitions.Count <= currentRow + 1)
            {
                RowDefinitions.Add(new RowDefinition { Height = RowHeight });
                return RowDefinitions.Count - 1;
            }
            return currentRow + 1;
        }

        #region Dependency Properties

        public static readonly DependencyProperty NumColumnsProperty =
            DependencyProperty.Register("NumColumns", typeof(int), typeof(AutoGrid), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty ColumnWidthProperty =
            DependencyProperty.Register("ColumnWidth", typeof(GridLength), typeof(AutoGrid), new PropertyMetadata(default(GridLength)));

        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(GridLength), typeof(AutoGrid), new PropertyMetadata(default(GridLength)));

        public static readonly DependencyProperty StretchLastRowProperty =
            DependencyProperty.Register("StretchLastRow", typeof(bool), typeof(AutoGrid), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(string), typeof(AutoGrid), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(string), typeof(AutoGrid), new PropertyMetadata(default(string)));

        #endregion

        /// <summary>
        /// Set the height of the last row *
        /// </summary>
        public bool StretchLastRow
        {
            get { return (bool)GetValue(StretchLastRowProperty); }
            set { SetValue(StretchLastRowProperty, value); }
        }

        /// <summary>
        /// The number of columns in the grid
        /// </summary>
        public int NumColumns
        {
            get { return (int)GetValue(NumColumnsProperty); }
            set { SetValue(NumColumnsProperty, value); }
        }

        /// <summary>
        /// The width of each column, can be *, auto or a pixel number
        /// </summary>
        public GridLength ColumnWidth
        {
            get { return (GridLength)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }

        /// <summary>
        /// The height of each row, can be *, auto or a pixel number
        /// </summary>
        public GridLength RowHeight
        {
            get { return (GridLength)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        /// <summary>
        /// Used to manually define rows using a string like "auto,auto,*,10"
        /// </summary>
        public string Rows
        {
            get { return (string)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        /// <summary>
        /// Used to manually define columns using a string like "auto,auto,*,10"
        /// </summary>
        public string Columns
        {
            get { return (string)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public static readonly DependencyProperty AutoplaceProperty =
            DependencyProperty.RegisterAttached("Autoplace", typeof (bool), typeof (AutoGrid), new PropertyMetadata(true));

        public static void SetAutoplace(UIElement element, bool value)
        {
            element.SetValue(AutoplaceProperty, value);
        }

        public static bool GetAutoplace(UIElement element)
        {
            return (bool) element.GetValue(AutoplaceProperty);
        }
    }
}
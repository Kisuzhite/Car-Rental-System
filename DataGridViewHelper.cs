namespace Car_Rental_System
{
    internal static class DataGridViewHelper
    {
        public static void HideColumn(DataGridView grid, string columnName)
        {
            DataGridViewColumn? column = grid.Columns[columnName];
            if (column != null)
            {
                column.Visible = false;
            }
        }

        public static void ClearSelectionWhenReady(DataGridView grid)
        {
            if (!grid.IsHandleCreated)
            {
                grid.ClearSelection();
                return;
            }

            grid.BeginInvoke(new Action(grid.ClearSelection));
        }
    }
}

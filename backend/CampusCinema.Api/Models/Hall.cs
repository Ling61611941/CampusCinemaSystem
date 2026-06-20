namespace CampusCinema.Api.Models;

public class Hall
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int RowCount { get; set; }

    public int ColumnCount { get; set; }

    public string Type { get; set; } = "普通厅";

    public string Status { get; set; } = "启用";
}

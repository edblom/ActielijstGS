namespace KlantBaseWebDemo.Models
{
    public class FieldConfigDto
    {
        public string FieldName { get; set; }
        public string Prompt { get; set; }
        public string DataType { get; set; }
        public bool IsSortable { get; set; }
        public bool IsFilterable { get; set; }
        public string? FormatString { get; set; }
        public int? Width { get; set; }
        public string? BackgroundColorRule { get; set; }
    }
}

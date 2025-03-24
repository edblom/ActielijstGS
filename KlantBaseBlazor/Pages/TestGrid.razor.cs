using Radzen;
using Radzen.Blazor;
namespace KlantBaseBlazor.Pages;

public partial class TestGrid
{
    private List<TestItem> testItems = new()
    {
        new TestItem { Id = 1, Name = "Item 1", Date = new DateTime(2025, 3, 1), IsActive = true },
        new TestItem { Id = 2, Name = "Item 2", Date = new DateTime(2025, 3, 2), IsActive = false },
        new TestItem { Id = 3, Name = "Item 3", Date = new DateTime(2025, 3, 3), IsActive = true },
        new TestItem { Id = 4, Name = "Item 4", Date = new DateTime(2025, 3, 4), IsActive = false },
        new TestItem { Id = 5, Name = "Item 5", Date = new DateTime(2025, 3, 5), IsActive = true },
    };

    public class TestItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}
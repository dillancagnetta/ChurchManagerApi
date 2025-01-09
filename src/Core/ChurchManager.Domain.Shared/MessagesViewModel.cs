namespace ChurchManager.Domain.Shared;

public record MessageViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime SentDateTime { get; set; }
    public string IconCssClass { get; set; }
    public string Classification { get; set; }
    public string Link { get; set; }
    public bool UseRouter { get; set; }
    public bool IsRead { get; set; }
    public string UserLoginId { get; set; }
};
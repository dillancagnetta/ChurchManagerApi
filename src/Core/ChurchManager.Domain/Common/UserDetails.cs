namespace ChurchManager.Domain.Common
{
    public record UserDetails
    {
        public string? Username { get; set; }
        public int PersonId { get; init; }
        public required string UserLoginId { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public string? Email { get; init; }
        public string? PhotoUrl { get; init; }
    }
}

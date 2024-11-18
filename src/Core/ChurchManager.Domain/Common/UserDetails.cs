namespace ChurchManager.Domain.Common
{
    public record UserDetails
    {
        public string Username { get; set; }
        public int PersonId { get; init; }
        public string UserLoginId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string PhotoUrl { get; init; }
    }
}

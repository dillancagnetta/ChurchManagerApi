using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Common.Extensions;

namespace ChurchManager.Features.People.Infrastructure.Extensions;

public static class ApplicationExtensions
{
    public static string? CloudinaryPublicId(this Person person)
    {
        // https://res.cloudinary.com/codebossza/image/upload/v1627875380/Development/lnaughtycscssronmncu.png
        return person.PhotoUrl?.CloudinaryPublicId();
    }
}
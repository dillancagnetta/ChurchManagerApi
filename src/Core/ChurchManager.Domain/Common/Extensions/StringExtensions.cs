namespace ChurchManager.Domain.Common.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Truncates a string after a max length and adds ellipsis.  Truncation will occur at first space prior to maxLength.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    public static string? Truncate( this string str, int maxLength )
    {
        return Truncate( str, maxLength, true );
    }

    /// <summary>
    /// Truncates a string after a max length with an option to add an ellipsis at the end.  Truncation will occur at first space prior to maxLength.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="maxLength">The maximum length of the return value, including the ellipsis if added.</param>
    /// <param name="addEllipsis">if set to <c>true</c> add an ellipsis to the end of the truncated string.</param>
    /// <returns></returns>
    public static string? Truncate( this string? str, int maxLength, bool addEllipsis )
    {
        if ( str == null )
        {
            return null;
        }

        if ( str.Length <= maxLength )
        {
            return str;
        }

        // Since we include the ellipsis in the number of max characters
        // we need to disable ellipsis if they told us to have a max
        // length of 3 or less - which is the number of periods that
        // would be added.
        if ( maxLength <= 3 )
        {
            addEllipsis = false;
        }

        // If adding an ellipsis then reduce the maxlength by three to allow for the additional characters
        maxLength = addEllipsis ? maxLength - 3 : maxLength;

        var truncatedString = str.Substring( 0, maxLength );
        var lastSpace = truncatedString.LastIndexOf( ' ' );
        if ( lastSpace > 0 )
        {
            truncatedString = truncatedString.Substring( 0, lastSpace );
        }

        return addEllipsis ? truncatedString + "..." : truncatedString;
    }
    
    public static string CloudinaryPublicId(this string photoUrl)
    {
        // https://res.cloudinary.com/codebossza/image/upload/v1627875380/Development/lnaughtycscssronmncu.png
        var parsed = photoUrl.Substring(8); // remove https://
        parsed = parsed.Remove(parsed.Length - 4); // remove file extension e.g. .png
        var split = parsed.Split("/");
        var splitLength = split.Length;
        var publicId = $"{split[splitLength - 2]}/{split[splitLength - 1]}";

        return publicId;
    }
    
    public static string? TrimLeadingZero( this string str )
    {
        if (str is {Length: > 0} && str.StartsWith( "0" )) return str.Substring( 1 );
      
        return str;
    }
    
    public static string? CleanPhoneNumber( this string str )
    {
        var trimmed = str.Trim().TrimLeadingZero();
      
        return trimmed;
    }
    
    /// <summary>
    /// Returns the initials of a string, capitalizing the first letter of each word.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToInitials(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        return string.Concat(input
            .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(word => char.ToUpperInvariant(word[0])));
    }
    
    public static string ToShortCode(this string input, int chars = 3)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        return input.Substring(0, chars).ToUpperInvariant();
    }
}
using System.Collections;
using SpotifyAPI.Web;
using System.Linq;

namespace Core;

public static class Guards
{
    public static bool IsNotNullOrEmpty(string obj, string customError = "")
    {
        if (obj.IsNullOrEmpty()) throw new ArgumentNullException(customError.IsNullOrEmpty() ? nameof(obj) : customError);
        return true;
    }

    public static bool IsNotNull(object obj, string customError = "")
    {
        if (obj == null) throw new ArgumentNullException(customError.IsNullOrEmpty() ? nameof(obj) : customError);
        return true;
    }

    public static bool IsNotEmpty(int obj, string customError = "")
    {
        if (obj <= 0) throw new ArgumentNullException(customError.IsNullOrEmpty() ? nameof(obj) : customError);
        return true;
    }

    public static bool IsNotEmpty<T>(ICollection obj, string customError = "")
    {
        return obj.Count <= 0 ? throw new ArgumentNullException(customError.IsNullOrEmpty() ? nameof(obj) : customError) : true;
    }

    public static bool IsNotEmpty<T>(IPaginatable<T> obj, string customError = "")
    {
        return obj.Items!.Count <= 0 ? throw new ArgumentNullException(customError.IsNullOrEmpty() ? nameof(obj) : customError) : true;
    }
}
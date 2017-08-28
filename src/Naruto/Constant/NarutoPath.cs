using System.IO;

namespace Naruto.Constant
{
    public class NarutoPath
    {
        public static string AppBaseDirectory;

        public static string MapPath(string path)
        {
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(AppBaseDirectory ?? "", path);
        }
    }
}

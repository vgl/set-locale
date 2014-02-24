using System.IO;
using System.Web;

namespace set.locale.Helpers
{
    public static class Stream
    {
        public static string Read(string file)
        {
            string path = HttpContext.Current.Server.MapPath("~");
            StreamReader objReader = new StreamReader(@path + file);
            string fileContent = objReader.ReadToEnd();
            objReader.Close();
            return fileContent;
        }
    }
}
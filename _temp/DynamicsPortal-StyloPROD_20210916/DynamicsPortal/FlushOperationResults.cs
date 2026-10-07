using System;
using System.Text;
using System.Web;

namespace DynamicsPortal
{
    public class FlushOperationResults
    {
        public void flushResults(string _message)
        {
            string fileName = "Results " + DateTime.Now.ToString("yyyyMMddHHmm"); ;
            string message = _message.Replace("\n", Environment.NewLine);
            byte[] buffer;
            using (var memoryStream = new System.IO.MemoryStream())
            {
                buffer = Encoding.Default.GetBytes(message);

                memoryStream.Write(buffer, 0, buffer.Length);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".txt");
                HttpContext.Current.Response.AddHeader("Content-Length", memoryStream.Length.ToString());
                HttpContext.Current.Response.ContentType = "text/plain"; //This is MIME type
                memoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
            }
            HttpContext.Current.Response.End();
        }

    }
}
using System.Drawing;
using System.Text;

namespace Infrastructure.ImageMiddleware;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class ImageMiddleware
{
    private readonly RequestDelegate _next;

    public ImageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        string url = httpContext.Request.Path;
        if (url.ToLower().Contains(".jpg"))
        {
            string newurl = Environment.CurrentDirectory + url;
            if (!File.Exists(newurl))
            {
                var bytes = Encoding.UTF8.GetBytes("This file do not exists!");
                return httpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            }
            string watermarkurl = Environment.CurrentDirectory + "\\watermark.png";
            using (Image image = Image.FromFile(newurl))
            using (Image watermarkImage = Image.FromFile(watermarkurl))
            using (Graphics imageGraphics = Graphics.FromImage(image))
            using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
            {
                int x = (image.Width / 2 - watermarkImage.Width / 2);
                int y = (image.Height / 2 - watermarkImage.Height / 2);
                watermarkBrush.TranslateTransform(x, y);
                imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));
                MemoryStream stream = new MemoryStream();
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                httpContext.Response.ContentType = "image/jpeg";
                return httpContext.Response.Body.WriteAsync(stream.ToArray(), 0, (int)stream.Length);
            }
        }
        return _next(httpContext);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class UseImageMiddlewareExtensions
{
    public static IApplicationBuilder UseImageMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ImageMiddleware>();
    }
}
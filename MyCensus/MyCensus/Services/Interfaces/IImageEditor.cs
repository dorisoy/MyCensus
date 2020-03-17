using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCensus.Services.Interfaces
{
    public interface IImageEditor
    {
        IEditableImage CreateImage(byte[] imageArray);
        Task<IEditableImage> CreateImageAsync(byte[] imageArray);
    }

    public interface IEditableImage : IDisposable
    {
        int Width { get; }
        int Height { get; }
        IEditableImage Resize(int width, int height);
        IEditableImage Crop(int x, int y, int width, int height);
        IEditableImage Rotate(float degree);
        byte[] ToJpeg(float quality = 80);
        byte[] ToPng();
        int[] ToArgbPixels();
    }
}

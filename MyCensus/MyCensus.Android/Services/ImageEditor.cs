using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Threading.Tasks;
using Xamarin.Forms;
using MyCensus.Droid.Services;


using Android.Graphics;
using System.IO;

using MyCensus.Services.Interfaces;


[assembly: Dependency(typeof(ImageEditor))]
namespace MyCensus.Droid.Services
{
    public class ImageEditor : IImageEditor
    {
        public IEditableImage CreateImage(byte[] imageArray)
        {
            return new EditableImage(imageArray);
        }

        public async Task<IEditableImage> CreateImageAsync(byte[] imageArray)
        {
            return await Task.Run(() => CreateImage(imageArray));
        }
    }


    public class EditableImage : IEditableImage
    {
        public int Height { get; private set; }
        public int Width { get; private set; }

        private Bitmap _image;

        public EditableImage(byte[] bin)
        {
            _image = BitmapFactory.DecodeByteArray(bin, 0, bin.Length);
            UpdateSize();
        }

        public IEditableImage Resize(int width, int height)
        {
            int w;
            int h;
            if (width <= 0 && height <= 0)
            {
                return this;
            }
            else if (width <= 0)
            {
                h = height;
                w = (int)(height * ((double)Width / (double)Height));
            }
            else if (height <= 0)
            {
                w = width;
                h = (int)(width * ((double)Height / (double)Width));
            }
            else
            {
                w = width;
                h = height;
            }

            var resized = Bitmap.CreateScaledBitmap(_image, w, h, true);

            //_image.Recycle();

            //if (!_image.IsRecycled)
            //    _image.Dispose();

            _image = resized;
            resized = null;

            UpdateSize();

            return this;
        }

        public IEditableImage Crop(int x, int y, int width, int height)
        {
            var croped = Bitmap.CreateBitmap(_image, x, y, width, height);
            _image.Recycle();
            _image.Dispose();
            _image = croped;
            croped = null;

            UpdateSize();

            return this;
        }

        public IEditableImage Rotate(float degree)
        {
            var matrix = new Matrix();
            matrix.PostRotate(degree);
            var rotated = Bitmap.CreateBitmap(_image, 0, 0, _image.Width, _image.Height, matrix, true);

            _image.Recycle();
            _image.Dispose();
            _image = rotated;
            rotated = null;

            UpdateSize();

            return this;
        }

        public byte[] ToJpeg(float quality = 80)
        {
            using (var ms = new MemoryStream())
            {
                _image.Compress(Bitmap.CompressFormat.Jpeg, (int)quality, ms);
                return ms.ToArray();
            }
        }

        public byte[] ToPng()
        {
            using (var ms = new MemoryStream())
            {
                _image.Compress(Bitmap.CompressFormat.Png, 100, ms);
                return ms.ToArray();
            }
        }

        public int[] ToArgbPixels()
        {
            //ARGBの順番
            var pixels = new int[_image.Width * _image.Height];
            _image.GetPixels(pixels, 0, _image.Width, 0, 0, _image.Width, _image.Height);
            return pixels;
        }

        public void Dispose()
        {
            _image.Recycle();
            _image.Dispose();
            _image = null;
        }

        void UpdateSize()
        {
            try
            {
                Width = _image != null ? (int)_image.Width : 480;
                Height = _image != null ? (int)_image.Height : 480;
            }
            catch (Exception) { }
        }
    }
}
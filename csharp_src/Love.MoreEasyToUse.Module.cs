﻿// Author : endlesstravel
// this part aim is that make love2d for cs more easy to be used

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Love
{


    public partial class Window
    {
        /// <summary>
        /// Enters or exits fullscreen. The display to use when entering fullscreen is chosen based on which display the window is currently in, if multiple monitors are connected.
        /// </summary>
        /// <param name="fullscreen">Whether to enter or exit fullscreen mode.</param>
        /// <returns>True if an attempt to enter fullscreen was successful, false otherwise.</returns>
        public static bool SetFullscreen(bool fullscreen)
        {
            bool out_fullscreen; FullscreenType out_fstype;
            GetFullscreen(out out_fullscreen, out out_fstype);
            return SetFullscreen(fullscreen, out_fstype);
        }

        /// <summary>
        /// Gets whether the window is fullscreen.
        /// </summary>
        /// <returns>True if the window is fullscreen, false otherwise.</returns>
        public static bool GetFullscreen()
        {
            bool out_fullscreen; FullscreenType out_fstype;
            GetFullscreen(out out_fullscreen, out out_fstype);
            return out_fullscreen;
        }
    }

    public partial class FileSystem
    {
        /// <summary>
        /// Creates a new FileData from a file on the storage device.
        /// </summary>
        /// <param name="filename">Path to the file.</param>
        /// <returns></returns>
        public static FileData NewFileData(string filename)
        {
            return NewFileData(NewFile(filename));
        }
    }

    public partial class Sound
    {
        /// <summary>
        /// Attempts to find a decoder for the encoded sound data in the specified file.
        /// </summary>
        /// <param name="filename">The filename of the file with encoded sound data.</param>
        /// <param name="bufferSize">The size of each decoded chunk, in bytes.</param>
        /// <returns></returns>
        public static Decoder NewDecoder(string filename, int bufferSize = Decoder.DEFAULT_BUFFER_SIZE)
        {
            var fdata = FileSystem.NewFileData(filename);
            return NewDecoder(fdata, bufferSize);
        }

        /// <summary>
        /// <para> Creates a new SoundData.</para>
        /// <para>It's also possible to create SoundData with a custom sample rate, channel and bit depth.</para>
        /// <para>The sound data will be decoded to the memory in a raw format. It is recommended to create only short sounds like effects, as a 3 minute song uses 30 MB of memory this way.</para>
        /// </summary>
        /// <param name="file">A File pointing to an audio file.</param>
        /// <returns>A new SoundData object.</returns>
        public static SoundData NewSoundData(File file)
        {
            var decoder = NewDecoder(file);
            return NewSoundData(decoder);
        }

        /// <summary>
        /// <para> Creates a new SoundData.</para>
        /// <para>It's also possible to create SoundData with a custom sample rate, channel and bit depth.</para>
        /// <para>The sound data will be decoded to the memory in a raw format. It is recommended to create only short sounds like effects, as a 3 minute song uses 30 MB of memory this way.</para>
        /// </summary>
        /// <param name="filename">The file name of the file to load.</param>
        /// <returns>A new SoundData object.</returns>
        public static SoundData NewSoundData(string filename)
        {
            var decoder = NewDecoder(filename);
            return NewSoundData(decoder);
        }

    }

    public partial class Audio
    {
        /// <summary>
        /// Creates a new Source from file data. 
        /// </summary>
        /// <param name="fdata">The FileData to create a Source from.</param>
        /// <param name="type">Streaming or static source.</param>
        /// <returns></returns>
        public static Source NewSource(FileData fdata, SourceType type)
        {
            return NewSource(Sound.NewDecoder(fdata), type);
        }

        /// <summary>
        /// Creates a new Source from file name. 
        /// </summary>
        /// <param name="filename">The filepath to the audio file.</param>
        /// <param name="type">Streaming or static source.</param>
        /// <returns></returns>
        public static Source NewSource(string filename, SourceType type)
        {
            var file = FileSystem.NewFile(filename);
            var dec = Sound.NewDecoder(file);
            return NewSource(dec, type);
        }

        /// <summary>
        /// Returns the orientation of the listener.
        /// </summary>
        /// <returns>tuple (Forward vector of the listener orientation, Up vector of the listener orientation.)</returns>
        public static Tuple<Vector3, Vector3> GetOrientation()
        {
            Vector3 forword, up;
            GetOrientation(out forword, out up);
            return Tuple.Create(forword, up);
        }
    }

    public partial class Image  // this is part of love module
    {
        /// <summary>
        /// Creates a new <see cref="ImageData"/> object.
        /// </summary>
        /// <param name="filename">The filename of the image file.</param>
        /// <returns>The new ImageData object.</returns>
        public static ImageData NewImageData(string filename)
        {
            var filedata = FileSystem.NewFileData(filename);
            return NewImageData(filedata);
        }

        /// <summary>
        /// Create a new <see cref="CompressedImageData"/> object from a compressed image file. LÖVE supports several compressed texture formats, enumerated in the <see cref="PixelFormat"/> page.
        /// </summary>
        /// <param name="filename">The filename of the compressed image file.</param>
        /// <returns>The new CompressedImageData object.</returns>
        public static CompressedImageData NewCompressedData(string filename)
        {
            var filedata = FileSystem.NewFileData(filename);
            return NewCompressedData(filedata);
        }

        /// <summary>
        /// Creates a new ImageData object.
        /// <para> Vector4[x, y] - new Vector4(0.1f, 0.2f, 0.3f, 0.4f) </para>
        /// </summary>
        /// <param name="rawData">color data to set</param>
        /// <param name="format">The pixel format of the ImageData.</param>
        /// <returns></returns>
        public static ImageData NewImageData(Vector4[,] rawData, ImageDataPixelFormat format)
        {
            Check.ArgumentNull(rawData, "rawData");
            int w = rawData.GetLength(0);
            int h = rawData.GetLength(1);
            Vector4[] data = new Vector4[w * h];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    data[x + y * w] = rawData[x, y];
                }
            }
            var imageData = NewImageData(w, h, format);
            imageData.SetPixelsFloat(data);
            return imageData;
        }


        /// <summary>
        /// Creates a new ImageData object.
        /// </summary>
        /// <param name="rawData">Optional raw byte data to load into the ImageData, in the format specified by format.</param>
        /// <param name="format">The pixel format of the ImageData.</param>
        /// <returns></returns>
        public static ImageData NewImageData(Vector4[][] rawData, ImageDataPixelFormat format)
        {
            Check.ArgumentNull(rawData, "data");
            int w = 0;
            int h = rawData.Length;
            if (h > 0)
            {
                if (rawData[0] == null)
                {
                    throw new Exception("col[0] was null !");
                }
                w = rawData[0].Length;

                for (int i = 1; i < rawData.Length; i++)
                {
                    var row = rawData[i];
                    if (row == null)
                    {
                        throw new Exception($"col[{i}] was null !");
                    }

                    if (row.Length != w)
                    {
                        throw new Exception($"w not equal with col[{i}]");
                    }
                }
            }

            // copy data
            Vector4[] data = new Vector4[w * h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    data[x + y * w] = rawData[x][y];
                }
            }

            var imageData = NewImageData(w, h, format);
            imageData.SetPixelsFloat(data);
            return imageData;
        }


        /// <summary>
        /// Determines whether a file can be loaded as <see cref="CompressedImageData"/>.
        /// </summary>
        /// <param name="filename">The filename of the potentially compressed image file.</param>
        /// <returns>Whether the file can be loaded as <see cref="CompressedImageData"/> or not.</returns>
        public static bool IsCompressed(string filename)
        {
            return IsCompressed(FileSystem.NewFileData(filename));
        }
    }
    public partial class ImageData
    {
        ////// https://stackoverflow.com/questions/5155180/changing-a-c-sharp-delegates-calling-convention-to-cdecl
        ////[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //// public delegate Pixel MapPixelDelegate(int x, int y, Pixel p);
        ////public void MapPixel(int x, int y, int w, int h, MapPixelDelegate func)
        ////{
        ////    Love2dDll.wrap_love_dll_type_ImageData_mapPixel(p, x, y, w, h, new MapPixelDelegate(func));
        ////}

        public delegate Pixel MapPixelDelegate(int x, int y, Pixel p);
        public delegate Vector4 MapPixelColorDelegate(int x, int y, Vector4 p);
        public delegate Color MapColorDelegate(int x, int y, Color color);

        static void WriteVector4(IntPtr dest, int offset, Vector4 input, PixelFormat format)
        {
            Pixel pixel = new Pixel();
            if (format == PixelFormat.RGBA8)
            {
                unchecked
                {
                    pixel.rgba8.r = (byte)(input.r * byte.MaxValue);
                    pixel.rgba8.g = (byte)(input.g * byte.MaxValue);
                    pixel.rgba8.b = (byte)(input.b * byte.MaxValue);
                    pixel.rgba8.a = (byte)(input.a * byte.MaxValue);
                }
            }
            else if (format == PixelFormat.RGBA16)
            {
                unchecked
                {
                    pixel.rgba16.r = (ushort)(input.r * ushort.MaxValue);
                    pixel.rgba16.g = (ushort)(input.g * ushort.MaxValue);
                    pixel.rgba16.b = (ushort)(input.b * ushort.MaxValue);
                    pixel.rgba16.a = (ushort)(input.a * ushort.MaxValue);
                }
            }
            else if (format == PixelFormat.RGBA16F)
            {
                pixel.rgba16f.r = Half.FromFloat(input.r);
                pixel.rgba16f.g = Half.FromFloat(input.g);
                pixel.rgba16f.b = Half.FromFloat(input.b);
                pixel.rgba16f.a = Half.FromFloat(input.a);
            }
            else if (format == PixelFormat.RGBA32F)
            {
                pixel.rgba32f.r = input.r;
                pixel.rgba32f.g = input.g;
                pixel.rgba32f.b = input.b;
                pixel.rgba32f.a = input.a;
            }
            WritePixel(dest, offset, pixel, format); ;
        }

        static void ReadVector4(IntPtr dest, int offset, PixelFormat format, ref Vector4 output)
        {
            Pixel pixel = ReadPixel(dest, offset, format);
            if (format == PixelFormat.RGBA8)
            {
                output.r = (byte)(pixel.rgba8.r * byte.MaxValue);
                output.g = (byte)(pixel.rgba8.g * byte.MaxValue);
                output.b = (byte)(pixel.rgba8.b * byte.MaxValue);
                output.a = (byte)(pixel.rgba8.a * byte.MaxValue);
            }
            else if (format == PixelFormat.RGBA16)
            {
                output.r = pixel.rgba16.r / (float)ushort.MaxValue;
                output.g = pixel.rgba16.g / (float)ushort.MaxValue;
                output.b = pixel.rgba16.b / (float)ushort.MaxValue;
                output.a = pixel.rgba16.a / (float)ushort.MaxValue;
            }
            else if (format == PixelFormat.RGBA16F)
            {
                output.r = pixel.rgba16f.r.ToFloat();
                output.g = pixel.rgba16f.g.ToFloat();
                output.b = pixel.rgba16f.b.ToFloat();
                output.a = pixel.rgba16f.a.ToFloat();
            }
            else if (format == PixelFormat.RGBA32F)
            {
                output.r = pixel.rgba32f.r;
                output.g = pixel.rgba32f.g;
                output.b = pixel.rgba32f.b;
                output.a = pixel.rgba32f.a;
            }
        }

        /// <summary>
        /// <para> Transform an image by applying a function to every pixel. </para>
        /// <para> This function is a higher-order function(https://en.wikipedia.org/wiki/Higher-order_function). It takes another function as a parameter, and calls it once for each pixel in the ImageData. </para>
        /// <para>The passed function is called with six parameters for each pixel in turn. The parameters are numbers that represent the x and y coordinates of the pixel and its red, green, blue and alpha values. The function should return the new red, green, blue, and alpha values for that pixel.</para>
        /// </summary>
        /// <param name="func">Function to apply to every pixel.</param>
        public void MapPixel(MapPixelColorDelegate func)
        {
            var buffer = GetPixelsFloat();
            int w = GetWidth();
            int h = GetHeight();
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int offset = x + y * h;
                    buffer[offset] = func(x, y, buffer[offset]);
                }
            }
            SetPixelsFloat(buffer);
        }


        /// <summary>
        /// <para> Transform an image by applying a function to every pixel. </para>
        /// <para> This function is a higher-order function(https://en.wikipedia.org/wiki/Higher-order_function). It takes another function as a parameter, and calls it once for each pixel in the ImageData. </para>
        /// <para>The passed function is called with six parameters for each pixel in turn. The parameters are numbers that represent the x and y coordinates of the pixel and its red, green, blue and alpha values. The function should return the new red, green, blue, and alpha values for that pixel.</para>
        /// </summary>
        /// <param name="func">Function to apply to every pixel.</param>
        public void MapPixel(MapColorDelegate func)
        {
            var buffer = GetPixelsFloat();
            int w = GetWidth();
            int h = GetHeight();
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int offset = x + y * h;
                    var vc = buffer[offset];
                    var c = func(x, y, new Color(vc.r, vc.g, vc.b, vc.a));
                    buffer[offset] = new Vector4(c.r, c.g, c.b, c.a);
                }
            }
            SetPixelsFloat(buffer);
        }

        /// <summary>
        /// <para> Transform an image by applying a function to every pixel. </para>
        /// <para> This function is a higher-order function(https://en.wikipedia.org/wiki/Higher-order_function). It takes another function as a parameter, and calls it once for each pixel in the ImageData. </para>
        /// <para>The passed function is called with six parameters for each pixel in turn. The parameters are numbers that represent the x and y coordinates of the pixel and its red, green, blue and alpha values. The function should return the new red, green, blue, and alpha values for that pixel.</para>
        /// </summary>
        /// <param name="func">Function to apply to every pixel.</param>
        /// <param name="sx">The x-axis of the top-left corner of the area within the ImageData to apply the function to.</param>
        /// <param name="sy">The y-axis of the top-left corner of the area within the ImageData to apply the function to.</param>
        /// <param name="w">The width of the area within the ImageData to apply the function to.</param>
        /// <param name="h">The height of the area within the ImageData to apply the function to.</param>
        public void MapPixel(MapPixelColorDelegate func, int sx, int sy, int w, int h)
        {
            int ex = sx + w;
            int ey = sy + h;
            if ((0 <= sx && ex - 1 < GetWidth() && 0 <= sy && ey - 1 < GetHeight()) == false)
            {
                throw new Exception("Invalid rectangle dimensions.");
            }

            int pixelSize = 0;
            Love2dDll.inner_wrap_love_dll_type_ImageData_getPixelSize(p, out pixelSize);

            Love2dDll.inner_wrap_love_dll_type_ImageData_lock(p);
            var pointer = GetPointer();
            int iw = GetWidth();
            var format = GetFormat();

            Vector4 input = new Vector4();
            for (int y = sy; y < ey; y++)
            {
                for (int x = sx; x < ex; x++)
                {
                    int offset = (y * iw + x) * pixelSize;
                    ReadVector4(pointer, offset, format, ref input);
                    var output = func(x, y, input);
                    WriteVector4(pointer, offset, output, format);
                }
            }
            Love2dDll.inner_wrap_love_dll_type_ImageData_unlock(p);
        }

        static void WritePixel(IntPtr dest, int offset, Pixel pixel, PixelFormat format)
        {
            if (format == PixelFormat.RGBA8)
            {
                Marshal.WriteInt32(dest, offset, pixel.intValue0);
            }
            else if (format == PixelFormat.RGBA16)
            {
                Marshal.WriteInt64(dest, offset, pixel.longValue0);
            }
            else if (format == PixelFormat.RGBA16F)
            {
                Marshal.WriteInt64(dest, offset, pixel.longValue0);
            }
            else if (format == PixelFormat.RGBA32F)
            {
                Marshal.WriteInt64(dest, offset, pixel.longValue0);
                Marshal.WriteInt64(dest, offset + 8, pixel.longValue1);
            }
        }

        static Pixel ReadPixel(IntPtr dest, int offset, PixelFormat format)
        {
            Pixel pixel = new Pixel();
            if (format == PixelFormat.RGBA8)
            {
                pixel.intValue0 = Marshal.ReadInt32(dest, offset);
            }
            else if (format == PixelFormat.RGBA16)
            {
                pixel.longValue0 = Marshal.ReadInt64(dest, offset);
            }
            else if (format == PixelFormat.RGBA16F)
            {
                pixel.longValue0 = Marshal.ReadInt64(dest, offset);
            }
            else if (format == PixelFormat.RGBA32F)
            {
                pixel.longValue0 = Marshal.ReadInt64(dest, offset);
                pixel.longValue1 = Marshal.ReadInt64(dest, offset + 8);
            }

            return pixel;
        }

        /// <summary>
        /// <para> Advance version of <see cref="MapPixel(MapPixelColorDelegate, int, int, int, int)"/>,</para>
        /// <para>if you don't know how to handle pixel format, please use <see cref="MapPixel(MapPixelColorDelegate, int, int, int, int)"/> </para>
        /// <para>if you need speed, consider use <see cref="SetPixels(Pixel[])"/></para>
        /// </summary>
        /// <param name="func">Function to apply to every pixel.</param>
        /// <param name="sx">The x-axis of the top-left corner of the area within the ImageData to apply the function to.</param>
        /// <param name="sy">The y-axis of the top-left corner of the area within the ImageData to apply the function to.</param>
        /// <param name="w">The width of the area within the ImageData to apply the function to.</param>
        /// <param name="h">The height of the area within the ImageData to apply the function to.</param>
        public void MapPixel(MapPixelDelegate func, int sx, int sy, int w, int h)
        {
            int ex = sx + w;
            int ey = sy + h;
            if ((0 <= sx && ex - 1 < GetWidth() && 0 <= sy && ey - 1 < GetHeight()) == false)
            {
                throw new Exception("Invalid rectangle dimensions.");
            }


            int pixelSize = 0;
            Love2dDll.inner_wrap_love_dll_type_ImageData_getPixelSize(p, out pixelSize);

            Love2dDll.inner_wrap_love_dll_type_ImageData_lock(p);
            var pointer = GetPointer();
            int iw = GetWidth();
            var format = GetFormat();


            for (int y = sy; y < ey; y++)
            {
                for (int x = sx; x < ex; x++)
                {
                    int offset = (y * iw + x) * pixelSize;
                    var input = ReadPixel(pointer, offset, format);
                    var output = func(x, y, input);
                    WritePixel(pointer, offset, output, format);
                }
            }
            Love2dDll.inner_wrap_love_dll_type_ImageData_unlock(p);
        }

        /// <summary>
        /// <para> Advance version of <see cref="MapPixel(MapPixelColorDelegate, int, int, int, int)"/>,</para>
        /// <para>if you don't know how to handle pixel format, please use <see cref="MapPixel(MapPixelColorDelegate, int, int, int, int)"/> </para>
        /// <para>if you need speed, consider use <see cref="SetPixels(Pixel[])"/></para>
        /// </summary>
        /// <param name="func">Function to apply to every pixel.</param>
        public void MapPixel(MapPixelDelegate func)
        {
            var buffer = GetPixels();
            int w = GetWidth();
            int h = GetHeight();
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int offset = x + y * h;
                    buffer[offset] = func(x, y, buffer[offset]);
                }
            }
            SetPixels(buffer);
        }


        /// <summary>
        /// get every pixel
        /// </summary>
        public Pixel[] GetPixels()
        {
            int w = GetWidth();
            int h = GetHeight();
            Pixel[] output = new Pixel[w * h];

            int pixelSize = 0;
            Love2dDll.inner_wrap_love_dll_type_ImageData_getPixelSize(p, out pixelSize);
            if (pixelSize == 16) // r,g,b,a 32 / 32f (16 bytes)
            {
                long[] buffer = new long[w * h * 2];
                Love2dDll.inner_wrap_love_dll_type_ImageData_lock(p);
                var pointer = GetPointer();
                Marshal.Copy(pointer, buffer, 0, buffer.Length);
                Love2dDll.inner_wrap_love_dll_type_ImageData_unlock(p);

                for (int i = 0; i < w * h; i++)
                {
                    output[i].longValue0 = buffer[i * 2];
                    output[i].longValue1 = buffer[i * 2 + 1];
                }
            }
            else if (pixelSize == 8) // r,g,b,a 16 / 16f  (8 bytes)
            {
                long[] buffer = new long[w * h];
                Love2dDll.inner_wrap_love_dll_type_ImageData_lock(p);
                var pointer = GetPointer();
                Marshal.Copy(pointer, buffer, 0, buffer.Length);
                Love2dDll.inner_wrap_love_dll_type_ImageData_unlock(p);

                for (int i = 0; i < w * h; i++)
                {
                    output[i].longValue0 = buffer[i];
                }
            }
            else if (pixelSize == 4)// r,g,b,a 8  (4 bytes)
            {
                int[] buffer = new int[w * h];
                Love2dDll.inner_wrap_love_dll_type_ImageData_lock(p);
                var pointer = GetPointer();
                Marshal.Copy(pointer, buffer, 0, buffer.Length);
                Love2dDll.inner_wrap_love_dll_type_ImageData_unlock(p);

                for (int i = 0; i < w * h; i++)
                {
                    output[i].intValue0 = buffer[i];
                }
            }

            return output;
        }

        /// <summary>
        /// get every pixel, as Float4 format
        /// </summary>
        /// <returns></returns>
        public Vector4[] GetPixelsFloat()
        {
            int w = GetWidth();
            int h = GetHeight();
            Vector4[] output = new Vector4[w * h];
            DllTool.ExecuteAsIntprt(output, dest => Love2dDll.inner_wrap_love_dll_type_ImageData_getPixels_float4(p, dest));
            return output;
        }

        /// <summary>
        /// set every pixel with given data
        /// </summary>
        /// <param name="data"></param>
        public void SetPixels(Pixel[] data)
        {
            Check.ArgumentNull(data, "data");
            int w = GetWidth();
            int h = GetHeight();
            if (data.Length != w * h)
            {
                throw new Exception("Length of input data not equal with GetWidth() * GetHeight() ");
            }

            int pixelSize = 0;
            Love2dDll.inner_wrap_love_dll_type_ImageData_getPixelSize(p, out pixelSize);


            if (pixelSize == 16) // r,g,b,a 32 / 32f (16 bytes)
            {
                long[] buffer = new long[w * h * 2];
                for (int i = 0; i < w * h; i++)
                {
                    buffer[i * 2] = data[i].longValue0;
                    buffer[i * 2 + 1] = data[i].longValue1;
                }

                Love2dDll.inner_wrap_love_dll_type_ImageData_lock(p);
                var pointer = GetPointer();
                Marshal.Copy(buffer, 0, pointer, buffer.Length);
                Love2dDll.inner_wrap_love_dll_type_ImageData_unlock(p);
            }
            else if (pixelSize == 8) // r,g,b,a 16 / 16f  (8 bytes)
            {
                long[] buffer = new long[w * h];
                for (int i = 0; i < w * h; i++)
                {
                    buffer[i] = data[i].longValue0;
                }

                Love2dDll.inner_wrap_love_dll_type_ImageData_lock(p);
                var pointer = GetPointer();
                Marshal.Copy(buffer, 0, pointer, buffer.Length);
                Love2dDll.inner_wrap_love_dll_type_ImageData_unlock(p);
            }
            else if (pixelSize == 4)// r,g,b,a 8  (4 bytes)
            {
                int[] buffer = new int[w * h];
                for (int i = 0; i < w * h; i++)
                {
                    buffer[i] = data[i].intValue0;
                }

                Love2dDll.inner_wrap_love_dll_type_ImageData_lock(p);
                var pointer = GetPointer();
                Marshal.Copy(buffer, 0, pointer, buffer.Length);
                Love2dDll.inner_wrap_love_dll_type_ImageData_unlock(p);
            }
        }

        /// <summary>
        /// set every pixel with given data, function will convert Float4 to correct pixel format automatically
        /// </summary>
        /// <param name="data">color data to set</param>
        public void SetPixelsFloat(Vector4[] data)
        {
            Check.ArgumentNull(data, "data");
            int w = GetWidth();
            int h = GetHeight();
            if (data.Length != w * h)
            {
                throw new Exception("Length of input data not equal with GetWidth() * GetHeight() ");
            }

            Love2dDll.inner_wrap_love_dll_type_ImageData_setPixels_float4(p, data);
        }

    }

    public partial class Video
    {
        /// <summary>
        /// Creates a new VideoStream. Currently only Ogg Theora video files are supported. VideoStreams can't draw videos, see love.graphics.newVideo for that.
        /// </summary>
        /// <param name="filename">The file path to the Ogg Theora video file.</param>
        /// <returns>A new VideoStream.</returns>
        public static VideoStream NewVideoStream(string filename)
        {
            var file = FileSystem.NewFile(filename);
            return NewVideoStream(file);
        }

        /// <summary>
        /// Starts playing the Video.
        /// </summary>
        public void Play()
        {
            GetStream().Play();
        }

        /// <summary>
        /// Pauses the Video.
        /// </summary>
        public void Pause()
        {
            GetStream().Pause();
        }

        /// <summary>
        /// Sets the current playback position of the Video.
        /// </summary>
        /// <param name="offset"></param>
        public void Seek(double offset)
        {
            GetStream().Seek(offset);
        }

        /// <summary>
        /// Rewinds the Video to the beginning.
        /// </summary>
        public void Rewind()
        {
            GetStream().Rewind();
        }

        /// <summary>
        /// Gets the current playback position of the Video.
        /// </summary>
        /// <returns></returns>
        public double Tell()
        {
            return GetStream().Tell();
        }

        /// <summary>
        /// Gets whether the Video is currently playing.
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            return GetStream().IsPlaying();
        }

    }

    public partial class Graphics
    {

        /// <summary>
        /// Creates a new Canvas.
        /// <para>This function can be slow if it is called repeatedly, such as from love.update or love.draw. If you need to use a specific resource often, create it once and store it somewhere it can be reused!</para>
        /// </summary>
        /// <returns>A new Canvas with dimensions equal to the window's size in pixels.</returns>
        public static Canvas NewCanvas()
        {
            Window.GetMode(out int w, out int h);
            return NewCanvas(w, h);
        }

        /// <summary>
        /// Creates a new drawable Text object.
        /// </summary>
        /// <param name="font">The font to use for the text.</param>
        /// <param name="coloredStr">The initial string of text that the new Text object will contain. May be nil.</param>
        /// <returns></returns>
        public static Text NewText(Font font, string coloredStr)
        {
            Check.ArgumentNull(font, "font");
            return NewText(font, ColoredStringArray.Create(coloredStr));
        }

        public static Image NewImage(ImageData imageData, bool flagMipmaps = false, bool flagLinear = false)
        {
            return NewImage(new ImageData[] { imageData }, flagMipmaps, flagLinear);
        }
        public static Image NewImage(CompressedImageData compressedImageData, bool flagMipmaps = false, bool flagLinear = false)
        {
            return NewImage(new CompressedImageData[] { compressedImageData }, flagMipmaps, flagLinear);
        }

        ///// <summary>
        ///// Creates a new Image from a filepath.
        ///// </summary>
        ///// <param name="filename">The filepath to the image file.</param>
        ///// <returns></returns>
        //public static Image NewImage(string filename)
        //{
        //    var imagedata = Image.NewImageData(filename);
        //    return NewImage(imagedata);
        //}

        /// <summary>
        /// Creates a new Image from a filepath.
        /// </summary>
        /// <param name="filename">The filepath to the image file .</param>
        /// <param name="flagMipmaps">If true, mipmaps for the image will be automatically generated (or taken from the images's file if possible, if the image originated from a CompressedImageData). If this value is a table, it should contain a list of other filenames of images of the same format that have progressively half-sized dimensions, all the way down to 1x1. Those images will be used as this Image's mipmap levels.</param>
        /// <param name="flagLinear">True if the image's pixels should be interpreted as being linear RGB rather than sRGB-encoded, if gamma-correct rendering is enabled. Has no effect otherwise.</param>
        /// <returns></returns>
        public static Image NewImage(string filename, bool flagMipmaps = false, bool flagLinear = false)
        {
            var imagedata = Image.NewImageData(filename);
            return NewImage(imagedata, flagMipmaps, flagLinear);
        }

        /// <summary>
        /// Create a new instance of the default font (Vera Sans) with a custom size.
        /// </summary>
        /// <returns></returns>
        public static Font NewFont(int size, HintingMode hinting = HintingMode.Normal)
        {
            var rasterizer = Font.NewTrueTypeRasterizer(size, hinting);
            return NewFont(rasterizer);
        }

        /// <summary>
        /// </summary>
        /// <param name="filename">The filepath to the BMFont file.</param>
        /// <param name="imageFileName">The filepath to the BMFont's image file.</param>
        /// <returns></returns>
        public static Font NewBMFont(string filename, params string[] imageFileName)
        {
            if (imageFileName == null)
            {
                throw new Exception("imageFilename array can't be null !");
            }

            var imageData = new ImageData[imageFileName.Length];
            for (int i = 0; i < imageFileName.Length; i++)
            {
                imageData[i] = Image.NewImageData(imageFileName[i]);
            }
            var filedata  = FileSystem.NewFileData(filename);
            var rasterizerImage = Font.NewBMFontRasterizer(filedata, imageData);
            return NewFont(rasterizerImage);
        }


        /// <summary>
        /// Create a new BMFont. The filepath to the BMFont's image file specified inside the BMFont file will be used.
        /// </summary>
        /// <param name="filename">The filepath to the BMFont file.</param>
        /// <returns></returns>
        public static Font NewBMFont(string filename)
        {
            return NewBMFont(filename, new string[0]);
        }

        /// <summary>
        /// Creates a new Font by loading a specifically formatted image.
        /// <para>In versions prior to 0.9.0, LÖVE expects ISO 8859-1 encoding for the glyphs string.</para>
        /// <para>	This function can be slow if it is called repeatedly, such as from Scene.Update. If you need to use a specific resource often, create it once and store it somewhere it can be reused!</para>
        /// </summary>
        /// <param name="filename">The filepath to the image file.</param>
        /// <param name="glyphs">A string of the characters in the image in order from left to right.</param>
        /// <param name="extraspacing">Additional spacing (positive or negative) to apply to each glyph in the Font.</param>
        /// <returns></returns>
        public static Font NewImageFont(string filename, string glyphs, int extraspacing = 0)
        {
            var imageData = Image.NewImageData(filename);
            var glyphsBytes = DllTool.GetNullTailUTF8Bytes(glyphs);
            var rasterizerImage = Font.NewImageRasterizer(imageData, glyphsBytes, extraspacing);
            return NewFont(rasterizerImage);
        }

        /// <summary>
        /// Create a new TrueType font.
        /// </summary>
        /// <param name="filename">The filepath to the TrueType font file.</param>
        /// <param name="size">The size of the font in pixels.</param>
        /// <param name="hinting">True Type hinting mode.</param>
        /// <returns>A Font object which can be used to draw text on screen.</returns>
        public static Font NewFont(string filename, int size = 12, HintingMode hinting = HintingMode.Normal)
        {
            var fileData = FileSystem.NewFileData(filename);
            var rasterizer = Font.NewTrueTypeRasterizer(fileData, size, hinting);
            return NewFont(rasterizer);
        }

        /// <summary>
        /// Creates a new drawable Video. Currently only Ogg Theora video files are supported.
        /// </summary>
        /// <param name="filename">The file path to the Ogg Theora video file.</param>
        /// <param name="audio">Whether to try to load the video's audio into an audio Source. If not explicitly set to true or false, it will try without causing an error if the video has no audio.</param>
        /// <param name="dipScale">The DPI scale factor of the video. if it was null, value will be Graphics.GetDPIScale()</param>
        /// <returns></returns>
        Video NewVideo(string filename, bool audio = true, float? dipScale = null)
        {
            return NewVideo(Video.NewVideoStream(filename), audio, dipScale);
        }


        /// <summary>
        /// Clears the screen to transparent black (0, 0, 0, 0).
        /// </summary>
        public static void Clear()
        {
            var color = GetBackgroundColor();
            Clear(0, 0, 0, 0);
        }

        /// <summary>
        /// Clears the screen to transparent black (0, 0, 0, 0).
        /// </summary>
        public static void Clear(Color c)
        {
            Clear(c.R, c.G, c.B, c.A);
        }

        /// <summary>
        /// Sets the color used for drawing.
        /// </summary>
        public static void SetColor(Color color)
        {
            SetColor(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Sets the color used for drawing.
        /// </summary>
        public static void SetColor(Vector4 color)
        {
            SetColor(color.X, color.Y, color.Z, color.W);
        }

        /// <summary>
        /// Draws a circle.
        /// </summary>
        /// <param name="mode_type">How to draw the circle.</param>
        /// <param name="center">The position of the center .</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="points">The number of segments used for drawing the circle. Note: The default variable for the segments parameter varies between different versions of LÖVE.</param>
        /// <returns></returns>
        public static void Circle(DrawMode mode_type, Vector2 center, float radius)
        {
            Circle(mode_type, center.X, center.Y, radius);
        }
        /// <summary>
        /// Draws a circle.
        /// </summary>
        /// <param name="mode_type">How to draw the circle.</param>
        /// <param name="center">The position of the center .</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="points">The number of segments used for drawing the circle. Note: The default variable for the segments parameter varies between different versions of LÖVE.</param>
        /// <returns></returns>
        public static void Circle(DrawMode mode_type, Vector2 center, float radius, int point)
        {
            Circle(mode_type, center.X, center.Y, radius, point);
        }


        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="mode">How to draw the rectangle.</param>
        /// <param name="rect">the rectangle to draw.</param>
        public static void Rectangle(DrawMode mode, RectangleF rect)
        {
            Rectangle(mode, rect.x, rect.y, rect.width, rect.height);
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="mode">How to draw the rectangle.</param>
        /// <param name="rect">the rectangle to draw.</param>
        public static void Rectangle(DrawMode mode, Rectangle rect)
        {
            Rectangle(mode, rect.x, rect.y, rect.width, rect.height);
        }

        /// <summary>
        /// Draws an arc using the "pie" ArcType.
        /// </summary>
        /// <param name="draw_mode"></param>
        /// <param name="x">The position of the center along x-axis.</param>
        /// <param name="y">The position of the center along y-axis.</param>
        /// <param name="radius">Radius of the arc.</param>
        /// <param name="angle1">The angle at which the arc begins.</param>
        /// <param name="angle2">The angle at which the arc terminates.</param>
        public static void Arc(DrawMode draw_mode, float x, float y, float radius, float angle1, float angle2)
        {
            Arc(draw_mode, ArcType.Pie, x, y, radius, angle1, angle2);
        }

        /// <summary>
        /// Discards (trashes) the contents of the screen or active Canvas. This is a performance optimization function with niche use cases.
        /// </summary>
        /// <param name="discardColor">Whether to discard the texture(s) of the active Canvas(es) (the contents of the screen if no Canvas is active.)</param>
        /// <param name="discardStencil">Whether to discard the contents of the stencil buffer of the screen / active Canvas.</param>
        public static void Discard(bool discardColor = true, bool discardStencil = true)
        {
            var dc = new bool[] { discardColor };
            Discard(dc, discardStencil);
        }

        public static Mesh NewMesh(IEnumerable<MeshAttribFormat> formatList, byte[] data, 
            MeshDrawMode drawMode = MeshDrawMode.Fan, SpriteBatchUsage usage = SpriteBatchUsage.Dynamic)
        {
            string[] strList = formatList.Select(item => item.name).ToArray();
            int[] typeList = formatList.Select(item => (int)item.type).ToArray();
            int[] comCountList = formatList.Select(item => item.componentCount).ToArray();

            IntPtr meshPtr = IntPtr.Zero;
            DllTool.ExecuteNullTailStringArray(strList, (strListPtr) =>
            {
                Love2dDll.wrap_love_dll_graphics_newMesh_custom(strListPtr, typeList, comCountList, strListPtr.Length, 
                    true, data, data.Length, (int)drawMode, (int)usage, out meshPtr);
            });
            return LoveObject.NewObject<Mesh>(meshPtr);
        }
        public static Mesh NewMesh(IEnumerable<MeshAttribFormat> formatList, int count, MeshDrawMode drawMode = MeshDrawMode.Fan, SpriteBatchUsage usage = SpriteBatchUsage.Dynamic)
        {
            string[] strList = formatList.Select(item => item.name).ToArray();
            int[] typeList = formatList.Select(item => (int)item.type).ToArray();
            int[] comCountList = formatList.Select(item => item.componentCount).ToArray();

            IntPtr meshPtr = IntPtr.Zero;
            DllTool.ExecuteNullTailStringArray(strList, (strListPtr) =>
            {
                Love2dDll.wrap_love_dll_graphics_newMesh_custom(strListPtr, typeList, comCountList, strListPtr.Length,
                    false, null, count, (int)drawMode, (int)usage, out meshPtr);
            });
            return LoveObject.NewObject<Mesh>(meshPtr);
        }

        public static void Points(ColoredPoint[] coloredPoints)
        {
            Vector2[] coords = new Vector2[coloredPoints.Length];
            Vector4[] colors = new Vector4[coloredPoints.Length];
            for (int i = 0; i< coloredPoints.Length; i++)
            {
                coords[i] = coloredPoints[i].pos;
                colors[i] = coloredPoints[i].color;
            }
            Points(coords, colors);
        }

        public static void Print(ColoredString[] coloredStr, float x, float y, float angle = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, float kx = 0, float ky = 0)
        {
            Print(new ColoredStringArray(coloredStr), x, y, angle, sx, sy, ox, oy, kx, ky);
        }

        /// <summary>
        /// <para> Draws formatted text, with word wrap and alignment.</para>
        /// <para> See additional notes in love.graphics.print. </para>
        /// <para>In version 0.9.2 and earlier, wrapping was implemented by breaking up words by spaces and putting them back together to make sure things fit nicely within the limit provided. However, due to the way this is done, extra spaces between words would end up missing when printed on the screen, and some lines could overflow past the provided wrap limit. In version 0.10.0 and newer this is no longer the case.</para>
        /// <para>Aligning does not work as one might expect! It doesn't align to the x/y coordinates given, but in a rectangle, where the limit is the width.</para>
        /// <para>Text may appear blurry if it's rendered at non-integer pixel locations.</para>
        /// </summary>
        /// <param name="text">A text string.</param>
        /// <param name="x">The position on the x-axis.</param>
        /// <param name="y">The position on the y-axis.</param>
        /// <param name="wrap"></param>
        /// <param name="align_type">Wrap the line after this many horizontal pixels.</param>
        /// <param name="angle">Orientation (radians).</param>
        /// <param name="sx">Scale factor (x-axis).</param>
        /// <param name="sy">Scale factor (y-axis).</param>
        /// <param name="ox">Origin offset (x-axis).</param>
        /// <param name="oy">Origin offset (y-axis).</param>
        /// <param name="kx">Shearing factor (x-axis).</param>
        /// <param name="ky">Shearing factor (y-axis).</param>
        public static void Printf(string text, float x, float y, float wrap, AlignMode align_type = AlignMode.Left, float angle = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, float kx = 0, float ky = 0)
        {
            Printf(ColoredStringArray.Create(text), x, y, wrap, align_type, angle, sx, sy, ox, oy, kx, ky);
        }

        /// <summary>
        /// Disables stencil testing.
        /// </summary>
        public static void SetStencilTest()
        {
            SetStencilTest(CompareMode.Always, 0);
        }

        /// <summary>
        /// Gets the width and height of the window.
        /// </summary>
        /// <returns></returns>
        public static Size GetDimensions()
        {
            return new Size(GetWidth(), GetHeight());
        }

        /// <summary>
        /// Draws lines between points.
        /// </summary>
        /// <param name="points">must be an integer multiple of 2. [first(x, y), second(x, y) ....]</param>
        public static void Line(params float[] points)
        {
            Line(Vector2.Array(points));
        }

        /// <summary>
        /// Draws one or more points.
        /// </summary>
        /// <param name="points">must be an integer multiple of 2. [first(x, y), second(x, y) ....]</param>
        public static void Points(params float[] points)
        {
            Points(Vector2.Array(points));
        }

        /// <summary>
        /// Draw a polygon.
        /// </summary>
        /// <param name="mode">How to draw the polygon.</param>
        /// <param name="points">must be an integer multiple of 2. [first(x, y), second(x, y) ....]</param>
        public static void Polygon(DrawMode mode, params float[] points)
        {
            Polygon(mode, Vector2.Array(points));
        }

        /// <summary>
        /// <para>以二维方式缩放坐标系。</para>
        /// <para>默认情况下，LÖVE中的坐标系在水平和垂直方向上一对一显示像素，x轴向右增加，y轴向下增加。 缩放坐标系会改变这种关系。</para>
        /// <para>在通过sx和sy进行缩放之后，所有坐标都被视为与sx和sy相乘。 绘图操作的每个结果也相应地缩放，例如按（2,2）缩放将意味着在x和y方向上使所有内容都变为两倍。 按负值缩放会使坐标系在相应的方向上翻转，所有内容都会被翻转或颠倒（或两者兼而有之）。 按零缩放没有意义。</para>
        /// <para>缩放(Scale)操作和平移(Translate)操作不是可交换操作，以不同的顺序调用它们会产生不同的结果。</para>
        /// <para>效果持续到 Scene.Draw 调用结束（每一帧画面绘制结束都会自动重置为1倍。）</para>
        /// 
        /// <para>Scales the coordinate system in two dimensions.</para> 
        /// <para>By default the coordinate system in LÖVE corresponds to the display pixels in horizontal and vertical directions one-to-one, and the x-axis increases towards the right while the y-axis increases downwards. Scaling the coordinate system changes this relation.</para>
        /// <para>After scaling by sx and sy, all coordinates are treated as if they were multiplied by sx and sy. Every result of a drawing operation is also correspondingly scaled, so scaling by (2, 2) for example would mean making everything twice as large in both x- and y-directions. Scaling by a negative value flips the coordinate system in the corresponding direction, which also means everything will be drawn flipped or upside down, or both. Scaling by zero is not a useful operation.</para>
        /// <para>Scale and translate are not commutative operations, therefore, calling them in different orders will change the outcome.</para>
        /// <para>Scaling lasts until Scene.Draw exits.</para>
        /// </summary>
        /// <param name="scale">The scaling on each axis.</param>
        public static void Scale(Vector2 scale)
        {
            Scale(scale.X, scale.Y);
        }








        /// <summary>
        /// Captures drawing operations to a Canvas
        /// <para>Sets the render target to a specified Canvas. All drawing operations until the next love.graphics.setCanvas call will be redirected to the Canvas and not shown on the screen.</para>
        /// <para>if Length of canvas is zero, then resets the render target to the screen, i.e. re-enables drawing to the screen.</para>
        /// </summary>
        /// <param name="canvas"></param>
        public static void SetCanvas(RenderTargetInfo renderTargetInfo)
        {

            if (renderTargetInfo.RenderTargetList.Count == 0 && renderTargetInfo.DepthStencil == null)
            {
                SetCanvas();
                return;
            }

            int tcount = renderTargetInfo.RenderTargetList.Count + 1;
            IntPtr[] canvaList = new IntPtr[tcount];
            int[] sliceList = new int[tcount];
            int[] mipmapList = new int[tcount];

            if (renderTargetInfo.DepthStencil != null && renderTargetInfo.DepthStencil.canvas != null)
            {
                canvaList[0] = renderTargetInfo.DepthStencil.canvas.p;
                sliceList[0] = renderTargetInfo.DepthStencil.slice;
                mipmapList[0] = renderTargetInfo.DepthStencil.mipmap;
            }

            for (int i = 1; i < tcount; i++)
            {
                var rt = renderTargetInfo.RenderTargetList[i - 1];
                canvaList[i] = rt.canvas.p;
                sliceList[i] = rt.slice;
                mipmapList[i] = rt.mipmap;
            }
            Love2dDll.wrap_love_dll_graphics_setCanvasRenderTagets(canvaList, sliceList, mipmapList, canvaList.Length, renderTargetInfo.tempDepthFlag, renderTargetInfo.tempStencilFlag);
        }


        /// <summary>
        /// Returns the current target Canvas. Returns zero length array if drawing to the real screen.
        /// </summary>
        public static RenderTargetInfo GetCanvas()
        {
            Love2dDll.wrap_love_dll_graphics_getCanvasTagets(out var canvasPtr, out var sliceListPtr, out var mipmapListPtr, out int targetCount);
            var canvasList = DllTool.ReadIntPtrsWithConvertAndRelease<Canvas>(canvasPtr, targetCount);
            var sliceList = DllTool.ReadInt32sAndRelease(sliceListPtr, targetCount);
            var mipmapList = DllTool.ReadInt32sAndRelease(mipmapListPtr, targetCount);

            List<RenderTarget> list = new List<RenderTarget>(targetCount);
            RenderTarget depthStencil = null;
            for (int i = 0; i < targetCount; i++)
            {
                var rt = RenderTarget.FromCanvas(canvasList[i], sliceList[i], mipmapList[i]);
                if (i == 0)
                    depthStencil = rt;
                else
                    list.Add(rt);
            }

            return new RenderTargetInfo(list, depthStencil);
        }



        /// <summary>
        /// Captures drawing operations to a Canvas
        /// <para>Sets the render target to a specified Canvas. All drawing operations until the next love.graphics.setCanvas call will be redirected to the Canvas and not shown on the screen.</para>
        /// <para>if Length of canvas is zero, then resets the render target to the screen, i.e. re-enables drawing to the screen.</para>
        /// </summary>
        /// <param name="canvas"></param>
        public static void SetCanvas(params Canvas[] canvas)
        {
            if (canvas == null)
            {
                canvas = new Canvas[0];
            }

            if (canvas.Length == 0)
            {
                Love2dDll.wrap_love_dll_graphics_setCanvasEmpty();
                return;
            }

            SetCanvas(RenderTargetInfo.CreateWithDepthStencil(null, canvas));
        }

        //public static Canvas[] GetCanvas()
        //{
        //    IntPtr out_canvaList = IntPtr.Zero;
        //    int out_canvaList_length = 0;
        //    Love2dDll.wrap_love_dll_graphics_getCanvas(out out_canvaList, out out_canvaList_length);
        //    var buffer = DllTool.ReadIntPtrsAndRelease(out_canvaList, out_canvaList_length);

        //    Canvas[] canvas = new Canvas[buffer.Length];
        //    for (int i = 0; i < buffer.Length; i++)
        //    {
        //        canvas[i] = LoveObject.NewObject<Canvas>(buffer[i]);
        //    }

        //    return canvas;
        //}
    }
}

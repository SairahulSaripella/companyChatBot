using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dialogs
{
       ///
        /// This class shall keep all the functionality for capturing
        /// the desktop.
        ///
        public class CaptureScreen
        {

            protected static IntPtr m_HBitmap;


            public static Bitmap GetDesktopImage()
            {

            WIN32_API.SIZE size;

                // //



            

                //Variable to keep the handle to bitmap.
                IntPtr hBitmap;

                //Here we get the handle to the desktop device context.
                IntPtr hDC = WIN32_API.GetDC(WIN32_API.GetDesktopWindow());
                

                //Here we make a compatible device context in memory for screen
                //device context.
                IntPtr hMemDC = WIN32_API.CreateCompatibleDC(hDC);

                //We pass SM_CXSCREEN constant to GetSystemMetrics to get the
                //X coordinates of the screen.
                size.cx = WIN32_API.GetSystemMetrics(WIN32_API.SM_CXSCREEN);

                //We pass SM_CYSCREEN constant to GetSystemMetrics to get the
                //Y coordinates of the screen.
                size.cy = WIN32_API.GetSystemMetrics(WIN32_API.SM_CYSCREEN);

                //We create a compatible bitmap of the screen size and using
                //the screen device context.
                hBitmap = WIN32_API.CreateCompatibleBitmap(hDC, size.cx, size.cy);

                //As hBitmap is IntPtr, we cannot check it against null.
                //For this purpose, IntPtr.Zero is used.
                if (hBitmap != IntPtr.Zero)
                {
                    //Here we select the compatible bitmap in the memeory device
                    //context and keep the refrence to the old bitmap.
                    IntPtr hOld = (IntPtr)WIN32_API.SelectObject(hMemDC, hBitmap);
                //We copy the Bitmap to the memory device context.
                WIN32_API.BitBlt(hMemDC, 0, 0, size.cx, size.cy, hDC, 0, 0, WIN32_API.SRCCOPY);
                //We select the old bitmap back to the memory device context.
                WIN32_API.SelectObject(hMemDC, hOld);
                //We delete the memory device context.
                WIN32_API.DeleteDC(hMemDC);
                //We release the screen device context.
                WIN32_API.ReleaseDC(WIN32_API.GetDesktopWindow(), hDC);
                    //Image is created by Image bitmap handle and stored in
                    //local variable.
                    return System.Drawing.Image.FromHbitmap(hBitmap);
                     
                
                }
                //If hBitmap is null, retun null.
                return null;
            }
            
        }
 

}
public class WIN32_API
{

    public struct SIZE
    {
        public int cx;
        public int cy;
    }

    public const int SRCCOPY = 13369376;

    [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
    public static extern IntPtr DeleteDC(IntPtr hDc);

    [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
    public static extern IntPtr DeleteObject(IntPtr hDc);

    [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
    public static extern bool BitBlt(IntPtr hdcDest, int xDest,
        int yDest, int wDest, int hDest, IntPtr hdcSource,
        int xSrc, int ySrc, int RasterOp);

    [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
    public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc,
        int nWidth, int nHeight);

    [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);

    public const int SM_CXSCREEN = 0;
    public const int SM_CYSCREEN = 1;



    [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
    public static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll", EntryPoint = "GetDC")]
    public static extern IntPtr GetDC(IntPtr ptr);

    [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
    public static extern int GetSystemMetrics(int abc);

    [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
    public static extern IntPtr GetWindowDC(Int32 ptr);

    [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
    public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

}
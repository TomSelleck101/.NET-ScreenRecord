using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenRecordLibrary
{
    public class ScreenRecord
    {
        private const string ScreenshotFolder = "Screenshots";
        private const string RecordingsFolder = "Recordings";

        // Account for scaled screens 
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        // Create map of extensions to ImageFormats
        private Dictionary<string, ImageFormat> extensionMap = new()
        {
            { "jpg", ImageFormat.Jpeg },
            { "png", ImageFormat.Png }
        };

        public ScreenRecord()
        {
            SetProcessDPIAware();

            // Create screenshot and recording folders if they don't exist
            if (!Directory.Exists(ScreenshotFolder))
            {
                Directory.CreateDirectory(ScreenshotFolder);
            }

            if (!Directory.Exists(RecordingsFolder))
            {
                Directory.CreateDirectory(RecordingsFolder);
            }
        }
        /// <summary>
        /// Capture and save a screenshot to the disk
        /// </summary>
        public void CaptureScreenshot(string destinationDirectory = ScreenshotFolder, string targetImageFormat = "png")
        {
            // Check if a valid filetype was supplied
            if (!extensionMap.ContainsKey(targetImageFormat))
            {
                throw new ArgumentException($"Supplied target format \"{targetImageFormat}\" is not supported. Supported formats are {extensionMap.Keys}");
            }

            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);

                string filename = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}";

                // Save image to the destination path with the supplied format
                bitmap.Save($"{destinationDirectory}\\{filename}.{targetImageFormat}", extensionMap[targetImageFormat]);
            }
        }
    }
}

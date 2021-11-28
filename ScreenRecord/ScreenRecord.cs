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
        public bool IsRecording = false;

        private string CurrentRecordingFolder = "";
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
        public string CaptureScreenshot(string destinationDirectory = ScreenshotFolder, string targetImageFormat = "png")
        {
            // Check if a valid filetype was supplied
            if (!extensionMap.ContainsKey(targetImageFormat))
            {
                throw new ArgumentException($"Supplied target format \"{targetImageFormat}\" is not supported. Supported formats are {extensionMap.Keys}");
            }

            // Set the filename for the image to be created
            string filename = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}";
            string destinationPath = $"{destinationDirectory}\\{filename}.{targetImageFormat}";

            // Capture the screenshot and write to disk
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);

                // Save image to the destination path with the supplied format
                bitmap.Save(destinationPath, extensionMap[targetImageFormat]);
            }

            // Return the relative path to the image
            return destinationPath;
        }

        /// <summary>
        /// Begin capturing images on seperate thread
        /// </summary>
        public bool StartRecording(out string error, string destinationDirectory = RecordingsFolder)
        {
            if (IsRecording)
            {
                error = $"Recording already in progress  {CurrentRecordingFolder}";
                return false;
            }

            // Create a folder with the current 
            string destinationFolderName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}";
            CurrentRecordingFolder = Path.Combine(RecordingsFolder, destinationFolderName);

            Directory.CreateDirectory(CurrentRecordingFolder);


        }
    }
}

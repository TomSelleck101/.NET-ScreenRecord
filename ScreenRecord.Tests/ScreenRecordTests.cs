using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScreenRecordLibrary;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace ScreenRecordTests
{
    [TestClass]
    public class ScreenRecordTests
    {
        private const string ScreenshotFolder = "Screenshots";
        private const string RecordingFolder = "Recordings";


        [TestInitialize]
        public void InitialiseTest()
        {
            // If the directory exists in the test folder, delete it and any files
            if (Directory.Exists(ScreenshotFolder))
            {
                Directory.Delete(ScreenshotFolder, recursive: true);
            }

            if (Directory.Exists(RecordingFolder))
            {
                Directory.Delete(RecordingFolder, recursive: true);
            }
        }

        [TestMethod]
        public void CaptureScreenshotTest()
        {
            // Define a service to capture a screenshot
            ScreenRecord screenRecord = new ScreenRecord();

            // Capture a screenshot
            screenRecord.CaptureScreenshot();

            // Verify the folder exists
            if (!Directory.Exists(ScreenshotFolder))
            {
                Assert.Fail("Screenshot folder does not exist");
            }

            // Verify the directory contains exactly one file
            string[] screenshotFolderFiles = Directory.GetFiles(ScreenshotFolder);
            if (screenshotFolderFiles.Length != 1)
            {
                Assert.Fail("Screenshot folder does not contain the expected file");
            }

            // Verify the file size
            long imageFileSize = (new FileInfo(screenshotFolderFiles[0])).Length;
            if (imageFileSize <= 0L)
            {
                Assert.Fail("Image contains no data");
            }
        }

        [TestMethod]
        public void VerifyFileTypes()
        {
            // Capture two screenshots for each expected filetype
            ScreenRecord screenRecord = new ScreenRecord();
            screenRecord.CaptureScreenshot(targetImageFormat: "png");
            screenRecord.CaptureScreenshot(targetImageFormat: "jpg");

            // Verify expected files 
            string[] screenshotFolderFiles = Directory.GetFiles(ScreenshotFolder);
            if (screenshotFolderFiles.Length != 2)
            {
                Assert.Fail("Screenshot folder does not contain the two expected files");
            }

            // Make sure both expected file types are present
            if (!screenshotFolderFiles.Any(file => file.Contains("png")) ||
                !screenshotFolderFiles.Any(file => file.Contains("jpg")))
            {
                throw new Exception("Expected file types jpg and png");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VerifyExceptionThrownForIncorrectFileType()
        {
            // Pass an incorrect filetype to the screenshot method
            ScreenRecord screenRecord = new ScreenRecord();
            screenRecord.CaptureScreenshot(targetImageFormat: "unsupported");
        }

        [TestMethod]
        public void CaptureScreenRecordingTest()
        {
            ScreenRecord screenRecord = new ScreenRecord();

            // Begin recording
            screenRecord.StartRecording();

            // Sleep for 10 seconds
            Thread.Sleep(10000);

            // Stop recording
            screenRecord.StopRecording();

            // Check if expected output file is created
            if (File.Exists())
        }
    }
}

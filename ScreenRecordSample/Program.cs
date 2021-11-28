using ScreenRecordLibrary;
using System;

namespace ScreenRecordSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ScreenRecord screenRecord = new ScreenRecord();

            screenRecord.CaptureScreenshot();        
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;

namespace FileMonitoringService
{
    public class clsUtilities
    {

        public static string GenerateGuid()
        {
            Guid NewGuid = Guid.NewGuid();

            return NewGuid.ToString();

        }

        public static string ReplaceFileWithGuid(string SourceFile)
        {
            FileInfo File = new FileInfo(SourceFile);

            return GenerateGuid() + File.Extension;
        }

        public static bool MoveFileToDestination(string SourceFile, string DestinationFolder, out string DestinationFile)
        {

            DestinationFile = DestinationFolder + ReplaceFileWithGuid(SourceFile);

            try
            {
                File.Move(SourceFile, DestinationFile);
                return true;
            }
            catch (IOException iox)
            {
                EventLog.WriteEntry("FileMonitoringService", iox.Message, EventLogEntryType.Error);
            }
            return false;

        }
    }
}

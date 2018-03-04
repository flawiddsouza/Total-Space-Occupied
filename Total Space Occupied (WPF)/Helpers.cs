using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TotalSpaceOccupied
{
    class Helpers
    {
        public static long GetDirectorySize(DirectoryInfo MyDir)
        {
            long Size = 0;
            // Add file sizes.
            FileInfo[] fis = MyDir.GetFiles();
            foreach (FileInfo fi in fis)
            {
                Size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dirs = MyDir.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                Size += GetDirectorySize(dir);
            }
            return (Size);
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatByteSize(long fileSize, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);

        /// <summary>
        /// Converts a numeric value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
        /// </summary>
        /// <param name="FileSize">The numeric value to be converted.</param>
        /// <returns>the converted string</returns>
        public static string StrFormatByteSize(long FileSize)
        {
            StringBuilder sb = new StringBuilder(11);
            StrFormatByteSize(FileSize, sb, sb.Capacity);
            return sb.ToString();
        }

        public static void StringToTextFile(string MyInput, string TextFileName)
        {
            using (var writer = new StreamWriter(TextFileName))
            {
                writer.Write(MyInput);
            }
        }

        public static string TextFileToString(string TextFileName)
        {
            if (File.Exists(TextFileName))
            {
                using (var writer = new StreamReader(TextFileName))
                {
                    return writer.ReadToEnd();
                }
            }
            else
            {
                return null;
            }
        }
    }
}

using System.Runtime.InteropServices;

namespace AusmaProgram.Helpers
{
    public class Environment
    {
        
        private static bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        private static bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        private static bool isOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static string CurrentOS
        {
            get
            {
                if (isWindows)
                {
                    return "win";
                }
                else if (isLinux)
                {
                    return "linux";
                }
                else if (isOSX)
                {
                    return "osx";
                }
                return "unknown";
            }
        }

        public static string OSDescription 
        {
            get
            {
                return RuntimeInformation.OSDescription;
            }
        }

        public static string Architecture
        {
            get
            {
                return nameof(RuntimeInformation.OSArchitecture);
            }
        }

    }
}
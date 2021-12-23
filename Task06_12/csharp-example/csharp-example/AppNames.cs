using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GibrPlan.Test
{
    public class AppNames
    {
        public static string AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        public static string AssemblyVer = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static string GetTestDataPath() => Path.Combine(GetTestPath(), @"TestData");
        public static string GetTestResultsPath() => Path.Combine(GetTestPath(), @"TestResults");
        public static string GetTestVideoPath() => Path.Combine(GetTestPath(), @"Video");
        public static string GetTestLogPath() => Path.Combine(GetTestPath(), @"Log");
        public static string GetTestBasePath() => Path.Combine(GetTestPath(), @"Tests");
        public static string GetTestDebugPath() => Path.Combine(GetTestPath(), $@"\{AssemblyName}\bin\Debug");


        public static string GetTestPath()
        {
            string dir = AssemblyPath; //AssemblyPath.AddDelimeter();

            int pos = dir.LastIndexOf(@"\TestResults\"); //CodeUITest
            if (pos >= 0)
            {
                return dir.Substring(0, pos);
            }

            pos = dir.ToLower().LastIndexOf(@"\bin\debug\"); //UnitTest
            if (pos >= 0)
            {
                DirectoryInfo di = Directory.GetParent(dir.Substring(0, pos));
                return di.FullName;
            }

            pos = dir.ToLower().LastIndexOf($@"\{AssemblyName}\"); //UnitTest alter
            if (pos >= 0)
            {
                return dir.Substring(0, pos);
            }

            pos = dir.ToLower().LastIndexOf(@"\start\"); //ToolTest
            if (pos >= 0)
            {
                return dir.Substring(0, pos);
            }

            pos = dir.LastIndexOf(@"\Tests\"); //vsTest
            if (pos >= 0)
            {
                return dir.Substring(0, pos);
            }

            return dir;
        }

    }
}

using System.Reflection;
using System.Diagnostics;
using System;

public class Meta
{
    public static string getRwsVersion()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
        return fvi.ProductVersion;
    }
    public static bool IsReentryRunning()
    {
        foreach(var process in Process.GetProcesses())
        {
            if (process.ProcessName == "ReEntry")
                return true;
        }
        return false;
    }
}
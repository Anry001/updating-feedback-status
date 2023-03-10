using System;
using WallaShops.Utils;

namespace UpdatingFeedBackStatus.LogToConsole
{
  public class Logger
  {
    public static void LogToConsole(string messageToLog = "no message written to console logger")
    {
      if (WSGeneralUtils.GetAppSettingsBoolean("DebugMode", "true"))
      {
        Console.WriteLine($"{messageToLog}");
      }
    }
  }
}
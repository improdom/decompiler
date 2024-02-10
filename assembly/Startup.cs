// Decompiled with JetBrains decompiler
// Type: JustAssembly.CommandLineTool.Startup
// Assembly: JustAssembly.CommandLineTool, Version=2018.2.806.1, Culture=neutral, PublicKeyToken=2b2cea67609c9510
// MVID: FFCD44F8-93E9-4004-A77E-CAEAE2AFBF45
// Assembly location: C:\temp\JustAssembly\Libraries\JustAssembly.CommandLineTool.exe

using JustAssembly.API.Analytics;
using JustAssembly.Core;
using JustAssembly.Infrastructure.Analytics;
using System;
using System.IO;


namespace JustAssembly.CommandLineTool
{
  public class Startup
  {
    private static IAnalyticsService analytics;

    private static void Main(string[] args)
    {
      Startup.analytics = AnalyticsServiceImporter.Instance.Import();
      Startup.analytics.Start();
      Startup.analytics.TrackFeature("Mode.CommandLine");
      try
      {
        Startup.RunMain(args);
      }
      catch (Exception ex)
      {
        Startup.analytics.TrackException(ex);
        Startup.analytics.Stop();
        throw;
      }
      Startup.analytics.Stop();
    }

    private static void RunMain(string[] args)
    {
      if (args.Length != 3)
        Startup.WriteErrorAndSetErrorCode("Wrong number of arguments." + Environment.NewLine + Environment.NewLine + "Sample:" + Environment.NewLine + "justassembly.commandlinetool Path\\To\\Assembly1 Path\\To\\Assembly2 Path\\To\\XMLOutput");
      else if (!FilePathValidater.ValidateInputFile(args[0]))
        Startup.WriteErrorAndSetErrorCode("First assembly path is in incorrect format or file not found.");
      else if (!FilePathValidater.ValidateInputFile(args[1]))
        Startup.WriteErrorAndSetErrorCode("Second assembly path is in incorrect format or file not found.");
      else if (!FilePathValidater.ValidateOutputFile(args[2]))
      {
        Startup.WriteErrorAndSetErrorCode("Output file path is in incorrect format.");
      }
      else
      {
        string str = string.Empty;
        try
        {
          IDiffItem apiDifferences = (IDiffItem) APIDiffHelper.GetAPIDifferences(args[0], args[1]);
          if (apiDifferences != null)
            str = apiDifferences.ToXml();
        }
        catch (Exception ex)
        {
          Startup.WriteExceptionAndSetErrorCode("A problem occurred while creating the API diff.", ex);
          return;
        }
        try
        {
          using (StreamWriter streamWriter = new StreamWriter(args[2]))
            streamWriter.Write(str);
        }
        catch (Exception ex)
        {
          Startup.WriteExceptionAndSetErrorCode("There was a problem while writing output file.", ex);
          return;
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("API differences calculated successfully.");
        Console.ResetColor();
      }
    }

    private static void WriteErrorAndSetErrorCode(string message)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(message);
      Console.ResetColor();
      Environment.ExitCode = 1;
    }

    private static void WriteExceptionAndSetErrorCode(string message, Exception ex)
    {
      Startup.analytics.TrackException(ex);
      Startup.WriteErrorAndSetErrorCode(string.Format("{0}{1}{2}", (object) message, (object) Environment.NewLine, (object) ex));
    }
  }
}

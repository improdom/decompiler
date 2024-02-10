// Decompiled with JetBrains decompiler
// Type: JustAssembly.CommandLineTool.FilePathValidater
// Assembly: JustAssembly.CommandLineTool, Version=2018.2.806.1, Culture=neutral, PublicKeyToken=2b2cea67609c9510
// MVID: FFCD44F8-93E9-4004-A77E-CAEAE2AFBF45
// Assembly location: C:\temp\JustAssembly\Libraries\JustAssembly.CommandLineTool.exe

using System.Collections.Generic;
using System.IO;


namespace JustAssembly.CommandLineTool
{
  internal static class FilePathValidater
  {
    private static List<string> ValidInputFileExtensions = new List<string>()
    {
      ".dll",
      ".exe"
    };
    private static List<string> ValidOutputFileExtensions = new List<string>()
    {
      ".xml"
    };

    internal static bool ValidateInputFile(string filePath)
    {
      return FilePathValidater.ValidateFile(filePath, FilePathValidater.ValidInputFileExtensions) && File.Exists(filePath);
    }

    internal static bool ValidateOutputFile(string filePath)
    {
      return FilePathValidater.ValidateFile(filePath, FilePathValidater.ValidOutputFileExtensions);
    }

    private static bool ValidateFile(string filePath, List<string> validExtensions)
    {
      if (string.IsNullOrWhiteSpace(filePath))
        return false;
      string extension = Path.GetExtension(filePath);
      return validExtensions.Contains(extension);
    }
  }
}

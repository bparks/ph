using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public static class Program
{
    private static string[] exclude_paths = new [] {
       "/usr/lib/gcc/x86_64-linux-gnu/7/include",
       "/usr/local/include",
       "/usr/lib/gcc/x86_64-linux-gnu/7/include-fixed",
       "/usr/include/x86_64-linux-gnu",
       "/usr/include"
    };

    private static string[] include_paths = new [] {
        "./google_breakpad/common",
        "./common",
        "./"
    };

    private static List<string> already_included = new List<string>();

    public static void Main(string[] args)
    {

         if (args.Length < 1)
         {
             Console.Error.WriteLine("USAGE: this_app your_file");
             return;
         }

         ProcessFile(args[0]);
    }

    public static void ProcessFile(string inputFile)
    {
         string allText = File.ReadAllText(inputFile);
         int oldIdx = 0;

         var matches = new Regex(@"#include\s*(""(.*)\""|<(.*)>)").Matches(allText);
         foreach (Match match in matches)
         {
             bool system = false;
             var fname = string.IsNullOrWhiteSpace(match.Groups[2].Value) ? match.Groups[3].Value : match.Groups[2].Value;

             foreach (string root in exclude_paths)
             {
                 if (File.Exists(Path.Combine(root, fname)))
                 {
                     //Console.Error.WriteLine("Found " + Path.Combine(root, fname));
                     system = true;
                     break;
                 }
             }

             if (system)
                 continue;

             if (!include_paths.Any(root => File.Exists(Path.Combine(root, fname))))
             {
                 if (match.Groups[1].Value.StartsWith("<"))
                 {
                     //Console.Error.WriteLine($"Could not find included file {fname}");
                     //Console.Error.WriteLine("  Marked as system include, so ignoring");
                     continue;
                 }
                 else
                 {
                     Console.Error.WriteLine($"Could not find included file {fname}");
                     break;
                 }
             }

             Console.WriteLine(allText.Substring(oldIdx, match.Index - oldIdx));
             oldIdx = match.Index + match.Length;

             string full_fname = include_paths.Select(root => Path.Combine(root, fname)).First(p => File.Exists(p));

             //Console.Error.WriteLine(full_fname);
             if (!already_included.Contains(full_fname))
             {
                ProcessFile(full_fname);
                already_included.Add(full_fname);
             }
         }

         Console.WriteLine(allText.Substring(oldIdx, allText.Length - oldIdx));
    }
}

// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

FileStream? fileStream = new FileInfo("testcs.txt")?.OpenRead();
if (fileStream is null) return; 
StreamReader reader = new(fileStream);
string? line = string.Empty;
StringBuilder stringBuilder = new();
while((line = reader.ReadLine()) is not null)
{
    if(line.Contains("[JsonProperty(\"\")]"))
    {
        if((line = reader.ReadLine()) is not null)
        {
            Regex regex = new Regex("public [A-z,0-9,_,?,\\[,\\]]+ (?<varName>[A-z,0-9,_]+) .+");
            string? varName = regex.Match(line).Groups["varName"].Value;
            if (!string.IsNullOrEmpty(varName))
            {
                StringBuilder tempSb = new();
                string[] subVarNames = varName.Split("_");
                for(int i = 0; i < subVarNames.Length; i++)
                {
                    tempSb.Append($"{subVarNames[i][0].ToString().ToUpper()}{subVarNames[i].Substring(1)}");
                }
                stringBuilder.Append(
                    $"[JsonProperty(\"{varName}\")]\n{line.Replace(varName, tempSb.ToString())}\n");
            }
        }
    }
    else
    {
        stringBuilder.AppendLine(line);
    }
}
Console.WriteLine("Done!!\n\n");

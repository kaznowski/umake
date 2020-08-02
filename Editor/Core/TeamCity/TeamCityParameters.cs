using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TeamCityParameters
{
	public string BuildNumber => GetValue("build.number");
	public string ProjectName => GetValue("teamcity.projectName");
	public string BuildConfigurationName => GetValue("teamcity.buildConfName");
	public string UMakeTarget => GetValue("umake.target");
	public string UMakeBuildPath => GetValue("umake.buildpath");

	private Dictionary<string, string> dictionary;
    
	public TeamCityParameters()
	{
		var args = Environment.GetCommandLineArgs();
		var filePath = "";   
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == "-executeMethod")
			{
				filePath = args[i + 2];
			}
		}
		
		dictionary = BuildParameterDictionary(filePath);
	}

	public string GetValue(string key, string defaultValue = "")
	{
		if (dictionary.TryGetValue(key, out var ret))
			return ret;
		return defaultValue;
	}
	
	private Dictionary<string, string> BuildParameterDictionary(string filePath)
	{
		var dict = new Dictionary<string, string>();
		var lines = File.ReadAllLines(filePath);

		var sb = new StringBuilder();
        
		foreach (var line in lines)
		{
			sb.AppendLine(line);
			var split = line.Split('=');
			if (split.Length >= 2)
			{
				dict.Add(split[0], split[1]);
			}
		}
		Debug.Log(sb.ToString());
		return dict;
	}
}
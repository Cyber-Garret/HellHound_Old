using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bot.Core
{
	internal static class DataStorage
	{
		private const string res = "resources";
		private static readonly string resDirectory;
		static DataStorage()
		{
			resDirectory = Path.Combine(Directory.GetCurrentDirectory(), res);
		}
		internal static T LoadJSONFromHDD<T>(string fileName)
		{
			var directory = Directory.CreateDirectory(resDirectory);
			var files = directory.GetFiles($"{fileName}.json");
			if (files.Length > 0)
			{

				var file = RestoreObject<T>(files[0].FullName);
				return file;
			}
			return default;
		}

		internal static void SaveObject(object obj, string fileName, bool useIndentations)
		{
			var formatting = (useIndentations) ? Formatting.Indented : Formatting.None;
			var path = Path.Combine(resDirectory, $"{fileName}.json");
			string json = JsonConvert.SerializeObject(obj, formatting);
			File.WriteAllText(path, json, Encoding.UTF8);
		}

		private static T RestoreObject<T>(string filePath)
		{
			var json = GetOrCreateFileContents(filePath);
			return JsonConvert.DeserializeObject<T>(json);
		}

		private static string GetOrCreateFileContents(string filePath)
		{
			if (!File.Exists(filePath))
			{
				File.WriteAllText(filePath, "", Encoding.UTF8);
				return "";
			}
			return File.ReadAllText(filePath, Encoding.UTF8);
		}
	}
}

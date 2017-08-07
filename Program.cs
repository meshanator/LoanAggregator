using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace LoanAggregator
{
	class Program
	{
		public static readonly string OutputFileHeader =
			ConfigurationManager.AppSettings["OutputFileHeader"];

		public static readonly string OutputFileName =
			ConfigurationManager.AppSettings["OutputFileName"];

		public static readonly string InputFileDateFormat =
			ConfigurationManager.AppSettings["InputFileDateFormat"];

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("No input file specified");
				return;
			}

			var filename = args[0];
			if (!File.Exists(args[0]))
			{
				Console.WriteLine($"Input file '{args[0]}' doesn't exist");
				return;
			}

			var output = new Dictionary<string, string>();

			var i = 0;

			//Read the input file line by line
			using (var streamReader = new StreamReader(filename))
			{
				while (!streamReader.EndOfStream)
				{
					i++;
					var line = streamReader.ReadLine();

					//Ignore the first line
					if (i == 1)
						continue;

					var splitLine = line.Replace("'", string.Empty).Split(',');
					var month = DateTime.ParseExact(splitLine[2], InputFileDateFormat, CultureInfo.InvariantCulture).ToString("MMM");

					//Build a key consisting of the Network, Product and amount
					//The value will consist of the Count and Sum.
					var key = $"{splitLine[1]},{splitLine[3]},{month}";
					var value = decimal.Parse(splitLine[4]);

					if (output.ContainsKey(key))
					{
						var current = output[key];
						var currentCount = int.Parse(current.Split(',')[0]);
						var currentAmount = decimal.Parse(current.Split(',')[1]);

						currentCount++;
						currentAmount += value;
						output[key] = $"{currentCount},{currentAmount}";
					}
					else
						output.Add(key, $"{1},{value}");
				}
			}

			//Archive the current file if it already exists
			if (File.Exists(OutputFileName))
				File.Move(OutputFileName, $"Archive_{DateTime.Now.Ticks}_{OutputFileName}");

			//Write the output file line by line
			using (var streamWriter = new StreamWriter(OutputFileName))
			{
				streamWriter.WriteLine(OutputFileHeader);
				foreach (var line in output)
					streamWriter.WriteLine($"{line.Key},{line.Value}");
			}
		}
	}
}
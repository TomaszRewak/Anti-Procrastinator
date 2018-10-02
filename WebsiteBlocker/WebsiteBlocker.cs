using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebsiteBlocker
{
	internal class WebsiteBlocker
	{
		private const string motivator = "Get to work you piece of shit";

		private readonly string hostFilePath;
		private readonly string banFilePath;

		ICollection<string> blockList = new List<string>();

		DateTime lastBanUpdate = new DateTime(1990, 1, 1);
		DateTime lastHostUpdate = new DateTime(1990, 1, 1);

		public WebsiteBlocker(string hostFilePath, string banFilePath)
		{
			this.hostFilePath = hostFilePath;
			this.banFilePath = banFilePath;
		}

		public void UpdateBanList()
		{
			if (File.GetLastWriteTime(banFilePath) > lastBanUpdate)
			{
				var banFile = File.ReadAllLines(banFilePath)
					.Select(line => line.Trim())
					.Where(line => !String.IsNullOrWhiteSpace(line))
					.ToList();

				foreach (var line in banFile)
					if (!blockList.Contains(line))
						blockList.Add(line);

				lastBanUpdate = DateTime.Now;
			}
		}

		public void UpdateHostFile()
		{
			if (File.GetLastWriteTime(hostFilePath) > lastHostUpdate || lastBanUpdate > lastHostUpdate)
			{
				var hostFile = File.ReadAllLines(hostFilePath)
					.Select(line => Regex.Match(line, $@"127\.0\.0\.1\s+(\S+)\s*\# {motivator}"))
					.Where(match => match.Success)
					.Select(match => match.Groups[1].Value)
					.ToList();

				var newLines = blockList
					.Where(line => !hostFile.Contains(line))
					.Select(line => $"127.0.0.1 {line} # {motivator}")
					.ToList();

				File.AppendAllLines(hostFilePath, newLines);

				lastHostUpdate = DateTime.Now;
			}
		}
	}
}

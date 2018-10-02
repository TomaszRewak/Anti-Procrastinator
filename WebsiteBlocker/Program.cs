using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebsiteBlocker
{
	class Program
	{
		const string hostFilePath = @"C:\Windows\System32\drivers\etc\hosts";
		const string banFilePath = @".\block-list";

		static void Main(string[] args)
		{
			var websiteBlocker = new WebsiteBlocker(hostFilePath, banFilePath);

			while(true)
			{
				websiteBlocker.UpdateBanList();
				websiteBlocker.UpdateHostFile();

				Thread.Sleep(5000);
			}
		}
	}
}

using System.Reflection;
using System.IO;
using System.Text;
using System;
using System.Threading;

namespace Sleep
{
	public class Sleep
	{
		public static int Main(string[] args)
		{
			if (args.Length != 1)
			{
				Console.WriteLine(ReadFromResource("src/Help.txt"));
				return 0;
			}

			if (args[0] == "moo")
			{
				Console.WriteLine(ReadFromResource("src/moo.txt"));
				return 0;
			}

			string[] interval = args[0].Split(new string[]{"."}, StringSplitOptions.RemoveEmptyEntries);
			if (!(1 <= interval.Length && interval.Length <= 3))
			{
				Console.WriteLine(ReadFromResource("Help.txt"));
				return 0;
			}

			int hour;
			int minute;
			int second;

			try
			{
				if (interval.Length == 1)
				{
					hour = 0;
					minute = 0;
					second = int.Parse(interval[0]);
				}
				else if(interval.Length == 2)
				{
					hour = 0;
					minute = int.Parse(interval[0]);
					second = int.Parse(interval[1]);
				}
				else
				{
					hour = int.Parse(interval[0]);
					minute = int.Parse(interval[1]);
					second = int.Parse(interval[2]);
				}
			}
			catch (Exception)
			{
				Console.WriteLine(@"format error");
				return 0;
			}
			
			var endTime = DateTime.Now.AddHours(hour).AddMinutes(minute).AddSeconds(second);

			while (endTime > DateTime.Now)
			{
				var a = endTime - DateTime.Now;
				Console.CursorLeft = 0;
				if (a.Hours == 0 && a.Minutes == 0)
				{
					Console.ForegroundColor = ConsoleColor.Red;
				}
				Console.Write("{0:D2}:{1:D2}:{2:D2}", a.Hours, a.Minutes, a.Seconds);
				Console.ForegroundColor = ConsoleColor.Gray;
				Thread.Sleep(1000);
			}

			return 0;
		}

		private static string ReadFromResource(string resourceName)
		{
			var myAssembly = Assembly.GetExecutingAssembly();
			using (var r = new StreamReader(
					myAssembly.GetManifestResourceStream(resourceName),
        			System.Text.Encoding.GetEncoding("UTF-8")))
			{
				return r.ReadToEnd();
			}
		}
	}
}
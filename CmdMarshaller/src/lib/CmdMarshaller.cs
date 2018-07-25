using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Jyuch.CuiSupport
{
	public class CmdMarshaller
	{
		public string Prompt {set; get;}
		public string ExitCmd {set; get;}
		public string ErrMsg {set; get;}

		private Dictionary<string, Action> noArgHandler;
		private Dictionary<string, Action<string>> oneArgHandler;
		private Dictionary<string, Action<string, string>> twoArgHandler;
		private Dictionary<string, Action<string, string, string>> threeArgHandler;

		public CmdMarshaller()
		{
			Prompt = "> ";
			ExitCmd = "exit";
			ErrMsg = "No such command";
			noArgHandler = new Dictionary<string, Action>();
			oneArgHandler = new Dictionary<string, Action<string>>();
			twoArgHandler = new Dictionary<string, Action<string, string>>();
			threeArgHandler = new Dictionary<string, Action<string, string, string>>();
		}

		public void Start()
		{
			while (true)
			{
				Console.Write(Prompt);
				var cmd = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(cmd))
				{
					continue;
				}

				if (cmd == ExitCmd)
				{
					break;
				}

				var cmds = Regex.Split(cmd, "\\s+");

				if (cmds.Length == 1)
				{
					Action h;
					if (noArgHandler.TryGetValue(cmds[0], out h))
					{
						h.Invoke();
					}
					else
					{
						Console.WriteLine(ErrMsg);
					}
				}
				else if (cmds.Length == 2)
				{
					Action<string> h;
					if (oneArgHandler.TryGetValue(cmds[0], out h))
					{
						h.Invoke(cmds[1]);
					}
					else
					{
						Console.WriteLine(ErrMsg);
					}
				}
				else if (cmds.Length == 3)
				{
					Action<string, string> h;
					if (twoArgHandler.TryGetValue(cmds[0], out h))
					{
						h.Invoke(cmds[1], cmds[2]);
					}
					else
					{
						Console.WriteLine(ErrMsg);
					}
				}
				else if (cmds.Length == 4)
				{
					Action<string, string, string> h;
					if (threeArgHandler.TryGetValue(cmds[0], out h))
					{
						h.Invoke(cmds[1], cmds[2], cmds[3]);
					}
					else
					{
						Console.WriteLine(ErrMsg);
					}
				}
				else
				{
					Console.WriteLine(ErrMsg);
				}
			}
		}

		public CmdMarshaller AddCmdHandler(string cmd, Action handler)
		{
			noArgHandler[cmd] = handler;
			return this;
		}

		public CmdMarshaller AddCmdHandler(string cmd, Action<string> handler)
		{
			oneArgHandler[cmd] = handler;
			return this;
		}

		public CmdMarshaller AddCmdHandler(string cmd, Action<string, string> handler)
		{
			twoArgHandler[cmd] = handler;
			return this;
		}

		public CmdMarshaller AddCmdHandler(string cmd, Action<string, string, string> handler)
		{
			threeArgHandler[cmd] = handler;
			return this;
		}
	}
}
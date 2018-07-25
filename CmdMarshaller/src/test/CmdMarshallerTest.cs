using System;
using Jyuch.CuiSupport;

namespace Jyuch.CuiSupport.Test
{
	public class CmdMarshallerTest
	{
		public static void Main(string[] args)
		{
			var ms = new CmdMarshaller();
			var vu = new VeryUsefulClass("hello");

			ms
			.AddCmdHandler("cmda", vu.CmdA)
			.AddCmdHandler("cmdb", vu.CmdB)
			.AddCmdHandler("cmdc", vu.CmdC)
			.AddCmdHandler("cmdd", vu.CmdD);

			ms.Prompt = ">>> ";
			ms.ExitCmd = "bye";
			ms.ErrMsg = "No such command, sorry";

			ms.Start();
		}
	}

	public class VeryUsefulClass
	{
		string msg;

		public VeryUsefulClass(string arg)
		{
			msg = arg;
		}

		public void CmdA()
		{
			Console.WriteLine("CmdA {0}", msg);
		}

		public void CmdB(string arg)
		{
			Console.WriteLine("CmdB {0}", arg);
		}

		public void CmdC(string arg1, string arg2)
		{
			Console.WriteLine("CmdC {0} {1}", arg1, arg2);
		}

		public void CmdD(string arg1, string arg2, string arg3)
		{
			Console.WriteLine("CmdD {0} {1} {2}", arg1, arg2, arg3);
		}
	}
}
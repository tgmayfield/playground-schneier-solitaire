using System;
using System.Diagnostics;

namespace SolitaireCypher.Ui
{
	internal class Program
	{
		private static void Main()
		{
			var stream = new KeyStream(Card.UnkeyedDeck);

			Func<string, string> process;

			Console.Write("[E]ncrypt or [D]ecrypt? [E/D]: ");
			var key = Console.ReadKey();
			switch (key.KeyChar)
			{
				case 'e':
				case 'E':
					process = stream.Encrypt;
					break;
				case 'd':
				case 'D':
					process = stream.Decrypt;
					break;

				default:
					return;
			}
			Console.WriteLine();
			Console.WriteLine();

			Console.WriteLine("Text: ");
			string input = Console.ReadLine();
			if (string.IsNullOrEmpty(input))
			{
				return;
			}

			Console.WriteLine();
			Console.WriteLine("Output: ");
			string result = process(input);
			for (int index = 0; index < result.Length; index++)
			{
				Console.Write(result[index]);
				if (index > 0 && ((index + 1) % 5 == 0))
				{
					Console.Write(' ');
				}
			}
			Console.WriteLine();
			Console.WriteLine();

			if (Debugger.IsAttached)
			{
				Console.Write("- Done. Press any key to exit. - ");
				Console.ReadKey();
			}
		}
	}
}
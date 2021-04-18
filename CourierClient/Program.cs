using Newtonsoft.Json;
using System;
using System.IO;

namespace CourierClient
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Enter command:");
			var command = Console.ReadLine();
			var action = GetActionByCommand(command);
			action.Invoke();
			Console.ReadKey();
		}

		private static Action GetActionByCommand(string command)
		{
			var text = File.ReadAllText("Setting.json");
			var setting = JsonConvert.DeserializeObject<Setting>(text);
			var handler = new CouriersHandler(setting.CourierServiceHost);

			return command switch
			{
				"register" => handler.Register,
				"login" => handler.Login,
				_ => throw new ArgumentException($"invalid command - {command}")
			};
		}
	}
}

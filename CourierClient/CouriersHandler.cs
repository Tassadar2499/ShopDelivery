using CourierService;
using Grpc.Net.Client;
using Isopoh.Cryptography.Argon2;
using System;

namespace CourierClient
{
	public class CouriersHandler
	{
		private readonly string _serviceHost;

		public CouriersHandler(string serviceHost) => _serviceHost = serviceHost;

		public void Register()
		{
			Console.WriteLine("Enter login:");
			var login = Console.ReadLine();
			var password = InputManager.ReadPassword();
			var passwordHash = Argon2.Hash(password.ToString());

			using var channel = GrpcChannel.ForAddress(_serviceHost);
			var client = new CouriersAuth.CouriersAuthClient(channel);

			var random = new Random();

			var request = new RegisterCourierRequest()
			{
				Login = login,
				PasswordHash = passwordHash,
				Longitude = random.Next(1000) + random.NextDouble(),
				Latitude = random.Next(1000) + random.NextDouble()
			};

			client.Register(request);
			Console.WriteLine("User registered");
		}

		public void Login()
		{
			Console.WriteLine("Enter login:");
			var login = Console.ReadLine();
			var password = InputManager.ReadPassword();

			using var channel = GrpcChannel.ForAddress(_serviceHost);
			var client = new CouriersAuth.CouriersAuthClient(channel);
			var loginRequest = new LoginCourierRequest()
			{
				Login = login
			};

			var reply = client.Login(loginRequest);
			if (!reply.IsContains)
			{
				Console.WriteLine($"Cannot find user by login - {login}");
				return;
			}

			var passwordHash = reply.PasswordHash;

			var isCorrect = Argon2.Verify(passwordHash, password.ToString());
			if (!isCorrect)
				Console.WriteLine("Login or password is incorrect");
			else
				Update(login);

			Console.WriteLine("Login finished");
		}

		private void Update(string login)
		{
			using var channel = GrpcChannel.ForAddress(_serviceHost);
			var client = new CouriersAuth.CouriersAuthClient(channel);
			var random = new Random();
			var updateRequest = new UpdateCourierRequest()
			{
				Login = login,
				Longitude = random.Next(1000) + random.NextDouble(),
				Latitude = random.Next(1000) + random.NextDouble()
			};

			client.Update(updateRequest);
		}
	}
}
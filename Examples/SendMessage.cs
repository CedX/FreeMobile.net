using Belin.FreeMobile;
using System.Net.Http;

// Sends an SMS notification.
try {
	var client = new Client("your account identifier", "your API key");
	await client.SendMessageAsync("Hello World from .NET!");
	Console.WriteLine("The message was sent successfully.");
}
catch (HttpRequestException e) {
	Console.Error.WriteLine($"An error occurred: {e.Message}");
}

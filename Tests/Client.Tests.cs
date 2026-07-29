namespace Belin.FreeMobile;

/// <summary>
/// Tests the features of the <see cref="Client"/> class.
/// </summary>
/// <param name="testContext">The test context.</param>
[TestClass]
public sealed class ClientTests(TestContext testContext) {

	[TestMethod]
	public async Task NetworkError() {
		// It should throw a `HttpRequestException` if a network error occurred.
		var client = new Client("anonymous", "secret") { BaseUrl = new Uri("http://localhost:666") };
		await ThrowsAsync<HttpRequestException>(() => client.SendMessageAsync("Hello World!", testContext.CancellationToken));
	}

	[TestMethod]
	public async Task InvalidCredentials() {
		// It should throw a `HttpRequestException` if the credentials are invalid.
		var client = new Client("anonymous", "secret");
		await ThrowsAsync<HttpRequestException>(() => client.SendMessageAsync("Hello World!", testContext.CancellationToken));
	}

	[TestMethod]
	public async Task ValidCredentials() {
		// It should send SMS messages if the credentials are valid.
		var client = new Client(Environment.GetEnvironmentVariable("FREEMOBILE_ACCOUNT")!, Environment.GetEnvironmentVariable("FREEMOBILE_API_KEY")!);
		await client.SendMessageAsync("Hello Cédric, from .NET!", testContext.CancellationToken);
	}
}

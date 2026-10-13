namespace Belin.FreeMobile;

/// <summary>
/// Tests the features of the <see cref="Client"/> class.
/// </summary>
/// <param name="testContext">The test context.</param>
[TestClass]
public class ClientTests(TestContext testContext) {

	[TestMethod]
	public void NetworkError() {
		// It should throw a `HttpRequestException` if a network error occurred.
		using var client = new Client("anonymous", "secret") { BaseUrl = new Uri("http://localhost:666") };
		Should.Throw<HttpRequestException>(() => client.SendMessage("Hello World!", testContext.CancellationToken));
	}

	[TestMethod]
	public void InvalidCredentials() {
		// It should throw a `HttpRequestException` if the credentials are invalid.
		using var client = new Client("anonymous", "secret");
		Should.Throw<HttpRequestException>(() => client.SendMessage("Hello World!", testContext.CancellationToken));
	}

	[TestMethod]
	public void ValidCredentials() {
		// It should send SMS messages if the credentials are valid.
		using var client = new Client(Environment.GetEnvironmentVariable("FREEMOBILE_ACCOUNT")!, Environment.GetEnvironmentVariable("FREEMOBILE_API_KEY")!);
		client.SendMessage("Hello Cédric, from .NET!", testContext.CancellationToken);
	}
}

namespace Belin.FreeMobile;

using System.Net;
using System.Web;

/// <summary>
/// Sends messages by SMS to a <see href="https://mobile.free.fr">FreeMobile</see> account.
/// </summary>
/// <param name="credential">The Free Mobile user name and password.</param>
public class Client(NetworkCredential credential) {

	/// <summary>
	/// The assembly version.
	/// </summary>
	private static Version Version => typeof(Client).Assembly.GetName().Version!;

	/// <summary>
	/// The base URL of the remote API endpoint.
	/// </summary>
	public Uri BaseUrl { get; set; } = new Uri("https://smsapi.free-mobile.fr/");

	/// <summary>
	/// The Free Mobile user name and password.
	/// </summary>
	public NetworkCredential Credential => credential;

	/// <summary>
	/// The user agent string to use when making requests.
	/// </summary>
	public string UserAgent { get; set; } = $".NET/{Environment.Version} | Belin.FreeMobile/{Version.ToString(3)}";

	/// <summary>
	/// Creates a new client.
	/// </summary>
	/// <param name="userName">The Free Mobile user name.</param>
	/// <param name="password">The Free Mobile password.</param>
	public Client(string userName, string password): this(new NetworkCredential(userName, password)) {}

	/// <summary>
	/// Sends an SMS message to the underlying account.
	/// </summary>
	/// <param name="text">The message text.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>Completes when the message has been sent.</returns>
	/// <exception cref="HttpRequestException">The HTTP response is unsuccessful.</exception>
	public void SendMessage(string text, CancellationToken cancellationToken = default) =>
		SendMessageAsync(text, cancellationToken).GetAwaiter().GetResult();

	/// <summary>
	/// Sends an SMS message to the underlying account.
	/// </summary>
	/// <param name="text">The message text.</param>
	/// <param name="cancellationToken">The token to cancel the operation.</param>
	/// <returns>Completes when the message has been sent.</returns>
	public async Task SendMessageAsync(string text, CancellationToken cancellationToken = default) {
		var trimmedText = text.Trim();
		var queryString = HttpUtility.ParseQueryString("");
		queryString.Add("msg", trimmedText.Length > 160 ? trimmedText[0..160] : trimmedText);
		queryString.Add("pass", credential.Password);
		queryString.Add("user", credential.UserName);

		using var client = new HttpClient { BaseAddress = BaseUrl, Timeout = TimeSpan.FromMinutes(1) };
		client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

		using var response = await client.GetAsync($"sendmsg?{queryString}", cancellationToken);
		response.EnsureSuccessStatusCode();
	}
}

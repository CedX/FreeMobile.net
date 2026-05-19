namespace Belin.FreeMobile;

using System.Diagnostics.CodeAnalysis;
using System.Net;

/// <summary>
/// Sends messages by SMS to a <see href="https://mobile.free.fr">FreeMobile</see> account.
/// </summary>
/// <param name="credential">The Free Mobile user name and password.</param>
/// <param name="baseUrl">The base URL of the remote API endpoint.</param>
public class Client(NetworkCredential credential, Uri? baseUrl = null) {

	/// <summary>
	/// The assembly version.
	/// </summary>
	private static Version Version => typeof(Client).Assembly.GetName().Version!;

	/// <summary>
	/// The base URL of the remote API endpoint.
	/// </summary>
	public Uri BaseUrl { get; set; } = baseUrl ?? new Uri("https://smsapi.free-mobile.fr/");

	/// <summary>
	/// The Free Mobile user name and password.
	/// </summary>
	public NetworkCredential Credential => credential;

	/// <summary>
	/// Creates a new client.
	/// </summary>
	/// <param name="credential">The Free Mobile user name and password.</param>
	/// <param name="baseUrl">The base URL of the remote API endpoint.</param>
	public Client(NetworkCredential credential, [StringSyntax(StringSyntaxAttribute.Uri)] string baseUrl):
		this(credential, new Uri(baseUrl, UriKind.Absolute)) { }

	/// <summary>
	/// Creates a new client.
	/// </summary>
	/// <param name="userName">The Free Mobile user name.</param>
	/// <param name="password">The Free Mobile password.</param>
	/// <param name="baseUrl">The base URL of the remote API endpoint.</param>
	public Client(string userName, string password, Uri? baseUrl = null):
		this(new NetworkCredential(userName, password), baseUrl) {}

	/// <summary>
	/// Creates a new client.
	/// </summary>
	/// <param name="userName">The Free Mobile user name.</param>
	/// <param name="password">The Free Mobile password.</param>
	/// <param name="baseUrl">The base URL of the remote API endpoint.</param>
	public Client(string userName, string password, [StringSyntax(StringSyntaxAttribute.Uri)] string baseUrl):
		this(new NetworkCredential(userName, password), new Uri(baseUrl, UriKind.Absolute)) {}

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
		using var query = new FormUrlEncodedContent(new Dictionary<string, string> {
			["msg"] = trimmedText.Length > 160 ? trimmedText[0..160] : trimmedText,
			["pass"] = credential.Password,
			["user"] = credential.UserName
		});

		using var httpClient = new HttpClient();
		httpClient.DefaultRequestHeaders.Add("User-Agent", $".NET/{Environment.Version} | Belin.FreeMobile/{Version.ToString(3)}");

		var url = new Uri(BaseUrl, $"sendmsg?{await query.ReadAsStringAsync(cancellationToken)}");
		using var response = await httpClient.GetAsync(url, cancellationToken);
		response.EnsureSuccessStatusCode();
	}
}

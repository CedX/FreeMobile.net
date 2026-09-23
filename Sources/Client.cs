namespace Belin.FreeMobile;

using System.Net;
using System.Web;

/// <summary>
/// Sends messages by SMS to a <see href="https://mobile.free.fr">FreeMobile</see> account.
/// </summary>
/// <param name="credential">The Free Mobile user name and password.</param>
public class Client(NetworkCredential credential): IDisposable {

	/// <summary>
	/// The assembly version.
	/// </summary>
	private static readonly Version Version = typeof(Client).Assembly.GetName().Version!;

	/// <summary>
	/// The base URL of the remote API endpoint.
	/// </summary>
	public Uri BaseUrl { get; set; } = new("https://smsapi.free-mobile.fr/");

	/// <summary>
	/// The Free Mobile user name and password.
	/// </summary>
	public NetworkCredential Credential { get; set; } = credential;

	/// <summary>
	/// The user agent string to use when making requests.
	/// </summary>
	public string UserAgent { get; set; } = $".NET/{Environment.Version} | Belin.FreeMobile/{Version.ToString(3)}";

	/// <summary>
	/// Value indicating whether this object has been disposed.
	/// </summary>
	private bool disposed;

	/// <summary>
	/// The underlying HTTP client.
	/// </summary>
	private readonly HttpClient httpClient = new() { Timeout = TimeSpan.FromMinutes(1) };

	/// <summary>
	/// Creates a new client.
	/// </summary>
	/// <param name="userName">The Free Mobile user name.</param>
	/// <param name="password">The Free Mobile password.</param>
	public Client(string userName, string password): this(new NetworkCredential(userName, password)) {}

	/// <summary>
	/// Releases any resources associated with this object.
	/// </summary>
	public void Dispose() {
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

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
		queryString.Add("pass", Credential.Password);
		queryString.Add("user", Credential.UserName);

		using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(BaseUrl, $"sendmsg?{queryString}"));
		request.Headers.Add("User-Agent", UserAgent);

		using var response = await httpClient.SendAsync(request, cancellationToken);
		response.EnsureSuccessStatusCode();
	}

	/// <summary>
	/// Releases any resources associated with this object.
	/// </summary>
	/// <param name="disposing">Value indicating whether this object is currently being disposed.</param>
	protected virtual void Dispose(bool disposing) {
		if (disposed) return;
		if (disposing) httpClient.Dispose();
		disposed = true;
	}
}

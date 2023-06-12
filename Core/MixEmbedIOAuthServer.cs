using System.Reflection;
using System.Text;
using EmbedIO;
using EmbedIO.Actions;
using SpotifyAPI.Web.Auth;
using Swan.Logging;

namespace Core;

public class MixEmbedIOAuthServer : IAuthServer
{
     public event Func<object, AuthorizationCodeResponse, Task>? AuthorizationCodeReceived;
    public event Func<object, ImplictGrantResponse, Task>? ImplictGrantReceived;
    public event Func<object, string, string?, Task>? ErrorReceived;

    private const string AssetsResourcePath = "Core.Resources.auth_assets";
    private const string DefaultResourcePath = "Core.Resources.default_site";

    private CancellationTokenSource? _cancelTokenSource;
    private readonly WebServer _webServer;

    public MixEmbedIOAuthServer(Uri baseUri, int port)
      : this(baseUri, port, Assembly.GetExecutingAssembly(), DefaultResourcePath) { }

    public MixEmbedIOAuthServer(Uri baseUri, int port, Assembly resourceAssembly, string resourcePath)
    {
      Guards.IsNotNull(baseUri);

      BaseUri = baseUri;
      Port = port;

      Logger.UnregisterLogger<ConsoleLogger>();

      _webServer = new WebServer(port)
        .WithModule(new ActionModule("/", HttpVerbs.Post, (ctx) =>
        {
          var query = ctx.Request.QueryString;
          var error = query["error"];
          if (error != null)
          {
            ErrorReceived?.Invoke(this, error, query["state"]);
            throw new AuthException(error, query["state"]);
          }

          var requestType = query.Get("request_type");
          if (requestType == "token")
          {
            ImplictGrantReceived?.Invoke(this, new ImplictGrantResponse(
              query["access_token"]!, query["token_type"]!, int.Parse(query["expires_in"]!)
            )
            {
              State = query["state"]
            });
          }
          if (requestType == "code")
          {
            AuthorizationCodeReceived?.Invoke(this, new AuthorizationCodeResponse(query["code"]!)
            {
              State = query["state"]
            });
          }

          return ctx.SendStringAsync("OK", "text/plain", Encoding.UTF8);
        }))
        .WithEmbeddedResources("/auth_assets", Assembly.GetExecutingAssembly(), AssetsResourcePath)
        .WithEmbeddedResources(baseUri.AbsolutePath, resourceAssembly, resourcePath);
    }

    public Uri BaseUri { get; }
    public int Port { get; }

    public Task Start()
    {
      _cancelTokenSource = new CancellationTokenSource();
      _webServer.Start(_cancelTokenSource.Token);
      return Task.CompletedTask;
    }

    public Task Stop()
    {
      _cancelTokenSource?.Cancel();
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        _webServer?.Dispose();
      }
    }
}
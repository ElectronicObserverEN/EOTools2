using EOToolsWeb.Shared.Users;

namespace EOToolsWeb.Services;

public class CurrentSession : ICurrentSession
{
    public UserKind UserKind { get; set; }
}

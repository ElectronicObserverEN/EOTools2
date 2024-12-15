using EOToolsWeb.Shared.Users;

namespace EOToolsWeb.Services;

public interface ICurrentSession
{
    public UserKind UserKind { get; set; }
}

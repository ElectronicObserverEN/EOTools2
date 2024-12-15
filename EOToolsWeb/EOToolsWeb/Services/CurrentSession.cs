using EOToolsWeb.Shared.Users;

namespace EOToolsWeb.Services;

public class CurrentSession : ICurrentSession
{
    public UserModel? User { get; set; }
}

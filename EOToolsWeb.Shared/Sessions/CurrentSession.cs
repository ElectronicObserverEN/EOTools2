using EOToolsWeb.Shared.Users;

namespace EOToolsWeb.Shared.Sessions;

public class CurrentSession : ICurrentSession
{
    public UserModel? User { get; set; }
}

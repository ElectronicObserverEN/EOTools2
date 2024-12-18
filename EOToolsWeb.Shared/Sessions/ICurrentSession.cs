using EOToolsWeb.Shared.Users;

namespace EOToolsWeb.Shared.Sessions;

public interface ICurrentSession
{
    public UserModel? User { get; set; }
}

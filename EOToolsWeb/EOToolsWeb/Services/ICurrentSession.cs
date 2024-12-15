using EOToolsWeb.Shared.Users;

namespace EOToolsWeb.Services;

public interface ICurrentSession
{
    public UserModel? User { get; set; }
}

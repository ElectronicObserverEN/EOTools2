using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Shared.Users;

namespace EOToolsWeb.ViewModels.Users;

public partial class UserViewModel : ObservableObject
{
    [ObservableProperty] 
    public partial string Username { get; set; } = "";

    [ObservableProperty]
    public partial string Password { get; set; } = "";

    [ObservableProperty]
    public partial UserKind UserKind { get; set; }

    public List<UserKind> UserKinds => Enum.GetValues<UserKind>().ToList();

    public bool CanEditKind { get; set; } = false;

    public UserModel Model { get; set; } = new();

    public void LoadFromModel()
    {
        Username = Model.Username;
        Password = Model.Password;
        UserKind = Model.Kind;
    }

    public void SaveChanges()
    {
        Model.Username = Username;
        Model.Password = Password;
        Model.Kind = UserKind;
    }
}

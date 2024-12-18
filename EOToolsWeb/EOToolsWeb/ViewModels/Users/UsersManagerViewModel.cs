using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Users;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EOToolsWeb.Services;
using EOToolsWeb.Shared.Sessions;
using EOToolsWeb.Views.Users;

namespace EOToolsWeb.ViewModels.Users;

public partial class UsersManagerViewModel(HttpClient client, IAvaloniaShowDialogService dialogService, ICurrentSession session) : ViewModelBase
{
    public ObservableCollection<UserModel> UserList { get; set; } = new();
    
    private HttpClient HttpClient { get; } = client;
    private ICurrentSession CurrentSession { get; } = session;
    private IAvaloniaShowDialogService DialogService { get; } = dialogService;

    public async Task LoadAllUsers()
    {
        if (UserList.Count > 0) return;

        UserList = new(await HttpClient.GetFromJsonAsync<List<UserModel>>("Users") ?? []);
    }
    
    [RelayCommand]
    private async Task AddUser()
    {
        UserModel model = new();
        UserViewModel vm = new();
        vm.CanEditKind = true;
        vm.Model = model;
        vm.LoadFromModel();

        UserEditView dialog = new();
        dialog.DataContext = vm;

        if (await DialogService.ShowWindow(dialog))
        {
            vm.SaveChanges();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("Users", model);

            response.EnsureSuccessStatusCode();

            UserModel? postedModel = await response.Content.ReadFromJsonAsync<UserModel>();

            if (postedModel is not null)
            {
                UserList.Add(postedModel);
            }
        }
    }

    [RelayCommand]
    public async Task EditUser(UserModel vm)
    {
        UserViewModel vmEdit = new();
        vmEdit.CanEditKind = true;
        vmEdit.Model = vm;
        vmEdit.LoadFromModel();

        UserEditView dialog = new();
        dialog.DataContext = vmEdit;

        if (await DialogService.ShowWindow(dialog))
        {
            vmEdit.SaveChanges();

            await HttpClient.PutAsJsonAsync("Users", vm);
        }
    }

    public async Task EditCurrentUser()
    {
        UserViewModel vmEdit = new();
        vmEdit.CanEditKind = false;
        vmEdit.Model = session.User ?? new();
        vmEdit.LoadFromModel();

        UserEditView dialog = new();
        dialog.DataContext = vmEdit;

        if (await DialogService.ShowWindow(dialog))
        {
            vmEdit.SaveChanges();

            await HttpClient.PutAsJsonAsync("Users/currentUser", session.User);
        }
    }

    [RelayCommand]
    public async Task RemoveUser(UserModel vm)
    {
        await HttpClient.DeleteAsync($"Users/{vm.Id}");

        UserList.Remove(vm);
    }
}

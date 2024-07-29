using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Equipments;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.Equipments;

public partial class EquipmentManagerViewModel : ViewModelBase
{
    public List<EquipmentModel> AllEquipments { get; private set; } = [];

    public ObservableCollection<EquipmentModel> EquipmentList { get; set; } = new();
    
    [ObservableProperty]
    private string _filter = "";

    private HttpClient HttpClient { get; }
    private EquipmentViewModel EquipmentViewModel { get; }

    public Interaction<EquipmentViewModel, bool> ShowEditDialog { get; } = new();
    public Interaction<object?, EquipmentModel?> ShowPicker { get; } = new();

    public EquipmentManagerViewModel(HttpClient client, EquipmentViewModel viewModel)
    {
        HttpClient = client;
        EquipmentViewModel = viewModel;

        PropertyChanged += EquipmentManagerViewModel_PropertyChanged;
    }

    private async void EquipmentManagerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(Filter)) return;

        ReloadEquipmentList();
    }

    public async Task LoadAllEquipments()
    {
        if (AllEquipments.Count > 0) return;

        AllEquipments = await HttpClient.GetFromJsonAsync<List<EquipmentModel>>("Equipments") ?? [];

        ReloadEquipmentList();
    }

    private void ReloadEquipmentList()
    {
        string upperCaseFilter = Filter.ToUpperInvariant();

        EquipmentList = new(AllEquipments.Where(eq => string.IsNullOrEmpty(upperCaseFilter) || eq.NameEN.ToUpperInvariant().Contains(upperCaseFilter) || eq.NameJP.ToUpperInvariant().Contains(upperCaseFilter)));
        OnPropertyChanged(nameof(EquipmentList));
    }

    [RelayCommand]
    private async Task AddNewEquipment()
    {
        EquipmentModel model = new();

        EquipmentViewModel.Model = model;
        await EquipmentViewModel.LoadFromModel();

        if (await ShowEditDialog.Handle(EquipmentViewModel))
        {
            EquipmentViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("Equipments", model);

            response.EnsureSuccessStatusCode();

            EquipmentModel? postedModel = await response.Content.ReadFromJsonAsync<EquipmentModel>();

            if (postedModel is not null)
            {
                EquipmentList.Add(postedModel);

                ReloadEquipmentList();
            }
        }
    }
    
    [RelayCommand]
    private async Task EditEquipment(EquipmentModel vm)
    {
        EquipmentViewModel.Model = vm;
        await EquipmentViewModel.LoadFromModel();

        if (await ShowEditDialog.Handle(EquipmentViewModel))
        {
            EquipmentViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("Equipments", vm);

            response.EnsureSuccessStatusCode();

            ReloadEquipmentList();
        }
    }

    [RelayCommand]
    private async Task RemoveEquipment(EquipmentModel vm)
    {
        await HttpClient.DeleteAsync($"Equipments/{vm.Id}");

        EquipmentList.Remove(vm);

        ReloadEquipmentList();
    }

    [RelayCommand]
    private async Task UpdateTranslations()
    {
        await HttpClient.PutAsync("Equipments/pushEquipments", null);
    }

    [RelayCommand]
    private async Task UpdateUpgrades()
    {
        await HttpClient.PutAsync("EquipmentUpgrades/pushEquipments", null);
    }

    [RelayCommand]
    private void OpenEquipmentUpgradeChecker()
    {

    }

    [RelayCommand]
    private void UpdateFitBonus()
    {

    }
}

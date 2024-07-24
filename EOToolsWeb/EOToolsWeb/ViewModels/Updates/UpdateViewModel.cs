﻿using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Shared.Updates;
using System;

namespace EOToolsWeb.ViewModels.Updates;

public partial class UpdateViewModel : ObservableObject
{
    [ObservableProperty]
    private DateTimeOffset? _updateDate = DateTime.Now;

    [ObservableProperty] 
    private DateTimeOffset? _updateEndDate;

    [ObservableProperty]
    private TimeSpan? _updateStartTime = TimeSpan.Zero;

    [ObservableProperty]
    private TimeSpan? _updateEndTime;

    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private string _description = "";

    [ObservableProperty]
    private bool _wasLiveUpdate = false;

    [ObservableProperty]
    private string _startTweet = "";

    [ObservableProperty]
    private string _endTweet = "";

    public UpdateModel Model { get; set; } = new();

    public void LoadFromModel()
    {
        UpdateDate = Model.UpdateDate;
        Name = Model.Name;
        Description = Model.Description;
        WasLiveUpdate = Model.WasLiveUpdate;
        UpdateStartTime = Model.UpdateStartTime;
        UpdateEndTime = Model.UpdateEndTime;
        EndTweet = Model.EndTweetLink;
        StartTweet = Model.StartTweetLink;

        if (UpdateEndTime is { } endTime && UpdateDate is { } start)
        {
            UpdateEndDate = start.AddDays(endTime.Days);
        }
    }

    public void SaveChanges()
    {
        Model.UpdateDate = UpdateDate;
        Model.Name = Name;
        Model.Description = Description;
        Model.WasLiveUpdate = WasLiveUpdate;
        Model.UpdateStartTime = UpdateStartTime;
        Model.EndTweetLink = EndTweet;
        Model.StartTweetLink = StartTweet;

        if (UpdateEndDate is {} endDate && UpdateEndTime is {} endTime && UpdateDate is {} start)
        {
            Model.UpdateEndTime = endTime.Add(new TimeSpan((endDate - start).Days, 0, 0, 0));
        }
    }
}

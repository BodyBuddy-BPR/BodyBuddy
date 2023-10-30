using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BodyBuddy.Dtos
{
    public partial class StartupTestDto : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty] private string _name;

        [ObservableProperty] private string _gender;

        [ObservableProperty] private double _weight;

        [ObservableProperty] private int _height;

        [ObservableProperty] private DateTime _birthday;

        [ObservableProperty] private string _activeAmount;

        [ObservableProperty] private int _passiveCalorieBurn;

        [ObservableProperty] private string _goal;

        public List<string> FocusAreas { get; set; } = new();
    }
}

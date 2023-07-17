using BodyBuddy.Database;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels
{
    public partial class NewExerciseViewModel : BaseViewModel
    {
        private LocalDatabase _database;

        public Exercise NewExercise { get; set; }
        public NewExerciseViewModel(LocalDatabase localDatabase)
        {
            Title = "New Exercise";

            _database = localDatabase;
            this.NewExercise = new Exercise();
        }

        [RelayCommand]
        public async Task SaveExercise()
        {
            await _database.SaveItemAsync(this.NewExercise);

            await GoBackAsync();
        }

        // Dummy data for the combo box
        public List<string> Musclegroups { get; set; } = new List<string>()
        {
            new string("Chest"),
            new string("Shoulders"),
            new string("Biceps"),
            new string("Triceps"),
            new string("Legs"),
            new string("Back"),
        };
    }
}

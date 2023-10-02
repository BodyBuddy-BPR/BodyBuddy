using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels.StartupTest
{
    public partial class StartupTestViewModel : BaseViewModel
    {
        public StartupTestViewModel()
        {
            DisableAllText();
        }

        public void ButtonTest()
        {
            DisableAllText();
        }

        private void DisableAllText()
        {
            IsNameVisible = true;
            IsGenderVisible = false;
            IsWeightVisible = false;
            IsHeightVisible = false;
            IsAgeVisible = false;
            IsActiveVisible = false;
            IsPassiveCalorieBurnVisible = false;
            IsGoalVisible = false;
        }


        #region VisionTrigger
        private bool _isNameVisible;
        private bool _isGenderVisible;
        private bool _isWeightVisible;
        private bool _isHeightVisible;
        private bool _isAgeVisible;
        private bool _isActiveVisible;
        private bool _isPassiveCalorieBurnVisible;
        private bool _isGoalVisible;

        public bool IsNameVisible
        {
            get { return _isNameVisible; }
            set
            {
                _isNameVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsGenderVisible
        {
            get { return _isGenderVisible; }
            set
            {
                _isGenderVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsWeightVisible
        {
            get { return _isWeightVisible; }
            set
            {
                _isWeightVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsHeightVisible
        {
            get { return _isHeightVisible; }
            set
            {
                _isHeightVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsAgeVisible
        {
            get { return _isAgeVisible; }
            set
            {
                _isAgeVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsActiveVisible
        {
            get { return _isActiveVisible; }
            set
            {
                _isActiveVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsPassiveCalorieBurnVisible
        {
            get { return _isPassiveCalorieBurnVisible; }
            set
            {
                _isPassiveCalorieBurnVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsGoalVisible
        {
            get { return _isGoalVisible; }
            set
            {
                _isGoalVisible = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

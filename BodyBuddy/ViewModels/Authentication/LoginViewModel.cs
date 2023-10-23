using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.ViewModels.Authentication
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        public bool isLogin = true; 
        [ObservableProperty]
        public bool isSignUp = false;

        public LoginViewModel()
        {

        }

    }
}

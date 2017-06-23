﻿using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using InfusionGames.CityScramble.Events;
using InfusionGames.CityScramble.ViewModels;
using InfusionGames.CityScramble.Views;
using Xamarin.Forms;

namespace InfusionGames.CityScramble.Services
{
    public class ViewPresenter
    {
        private App _app => (App)Application.Current;

        private readonly IAuthenticationService _authService;

        private Page _mainStack;

        protected Page ActivePage
        {
            set
            {
                if (_app.MainPage != null)
                {
                    var deactive = _app.MainPage.BindingContext as IDeactivate;
                    deactive?.Deactivate(false);
                }

                _app.MainPage = value;

                if (_app.MainPage != null)
                {
                    var activate = _app.MainPage.BindingContext as IActivate;
                    activate?.Activate();
                }
                
            }
        }

        public ViewPresenter(IAuthenticationService authService)
        {
            _authService = authService;
        }

        private void ShowMainNavStack()
        {
            EnsureMainStackInitialized<RaceSelectionView>();

            ActivePage = _mainStack;
        }

        private void ShowJoinTeam()
        {
            EnsureMainStackInitialized<JoinTeamView>();

            ActivePage = _mainStack;
        }

        public async void ShowFirstPage()
        {
            bool loggedIn = await _authService.IsLoggedInAsync();
            if(loggedIn)
            {
                bool hasTeam = await _authService.IsUserLoggedInWithTeamAsync();
                if(hasTeam)
                {
                    ShowMainNavStack();
                }
                else
                {
                    ShowJoinTeam();
                }
            }
            else
            {
                ShowLogin();
            }
        }

        public void ShowLogin()
        {
            EnsureMainStackInitialized<LoginView>();

            ActivePage = _mainStack;
        }

        private void EnsureMainStackInitialized<T>()
        {
            if (_mainStack != null)
            {
                return;
            }

            var navigationPage = new NavigationPage();

            var infusionBlue = _app.Resources["InfusionBlue"];
            navigationPage.BarBackgroundColor = (Color)infusionBlue;

            var barTextColor = _app.Resources["BarTextColor"];
            navigationPage.BarTextColor = (Color)barTextColor;

            IoC.Get<SimpleContainer>().Instance<INavigationService>(new NavigationPageAdapter(navigationPage));

            _mainStack = navigationPage;

            var navigationService = IoC.Get<INavigationService>();
            navigationService.NavigateToViewAsync(typeof(T));

        }

        private Page CreateAndBind(Type viewModelType)
        {
            var viewModel = IoC.GetInstance(viewModelType, null);
            var view = ViewLocator.LocateForModel(viewModel, null, null);

            var page = view as Page;

            if (page == null)
                throw new NotSupportedException(String.Format("{0} does not inherit from {1}.", view.GetType(), typeof(Page)));

            ViewModelBinder.Bind(viewModel, view, null);

            return page;
        }
	}
}

using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using InfusionGames.CityScramble.Events;
using InfusionGames.CityScramble.Services;
using Xamarin.Forms;
using InfusionGames.CityScramble.Views;

namespace InfusionGames.CityScramble.ViewModels
{
    /// <summary>
    /// Login Screen
    /// </summary>
    public class LoginViewModel : BaseScreen
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authService;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoginViewModel(
            INavigationService navigationService, IAuthenticationService authService)
        {
            _navigationService = navigationService;
            _authService = authService;

        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

		protected override async void OnActivate()
		{
			base.OnActivate();
		}

        private async void Login()
        {
            // TODO: Trigger authentication flow
            bool loggedIn = await _authService.LoginAsync();

            await _navigationService.NavigateToViewModelAsync<JoinTeamViewModel>();
        }

    }
}

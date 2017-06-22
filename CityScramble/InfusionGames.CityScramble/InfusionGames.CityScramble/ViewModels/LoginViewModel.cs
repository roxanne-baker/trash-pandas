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

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoginViewModel(
            INavigationService navigationService)
        {
            _navigationService = navigationService;

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

            await _navigationService.NavigateToViewModelAsync<RaceSelectionViewModel>();
        }

    }
}

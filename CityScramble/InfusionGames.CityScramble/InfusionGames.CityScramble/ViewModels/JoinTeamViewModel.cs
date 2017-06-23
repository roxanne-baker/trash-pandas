using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using InfusionGames.CityScramble.Events;
using InfusionGames.CityScramble.Services;
using Xamarin.Forms;
using InfusionGames.CityScramble.Views;
using InfusionGames.CityScramble.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InfusionGames.CityScramble.ViewModels
{
    /// <summary>
    /// Join Team Screen
    /// </summary>
    public class JoinTeamViewModel : BaseScreen, INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public JoinTeamViewModel(
            INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }


        private string _teamCode;
        public string TeamCode
        {
            get { return _teamCode; }
            set
            {
                _teamCode = value;
                OnPropertyChanged();
            }
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override async void OnActivate()
        {
            base.OnActivate();
        }

        private async void JoinTeam()
        {
            Team team = await _dataService.JoinTeamAsync(this.TeamCode);

            await _navigationService.NavigateToViewModelAsync<RaceSelectionViewModel>();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

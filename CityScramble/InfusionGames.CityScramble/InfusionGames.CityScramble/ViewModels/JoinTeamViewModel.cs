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
        private readonly ISettingsService _settingsService;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public JoinTeamViewModel(
            INavigationService navigationService, IDataService dataService, ISettingsService settingsService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _settingsService = settingsService;
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
            TeamMembership team = await _dataService.JoinTeamAsync(this.TeamCode);
            _settingsService.MyTeamId = team.Id;
            _settingsService.MyTeamName = team.Name;

            await _navigationService.NavigateToViewModelAsync<RaceSelectionViewModel>();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

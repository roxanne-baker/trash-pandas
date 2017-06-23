using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using InfusionGames.CityScramble.Models;
using InfusionGames.CityScramble.Services;
using Xamarin.Forms;

namespace InfusionGames.CityScramble.ViewModels
{
    /// <summary>
    /// Displays Leader board for specified race
    /// </summary>
    public class LeaderViewModel : BaseScreen, IRaceTab
    {
        private readonly IDataService _dataService;
        private  BindableCollection<Team> _teams;

        public LeaderViewModel(IDataService dataService)
        {
        //    _dataService = dataService;
           _teams = new BindableCollection<Team>();
           
        }

        #region IRaceTab implementation
        /// <summary>
        /// Navigation Parameter
        /// </summary>`
        public Race SelectedRace { get; set; }

        public string Title { get; private set; } = "Leader Board";

        public string Icon { get; private set; } = "ic_leaderboard.png";

        public int Priority => 1;

        public bool IsSupported(Race race)
        {
            return true;
        }
        public BindableCollection<Team> teams {
            get { return _teams; }
            set { _teams = value; } }
        #endregion
        
        protected override async void OnActivate()
        {
            base.OnActivate();
           var response = await IoC.Get<IDataService>().GetRaceAsync(SelectedRace.Id);
            _teams.AddRange(response.Teams);
          

        }

    }    
}

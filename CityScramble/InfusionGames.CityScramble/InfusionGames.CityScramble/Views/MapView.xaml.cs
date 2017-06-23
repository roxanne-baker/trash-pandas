using InfusionGames.CityScramble.Controls;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace InfusionGames.CityScramble.Views
{
    public partial class MapView : ContentView
    {
        public MapView()
        {
            InitializeComponent();
        }

        public Map GetMap()
        {
            return this.MyMap;
        }
    }
}

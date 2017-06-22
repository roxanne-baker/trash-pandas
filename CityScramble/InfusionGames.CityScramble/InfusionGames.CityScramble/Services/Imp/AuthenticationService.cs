using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Auth;
using Xamarin.Forms;

namespace InfusionGames.CityScramble.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPlatformAuthenticator _authenticator;
        private readonly IMobileServiceClient _mobileService;
        private readonly IDataService _dataService;
        private readonly ISettingsService _settingsService;
        private readonly AccountStore _accountStore;
        

        private const MobileServiceAuthenticationProvider Provider = MobileServiceAuthenticationProvider.Google;
        private static string AuthProvider => Provider.ToString();

        public AuthenticationService(IPlatformAuthenticator authenticator, AccountStore accountStore, IMobileServiceClient mobileService, IDataService dataService, ISettingsService settingService)
        {
            _authenticator = authenticator;
            _mobileService = mobileService;
            _dataService = dataService;
            _settingsService = settingService;

            // can't use AccountStore.Create() for UWP
            _accountStore = accountStore;            
        }

		public async Task<bool> IsUserLoggedInWithTeamAsync() 
		{
			return await IsLoggedInAsync() && !string.IsNullOrEmpty(_settingsService.MyTeamId);
		}

        public async Task<bool> IsLoggedInAsync()
        {
			// First the app starts ensure that previous login is invalidated
			var value = _settingsService.GetValueOrDefault<string>("FirstRun", null);
			if (value == null)
			{
				await LogoutAsync();
				_settingsService.SetValue("FirstRun", "1stRun");
			}

            var accounts = await _accountStore.FindAccountsForServiceAsync(AuthProvider);

            Account account = accounts.FirstOrDefault();

            if (account == null) return false;

            MobileServiceUser user = new MobileServiceUser(account.Username)
            {
                MobileServiceAuthenticationToken = account.Properties["Password"]
            };

            _mobileService.CurrentUser = user;

            return true;
        }

        public async Task<bool> LoginAsync()
        {
            MobileServiceUser user = null;

            try
            {
                user = await _authenticator.Authenticate(Provider);
            }
            catch (InvalidOperationException)
            {
            }

            if (user == null)
            {
                return false;
            }

            _mobileService.CurrentUser = user;

            var account = new Account(user.UserId);

            account.Properties["Password"] = user.MobileServiceAuthenticationToken;

            await _accountStore.SaveAsync(account, AuthProvider);

            await GetProfileAndSaveItToSettings();

            return true;
        }

        public async Task LogoutAsync()
        {
            _settingsService.UserName = string.Empty;

            var accounts = await _accountStore.FindAccountsForServiceAsync(AuthProvider);

            foreach (var account in accounts)
            {
                await _accountStore.DeleteAsync(account, AuthProvider);
            }

            _authenticator.Logout();
        }

        private async Task GetProfileAndSaveItToSettings()
        {
            var profile = await _dataService.GetProfileAsync();
            if (profile != null)
            {
                _settingsService.UserName = profile.DisplayName;
            }
        }

    }
}

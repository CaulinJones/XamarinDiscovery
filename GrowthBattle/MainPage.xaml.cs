using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrowthBattler;
using Xamarin.Forms;
using GrowthBattle.Models;
using Refit;



namespace GrowthBattle
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoginButton.Clicked += LoginButton_Clicked;
            BattleButton.Clicked += BattleButton_Clicked;

        }
        protected async override void OnAppearing()
        { //In prod. would check for cache of names before pulling 
            await CallCountries();

        }
        public void PopulatePicker(CountryList clist)
        {
           foreach (string country in clist.Countries)
            {
                if (country.ToUpper() != country) {
                    PickerOne.Items.Add(country);
                    PickerTwo.Items.Add(country);
                }
            }

        }
        public void CalculateWinner(RootObject first_pop, RootObject second_pop, string first_country, string second_country) 
        {
            //Trouble with Accessing elements in so gross iteration methods will have to do the trick
             var first_dif = 0;
             var second_dif = 0;
            
            foreach (TotalPopulation tp in first_pop.total_population)
            {
                if (first_dif < 0) {
                    first_dif -= tp.population;
                }
                else {
                    first_dif += tp.population;
                }
                 
            };
            foreach (TotalPopulation tp in second_pop.total_population)
            {
                if (second_dif < 0)
                {
                    second_dif -= tp.population;
                }
                else
                {
                    second_dif += tp.population;
                }

            };

            if (first_dif == second_dif)
            {
                Results.Text = "We Have A Tie!";
            }
            else if (first_dif > second_dif)
            {
                Results.Text = first_country + " is the Winner!!";
            }
            else if (first_dif < second_dif)
            {
                Results.Text = second_country + " is the Winner!!";
            }
            else
            {
                Results.Text = "The Results are Inconclusive! Try another matchup.";
            }
            Results.IsVisible = true;

        }
        public void ChangeUI(String Name) 
        {
            ResultLabel.Text = $"Welcome {Name}";
            PickerOne.IsVisible = true;
            PickerTwo.IsVisible = true;
            Vs.IsVisible = true;
            Instructions.IsVisible = true;
            BattleButton.IsVisible = true;
        }
        private async void LoginButton_Clicked(object sender, EventArgs e)
        {

            var authenticationService = DependencyService.Get<IAuthenticationService>();
            var loginResult = await authenticationService.Authenticate();
            LoginButton.IsVisible = false;

            var sb = new StringBuilder();

            if (loginResult.IsError)
            {
                ResultLabel.Text = "An error occurred during login...";
                sb.AppendLine("An error occurred during login:");
                sb.AppendLine(loginResult.Error);
                LoginButton.IsVisible = true;
            }
            else
            {
                ChangeUI(loginResult.User.Identity.Name);
                sb.AppendLine($"ID Token: {loginResult.IdentityToken}");
                sb.AppendLine($"Access Token: {loginResult.AccessToken}");
                sb.AppendLine($"Refresh Token: {loginResult.RefreshToken}");
                sb.AppendLine();
                sb.AppendLine("-- Claims --");
                foreach (var claim in loginResult.User.Claims)
                {
                    sb.AppendLine($"{claim.Type} = {claim.Value}");
                }

            }

            System.Diagnostics.Debug.WriteLine(sb.ToString());


        }
         private async void BattleButton_Clicked(object sender, EventArgs e) 
        {
            var first_country = PickerOne.SelectedItem.ToString().Trim();
            var second_country = PickerTwo.SelectedItem.ToString().Trim();
            await CallPop(first_country, second_country );

        }
        async Task CallPop(string first_country, string second_country)
        { 
            try
            {
                var popAPI = RestService.For<IPopAPI>("http://api.population.io:80/1.0/");
                var first_pop = await popAPI.GetPopulation(first_country);

                var popAPI2 = RestService.For<IPopAPI>("http://api.population.io:80/1.0/");
                var second_pop = await popAPI2.GetPopulation(second_country);
                CalculateWinner(first_pop, second_pop, first_country, second_country);
            }catch (Exception e){
                Results.Text = "The Results are Inconclusive! Try another matchup.";

            }
        }
        async Task CallCountries()
        {
            var popAPI = RestService.For<IPopAPI>("http://api.population.io:80/1.0/");
            var countries = await popAPI.GetCountries();
            PopulatePicker(countries);
        }
    }
}

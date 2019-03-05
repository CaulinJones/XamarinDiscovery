using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace GrowthBattle.Droid
{
    [Activity(Label = "GrowthBattle", Icon = "@mipmap/icon", Theme = "@style/MainTheme",
         MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTask)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "com.caulinjones.growthbattle",
        DataHost = "growthbattle.auth0.com",
        DataPathPrefix = "/android/com.caulinjones.growthbattle/callback")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            Auth0.OidcClient.ActivityMediator.Instance.Send(intent.DataString);
        }
    }
}
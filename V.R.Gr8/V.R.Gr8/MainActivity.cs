using Android.App;
using Android.Widget;
using Android.OS;

namespace V.R.Gr8
{
    [Activity(Label = "V.R.Gr8", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //PMTest Change 

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}


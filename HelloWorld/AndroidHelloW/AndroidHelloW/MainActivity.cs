using Android.App;
using Android.Widget;
using Android.OS;

namespace AndroidHelloW
{
    [Activity(Label = "AndroidHelloW", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            TextView textView = this.FindViewById<TextView>(Resource.Id.textView1);
            Button button = this.FindViewById<Button>(Resource.Id.button1);
            button.Click += (sender, e) =>
            {
                textView.SetTextColor(Android.Graphics.Color.Pink);
                textView.Text = "章老师很可爱吖~";
            };
        }
    }
}


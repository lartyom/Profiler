using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Xamarin.Forms;
using Newtonsoft.Json;


namespace Profiler
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            infobox.Text = "Profiler_App\nv0.2xx[beta]";
            infobox.FontFamily = Device.RuntimePlatform == Device.Android ? "consolas.ttf#Consolas" : null; // set only for Android
            infobox_2.FontFamily = Device.RuntimePlatform == Device.Android ? "consolas.ttf#Consolas" : null; // set only for Android
            infobox_3.FontFamily = Device.RuntimePlatform == Device.Android ? "consolas.ttf#Consolas" : null; // set only for Android
            profile_url.FontFamily = Device.RuntimePlatform == Device.Android ? "consolas.ttf#Consolas" : null; // set only for Android
            open.FontFamily = Device.RuntimePlatform == Device.Android ? "consolas.ttf#Consolas" : null; // set only for Android
            infobox_2.Text = "**-***-***";
            
            
        }

        private void Open_profile(object sender, System.EventArgs e)
        {
            string profile = "";
            try
            {
                WebRequest request = WebRequest.Create($"https://api.vk.com/method/users.get?user_ids={profile_url.Text}&fields=status,photo_max,bdate,activities&access_token=3385c29d14bf9c3b2672011ca29b1e77407bbc6aff408f0dba25525232dfad0775ac591bf3d3330521673&v=5.92");
                WebResponse response = request.GetResponse();



                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {

                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            profile += line;
                        }
                    }
                }
                response.Close();



                if (profile.Contains("error"))
                {
                    DisplayAlert("Error", "Invalid user id", "OK");
                }
                else
                {

                    profile = profile.Substring(13);
                    profile = profile.Substring(0, profile.Length - 2);
                    Profile json_profile = JsonConvert.DeserializeObject<Profile>(@profile);

                    if (json_profile.status == "")
                    {
                        json_profile.status = "NO RECORD";
                    }
                    if (json_profile.activities == null || json_profile.activities == "")
                    {
                        json_profile.activities = "NO RECORD";
                    }
                    infobox_3.Text = json_profile.last_name;
                    infobox_3.Text += $", {json_profile.first_name}";
                    photo.Source = new Uri(json_profile.photo_max);
                    infobox_3.Text += $"\n{json_profile.status}";
                    infobox_3.Text += $"\nAge: {CalculateAge(json_profile.bdate)}";
                    infobox_3.Text += $"\nOccupation: {json_profile.activities}";                   
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }
        }
        class Profile
        {
            public string id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public bool is_closed { get; set; }
            public bool can_access_closed { get; set; }
            public string status { get; set; }
            public string photo_max { get; set; }
            public string bdate { get; set; }           
            public string activities { get; set; }
        }
        string CalculateAge(string birthday)
        {
            string[] date = new string[3];
            int count = 0;
            string age = "";
            if (birthday == null)
            {
                age = "NO RECORD";
            }
            else
            {
                for (int i = 0; i < birthday.Length; i++)
                {
                    if (birthday[i] != '.')
                    {
                        date[count] += birthday[i];
                    }
                    else { count += 1; }
                }
                if (date[2] == null)
                {
                    age = "NO RECORD";
                }
                else
                {
                    DateTime birthdate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                    age = (new DateTime(DateTime.Now.Subtract(birthdate).Ticks).Year - 1).ToString();
                }
            }

            return age;
        }
    }
}

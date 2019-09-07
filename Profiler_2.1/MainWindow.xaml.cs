using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace WpfApp1
{
    public partial class MainWindow
    {   
        static IniFile config = new IniFile("config.ini");
        public MainWindow()
        {
            Console.Out.WriteLine();
            Console.Out.WriteLine();
            Console.Out.WriteLine();
            Console.Out.WriteLine();
            Console.Out.WriteLine("                   ___        ____  ______ ____ ");
            Console.Out.WriteLine(@"                //_   \_    / __ \/ ____// __ \");
            Console.Out.WriteLine(@"                / / \___/- / / / / __/ // / / /\");
            Console.Out.WriteLine(@"               \ \        / /_/ / /__ // /_/ / /");
            Console.Out.WriteLine(@"                \ \\     /_____/_____//_____/ /");
            Console.Out.WriteLine(@"                 / / \   _\_____\_____\\_____\/");
            Console.Out.WriteLine(@"         ////\___\ \ / /  ___// ___// ____/\");
            Console.Out.WriteLine(@"        \/\\\\    \//  \__ \ / __/ / /\___\/   v2.0");
            Console.Out.WriteLine(@"       \\     \\////  ___/ // /__ / /_/__    /\//\\*=~-");
            Console.Out.WriteLine(@"       \--          /_____//_____/\ ____/\  //");
            Console.Out.WriteLine(@"        \ ----\     \_____\\_____\/\____\/ //");
            Console.Out.WriteLine(@"         \----\\____________________________//");
            Console.Out.WriteLine(@"               \_\\/__\/_\\/__\/__\\/__\/__\//");
            
            InitializeComponent();
            infobox.Content = "Profiler__App\nv0.2xx[beta]";
            infobox_2.Content = "**-***-***";
            Hyperlink path_to_profiles = new Hyperlink(new Run(config.Read("DirectoryPath") + " /"));         
            path_to_profiles.NavigateUri = new Uri(Directory.GetCurrentDirectory()+"/"+ config.Read("DirectoryPath"));
            path_to_profiles.RequestNavigate += Hyperlink_RequestNavigate;
            profile_dir.Inlines.Add(path_to_profiles);
            
            this.KeyDown += Form1_KeyDown;
            //if (clArgs.Length > 1)
            //{
            //    profile_url.Text = clArgs[clArgs.Length-1];
            //    path_to_file = $"{profile_url.Text}";
            //    Open_profile(null, null);
            //}
            //else
            //{
            //    path_to_file = $"{config.Read("DirectoryPath")}/{profile_url.Text}";
            //}
            
        }

        string[] clArgs = Environment.GetCommandLineArgs();       
        string[] dir = Directory.GetFiles(config.Read("DirectoryPath"), "* json");      
        int dir_count = 0;
        string[] param = new string[] { "nickname", "country", "relation","timezone", "city", "relatives"};

        class Profile           
        {
            public string[] relations = new string[] { "NO RECORD", "Single", "Have a friend", "Engaged", "Married", "It's Complicated", "Activity looking", "In love", "In a civil marriage" };
            public string id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public bool is_closed { get; set; }
            public bool can_access_closed { get; set; }
            public string status { get; set; }
            public string photo_max { get; set; }
            public string bdate { get; set; }
            public Dictionary<string, string> status_audio { get; set; }
            public string activities { get; set; }
            public string nickname { get; set; }
            public Dictionary<string, string> country { get; set; }
            public int relation { get; set; }
            public int timezone { get; set; }
            public Dictionary<string, string> city { get; set; }
            public List<Dictionary<string, string>> relatives { get; set; }

        }
        class Action
        {
            public string title { get; set; }
            public string html { get; set; }
            public string count { get; set; }
        }
        class Wall
        {
            public Dictionary<string, List<Dictionary<string, string>>> response { get; set; }
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

        WebClient Client = new WebClient();
        string access_key = config.Read("access_key", "Keys");
        string access_key_2 = config.Read("access_key_2", "Keys");

        public void Load_profile(object sender, RoutedEventArgs e)
        {           
            try
            {
                if (checkBox1.IsChecked == true)
                {
                    Client.DownloadFile($"https://api.vk.com/method/users.get?user_ids={profile_load.Text}&fields=status,photo_max,bdate,activities,{string.Join(",", param)}&access_token={access_key_2}&v=5.92", $"{config.Read("DirectoryPath")}/{profile_load.Text}.json");
                   
                }
                else
                {

                    Client.DownloadFile($"https://api.vk.com/method/users.get?user_ids={profile_load.Text}&fields=status,photo_max,bdate,activities&access_token={access_key}&v=5.92", $"{config.Read("DirectoryPath")}/{profile_load.Text}.json");
                    
                }
                //if (File.ReadAllLines($"{profile_url.Text}.json").Contains("error"))
                //{
                //    MessageBox.Show("Invalid user id");
                //    File.Delete($"{profile_url.Text}.json");
                //}
                
                dir = Directory.GetFiles(config.Read("DirectoryPath"), "*json");
            }
            catch
            {
                MessageBox.Show("Not connection");
            }          
        }

        private bool isFocused = false;
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            isFocused = true;
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (isFocused)
            {
                isFocused = false;
                (sender as TextBox).SelectAll();
            }
        }
        public void Update_profile(object sender, RoutedEventArgs e)
        {
            Console.Out.WriteLine($"Profile {profile_url.Text} updated");
            Load_profile(new object(), new RoutedEventArgs());

            Open_profile(new object(), new RoutedEventArgs());
           
        }
        public void OnlineOpenProfile(object sender, RoutedEventArgs e)
        {
            //profile_url.Text += ".json";

            string profile = "";
            WebRequest request;
            WebResponse response = null;
            try
            {

                try
                {
                    if (checkBox1.IsChecked == true)
                    {
                        
                        request = WebRequest.Create($"https://api.vk.com/method/users.get?user_ids={profile_load.Text}&fields=status,photo_max,bdate,activities,{string.Join(",", param)}&access_token={access_key_2}&v=5.92");
                        response = request.GetResponse();
                    }
                    else
                    {
                       
                        request = WebRequest.Create($"https://api.vk.com/method/users.get?user_ids={profile_load.Text}&fields=status,photo_max,bdate,activities&access_token={access_key}&v=5.92");
                        response = request.GetResponse();
                    }
                    //if (File.ReadAllLines($"{profile_url.Text}.json").Contains("error"))
                    //{
                    //    MessageBox.Show("Invalid user id");
                    //    File.Delete($"{profile_url.Text}.json");
                    //}

                   

                }
                catch
                {
                    MessageBox.Show("Not connection");
                }
                



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
                    MessageBox.Show("Invalid user id", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {
                    //Обрезка ненужного тэга
                    


                    profile = profile.Substring(13);
                    profile = profile.Substring(0, profile.Length - 2);
                    Profile json_profile = JsonConvert.DeserializeObject<Profile>(@profile); //Десериализация json файла профайла

                    if (json_profile.status == "")
                    {
                        json_profile.status = "NO RECORD";
                    }
                    if (json_profile.activities == null || json_profile.activities == "")
                    {
                        json_profile.activities = "NO RECORD";
                    }
                    //Фамилия, Имя
                    infobox_3.Content = $"{json_profile.last_name}, {json_profile.first_name}";
                    infobox_4.Content = $"  {json_profile.status}";
                    //Установка размеров
                    age.Margin = new Thickness(10, 10, 55 - (CalculateAge(json_profile.bdate).Length) * 4, 35);
                    infobox_4.Margin = new Thickness(117, 45, 27 - (json_profile.status.Length) * 3, 0);
                    occupation.Margin = new Thickness(10, 45, 15 - (json_profile.activities.Length) * 5, 0);
                    //Возраст
                    age.Content = $"Age: {CalculateAge(json_profile.bdate)}";
                    photo.Source = new BitmapImage(new Uri(json_profile.photo_max));
                    //Деятельность
                    occupation.Content = $"Occupation: {json_profile.activities}";
                    if (json_profile.status_audio != null)
                    {
                        add.Opacity = 100;
                        add.Content = "♫";
                        add.Background = Brushes.LightSkyBlue;
                        hack.IsEnabled = true;
                        hack.Click += Download;
                    }
                    else
                    {
                        add.Opacity = 0;
                        add.Content = "";
                        add.Background = Brushes.Black;
                        hack.IsEnabled = false;
                        hack.Click -= Download;

                    }

                    //Метод вычисляющий возраст

                }
            }

            catch (Exception ex)
            {
                if (ex.GetType().ToString() != "System.ArgumentNullException" && ex.GetType().ToString() != "System.IndexOutOfRangeException")
                {
                    MessageBox.Show(ex.Message + " " + ex.GetType().ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //path_to_file = $"{config.Read("DirectoryPath")}/{profile_url.Text}";
        }
        public void Open_profile(object sender, RoutedEventArgs e)
        {
            //profile_url.Text += ".json";
            dir = Directory.GetFiles(config.Read("DirectoryPath"), "*json");
            
            
            try
            {
                string[] profile = File.ReadAllLines($"{config.Read("DirectoryPath")}/{ profile_url.Text}");

                if (profile[0].Contains("error") || File.Exists($"{config.Read("DirectoryPath")}/{profile_url.Text}") == false)
                {
                    MessageBox.Show("Invalid user id", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {
                    //Обрезка ненужного тэга
                    switch (config.Read("Control")) {
                        case "mouse":
                        this.MouseWheel += this_MouseWheel;
                            break;
                        case "buttons":
                         prev_btn.IsEnabled = true;
                         prev_btn.Opacity = 100;
                         next_btn.IsEnabled = true;
                         next_btn.Opacity = 100;
                         break;
                }
                    

                    profile[0] = profile[0].Substring(13);
                    profile[0] = profile[0].Substring(0, profile[0].Length - 2);
                    Profile json_profile = JsonConvert.DeserializeObject<Profile>(@profile[0]); //Десериализация json файла профайла

                    if (json_profile.status == "")
                    {
                        json_profile.status = "NO RECORD";
                    }
                    if (json_profile.activities == null || json_profile.activities == "")
                    {
                        json_profile.activities = "NO RECORD";
                    }

                    //Фамилия, Имя
                    infobox_3.Content = $"{json_profile.last_name}, {json_profile.first_name}";
                    infobox_4.Content = $"  {json_profile.status}";
                    //Установка размеров
                    age.Margin = new Thickness(10, 10, 55 - (CalculateAge(json_profile.bdate).Length) * 4, 35);
                    infobox_4.Margin = new Thickness(117, 45, 27 - (json_profile.status.Length) * 3, 0);
                    occupation.Margin = new Thickness(10, 45, 15 - (json_profile.activities.Length) * 5, 0);
                    //Возраст
                    age.Content = $"Age: {CalculateAge(json_profile.bdate)}";
                    photo.Source = new BitmapImage(new Uri(json_profile.photo_max));
                    //Деятельность
                    occupation.Content = $"Occupation: {json_profile.activities}";
                    if (json_profile.status_audio != null)
                    {
                        add.Opacity = 100;
                        add.Content = "♫";
                        add.Background = Brushes.LightSkyBlue;
                        hack.IsEnabled = true;
                        hack.Click += Download;
                    }
                    else
                    {
                        add.Opacity = 0;
                        add.Content = "";
                        add.Background = Brushes.Black;
                        hack.IsEnabled = false;
                        hack.Click -= Download;

                    }

                    //Метод вычисляющий возраст
                    
                }
            }

            catch (Exception ex)
            {
                if (ex.GetType().ToString() != "System.ArgumentNullException" && ex.GetType().ToString() != "System.IndexOutOfRangeException")
                {
                    MessageBox.Show(ex.Message +" "+ ex.GetType().ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
            //path_to_file = $"{config.Read("DirectoryPath")}/{profile_url.Text}";
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Q" && hack.IsEnabled==true)
            {
                Download(new object(), new RoutedEventArgs());
            }

        }
        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Open_profile(new object(), new RoutedEventArgs());
            }

        }
        void this_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                Next(new object(), new RoutedEventArgs());
            if (e.Delta < 0)
                Prev(new object(), new RoutedEventArgs());
        }

        public void Download(object sender, RoutedEventArgs e)
        {
            string[] profile = File.ReadAllLines($"{config.Read("DirectoryPath")}/{profile_url.Text}");
            if (profile[0].Contains("error"))
            {
                MessageBox.Show("Invalid user id","Error");
            }
            else
            {

                //Обрезка ненужного тэга
                profile[0] = profile[0].Substring(13);
                profile[0] = profile[0].Substring(0, profile[0].Length - 2);
                Profile json_profile = JsonConvert.DeserializeObject<Profile>(@profile[0]);
                Console.Out.WriteLine("Current audio: "+json_profile.status_audio["title"]+" / " + json_profile.status_audio["artist"]);
                Console.Out.WriteLine("----------------------------------------------------");
                //Неработает 
                //Client.DownloadFile($"{json_profile.status_audio["url"]}", $"{json_profile.status_audio["title"]}" + ".mp3");
                //System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"LG_Cable_Connect.ogg");
                //player.Play()

                string URL = "https://vrit.me/action.php";
                WebClient webClient = new WebClient();
                System.Collections.Specialized.NameValueCollection formData = new System.Collections.Specialized.NameValueCollection();
                Console.Out.WriteLine("POST /action.php HTTP/1.1");
                Console.Out.WriteLine("Host: vrit.me");
                Console.Out.WriteLine("Origin: https://vrit.me");
                Console.Out.WriteLine("method: audio.search");
                Console.Out.WriteLine("q:  "+ json_profile.status_audio["title"] + " / " + json_profile.status_audio["artist"]);
                formData["method"] = "audio.search";
                formData["q"] = json_profile.status_audio["title"] + " / " + json_profile.status_audio["artist"];
                byte[] responseBytes = webClient.UploadValues(URL, "POST", formData);
                File.WriteAllText("action.json", Encoding.UTF8.GetString(responseBytes));
                string file = File.ReadAllText("action.json");
                Action json_action = JsonConvert.DeserializeObject<Action>(@file);
                Console.Out.WriteLine("----------------------------------------------------");
                Match m;
                string HRefPattern = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";
                try
                {
                    m = Regex.Match(json_action.html, HRefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
                    while (m.Success)
                    {                        
                            Console.Out.WriteLine("Found href " + m.Groups[1] + " at " + m.Groups[1].Index);
                            try
                            {
                                Client.DownloadFile("https://vrit.me" + m.Groups[1], "music.mp3");
                            Console.Out.WriteLine("Downloading .mp3...");
                          
                            m = m.NextMatch();
                        }
                        catch { };
                                                                        
                    }
                }
                catch (RegexMatchTimeoutException)
                {
                    Console.WriteLine("The matching operation timed out.");
                }
            }
        }


        public void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                MessageBox.Show(e.Result);
                //e.result fetches you the response against your POST request.         
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }
        public void Next(object sender, RoutedEventArgs e)
        {
            try
            {
                profile_url.Text = $"{dir[dir_count].Replace(config.Read("DirectoryPath") + "\\", "")}";
                dir_count += 1;

            }
            catch
            {
                dir_count = 0;
            }

            Open_profile(new object(), new RoutedEventArgs());


        }
        public void Prev(object sender, RoutedEventArgs e)
        {
            try
            {
                profile_url.Text = $"{dir[dir_count].Replace(config.Read("DirectoryPath")+"\\","")}";
                dir_count -= 1;
            }
            catch
            {
                dir_count = dir.Length - 1;
            }

            Open_profile(new object(), new RoutedEventArgs());



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] profile = File.ReadAllLines($"{config.Read("DirectoryPath")}/{profile_url.Text}");
                if (profile[0].Contains("error"))
                {
                    MessageBox.Show("Invalid user id");
                }
                else
                {
                    
                    //Обрезка ненужного тэга
                    profile[0] = profile[0].Substring(13);
                    profile[0] = profile[0].Substring(0, profile[0].Length - 2);
                    Profile json_profile = JsonConvert.DeserializeObject<Profile>(@profile[0]);
                    
                    if (json_profile.country["title"] == null || json_profile.country["title"] == "")
                    {
                      json_profile.country["title"] = "NO RECORD";
                    }
                    // Create an Image object. 
                    //System.Drawing.Image image = new System.Drawing.Bitmap(json_profile.photo_max);
                    //System.Drawing.Imaging.PropertyItem[] propItems = image.PropertyItems;
                    //List<string> gps = new List<string>();

                    //foreach (System.Drawing.Imaging.PropertyItem propItem in propItems)
                    //{
                    //    //0x00FE
                    //    if (propItem.Id == 0x9003)
                    //    {
                    //        gps.Add(new ASCIIEncoding().GetString(propItem.Value));

                    //    }
                    //}
                    // Get the PropertyItems property from image.

                    
                    Console.Out.WriteLine("----------------------------------------------------");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Out.WriteLine($"{json_profile.first_name} {json_profile.nickname} {json_profile.last_name}");
                    Console.ResetColor();
                        Console.Out.WriteLine($"Age > {CalculateAge(json_profile.bdate)}");                                                                              
                        Console.Out.WriteLine($"Nationality > {json_profile.country["title"]}");                                     
                        Console.Out.WriteLine($"Marital Status > {json_profile.relations[json_profile.relation]}");
                        Console.Out.WriteLine($"Last Known Loc. > {json_profile.timezone}, {json_profile.city["title"]}");
                    
                    new WebClient().DownloadFile($"https://api.vk.com/method/wall.get?owner_id={json_profile.id}&extended=1&access_token={access_key_2}&v=5.95", "wall.json");
                    string wall = File.ReadAllText("wall.json");
                    if (wall.Contains("long")){
                        string lon = wall.Substring(wall.IndexOf("long"), "long".Length + 2 + 9);
                        string lat = wall.Substring(wall.IndexOf("long") - 11, 9);
                        string lon2 = wall.Substring(wall.LastIndexOf("long"), "long".Length + 2 + 9);
                        string lat2 = wall.Substring(wall.LastIndexOf("long") - 11, 9);

                        Console.Out.WriteLine(String.Join(", ", new string[] { lon, lat, lon2, lat2 }));

                        Clipboard.SetText(String.Join(", ", new string[] { lon, lat, lon2, lat2 }));
                    }
                    List<string> names = new List<string>();
                    List<string> types = new List<string>();
                    string[] names_str = new string[names.Count];
                    for (int i = 0; i < json_profile.relatives.Count; i++)
                    {
                        names.Add(json_profile.relatives[i]["name"]);
                        types.Add(json_profile.relatives[i]["type"]);
                    }
                    Console.Out.WriteLine($"Children > {String.Join(", ", names)}");
                }
            }
            catch 
            {}
           
        }

        }
    }


    


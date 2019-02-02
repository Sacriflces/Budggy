using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Budggy;
using Windows.Storage;
using Newtonsoft.Json;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BudggyUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary> Don't like having to keep making a new Settings. Having an "ambiguity between Mainpage.Settings and Mainpage.Settings".. confused
    public sealed partial class MainPage : Page
    {
        Budget budget;
      //  MySettings mySettings = new MySettings();
        
        public MainPage()
        {
            Load();
            this.InitializeComponent();            
            budggyLB.SelectedIndex = 0;
            

        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            budggySV.IsPaneOpen = !budggySV.IsPaneOpen;
        }

        private void BudggyLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            switch (budggyLB.SelectedIndex)
            {
                case 0: TitleTB.Text = "Budggy";                    
                    contentFrame.Navigate(typeof(BudggyHome),budget);                       
                    break;
                case 1: TitleTB.Text = "Bins";
                    contentFrame.Navigate(typeof(BinsHome),budget);
                    break;
                case 2: TitleTB.Text = "Transfer";
                    contentFrame.Navigate(typeof(TransferHome),budget);
                    break;
                case 3: TitleTB.Text = "Statistics";
                    contentFrame.Navigate(typeof(StatisticsHome),budget);
                    break;
                case 4: TitleTB.Text = "Settings";
                    contentFrame.Navigate(typeof(Settings),budget);
                    break;

            }
           
        }

        private void SearchBT_Click(object sender, RoutedEventArgs e)
        {
            Save();

           
        }

        private async void Save()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            StorageFile budggyFile = await storageFolder.CreateFileAsync("budggy.txt", CreationCollisionOption.OpenIfExists);

            var serializedBudggy = JsonConvert.SerializeObject(budget);

            await FileIO.WriteTextAsync(budggyFile, serializedBudggy);
        }

        private async void Load()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            StorageFile budggyFile = await storageFolder.CreateFileAsync("budggy.txt", CreationCollisionOption.OpenIfExists);

            string serializedbudggy = await FileIO.ReadTextAsync(budggyFile);

            if (serializedbudggy != null && serializedbudggy != "null")
            {
                budget = JsonConvert.DeserializeObject<Budget>(serializedbudggy);
                //budget = new Budget();
            }
            else
                budget = new Budget();


        }
    }
}

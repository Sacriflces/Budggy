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
      
        
        public MainPage()
        {
          Load();
            //budget = new Budget();
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
           /* Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Windows.Storage.ApplicationDataContainer container =
                localSettings.CreateContainer("BudggyContainer", Windows.Storage.ApplicationDataCreateDisposition.Always);
            if(localSettings.Containers.ContainsKey("BudggyContainer"))
            {
                localSettings.Containers["BudggyContainer"].Values[]
                    
            } */

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            StorageFile budggyFile = await storageFolder.CreateFileAsync("Budggy.txt", CreationCollisionOption.ReplaceExisting);
           // File.Create("Budggy.txt", CreationCollisionOption.OpenIfExists)
            var serializedBudggy = JsonConvert.SerializeObject(budget,Formatting.Indented);
            
            await FileIO.WriteTextAsync(budggyFile, serializedBudggy); 
        } 

        private async void Load()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            StorageFile budggyFile = await storageFolder.CreateFileAsync("Budggy.txt", CreationCollisionOption.OpenIfExists);

            string serializedbudggy = await FileIO.ReadTextAsync(budggyFile);
        
            if (serializedbudggy != null && serializedbudggy != "null")
            {
                budget = JsonConvert.DeserializeObject<Budget>(serializedbudggy);
                //budget = new Budget();
            }
            else
                budget = new Budget(); 


        }

        private void SaveBT_Click(object sender, RoutedEventArgs e)
        {
            foreach (Bin  bin in budget.Bins)
            {
                bin.RefreshDrawers();
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
               Page page = contentFrame.Content as Page;
               
          
        }
    }
}

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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BudggyUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Budget budget = new Budget();

        public MainPage()
        {
            this.InitializeComponent();
            budggyLB.SelectedIndex = 0;
            
            
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            budggySV.IsPaneOpen = !budggySV.IsPaneOpen;
        }

        private void BudggyLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Page page;
            switch (budggyLB.SelectedIndex)
            {
                case 0: TitleTB.Text = "Budggy";                    
                    contentFrame.Navigate(typeof(BudggyHome));
                        contentFrame.DataContext = budget;                        
                    break;
                case 1: TitleTB.Text = "Bins";
                    contentFrame.Navigate(typeof(BinsHome));
                    break;
                case 2: TitleTB.Text = "Transfer";
                    contentFrame.Navigate(typeof(TransferHome));
                    break;
                case 3: TitleTB.Text = "Statistics";
                    contentFrame.Navigate(typeof(StatisticsHome));
                    break;
                case 4: TitleTB.Text = "Settings";
                    contentFrame.Navigate(typeof(Settings));
                    break;

            }
           
        }
    }
}

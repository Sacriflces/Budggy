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


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BudggyUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BinsHome : Page
    {
        BudgetViewModel budget;

        public BinsHome()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (BudgetViewModel)e.Parameter;
            budget = parameters;
            this.DataContext = budget;
            BinsLV.ItemsSource = budget.Budggy.Bins;

            



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index;
            FrameworkElement ele = sender as FrameworkElement;
            if(ele != null)
            {
                BinsLV.SelectedItem = ele.DataContext;
                index = BinsLV.SelectedIndex;

                budget.Budggy.DeleteBin(budget.Budggy.Bins[index].Name);
            }
            
        }
    }
}

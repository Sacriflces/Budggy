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
        Budget budget;

        public BinsHome()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (Budget)e.Parameter;
            budget = parameters;
            this.DataContext = budget;
            BinsLV.ItemsSource = budget.Bins;
            BinsEditLV.ItemsSource = budget.Bins;

            



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index;
            FrameworkElement ele = sender as FrameworkElement;
            if(ele != null)
            {
                BinsLV.SelectedItem = ele.DataContext;
                index = BinsLV.SelectedIndex;

                budget.DeleteBin(budget.Bins[index].Name);
            }
            
        }

        private void CreateBinButton_Click(object sender, RoutedEventArgs e)
        {
            decimal goalBalance;
            try
            {
                goalBalance = Convert.ToDecimal(BinGoalBalTB.Text);
            }
            catch
            {
                goalBalance = 2500m;
            }
            budget.AddBin(BinNameTB.Text, BinDescrTB.Text, Convert.ToDecimal(BinPercentageTB.Text), goalBalance);
        }

        private void BinPercentageTB_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            
            int index = BinPercentageTB.SelectionStart;
            int index2;
            try
            {
                char str = BinPercentageTB.Text[index - 1];
                if (str != '.' && str != ',' && !Char.IsControl(str) && !Char.IsNumber(str))
                {
                    index2 = BinPercentageTB.Text.IndexOf(str);

                    if (index2 >= 0)
                    {
                        BinPercentageTB.Text = BinPercentageTB.Text.Remove(index2, 1);
                        BinPercentageTB.SelectionStart = index - 1;
                    }
                }
                /*
                while(ValueTB.Text.Length < 2)
                {
                    ValueTB.Text = ValueTB.Text.Insert(0, "0");
                }

                ValueTB.Text = AddPeriod(ValueTB.Text); */

                else if (str == '.')

                {
                    if (BinPercentageTB.Text.IndexOf('.') != BinPercentageTB.Text.LastIndexOf('.'))
                    {

                        BinPercentageTB.Text = BinPercentageTB.Text.Remove(BinPercentageTB.Text.LastIndexOf('.'), 1);
                        BinPercentageTB.SelectionStart = index - 1;
                    }
                }

            }
            catch
            {
                return;
            }
        }

        private void BinGoalBalTB_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            int index = BinGoalBalTB.SelectionStart;
            int index2;
            try
            {
                char str = BinGoalBalTB.Text[index - 1];
                if (str != '.' && str != ',' && !Char.IsControl(str) && !Char.IsNumber(str))
                {
                    index2 = BinGoalBalTB.Text.IndexOf(str);

                    if (index2 >= 0)
                    {
                        BinGoalBalTB.Text = BinGoalBalTB.Text.Remove(index2, 1);
                        BinGoalBalTB.SelectionStart = index - 1;
                    }
                }
                /*
                while(ValueTB.Text.Length < 2)
                {
                    ValueTB.Text = ValueTB.Text.Insert(0, "0");
                }

                ValueTB.Text = AddPeriod(ValueTB.Text); */

                else if (str == '.')

                {
                    if (BinGoalBalTB.Text.IndexOf('.') != BinGoalBalTB.Text.LastIndexOf('.'))
                    {

                        BinGoalBalTB.Text = BinGoalBalTB.Text.Remove(BinGoalBalTB.Text.LastIndexOf('.'), 1);
                        BinGoalBalTB.SelectionStart = index - 1;
                    }
                }

            }
            catch
            {
                return;
            }
        }

        private void EditBinButton_Click(object sender, RoutedEventArgs e)
        {
            BinsEditLV.Visibility = (BinsEditLV.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}

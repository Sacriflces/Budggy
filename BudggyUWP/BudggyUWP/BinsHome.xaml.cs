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
        int index = 0;

        public BinsHome()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (Budget)e.Parameter;
            budget = parameters;
            BinBackBTB.Visibility = Visibility.Collapsed;
            if(budget.Bins.Count != 0)
            {
                this.DataContext = budget.Bins[index];
                BinsLV.ItemsSource = budget.Bins;
                BinsEditLV.ItemsSource = budget.Bins;

                DrawerLVNV.ItemsSource = budget.Bins[index].CurrDrawers;
                GoalLVNV.ItemsSource = budget.Bins[index].Goals;
            }
           





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
            budget.AddBin(BinNameTB.Text, BinDescrTB.Text, Convert.ToDecimal(BinPercentageTB.Text));
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

        private void DrawerDelBT_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement ele = sender as FrameworkElement;
            Drawer drawer = ele.DataContext as Drawer;
            if (ele != null)
            {
                budget.RemoveBinDrawer(drawer.BinName, drawer.Name);
            }
        }

        private void DrawerCreBT_Click(object sender, RoutedEventArgs e)
        {
            int index;
            FrameworkElement ele = sender as FrameworkElement;
            if (ele != null)
            {
                BinsLV.SelectedItem = ele.DataContext;
                index = BinsLV.SelectedIndex;

                budget.Bins[index].CreateDrawer();

            }
        }

        private void BinNameTBLNW_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            BinNameTBNWB.Visibility = Visibility.Visible;
            BinNameTBLNWB.Visibility = Visibility.Collapsed;
        }

        private void BinNameTBLNWB_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            BinNameTBNWB.Visibility = Visibility.Visible;
            BinNameTBLNWB.Visibility = Visibility.Collapsed;
        }

        private void BinNameTBNW_FocusDisengaged(Control sender, FocusDisengagedEventArgs args)
        {
            BinNameTBLNWB.Visibility = Visibility.Visible;
            BinNameTBNWB.Visibility = Visibility.Collapsed;
        }

        private void BinNameTBNW_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                BinNameTBLNWB.Visibility = Visibility.Visible;
                BinNameTBNWB.Visibility = Visibility.Collapsed;
            }
        }

        private void BinDescTBLNW_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            BinDescTBLNWB.Visibility = Visibility.Collapsed;
            BinDescTBNWB.Visibility = Visibility.Visible;
        }

        private void BinDescTBNW_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                BinDescTBLNWB.Visibility = Visibility.Visible;
                BinDescTBNWB.Visibility = Visibility.Collapsed;
            }
        }

        private void BinPerTBLNW_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            TestGrid.Children[4].Visibility = Visibility.Visible;
            TestGrid.Children[3].Visibility = Visibility.Collapsed;
        }

       

        private void BinPerTBNW_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Bin currentBin = (Bin)DataContext;
                TextBox text = (TextBox)TestGrid.Children[4];
                currentBin.Percentage = Convert.ToDecimal(text.Text);
                TestGrid.Children[3].Visibility = Visibility.Visible;
                TestGrid.Children[4].Visibility = Visibility.Collapsed;

            }
        }

        private void BinNextBT_Click(object sender, RoutedEventArgs e)
        {
            BinBackBTB.Visibility = Visibility.Visible;
            DataContext = budget.Bins[++index];
            DrawerLVNV.ItemsSource = budget.Bins[index].CurrDrawers;
            GoalLVNV.ItemsSource = budget.Bins[index].Goals;
            if (index == budget.Bins.Count - 1) {
                BinNextBTB.Visibility = Visibility.Collapsed;
            }
            
        }

        private void BinBackBT_Click(object sender, RoutedEventArgs e)
        {
            BinNextBTB.Visibility = Visibility.Visible;
            DataContext = budget.Bins[--index];
            DrawerLVNV.ItemsSource = budget.Bins[index].CurrDrawers;
            GoalLVNV.ItemsSource = budget.Bins[index].Goals;
            if (index == 0)
            {
                BinBackBTB.Visibility = Visibility.Collapsed;
            }
        }

        private void BinCreateBT_Click(object sender, RoutedEventArgs e)
        {
            
            budget.AddBin("New Bin", "Description", 0);
            index = budget.Bins.Count - 1;
            DataContext = budget.Bins[index];
            DrawerLVNV.ItemsSource = budget.Bins[index].CurrDrawers;
            GoalLVNV.ItemsSource = budget.Bins[index].Goals;
            BinNextBTB.Visibility = Visibility.Collapsed;

        }

        private void AddDrawerBTNV_Click(object sender, RoutedEventArgs e)
        {
            budget.Bins[index].CreateDrawer();
        }

        private void AddGoalBTNV_Click(object sender, RoutedEventArgs e)
        {
            budget.Bins[index].CreateGoal();
        }

        private void GoalDelBT_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement ele = sender as FrameworkElement;
            Goal goal = ele.DataContext as Goal;
            if (ele != null)
            {
                budget.Bins[index].RemoveGoal(goal.Name);                
            }
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TextBox ele = sender as TextBox;
                Goal goal = ele.DataContext as Goal;
                goal.Percentage = Convert.ToDecimal(ele.Text);
                //goal percentage
            }
        }

        private void TextBox_KeyDown_1(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TextBox ele = sender as TextBox;
                Goal goal = ele.DataContext as Goal;
                goal.GoalVal = Convert.ToDecimal(ele.Text);
                //goal percentage
            }
        }

        private void TextBox_KeyDown_2(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TextBox ele = sender as TextBox;
                Drawer drawer = ele.DataContext as Drawer;
                drawer.AvailAmount = Convert.ToDecimal(ele.Text);
                //goal percentage
            }
        }

        public static string GetPageStr()
        {
            return "BinsSTR!!";
        }

        private void TextBox_KeyDown_3(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TextBox ele = sender as TextBox;
                Drawer drawer = ele.DataContext as Drawer;
                drawer.MonthlyAmount = Convert.ToDecimal(ele.Text);
                //goal percentage
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton ele = sender as ToggleButton;
            Drawer drawer = ele.DataContext as Drawer;
            drawer.rollOver = (bool)ele.IsChecked;           
        }
    }
}

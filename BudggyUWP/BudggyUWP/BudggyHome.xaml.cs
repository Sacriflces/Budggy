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
using Windows.Storage;
using Budggy;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BudggyUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BudggyHome : Page
    {
        Budget budget;
        public BudggyHome()
        {
            this.InitializeComponent();                       
            HomeCDP.Date = DateTime.Now;
            
           
           



        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (Budget)e.Parameter;
            budget = parameters;         
            budget.CreateMonthlyBudget();
            BudgetBalRP.DataContext = budget.MonthlyBudgets[0];
            this.DataContext = budget;            
            BinsCB.ItemsSource = budget.Bins;
            BinsCB.SelectedIndex = 0;
            IncLB.ItemsSource = budget.transactions;
            ExpLB.ItemsSource = budget.transactions;
            BinLB.ItemsSource = budget.Bins;
            repeatedTransLB.ItemsSource = budget.repeatedTrans;
            IncExpGridBD.Visibility = Visibility.Collapsed;
            budget.Refresh();
        }

        private void ValueTB_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            
            int index = ValueTB.SelectionStart;
            int index2;
            try
            {
                char str = ValueTB.Text[index - 1];
                if (str != '.' && str != ',' && !Char.IsControl(str) && !Char.IsNumber(str))
                {
                    index2 = ValueTB.Text.IndexOf(str);

                    if (index2 >= 0)
                    {
                        ValueTB.Text = ValueTB.Text.Remove(index2, 1);
                        ValueTB.SelectionStart = index - 1;
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
                    if (ValueTB.Text.IndexOf('.') != ValueTB.Text.LastIndexOf('.'))
                    {

                        ValueTB.Text = ValueTB.Text.Remove(ValueTB.Text.LastIndexOf('.'), 1);
                        ValueTB.SelectionStart = index - 1;
                    }
                }
               
            }
            catch
            {
                return;
            }
            
        }

        private void ValueTB_KeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

      
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int index = HomeCDP.Date.ToString().IndexOf(' ');
            string datestr = HomeCDP.Date.ToString().Remove(index);
            string[] datearr = datestr.Split('/');
            string binName;            
            Drawer drawer = DrawersCB.SelectedItem as Drawer;
            Goal goal = DrawersCB.SelectedItem as Goal;
            string drawerName = (drawer != null) ? drawer?.Name : goal?.Name;
            



            if (SplitTSW.Visibility == Visibility.Visible)
            {
                if (SplitTSW.IsOn == true)
                {
                    budget.AddIncome(Convert.ToDecimal(ValueTB.Text), DescriptionTB.Text,
                        new DateTime(Convert.ToInt16(datearr[2]), Convert.ToInt16(datearr[0]), Convert.ToInt16(datearr[1])), "Split", true);
                }
                else
                {
                    binName = budget.Bins[BinsCB.SelectedIndex].Name;
                    budget.AddIncome(Convert.ToDecimal(ValueTB.Text), DescriptionTB.Text,
                      new DateTime(Convert.ToInt16(datearr[2]), Convert.ToInt16(datearr[0]), Convert.ToInt16(datearr[1])), 
                      budget.Bins[BinsCB.SelectedIndex].Name, false);
                }
            } else
            {
                binName = budget.Bins[BinsCB.SelectedIndex].Name;
                budget.AddExpense(Convert.ToDecimal(ValueTB.Text), DescriptionTB.Text,
                      new DateTime(Convert.ToInt16(datearr[2]), Convert.ToInt16(datearr[0]), Convert.ToInt16(datearr[1])),
                      budget.Bins[BinsCB.SelectedIndex].Name, DrawGoalTSW.IsOn, drawerName);
            }

            // clear values
            DescriptionTB.Text = "";
            ValueTB.Text = "";
            BudgetsignTB.Text = (budget.MonthlyBudgets[budget.MonthlyBudgets.Count - 1].Value < 0) ? "-" : "+";
        }
           

        string AddPeriod(string str)
        {
            string newStr = str;
            int index = newStr.IndexOf('.');

            if (index >= 0)
            {
                newStr = newStr.Remove(index);

            }

            newStr = newStr.Insert(newStr.Length - 2, ".");


            return newStr;
        }

     /*   private void IncExpCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IncExpCB.SelectedIndex == 0)
            {
                //SplitTSW.IsEnabled = true;              
              //  DrawersCB.Visibility = Visibility.Collapsed;
            }
            else
            { 
              //  SplitTSW.IsEnabled = false;
             //   DrawersCB.Visibility = Visibility.Visible;
            }
        } */

        private void IncDeleteBt_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            if(fe != null)
            {
                //So since the data context is the specific item in the list, the listbox item and the button's data
                // context are the same, so you can set that item as the selected item!!
                IncLB.SelectedItem = fe.DataContext;
                
            }
           
            int index = IncLB.SelectedIndex;
            if(budget.transactions[index].Value > 0m)
            {
               budget.DeleteIncome(budget.transactions[index].Value, budget.transactions[index].Description,
               budget.transactions[index].Date, budget.transactions[index].Bin);
            } else
            {
                budget.DeleteExpense(budget.transactions[index].Value, budget.transactions[index].Description,
               budget.transactions[index].Date, budget.transactions[index].Bin);
            }
           
            
        }

        private void ExpDeleteBt_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            if (fe != null)
            {
                //So since the data context is the specific item in the list, the listbox item and the button's data
                // context are the same, so you can set that item as the selected item!!
                ExpLB.SelectedItem = fe.DataContext;

            }
            int index = ExpLB.SelectedIndex;
            if (budget.transactions[index].Value > 0m)
            {
                budget.DeleteIncome(budget.transactions[index].Value, budget.transactions[index].Description,
                budget.transactions[index].Date, budget.transactions[index].Bin);
            }
            else
            {
                budget.DeleteExpense(budget.transactions[index].Value, budget.transactions[index].Description,
               budget.transactions[index].Date, budget.transactions[index].Bin);
            }
        }

        private void BinsCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DrawGoalTSW.IsOn)
            {
                DrawersCB.ItemsSource = budget.Bins[BinsCB.SelectedIndex].Drawers;
            } else
            {
                DrawersCB.ItemsSource = budget.Bins[BinsCB.SelectedIndex].Goals;
            }
           
        }

        private void ToggleIncBT_Click(object sender, RoutedEventArgs e)
        {
            BackBTBD.Visibility = Visibility.Visible;
            IncExpGridBD.Visibility = Visibility.Visible;
            SplitTSW.Visibility = Visibility.Visible;
            DrawersCB.Visibility = Visibility.Collapsed;
            ToggleIncBTBD.Visibility = Visibility.Collapsed;
            ToggleExpBTBD.Visibility = Visibility.Collapsed;
            DrawGoalTSW.Visibility = Visibility.Collapsed;            
            BinsCB.IsEnabled = (SplitTSW.IsOn) ? false : true;            
        }

        private void ToggleExpBT_Click(object sender, RoutedEventArgs e)
        {
            BackBTBD.Visibility = Visibility.Visible;
            IncExpGridBD.Visibility = Visibility.Visible;
            SplitTSW.Visibility = Visibility.Collapsed; 
            DrawersCB.Visibility = Visibility.Visible;
            DrawGoalTSW.Visibility = Visibility.Visible;
            ToggleIncBTBD.Visibility = Visibility.Collapsed;
            ToggleExpBTBD.Visibility = Visibility.Collapsed;
            BinsCB.IsEnabled = true;
        }

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            ToggleIncBTBD.Visibility = Visibility.Visible;
            ToggleExpBTBD.Visibility = Visibility.Visible;
            IncExpGridBD.Visibility = Visibility.Collapsed;
            BackBTBD.Visibility = Visibility.Collapsed;

        }

        private void RelativePanel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            RelativePanel relative = sender as RelativePanel;
            Transaction trans = (Transaction)relative.DataContext;
            budget.AddRepeatTransaction(trans);
        }

        private void IncfreqBt_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Transaction trans = (Transaction)button.DataContext;
            budget.AddRepeatTransaction(trans);
        }  

        private void RepeatDeleteBt_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            RepeatTransaction repTrans = (RepeatTransaction)button.DataContext;
            budget.RemoveRepeatTransaction(repTrans);
        }

        private void DrawGoalTSW_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {           
                if (DrawGoalTSW.IsOn)
                 {
                DrawersCB.ItemsSource = budget.Bins[BinsCB.SelectedIndex].Drawers;
                 }
                else
                {
                DrawersCB.ItemsSource = budget.Bins[BinsCB.SelectedIndex].Goals;
                 }
            }
            catch
            {

            }

        }
        

        private void DrawersCB_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.SelectedIndex = -1;
        }

        private void SplitTSW_Toggled(object sender, RoutedEventArgs e)
        {
            BinsCB.IsEnabled = (SplitTSW.IsOn) ? false: true; 
        }

        private void BinLB_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            try {
                Frame contentFrame = this.Parent as Frame;
                //TitleTB.Text = "Bins";
                contentFrame.Navigate(typeof(BinsHome), budget);
                Border border = contentFrame.Parent as Border;
                Grid grid = border.Parent as Grid;
                
                //BinLB.SelectedIndex 
            }
            catch {

            }
              
        }

        private void BinRemoveBT_Click(object sender, RoutedEventArgs e)
        {

        }
    }


}

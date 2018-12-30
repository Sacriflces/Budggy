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
    public sealed partial class BudggyHome : Page
    {
        BudgetViewModel budget;
        public BudggyHome()
        {
            this.InitializeComponent();                       
            HomeCDP.Date = DateTime.Now;
           
          



        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (BudgetViewModel)e.Parameter;
            budget = parameters;
            this.DataContext = budget;            
            BinsCB.ItemsSource = budget.Budggy.Bins;
            IncLB.ItemsSource = budget.Budggy.Incs;
            ExpLB.ItemsSource = budget.Budggy.Exps;
            BudgetBalRP.DataContext = budget.Budggy.MonthlyBudgets[budget.Budggy.MonthlyBudgets.Count - 1];




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
            if(IncExpCB.SelectedIndex == 0)
            {
                if (SplitTSW.IsOn == true)
                {
                    budget.Budggy.AddIncome(Convert.ToDouble(ValueTB.Text), DescriptionTB.Text,
                        new DateTime(Convert.ToInt16(datearr[2]), Convert.ToInt16(datearr[0]), Convert.ToInt16(datearr[1])), "Split");
                }
                else
                {
                    binName = budget.Budggy.Bins[BinsCB.SelectedIndex].Name;
                    budget.Budggy.AddIncome(Convert.ToDouble(ValueTB.Text), DescriptionTB.Text,
                      new DateTime(Convert.ToInt16(datearr[2]), Convert.ToInt16(datearr[0]), Convert.ToInt16(datearr[1])), 
                      budget.Budggy.Bins[BinsCB.SelectedIndex].Name);
                }
            } else
            {
                binName = budget.Budggy.Bins[BinsCB.SelectedIndex].Name;
                budget.Budggy.AddExpense(Convert.ToDouble(ValueTB.Text), DescriptionTB.Text,
                      new DateTime(Convert.ToInt16(datearr[2]), Convert.ToInt16(datearr[0]), Convert.ToInt16(datearr[1])),
                      budget.Budggy.Bins[BinsCB.SelectedIndex].Name);
            }

            // clear values
            DescriptionTB.Text = "";
            ValueTB.Text = "";
            BudgetsignTB.Text = (budget.Budggy.MonthlyBudgets[budget.Budggy.MonthlyBudgets.Count - 1].Value < 0) ? "-" : "+";
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

        private void IncExpCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IncExpCB.SelectedIndex == 0)
            {
                SplitTSW.IsEnabled = true;
            } else
            { 
                SplitTSW.IsEnabled = false;
            }
        }

        private void IncDeleteBt_Click(object sender, RoutedEventArgs e)
        {
           
            
            int index = IncLB.SelectedIndex;
            budget.Budggy.DeleteIncome(budget.Budggy.Incs[index].Value, budget.Budggy.Incs[index].Description,
                budget.Budggy.Incs[index].Date, budget.Budggy.Incs[index].Bin);
            
        }

        private void ExpDeleteBt_Click(object sender, RoutedEventArgs e)
        {
            int index = ExpLB.SelectedIndex;
            budget.Budggy.DeleteExpense(budget.Budggy.Exps[index].Value, budget.Budggy.Exps[index].Description,
                budget.Budggy.Exps[index].Date, budget.Budggy.Exps[index].Bin);
        }

        
    }


}

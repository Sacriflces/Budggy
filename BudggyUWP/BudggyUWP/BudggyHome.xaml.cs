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
        BudgetViewModel budget = new BudgetViewModel();
        public BudggyHome()
        {
            this.InitializeComponent();
            
          
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (BudgetViewModel)e.Parameter;
            budget = parameters;
            this.DataContext = budget;            
            BinsCB.ItemsSource = budget.Budggy.Bins;



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
           // budget.AddIncome
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

    }


}

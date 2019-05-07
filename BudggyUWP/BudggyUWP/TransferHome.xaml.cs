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
    public sealed partial class TransferHome : Page
    {
        Budget budget;
        public TransferHome()
        {
            this.InitializeComponent();
            THomeCDP.Date = DateTime.Now;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (Budget)e.Parameter;
            budget = parameters;
            this.DataContext = budget;
            FromBinsCB.ItemsSource = budget.Bins;
            ToBinsCB.ItemsSource = budget.Bins;
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            int index = THomeCDP.Date.ToString().IndexOf(' ');
            string datestr = THomeCDP.Date.ToString().Remove(index);
            string[] datearr = datestr.Split('/');
            try
            {
                budget.TransferFunds(budget.Bins[ToBinsCB.SelectedIndex].Name, budget.Bins[FromBinsCB.SelectedIndex].Name, Convert.ToDecimal(ValueTB.Text),
                     new DateTime(Convert.ToInt16(datearr[2]), Convert.ToInt16(datearr[0]), Convert.ToInt16(datearr[1])));
                ValueTB.Text = "";
            }
            catch
            {
                return;
            }
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
    }
}

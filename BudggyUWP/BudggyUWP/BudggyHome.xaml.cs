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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BudggyUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BudggyHome : Page
    {
        public BudggyHome()
        {
            this.InitializeComponent();
        }              

        private void ValueTB_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            int index = ValueTB.SelectionStart;
            if (Convert.ToInt16(e.Key) != '.' && !(Convert.ToInt16(e.Key) <= 40 && Convert.ToInt16(e.Key) >= 37)  &&
                !Char.IsControl(Convert.ToChar(e.Key))  && !Char.IsNumber(Convert.ToChar(e.Key)))
            {
               // ValueTB.Text = Convert.ToChar(e.Key).ToString().ToLower(); //String.Concat(e.Key.ToString(),Convert.ToInt16(e.Key));

                ValueTB.Text = ValueTB.Text.Remove(ValueTB.Text.IndexOf(e.Key.ToString().ToLower()),1);
                //ValueTB.SelectionStart = ValueTB.Text.Length;
                ValueTB.SelectionStart = index - 1;
                


            }
            /*
            while(ValueTB.Text.Length < 2)
            {
                ValueTB.Text = ValueTB.Text.Insert(0, "0");
            }

            ValueTB.Text = AddPeriod(ValueTB.Text); */
            
            
            
            else if(Convert.ToInt16(e.Key) == 190)           
                
            {               
                if (ValueTB.Text.IndexOf('.') != ValueTB.Text.LastIndexOf('.'))
                {
                    
                    ValueTB.Text = ValueTB.Text.Remove(ValueTB.Text.LastIndexOf('.'), 1);
                    ValueTB.SelectionStart = ValueTB.Text.Length;
                } 
            }

            // ValueTB.Text = Convert.ToInt16(e.Key).ToString();
            //ValueTB.Text = Convert.ToChar(e.Key).ToString();
        }

        private void ValueTB_KeyDown(object sender, KeyRoutedEventArgs e)
        {

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

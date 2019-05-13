using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace FloatConverterUWP
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            radioButtonSingle.IsChecked = true;
        }

        // ラジオボタンクリック（サイズ変更）
        private void radioButtonChecked(object sender, RoutedEventArgs e)
        {
            SizeChange();
        }
        // サイズ変更
        private void SizeChange()
        {
            if (radioButtonSingle.IsChecked == true)
            {
                textBlockBinExponent.Text = "Exponent (8bits)";
                textBlockBinFraction.Text = "Fraction (23bits)";
            }
            else
            {
                textBlockBinExponent.Text = "Exponent (11bits)";
                textBlockBinFraction.Text = "Fraction (52bits)";
            }
            //TryDecimalParse(true);
        }
        // Enterによるサイズ切替
        private void radioButton_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                RadioButton radioButton = sender as RadioButton;
                if ((radioButton == radioButtonSingle) && (radioButtonSingle.IsChecked == true)) return;
                if ((radioButton == radioButtonDouble) && (radioButtonDouble.IsChecked == true)) return;
                radioButton.IsChecked = true;
                SizeChange();
            }
        }
    }
}

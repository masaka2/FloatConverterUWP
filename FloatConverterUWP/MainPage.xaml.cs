using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI;
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
        }
        // 画面初期化
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            radioButtonSingle.IsChecked = true;
            //textBoxDecimal.Focus(FocusState.Pointer);
            textBoxDecimal.Focus(FocusState.Keyboard);
        }
        // メッセージ表示
        private void DispMsg(string msg)
        {
//            textBoxMsg.Foreground = Brushes.Black;
//            textBoxMsg.Text = msg;
        }
        // エラーメッセージ表示
        private void ErrMsg(string msg)
        {
//            textBoxMsg.Foreground = Brushes.Red;
//            textBoxMsg.Text = msg;
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
        // 十進数解析（再表示）
        private bool TryDecimalParse(bool bSizeChange)
        {
            if (radioButtonSingle.IsChecked == true)
            {
                float f;
                if (float.TryParse(textBoxDecimal.Text, out f))
                {
                    textBoxDecimal.Text = f.ToString();
                    byte[] byteArray = BitConverter.GetBytes(f);
                    UInt32 ui32 = BitConverter.ToUInt32(byteArray, 0);
                    textBoxHexadecimal.Text = ui32.ToString("X8");
                    PrintBinaryFloat(ui32);
                    return (true);
                }
            }
            else
            {
                double d;
                if (double.TryParse(textBoxDecimal.Text, out d))
                {
                    textBoxDecimal.Text = d.ToString();
                    byte[] byteArray = BitConverter.GetBytes(d);
                    UInt64 ui64 = BitConverter.ToUInt64(byteArray, 0);
                    textBoxHexadecimal.Text = ui64.ToString("X16");
                    PrintBinaryDouble(byteArray);
                    return (true);
                }
            }
            if (bSizeChange && (textBoxDecimal.Text.Length == 0)) return (true);
            textBoxDecimal.Foreground = new SolidColorBrush(Colors.Red);
            ErrMsg((textBoxDecimal.Text.Length == 0) ? "未入力です。" : "浮動小数点数として誤りがあります。");
            return (false);
        }
        // １６進数解析（再表示）
        private bool TryHexadecimalParse()
        {
            string str = textBoxHexadecimal.Text.Replace(" ", "");
            if (str.Length < 1)
            {
                ErrMsg("未入力です。");
                return (false);
            }
            if ((radioButtonSingle.IsChecked == true) && (str.Length > 8))
            {
                ErrMsg("８桁以内で入力してください。");
                return (false);
            }
            if (str.Length > 16)
            {
                ErrMsg("１６桁以内で入力してください。");
                return (false);
            }
            if (radioButtonSingle.IsChecked == true)
            {
                UInt32 ui32 = Convert.ToUInt32(str, 16);
                textBoxHexadecimal.Text = ui32.ToString("X8");
                byte[] byteArray = BitConverter.GetBytes(ui32);
                float f = BitConverter.ToSingle(byteArray, 0);
                textBoxDecimal.Text = f.ToString();
                PrintBinaryFloat(ui32);
            }
            else
            {
                UInt64 ui64 = Convert.ToUInt64(str, 16);
                textBoxHexadecimal.Text = ui64.ToString("X16");
                byte[] byteArray = BitConverter.GetBytes(ui64);
                double d = BitConverter.ToDouble(byteArray, 0);
                textBoxDecimal.Text = d.ToString();
                PrintBinaryDouble(byteArray);
            }
            return (true);
        }
        // バイナリ部分の表示（float）
        private void PrintBinaryFloat(UInt32 ui32)
        {
            textBoxBinSign.Text = ((ui32 & 0x80000000) == 0) ? "0" : "1";
            ui32 |= 0x80000000;
            string strBin = Convert.ToString(ui32, 2);
            textBoxBinExponent.Text = strBin.Substring(1, 8);
            textBoxBinFraction.Text = strBin.Substring(9);
        }
        // バイナリ部分の表示（double）
        private void PrintBinaryDouble(byte[] byteArray)
        {
            // string strBin = Convert.ToString(ui64, 2); ← これがエラーになる！
            string strBin =
                Convert.ToString((byteArray[7] + 0x100), 2).Substring(1) +
                Convert.ToString((byteArray[6] + 0x100), 2).Substring(1) +
                Convert.ToString((byteArray[5] + 0x100), 2).Substring(1) +
                Convert.ToString((byteArray[4] + 0x100), 2).Substring(1) +
                Convert.ToString((byteArray[3] + 0x100), 2).Substring(1) +
                Convert.ToString((byteArray[2] + 0x100), 2).Substring(1) +
                Convert.ToString((byteArray[1] + 0x100), 2).Substring(1) +
                Convert.ToString((byteArray[0] + 0x100), 2).Substring(1);
            textBoxBinSign.Text = strBin.Substring(0, 1);
            textBoxBinExponent.Text = strBin.Substring(1, 11);
            textBoxBinFraction.Text = strBin.Substring(12);
        }
        // 
        private void textBoxDecimal_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Enterは２回来る！
            if ((e.Key == VirtualKey.Enter) && (e.KeyStatus.RepeatCount == 1))
            {
                if (TryDecimalParse(false))
                {
                    textBoxHexadecimal.Focus(FocusState.Keyboard);
                    textBoxHexadecimal.Select(textBoxHexadecimal.Text.Length, 0);
                }
                else
                {
                    textBoxDecimal.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
        }

        private void textBoxHexadecimal_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Enterは２回来る！
            if (e.Key == VirtualKey.Enter)
            {
                if (e.KeyStatus.RepeatCount == 1)
                {
                    if (TryHexadecimalParse())
                    {
                        textBoxDecimal.Focus(FocusState.Keyboard);
                        textBoxDecimal.Select(textBoxDecimal.Text.Length, 0);
                    }
                    else
                    {
                        textBoxHexadecimal.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }
                return;
            }
            // １６進数（0-9,A-F,a-f）は許す。
            if (((e.Key >= VirtualKey.Number0) && (e.Key <= VirtualKey.Number9)) ||
                ((e.Key >= VirtualKey.NumberPad0) && (e.Key <= VirtualKey.NumberPad9)) ||
                ((e.Key >= VirtualKey.A) && (e.Key <= VirtualKey.F)))
            {
                return;
            }
            if (e.Key == VirtualKey.Tab) return;
            e.Handled = true;  // １６進数とタブ以外は無視！
        }

    }
}

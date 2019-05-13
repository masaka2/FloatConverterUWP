using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        }

        // ラジオボタンクリック（サイズ変更）
        // XAMLで初期値を設定すると起動直後にここに来てラベルの変更でエラーになります。
        private void radioButtonChecked(object sender, RoutedEventArgs e)
        {
//            SizeChange();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Hellow");
            await dialog.ShowAsync();
        }
    }
}

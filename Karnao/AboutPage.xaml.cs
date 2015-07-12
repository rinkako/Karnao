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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Karnao
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        string _alter = "【使用协议】" + Environment.NewLine +
                        "本协议是用户与作者间关于该软件的法律协议，一旦下载、复制、分享、利用" +
                        "或其他方式传播和使用本软件，即表示同意并接受本协议的所有约定，" +
                        "如果不接受本协议，那么用户没有使用本软件的权利。" + Environment.NewLine + Environment.NewLine +
                        "软件使用协议：" + Environment.NewLine +
                        "1、在使用本软件前请务必事先确认当前场合（如考试等）是否允许使用；" + Environment.NewLine +
                        "2、不允许对本程序进行反向工程（包括但不限于反编译、反汇编等）；" + Environment.NewLine +
                        "3、使用本软件造成的一切后果，均由软件使用者自己负全部责任；" + Environment.NewLine +
                        "4、软件中使用的图片素材版权归画师所有，不允许私自不标注出处地复制、截图、传播等；" + Environment.NewLine +
                        "5、作者对本软件保留最终解释权，并保留通过法律手段追究违反协议使用本软件者法律责任的权利。";
        string _right = "【版权信息】" + Environment.NewLine +
                        "程序：SYSU LOVELIVE" + Environment.NewLine +
                        "界面：凛佳 / 境界契约" + Environment.NewLine +
                        "版权绘：桐子 / 境界契约";

        public AboutPage()
        {
            this.InitializeComponent();

            infoImage.Width = 676;
            infoImage.Height = 1000;

            infoImage.Width *= MainPage.zoomConstant;
            infoImage.Height *= MainPage.zoomConstant;

            mainTextBlock.Text = _alter;
            mainTextBlock_Copy.Text = _right;

            inAnime.Begin();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // RETURN TO MAINPAGE
            Frame.Navigate(typeof(MainPage));
        }
    }
}
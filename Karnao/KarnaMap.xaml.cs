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
    public sealed partial class KarnaMap : Page
    {
        Service core = Service.get_Instance();
        public KarnaMap()
        {
            this.InitializeComponent();

            // 获得输出序列
            Queue<Karno> _out_buffer = core.获得输出卡诺序列();

            // 输出
            core.pout(_out_buffer.Dequeue(), resultBlock_J3);
            core.pout(_out_buffer.Dequeue(), resultBlock_J2);
            core.pout(_out_buffer.Dequeue(), resultBlock_J1);
            core.pout(_out_buffer.Dequeue(), resultBlock_J0);
            core.pout(_out_buffer.Dequeue(), resultBlock_K3);
            core.pout(_out_buffer.Dequeue(), resultBlock_K2);
            core.pout(_out_buffer.Dequeue(), resultBlock_K1);
            core.pout(_out_buffer.Dequeue(), resultBlock_K0);

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // BACK TO THE MAINPAGE
            Frame.Navigate(typeof(MainPage));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // RESET INTIME
            foreach (var t in intimeGrid.Children)
            {
                if (t is ComboBox)
                {
                    ((ComboBox)t).SelectedIndex = 2;
                }
            }
            intimeResultBox.Text = "Y = ";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // 录入卡诺图
            Karno _imedia = new Karno();
            int myx = 0, myy = 0;
            uint tempi = 0;

            foreach (var t in intimeGrid.Children)
            {
                if (t is ComboBox)
                {
                    if (((ComboBox)t).SelectedIndex == 2)
                    {
                        tempi = 8;
                    }
                    else
                    {
                        tempi = (uint)((ComboBox)t).SelectedIndex;
                    }
                    _imedia.table[myy, myx] = tempi;
                    myx++;
                    if (myx > 3)
                    {
                        myx = 0;
                        myy++;
                    }
                }
            }

            // 化简
            string _fres = _imedia.simplize();
            string[] _imename = new string[4];
            _imename[0] = "Q3";
            _imename[1] = "Q2";
            _imename[2] = "Q1";
            _imename[3] = "Q0";
            // 修改变量名
            for (int i = 0; i < 4; i++)
            {
                _fres = _fres.Replace("Q" + Convert.ToString(3 - i), _imename[i]);
            }
            // 打印
            intimeResultBox.Text = "Y = " + _fres;
        }

    }
}

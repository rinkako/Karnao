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
    public sealed partial class EditKarno : Page
    {
        Service core = Service.get_Instance();
        public EditKarno()
        {
            this.InitializeComponent();
            this.myEdit();
        }

        private void myEdit()
        {
            Queue<uint> qbuffer = core.读出次态表耶();
            // Q3
            foreach (var t in panelQ3.Children)
            {
                if (t is ComboBox)
                {
                    uint qb = qbuffer.Dequeue();
                    if (qb == 8)
                    {
                        ((ComboBox)t).SelectedIndex = 2;
                    }
                    else
                    {
                        ((ComboBox)t).SelectedIndex = (int)qb;
                    }
                }
            }
            // Q2
            foreach (var t in panelQ2.Children)
            {
                if (t is ComboBox)
                {
                    uint qb = qbuffer.Dequeue();
                    if (qb == 8)
                    {
                        ((ComboBox)t).SelectedIndex = 2;
                    }
                    else
                    {
                        ((ComboBox)t).SelectedIndex = (int)qb;
                    }
                }
            }
            // Q1
            foreach (var t in panelQ1.Children)
            {
                if (t is ComboBox)
                {
                    uint qb = qbuffer.Dequeue();
                    if (qb == 8)
                    {
                        ((ComboBox)t).SelectedIndex = 2;
                    }
                    else
                    {
                        ((ComboBox)t).SelectedIndex = (int)qb;
                    }
                }
            }
            // Q0
            foreach (var t in panelQ0.Children)
            {
                if (t is ComboBox)
                {
                    uint qb = qbuffer.Dequeue();
                    if (qb == 8)
                    {
                        ((ComboBox)t).SelectedIndex = 2;
                    }
                    else
                    {
                        ((ComboBox)t).SelectedIndex = (int)qb;
                    }
                }
            }

        }

        private void saveEdit()
        {
            Queue<uint> qbuffer = new Queue<uint>();

            foreach (var t in panelQ3.Children)
            {
                if (t is ComboBox)
                {
                    if (((ComboBox)t).SelectedIndex == 2)
                    {
                        qbuffer.Enqueue(8);
                    }
                    else
                    {
                        qbuffer.Enqueue((uint)(((ComboBox)t).SelectedIndex));
                    }
                }
            }
            foreach (var t in panelQ2.Children)
            {
                if (t is ComboBox)
                {
                    if (((ComboBox)t).SelectedIndex == 2)
                    {
                        qbuffer.Enqueue(8);
                    }
                    else
                    {
                        qbuffer.Enqueue((uint)(((ComboBox)t).SelectedIndex));
                    }
                }
            }
            foreach (var t in panelQ1.Children)
            {
                if (t is ComboBox)
                {
                    if (((ComboBox)t).SelectedIndex == 2)
                    {
                        qbuffer.Enqueue(8);
                    }
                    else
                    {
                        qbuffer.Enqueue((uint)(((ComboBox)t).SelectedIndex));
                    }
                }
            }
            foreach (var t in panelQ0.Children)
            {
                if (t is ComboBox)
                {
                    if (((ComboBox)t).SelectedIndex == 2)
                    {
                        qbuffer.Enqueue(8);
                    }
                    else
                    {
                        qbuffer.Enqueue((uint)(((ComboBox)t).SelectedIndex));
                    }
                }
            }

            core.录入次态表耶(qbuffer);

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // BACK
            Frame.Navigate(typeof(MainPage));
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            // APPBAR SAVE AND BACK
            this.saveEdit();
            Frame.Navigate(typeof(MainPage));
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            // UNDO
            this.myEdit();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // PUT X
            Panel workPanel;
            if (((Button)sender).Name == "B3")
            {
                workPanel = panelQ3;
            }
            else if (((Button)sender).Name == "B2")
            {
                workPanel = panelQ2;
            }
            else if (((Button)sender).Name == "B1")
            {
                workPanel = panelQ1;
            }
            else
            {
                workPanel = panelQ0;
            }

            foreach (var t in workPanel.Children)
            {
                if (t is ComboBox)
                {
                    ((ComboBox)t).SelectedIndex = 2;
                }
            }
        }

    }
}

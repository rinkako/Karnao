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
    public sealed partial class WithLoop : Page
    {
        Service core = Service.get_Instance();
        public WithLoop()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // BACK
            Frame.Navigate(typeof(MainPage));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (textBlockHint == null)
            {
                return;
            }
            textBlockHint.Visibility = Visibility.Visible;
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                textBlockHint.Text = "单向循环有限状态机（十进制输入，范围0-15）：";
                // 开1
                panel1.Visibility = Visibility.Visible;
                numOfStateHint1.Visibility = Visibility.Visible;
                comboBoxStateNum1.Visibility = Visibility.Visible;
                // 关2
                cirHint_Copy.Visibility = Visibility.Collapsed;
                comboBoxStateNum1_Copy.Visibility = Visibility.Collapsed;
                numOfStateHint1_Copy.Visibility = Visibility.Collapsed;
                panel1_Copy.Visibility = Visibility.Collapsed;
                // 关3
                comboBoxStateNum1_Copy1.Visibility = Visibility.Collapsed;
                numOfStateHint1_Copy1.Visibility = Visibility.Collapsed;
                panel1_Copy1.Visibility = Visibility.Collapsed;
                comboBoxStateNum1_Copy2.Visibility = Visibility.Collapsed;
                numOfStateHint1_Copy2.Visibility = Visibility.Collapsed;
                panel1_Copy2.Visibility = Visibility.Collapsed;
            }
            else if (((ComboBox)sender).SelectedIndex == 1)
            {
                textBlockHint.Text = "单环双向，Q3持续影响（十进制输入，范围0-7）：";
                // 关1
                panel1.Visibility = Visibility.Collapsed;
                numOfStateHint1.Visibility = Visibility.Collapsed;
                comboBoxStateNum1.Visibility = Visibility.Collapsed;
                // 开2
                cirHint_Copy.Visibility = Visibility.Visible;
                comboBoxStateNum1_Copy.Visibility = Visibility.Visible;
                numOfStateHint1_Copy.Visibility = Visibility.Visible;
                panel1_Copy.Visibility = Visibility.Visible;
                // 关3
                comboBoxStateNum1_Copy1.Visibility = Visibility.Collapsed;
                numOfStateHint1_Copy1.Visibility = Visibility.Collapsed;
                panel1_Copy1.Visibility = Visibility.Collapsed;
                comboBoxStateNum1_Copy2.Visibility = Visibility.Collapsed;
                numOfStateHint1_Copy2.Visibility = Visibility.Collapsed;
                panel1_Copy2.Visibility = Visibility.Collapsed;
            }
            else
            {
                textBlockHint.Text = "双环单向，Q3只影响入口值的后续走向（十进制输入，范围0-7）：";
                // 关1
                panel1.Visibility = Visibility.Collapsed;
                numOfStateHint1.Visibility = Visibility.Collapsed;
                comboBoxStateNum1.Visibility = Visibility.Collapsed;
                // 关2
                cirHint_Copy.Visibility = Visibility.Collapsed;
                comboBoxStateNum1_Copy.Visibility = Visibility.Collapsed;
                numOfStateHint1_Copy.Visibility = Visibility.Collapsed;
                panel1_Copy.Visibility = Visibility.Collapsed;
                // 开3
                comboBoxStateNum1_Copy1.Visibility = Visibility.Visible;
                numOfStateHint1_Copy1.Visibility = Visibility.Visible;
                panel1_Copy1.Visibility = Visibility.Visible;
                comboBoxStateNum1_Copy2.Visibility = Visibility.Visible;
                numOfStateHint1_Copy2.Visibility = Visibility.Visible;
                panel1_Copy2.Visibility = Visibility.Visible;
            }
            sureButton.Visibility = Visibility.Visible;
        }

        private void comboBoxStateNum1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (panel1 == null) return;
            foreach (var t in panel1.Children)
            {
                if (t is TextBox)
                {
                    ((TextBox)t).Visibility = Visibility.Visible;
                }
            }
            int dvar = 0, evar = ((ComboBox)sender).SelectedIndex + 1;
            foreach (var t in panel1.Children)
            {
                if (t is TextBox)
                {
                    if (dvar > evar)
                    {
                        ((TextBox)t).Visibility = Visibility.Collapsed;
                    }
                    dvar++;
                }
            }
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            // SAVE AND EXIT
            sureButton_Click(null, null);
        }

        private void comboBoxStateNum1_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (panel1_Copy == null) return;
            foreach (var t in panel1_Copy.Children)
            {
                if (t is TextBox)
                {
                    ((TextBox)t).Visibility = Visibility.Visible;
                }
            }
            int dvar = 0, evar = ((ComboBox)sender).SelectedIndex + 1;
            foreach (var t in panel1_Copy.Children)
            {
                if (t is TextBox)
                {
                    if (dvar > evar)
                    {
                        ((TextBox)t).Visibility = Visibility.Collapsed;
                    }
                    dvar++;
                }
            }
        }

        private void comboBoxStateNum1_Copy1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (panel1_Copy1 == null) return;
            foreach (var t in panel1_Copy1.Children)
            {
                if (t is TextBox)
                {
                    ((TextBox)t).Visibility = Visibility.Visible;
                }
            }
            int dvar = 0, evar = ((ComboBox)sender).SelectedIndex + 1;
            foreach (var t in panel1_Copy1.Children)
            {
                if (t is TextBox)
                {
                    if (dvar > evar)
                    {
                        ((TextBox)t).Visibility = Visibility.Collapsed;
                    }
                    dvar++;
                }
            }
        }

        private void comboBoxStateNum1_Copy2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (panel1_Copy2 == null) return;
            foreach (var t in panel1_Copy2.Children)
            {
                if (t is TextBox)
                {
                    ((TextBox)t).Visibility = Visibility.Visible;
                }
            }
            int dvar = 0, evar = ((ComboBox)sender).SelectedIndex + 1;
            foreach (var t in panel1_Copy2.Children)
            {
                if (t is TextBox)
                {
                    if (dvar > evar)
                    {
                        ((TextBox)t).Visibility = Visibility.Collapsed;
                    }
                    dvar++;
                }
            }
        }

        private async void sureButton_Click(object sender, RoutedEventArgs e)
        {
            uint tempValue;
            if (stateCombo.SelectedIndex == 0)
            {
                Queue<Fbit> _qbuffer = new Queue<Fbit>();
                foreach (var t in panel1.Children)
                {
                    if (t is TextBox && t.Visibility == Visibility.Visible)
                    {
                        if (((TextBox)t).Text == "")
                        {
                            var alter = new Windows.UI.Popups.MessageDialog("请完整填写！", "提示");
                            await alter.ShowAsync();
                            return;
                        }
                        else
                        {
                            tempValue = Convert.ToUInt32(((TextBox)t).Text);
                            if (tempValue < 0 || tempValue > 15)
                            {
                                var alter = new Windows.UI.Popups.MessageDialog("值错误，请确认数字在约定范围内！", "提示");
                                await alter.ShowAsync();
                                return;
                            }
                            _qbuffer.Enqueue(core.十转二(tempValue));
                        }
                    }
                }
                core.初始化次态表();
                core.单环写表(_qbuffer);
            }
            else if (stateCombo.SelectedIndex == 1)
            {
                foreach (var t in panel1_Copy.Children)
                {
                    if (t is TextBox && t.Visibility == Visibility.Visible)
                    {
                        if (((TextBox)t).Text == "")
                        {
                            var alter = new Windows.UI.Popups.MessageDialog("请完整填写！", "提示");
                            await alter.ShowAsync();
                            return;
                        }
                    }
                }
                Queue<Fbit> _qbuffer = new Queue<Fbit>();
                Stack<Fbit> _stackbuffer = new Stack<Fbit>();
                core.初始化次态表();
                foreach (var t in panel1_Copy.Children)
                {
                    if (t is TextBox && t.Visibility == Visibility.Visible)
                    {
                        tempValue = Convert.ToUInt32(((TextBox)t).Text);
                        if (tempValue < 0 || tempValue > 7)
                        {
                            var alter = new Windows.UI.Popups.MessageDialog("值错误，请确认数字在约定范围内！", "提示");
                            await alter.ShowAsync();
                            return;
                        }
                        _qbuffer.Enqueue(core.十转二(tempValue));
                        _stackbuffer.Push(core.十转二(tempValue));
                    }
                }
                core.单环写表(_qbuffer);
                _qbuffer.Clear();
                while (_stackbuffer.Count != 0)
                {
                    _qbuffer.Enqueue(_stackbuffer.Pop());
                }
                core.反向写表(_qbuffer);
            }
            else 
            {
                if (comboBoxStateNum1_Copy1.SelectedIndex + comboBoxStateNum1_Copy2.SelectedIndex + 3 > 8)
                {
                    var alter = new Windows.UI.Popups.MessageDialog("不允许一共超过8个状态！", "提示");
                    await alter.ShowAsync();
                    return;
                }

                Queue<Fbit> _qbuffer = new Queue<Fbit>();

                foreach (var t in panel1_Copy1.Children)
                {
                    if (t is TextBox && t.Visibility == Visibility.Visible)
                    {
                        if (((TextBox)t).Text == "")
                        {
                            var alter = new Windows.UI.Popups.MessageDialog("请完整填写！", "提示");
                            await alter.ShowAsync();
                            return;
                        }
                    }
                }
                foreach (var t in panel1_Copy2.Children)
                {
                    if (t is TextBox && t.Visibility == Visibility.Visible)
                    {
                        if (((TextBox)t).Text == "")
                        {
                            var alter = new Windows.UI.Popups.MessageDialog("请完整填写！", "提示");
                            await alter.ShowAsync();
                            return;
                        }
                    }
                }
                core.初始化次态表();
                foreach (var t in panel1_Copy1.Children)
                {
                    if (t is TextBox && t.Visibility == Visibility.Visible)
                    {
                        tempValue = Convert.ToUInt32(((TextBox)t).Text);
                        if (tempValue < 0 || tempValue > 7)
                        {
                            var alter = new Windows.UI.Popups.MessageDialog("值错误，请确认数字在约定范围内！", "提示");
                            await alter.ShowAsync();
                            return;
                        }
                        _qbuffer.Enqueue(core.十转二(tempValue));
                    }
                }
                core.单环写表(_qbuffer);
                _qbuffer.Clear();
                bool firflag = false;
                foreach (var t in panel1_Copy1.Children)
                {
                    if (t is TextBox && t.Visibility == Visibility.Visible)
                    {
                        if (firflag == false)
                        {
                            firflag = true;
                            continue;
                        }
                        else
                        {
                            tempValue = Convert.ToUInt32(((TextBox)t).Text);
                            if (tempValue < 0 || tempValue > 7)
                            {
                                var alter = new Windows.UI.Popups.MessageDialog("值错误，请确认数字在约定范围内！", "提示");
                                await alter.ShowAsync();
                                return;
                            }
                            _qbuffer.Enqueue(core.十转二(tempValue));
                        }
                    }
                }
                core.二环写表(_qbuffer, core.十转二(Convert.ToUInt32(firBox1.Text)));
                _qbuffer.Clear();
                foreach (var t in panel1_Copy2.Children)
                {
                    if (t is TextBox && t.Visibility == Visibility.Visible)
                    {
                        tempValue = Convert.ToUInt32(((TextBox)t).Text);
                        if (tempValue < 0 || tempValue > 7)
                        {
                            var alter = new Windows.UI.Popups.MessageDialog("值错误，请确认数字在约定范围内！", "提示");
                            await alter.ShowAsync();
                            return;
                        }
                        _qbuffer.Enqueue(core.十转二(tempValue));
                    }
                }
                core.二环一(_qbuffer);
                _qbuffer.Clear();
                firflag = false;
                foreach (var t in panel1_Copy2.Children)
                {
                    if (t is TextBox && t.Visibility == Visibility.Visible)
                    {
                        if (firflag == false)
                        {
                            firflag = true;
                            continue;
                        }
                        else
                        {
                            tempValue = Convert.ToUInt32(((TextBox)t).Text);
                            if (tempValue < 0 || tempValue > 7)
                            {
                                var alter = new Windows.UI.Popups.MessageDialog("值错误，请确认数字在约定范围内！", "提示");
                                await alter.ShowAsync();
                                return;
                            }
                            _qbuffer.Enqueue(core.十转二(tempValue));
                        }
                    }
                }
                core.二环一二环(_qbuffer, core.十转二(Convert.ToUInt32(firBox2.Text)));
                core.填控制变量无关();
            }

            // RETURN TO THE MAINPAGE
            Frame.Navigate(typeof(MainPage));
        }
    }
}

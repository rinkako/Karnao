using System;
using Karnao.ViewModel;
using Windows.UI.Xaml;
using Windows.ApplicationModel.DataTransfer;
using System.Collections.Generic;

namespace Karnao
{
    public sealed partial class MainPage
    {
        /// <summary>
        /// Gets the view's ViewModel.
        /// </summary>

        double screenW;
        double screenH;
        public static double zoomConstant;

        public static string NameQ3 = "Q3";
        public static string NameQ2 = "Q2";
        public static string NameQ1 = "Q1";
        public static string NameQ0 = "Q0";

        Service core = Service.get_Instance();

        public MainViewModel Vm
        {
            get
            {
                return (MainViewModel)DataContext;
            }
        }

        public MainPage()
        {
            InitializeComponent();

            // 调整窗口尺寸
            myWindowSize();

            inAnime.Begin();
            // 刷新
            refresh_nstable();
            mainPageSimplizer();
            NQ3.Text = NameQ3;
            NQ2.Text = NameQ2;
            NQ1.Text = NameQ1;
            NQ0.Text = NameQ0;
        }

        private void myWindowSize()
        {
            screenH = Window.Current.Bounds.Height;
            screenW = Window.Current.Bounds.Width;

            bottomRectangle.Height = 88;
            bottomRectangle.Width = screenW;
            bottonCopy.Margin = new Thickness(10);

            zoomConstant = screenW / 1920.0;

            mainButtonG1.Width *= zoomConstant;
            mainButtonG1.Height *= zoomConstant;
            mainButtonG2.Width *= zoomConstant;
            mainButtonG2.Height *= zoomConstant;
            mainButtonG3.Width *= zoomConstant;
            mainButtonG3.Height *= zoomConstant;
            mainButtonG4.Width *= zoomConstant;
            mainButtonG4.Height *= zoomConstant;
            mainButtonG1.Margin = new Thickness(118, 37, 0, 0);
            mainButtonG2.Margin = new Thickness(118 + 10 + mainButtonG1.Width, 37, 0, 0);
            mainButtonG3.Margin = new Thickness(118, 37 + 10 + mainButtonG1.Height, 0, 0);
            mainButtonG4.Margin = new Thickness(118 + 10 + mainButtonG1.Width, 37 + 10 + mainButtonG2.Height, 0, 0);

        }

        protected override void LoadState(object state)
        {
            var casted = state as MainPageState;

            if (casted != null)
            {
                Vm.Load(casted.LastVisit);
            }
        }

        protected override object SaveState()
        {
            return new MainPageState
            {
                LastVisit = DateTime.Now
            };
        }

        private async void bottonCopy_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            string resultStr = "";
            DataPackage myData = new DataPackage();
            resultStr += Jst.Text;
            resultStr += Environment.NewLine;
            resultStr += Kst.Text;
            myData.SetText(resultStr);
            Clipboard.SetContent(myData);
            var msg = new Windows.UI.Popups.MessageDialog("结果已成功复制到剪贴板！");
            await msg.ShowAsync();
        }

        private void mainButtonG1_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ((Windows.UI.Xaml.Controls.Image)sender).Opacity = 1;
        }

        private void mainButtonG1_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ((Windows.UI.Xaml.Controls.Image)sender).Opacity = 0.7;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // ABOUT
            Frame.Navigate(typeof(AboutPage));
        }

        private void mainButtonG4_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // 清空数据
            this.core.reuse();
            this.refresh_nstable();
            this.mainPageSimplizer();
        }

        private void refresh_nstable()
        {
            // 取得
            Queue<uint> qbuffer = core.获得次态表耶();
            // 打印
            {
                resultBlock.Text = "";
                resultBlock.Text += NameQ3 + "   " + NameQ2 + "   " + NameQ1 + "   " + NameQ0 + Environment.NewLine;
                uint _nwr = 0;
                while (qbuffer.Count != 0)
                {
                    if (_nwr >= 4)
                    {
                        _nwr = 0;
                        resultBlock.Text += Environment.NewLine;
                    }
                    string tstr = Convert.ToString(qbuffer.Dequeue());
                    if (tstr == "8")
                    {
                        tstr = "X";
                    }
                    _nwr++;
                    if (_nwr == 4)
                    {
                        resultBlock.Text += tstr;
                    }
                    else
                    {
                        resultBlock.Text += tstr + "      ";
                    }
                }
            }
        }

        string[] oldName = new string[4] {"Q3", "Q2", "Q1", "Q0"};

        private void mainPageSimplizer()
        {
            core.homura();
            Queue<Karno> _obuffer = core.获得输出卡诺序列();
            if (_obuffer != null && _obuffer.Count != 0)
            {
                string Jtemp = "", Ktemp = "";
                Jtemp += "J1 = " + _obuffer.Dequeue().simplize() +
                    "     J2 = " + _obuffer.Dequeue().simplize() +
                    "     J3 = " + _obuffer.Dequeue().simplize() +
                    "     J4 = " + _obuffer.Dequeue().simplize();
                Ktemp += "K1 = " + _obuffer.Dequeue().simplize() +
                    "     K2 = " + _obuffer.Dequeue().simplize() +
                    "     K3 = " + _obuffer.Dequeue().simplize() +
                    "     K4 = " + _obuffer.Dequeue().simplize();

                Jtemp = Jtemp.Replace(oldName[0], NameQ3);
                Jtemp = Jtemp.Replace(oldName[1], NameQ2);
                Jtemp = Jtemp.Replace(oldName[2], NameQ1);
                Jtemp = Jtemp.Replace(oldName[3], NameQ0);
                Ktemp = Ktemp.Replace(oldName[0], NameQ3);
                Ktemp = Ktemp.Replace(oldName[1], NameQ2);
                Ktemp = Ktemp.Replace(oldName[2], NameQ1);
                Ktemp = Ktemp.Replace(oldName[3], NameQ0);

                Jst.Text = Jtemp;
                Kst.Text = Ktemp;
            }
        }

        private void mainButtonG1_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // EDIT NEXT STATE TABLE
            Frame.Navigate(typeof(EditKarno));
        }

        private void mainButtonG3_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(WithLoop), new WithLoop());
        }

        private void NQ_TextChanged(object sender, Windows.UI.Xaml.Controls.TextChangedEventArgs e)
        {
            oldName[0] = NameQ3;
            oldName[1] = NameQ2;
            oldName[2] = NameQ1;
            oldName[3] = NameQ1;
            NameQ3 = NQ3.Text != "" ? NQ3.Text : "Q3";
            NameQ2 = NQ2.Text != "" ? NQ2.Text : "Q2";
            NameQ1 = NQ1.Text != "" ? NQ1.Text : "Q1";
            NameQ0 = NQ0.Text != "" ? NQ0.Text : "Q0";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 刷新
            refresh_nstable();
            mainPageSimplizer();
        }

        private void AppBar_Unloaded(object sender, RoutedEventArgs e)
        {
            NQ3.Text = NameQ3;
            NQ2.Text = NameQ2;
            NQ1.Text = NameQ1;
            NQ0.Text = NameQ0;
        }

        private void mainButtonG2_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(KarnaMap));
        }

    }

    public class MainPageState
    {
        public DateTime LastVisit
        {
            get;
            set;
        }
    }
}
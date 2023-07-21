using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ma.Terminal.SelfService.Controls
{
    /// <summary>
    /// FlowControl.xaml 的交互逻辑
    /// </summary>
    public partial class FlowControl : UserControl
    {
        const string ONFIRM_INFO_PATH = @"pack://SiteOfOrigin:,,,/Resource/Image/ConfirmInfo.png";
        const string QUERY_PATH = @"pack://SiteOfOrigin:,,,/Resource/Image/Query.png";
        const string TAKE_CARD_PATH = @"pack://SiteOfOrigin:,,,/Resource/Image/TakeCard.png";
        const string WAIT_CARD_PATH = @"pack://SiteOfOrigin:,,,/Resource/Image/WaitCard.png";

        const string ONFIRM_INFO_YELLOW_PATH = @"pack://SiteOfOrigin:,,,/Resource/Image/ConfirmInfo_Yellow.png";
        const string QUERY_YELLOW_PATH = @"pack://SiteOfOrigin:,,,/Resource/Image/Query_Yellow.png";
        const string TAKE_CARD_YELLOW_PATH = @"pack://SiteOfOrigin:,,,/Resource/Image/TakeCard_Yellow.png";
        const string WAIT_CARD_YELLOW_PATH = @"pack://SiteOfOrigin:,,,/Resource/Image/WaitCard_Yellow.png";

        static BitmapImage QueryImage;
        static BitmapImage ConfirmInfoImage;
        static BitmapImage WaitCardImage;
        static BitmapImage TakeCardImage;

        static BitmapImage QueryImage_Yellow;
        static BitmapImage ConfirmInfoImage_Yellow;
        static BitmapImage WaitCardImage_Yellow;
        static BitmapImage TakeCardImage_Yellow;

        public FlowControl()
        {
            InitializeComponent();
            this.Loaded += FlowControl_Loaded;

            if (QueryImage == null)
            {
                QueryImage = new BitmapImage(new Uri(QUERY_PATH));
            }

            if (ConfirmInfoImage == null)
            {
                ConfirmInfoImage = new BitmapImage(new Uri(ONFIRM_INFO_PATH));
            }

            if (WaitCardImage == null)
            {
                WaitCardImage = new BitmapImage(new Uri(WAIT_CARD_PATH));
            }

            if (TakeCardImage == null)
            {
                TakeCardImage = new BitmapImage(new Uri(TAKE_CARD_PATH));
            }

            if (QueryImage_Yellow == null)
            {
                QueryImage_Yellow = new BitmapImage(new Uri(QUERY_YELLOW_PATH));
            }

            if (ConfirmInfoImage_Yellow == null)
            {
                ConfirmInfoImage_Yellow = new BitmapImage(new Uri(ONFIRM_INFO_YELLOW_PATH));
            }

            if (WaitCardImage_Yellow == null)
            {
                WaitCardImage_Yellow = new BitmapImage(new Uri(WAIT_CARD_YELLOW_PATH));
            }

            if (TakeCardImage_Yellow == null)
            {
                TakeCardImage_Yellow = new BitmapImage(new Uri(TAKE_CARD_YELLOW_PATH));
            }
        }

        private void FlowControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetStep(Step);
        }

        public int Step
        {
            get { return (int)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Text", typeof(int), typeof(FlowControl), new PropertyMetadata(0));

        private void SetStep(int step)
        {
            Query.Source = QueryImage;
            ConfirmInfo.Source = ConfirmInfoImage;
            WaitCard.Source = WaitCardImage;
            TakeCard.Source = TakeCardImage;

            switch (Step)
            {
                case 0:
                    Query.Source = QueryImage_Yellow;
                    break;
                case 1:
                    ConfirmInfo.Source = ConfirmInfoImage_Yellow;
                    break;
                case 2:
                    WaitCard.Source = WaitCardImage_Yellow;
                    break;
                case 3:
                    TakeCard.Source = TakeCardImage_Yellow;
                    break;
            }
        }
    }
}

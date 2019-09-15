using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GiantappConfiger.Views.Controls
{
    /// <summary>
    /// NumericUpDown.xaml 的交互逻辑
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
            txtNum.Text = NumValue.ToString();
        }

        #region NumValue

        public int NumValue
        {
            get { return (int)GetValue(NumValueProperty); }
            set { SetValue(NumValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumValueProperty =
            DependencyProperty.Register("NumValue", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0, new PropertyChangedCallback(ValueChanged)));

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as NumericUpDown;
            control.txtNum.Text = e.NewValue.ToString();
        }

        #endregion

        private void CmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void CmdDown_Click(object sender, RoutedEventArgs e)
        {
            NumValue--;
        }

        private void TxtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null)
                return;

            if (!int.TryParse(txtNum.Text, out int newNum))
                txtNum.Text = NumValue.ToString();
            else
                SetCurrentValue(NumValueProperty, newNum);
        }
    }
}

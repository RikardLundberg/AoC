using System.Windows.Controls;
using System.Windows.Input;

namespace AoC.Common.View
{
    public class ScrollableComboBox : ComboBox
    {
        public ScrollableComboBox()
        {
            MouseEnter += ScrollableComboBox_MouseEnter;
        }

        private void ScrollableComboBox_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Focus();
        }
    }
}

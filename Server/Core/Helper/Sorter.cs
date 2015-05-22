using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace xServer.Core.Helper
{
    internal class Sorter : IComparer
    {
        public int Column = 0;
        public SortOrder Order = SortOrder.Ascending;
        public int Compare(object x, object y)
        {
            int result;
            try
            {
                if (!(x is ListViewItem))
                {
                    result = 0;
                }
                else
                {
                    if (!(y is ListViewItem))
                    {
                        result = 0;
                    }
                    else
                    {
                        ListViewItem listViewItem = (ListViewItem)x;
                        ListViewItem listViewItem2 = (ListViewItem)y;
                        if (listViewItem.ListView.Columns[this.Column].Tag == null)
                        {
                            listViewItem.ListView.Columns[this.Column].Tag = "Text";
                        }
                        if (listViewItem.ListView.Columns[this.Column].Tag.ToString() == "Numeric")
                        {
                            double value = double.Parse(listViewItem.SubItems[this.Column].Text);
                            double value2 = double.Parse(listViewItem2.SubItems[this.Column].Text);
                            if (this.Order == SortOrder.Ascending)
                            {
                                result = value.CompareTo(value2);
                            }
                            else
                            {
                                result = value2.CompareTo(value);
                            }
                        }
                        else
                        {
                            string text = listViewItem.SubItems[this.Column].Text;
                            string text2 = listViewItem2.SubItems[this.Column].Text;
                            if (this.Order == SortOrder.Ascending)
                            {
                                result = text.CompareTo(text2);
                            }
                            else
                            {
                                result = text2.CompareTo(text);
                            }
                        }
                    }
                }
            }
            catch
            {
                result = 0;
            }
            return result;
        }
    }
}

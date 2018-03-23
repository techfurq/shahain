using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Accounts
{
    public partial class balSheet : Form
    {
        public balSheet()
        {
            InitializeComponent();
        }

        private void balSheet_Load(object sender, EventArgs e)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load("clients.dbs");
            //var root = doc.DocumentElement;
            //var namelist = root.ChildNodes;

            //for (int i = 0; i < namelist.Count; i++)
            //{
            //    var item = (new ListViewItem(new[] { namelist.Item(i).Attributes[0].InnerText,
            //                                         namelist.Item(i).ChildNodes[0].InnerText,
            //                                         namelist.Item(i).ChildNodes[1].InnerText,
            //                                         (Convert.ToInt32(namelist.Item(i).ChildNodes[0].InnerText) - Convert.ToInt32(namelist.Item(i).ChildNodes[1].InnerText)).ToString()}));
            //    listView1.Items.Add(item);
            //}

            comboBox1.Items.Clear();
            var list1 = System.IO.File.ReadAllLines("category.dbs");
            for (int i = 0; i < list1.Length; i++)
            {
                comboBox1.Items.Add(list1[i]);
            }
            comboBox1.Items.Insert(0, "All");
            comboBox1.SelectedIndex = 0;

            ColumnSorter(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExportToExcel("ok.csv", listView1);
        }

        private void ExportToExcel(string path, ListView listsource)
        {
            StringBuilder CVS = new StringBuilder();
            for (int i = 0; i < listsource.Columns.Count; i++)
            {
                CVS.Append(listsource.Columns[i].Text + ",");
            }
            CVS.Append(Environment.NewLine);
            for (int i = 0; i < listsource.Items.Count; i++)
            {
                for (int j = 0; j < listsource.Columns.Count; j++)
                {
                    CVS.Append(listsource.Items[i].SubItems[j].Text + ",");
                }
                CVS.Append(Environment.NewLine);
            }
            System.IO.File.WriteAllText(path, CVS.ToString());
            Process.Start(path);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double rec = 0;
            double crd = 0;
            XDocument X = XDocument.Load("clients.dbs");
            var FilterList = X.Element("clients").Elements("Client").Where
                (E => (((comboBox1.Text == "All" )? true : (E.Element("cate").Value == comboBox1.Text)) && (E.Element("des").Value.ToUpper().Contains(textBox1.Text.ToUpper()))));
           listView1.BeginUpdate();
           listView1.ListViewItemSorter = null;
            listView1.Items.Clear();
            foreach (var item in FilterList)
            {
                listView1.Items.Add(new ListViewItem(new[] { item.FirstAttribute.Value,
                                                             item.Element("total").Value,
                                                             item.Element ("recieve").Value,
                                                             (double.Parse(item.Element("total").Value) - double.Parse(item.Element("recieve").Value)).ToString(),
                                                      
                }));
                crd += double.Parse(item.Element("total").Value);
                rec += double.Parse(item.Element("recieve").Value);
            }
            ColumnSorter(0);
            listView1.EndUpdate();
            total_crd.Text = crd.ToString();
            total_Rec.Text = rec.ToString();
            label4.Text = (crd - rec).ToString();

        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ColumnSorter(e.Column);
        }

        public void ColumnSorter(int index)
        {
            sorter sorter = listView1.ListViewItemSorter as sorter;
            if (sorter == null)
            {
                sorter = new sorter(index);
                listView1.ListViewItemSorter = sorter;
            }
            else
            {
                sorter.Column = index;
            }
            listView1.Sort();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //int heingh = 170;

            //Font fbold = new Font("Arial", 8.25f, FontStyle.Bold);
            //Font body = new Font("Arial", 8.25f, FontStyle.Regular);

            //e.Graphics.DrawString("SAMI TRADERS", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, 325, 35);
            //e.Graphics.DrawString("Shershah, Gulbai, Karachi, Pakistan.", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 315, 61);
            //e.Graphics.DrawString("0333-2209910, 0332-3825634", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 330, 77);


            //e.Graphics.DrawString(DateTime.Now.ToLongDateString(), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 30, 45);
            //e.Graphics.DrawString(DateTime.Now.ToLongTimeString(), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 30, 62);
            //e.Graphics.DrawString("Page: " + currentpage.ToString(), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 750, 45);

            //e.Graphics.DrawString(nameBox.Text, new Font("Arial", 9.5f, FontStyle.Bold), Brushes.Black, 30, 105);
            //e.Graphics.DrawString(dateTimeFrom.Text + " - " + dateTimeTo.Text, body, Brushes.Black, 343, 105);
            //e.Graphics.DrawString("Previous Balance:  " + prevBal.Text.ToString(), body, Brushes.Black, 635, 105);
            //e.Graphics.DrawLine(Pens.Black, 30, 125, 795, 125);

            //for (int i = 0; i < listView1.Columns.Count; i++)
            //{
            //    e.Graphics.DrawString(listView1.Columns[i].Text, fbold, Brushes.Black, weight[i] + (i * 100), 145);
            //    //     e.Graphics.DrawLine(Pens.Black, 40 + (i * 100), 35, 40 + (i * 100), 1000);
            //}
            //while (totalitem < listView1.Items.Count)
            //{

            //    for (int j = 0; j < listView1.Columns.Count; j++)
            //    {
            //        e.Graphics.DrawString(listView1.Items[totalitem].SubItems[j].Text, body, Brushes.Black, weight[j] + (j * 100), heingh);

            //    }
            //    totalitem++;
            //    heingh += 18;

            //    if (itemperpage <= 48) // check whether  the number of item(per page) is more than 20 or not
            //    {
            //        itemperpage += 1; // increment itemperpage by 1
            //        e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added

            //    }
            //    else // if the number of item(per page) is more than 20 then add one page
            //    {
            //        itemperpage = 0; //initiate itemperpage to 0 .
            //        e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
            //        currentpage++;
            //        return;//It will call PrintPage event again
            //    }
            //}
            //e.Graphics.DrawString("Grand Total:", new Font("Arial", 9f, FontStyle.Bold), Brushes.Black, 310, heingh + 6);
            //e.Graphics.DrawString(total_crd.Text, new Font("Arial", 9f, FontStyle.Bold), Brushes.Black, 500, heingh + 6);
            //e.Graphics.DrawString(total_Rec.Text, new Font("Arial", 9f, FontStyle.Bold), Brushes.Black, 591, heingh + 6);
            //e.Graphics.DrawString(listView1.Items[listView1.Items.Count - 1].SubItems[5].Text, new Font("Arial", 9f, FontStyle.Bold), Brushes.Black, 700, heingh + 6);
            //Pen pen = new Pen(Color.Black, 1);
            //pen.Alignment = PenAlignment.Inset; //<-- this
            //e.Graphics.DrawRectangle(pen, 695, heingh + 2, 95, 20);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.Xml.Linq;

namespace Accounts
{
    public partial class Lesasure : Form
    {
        
        int itemperpage = 0;
        int[] weight = new int[] { 30, 30, 113, 200, 192, 200 };
        int totalitem = 0;     
        int currentpage = 1;
        public Lesasure()
        {
            InitializeComponent();
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Enter))
            {
                SendKeys.Send("{TAB}");
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            double recv = 0;
            double crd = 0;
            XDocument X = XDocument.Load("main.dbs");


            var FilterList = X.Element("all").Elements("person").Where
             (E => (E.Element("name").Value == nameBox.Text) &&
             ((DateTime.ParseExact((E.Element("date").Value), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) <= dateTimeFrom.Value.AddDays(-1))));
            foreach (var item in FilterList)
            {
                crd += Convert.ToDouble(item.Element("total").Value);
                recv += Convert.ToDouble(item.Element("recieve").Value);

            }
            prevBal.Text = (crd - recv).ToString();


            FilterList = X.Element("all").Elements("person").Where
           (E => (E.Element("name").Value == nameBox.Text) &&
            ((DateTime.ParseExact((E.Element("date").Value), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) >= dateTimeFrom.Value.AddDays(-1) && (DateTime.ParseExact((E.Element("date").Value), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) < dateTimeTo.Value))));
            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (var item in FilterList)
            {
                listView1.Items.Add(new ListViewItem(new[] { 
                                                             item.Element("date").Value,
                                                             item.Element("qty").Value +"X"+item.Element ("peritem").Value+"-"+ item.Element("company").Value+"-"+  item.Element ("item").Value,
                                                             item.Element("details").Value,
                                                             (item.Element("total").Value),
                                                             (item.Element("recieve").Value),"0"
                                                              }));
                //crd += (Convert.ToInt32(item.Element("total").Value));
                //recv += int.Parse(item.Element("recieve").Value);
            }
            ColumnSorter(0);
            double total;
            double recieve;
            double balanceBefore;
         
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                total = Convert.ToDouble(listView1.Items[i].SubItems[3].Text);
                recieve = Convert.ToDouble(listView1.Items[i].SubItems[4].Text);

                if (i == 0)
                {
                    listView1.Items[0].SubItems[5].Text = (total + double.Parse(prevBal.Text) - recieve).ToString();
                }
                else
                {
                    balanceBefore = Convert.ToDouble(listView1.Items[i - 1].SubItems[5].Text);
                    listView1.Items[i].SubItems[5].Text = (balanceBefore + total - recieve).ToString();
                }
            }
            listView1.EndUpdate();


            XmlDocument docC = new XmlDocument();
            docC.Load("clients.dbs");
            total_Rec.Text =  docC.SelectSingleNode("//Client[@name='" + nameBox.Text + "']" + "//recieve").InnerText;
            total_crd.Text = docC.SelectSingleNode("//Client[@name='" + nameBox.Text + "']" + "//total").InnerText;


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

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           
        }


        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int heingh = 170;
            
            Font fbold = new Font("Arial", 8.25f, FontStyle.Bold);
            Font body = new Font("Arial", 8.25f, FontStyle.Regular);

            e.Graphics.DrawString("SAMI TRADERS", new Font("Arial", 15, FontStyle.Bold),Brushes.Black, 325, 35);
            e.Graphics.DrawString("Shershah, Gulbai, Karachi, Pakistan.", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 315, 61);
            e.Graphics.DrawString("0333-2209910, 0332-3825634", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 330, 77);


            e.Graphics.DrawString(DateTime.Now.ToLongDateString(), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 30, 45);
            e.Graphics.DrawString(DateTime.Now.ToLongTimeString(), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 30, 62);
            e.Graphics.DrawString("Page: "+currentpage.ToString(), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 750, 45);

            e.Graphics.DrawString(nameBox.Text, new Font("Arial", 9.5f, FontStyle.Bold), Brushes.Black, 30, 105);
            e.Graphics.DrawString(dateTimeFrom.Text + " - " + dateTimeTo.Text, body, Brushes.Black, 343, 105);
            e.Graphics.DrawString("Previous Balance:  " + prevBal.Text.ToString(), body, Brushes.Black, 635, 105);
            e.Graphics.DrawLine(Pens.Black, 30, 125, 795, 125);

            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                e.Graphics.DrawString(listView1.Columns[i].Text, fbold, Brushes.Black, weight[i] + (i * 100), 145);
         //     e.Graphics.DrawLine(Pens.Black, 40 + (i * 100), 35, 40 + (i * 100), 1000);
            }
                while(totalitem < listView1.Items.Count)
                {

                    for (int j = 0; j < listView1.Columns.Count; j++)
                    {
                        e.Graphics.DrawString(listView1.Items[totalitem].SubItems[j].Text, body, Brushes.Black, weight[j] + (j * 100), heingh);
                      
                }
                    totalitem++;
                    heingh += 18;

                    if (itemperpage <= 48) // check whether  the number of item(per page) is more than 20 or not
                    {
                        itemperpage += 1; // increment itemperpage by 1
                        e.HasMorePages = false; // set the HasMorePages property to false , so that no other page will not be added

                }
                else // if the number of item(per page) is more than 20 then add one page
                    {
                        itemperpage = 0; //initiate itemperpage to 0 .
                        e.HasMorePages = true; //e.HasMorePages raised the PrintPage event once per page .
                        currentpage++;
                        return;//It will call PrintPage event again
                    }
            }
            e.Graphics.DrawString("Grand Total:", new Font("Arial", 9f, FontStyle.Bold), Brushes.Black, 310, heingh + 6);
            e.Graphics.DrawString(total_crd.Text, new Font("Arial", 9f, FontStyle.Bold), Brushes.Black, 500, heingh + 6);
            e.Graphics.DrawString(total_Rec.Text, new Font("Arial", 9f, FontStyle.Bold), Brushes.Black, 591, heingh + 6);
            e.Graphics.DrawString(listView1.Items[listView1.Items.Count -1].SubItems[5].Text, new Font("Arial", 9f, FontStyle.Bold), Brushes.Black, 700, heingh + 6);
            Pen pen = new Pen(Color.Black, 1);
            pen.Alignment = PenAlignment.Inset; //<-- this
            e.Graphics.DrawRectangle(pen, 695,heingh +2,95,20);
        }
        //if (pages < 3)
        //{
        //    e.HasMorePages = true;
        //    pages++;
        //}
        //else
        //    e.HasMorePages = false;



        private void button1_Click(object sender, EventArgs e)
        {
            //  printDocument1.Print();
            itemperpage = totalitem = 0;
            currentpage = 1;
            printPreviewDialog1.Document = printDocument1;
            //    printDocument1.DefaultPageSettings.PaperSize = pages;
            printPreviewDialog1.ShowDialog();

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

        private void button4_Click(object sender, EventArgs e)
        {
            itemperpage = totalitem = 0;
            currentpage = 1;
            printDocument1.Print();
        }

        private void nameBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void nameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string strFindStr = "";
            if (e.KeyChar == (char)8)
            {
                if (cb.SelectionStart <= 1)
                {
                    cb.Text = "";
                    return;
                }

                if (cb.SelectionLength == 0)
                    strFindStr = cb.Text.Substring(0, cb.Text.Length - 1);
                else
                    strFindStr = cb.Text.Substring(0, cb.SelectionStart - 1);
            }
            else
            {
                if (cb.SelectionLength == 0)
                    strFindStr = cb.Text + e.KeyChar;
                else
                    strFindStr = cb.Text.Substring(0, cb.SelectionStart) + e.KeyChar;
            }
            int intIdx = -1;
            // Search the string in the ComboBox list.
            intIdx = cb.FindString(strFindStr);
            if (intIdx != -1)
            {
                cb.SelectedText = "";
                cb.SelectedIndex = intIdx;
                cb.SelectionStart = strFindStr.Length;
                cb.SelectionLength = cb.Text.Length;
                e.Handled = true;
            }
            else
                e.Handled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void Lesasure_Load(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("stocks.dbs");
            XmlElement rootElement = doc.DocumentElement;
            var list = rootElement.ChildNodes;
            try
            {
                nameBox.Items.Clear();
                doc.Load("clients.dbs");
                rootElement = doc.DocumentElement;
                list = rootElement.ChildNodes;
                for (int i = 0; i < list.Count; i++)
                    nameBox.Items.Add(list.Item(i).Attributes[0].InnerText);
            }
            catch (Exception)
            {
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            float factor = 1.4f; //zoomfactor 1 for no zoom
           int heingh = 170;
          
            Bitmap b = new Bitmap(Convert.ToInt32(820 * factor), Convert.ToInt32((225 + (18*listView1.Items.Count)) * factor));
            using (var graphics = Graphics.FromImage(b))
            {

                Font fbold = new Font("Arial", 8.25f *factor, FontStyle.Bold);
                Font body = new Font("Arial", 8.25f * factor, FontStyle.Regular);
                graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, b.Width, b.Height);
                graphics.DrawString("SAMI TRADERS", new Font("Arial", 15 * factor, FontStyle.Bold), Brushes.Black, 325 * factor, 35 * factor);
                graphics.DrawString("Shershah, Gulbai, Karachi, Pakistan.", new Font("Arial", 8 * factor, FontStyle.Regular), Brushes.Black, 315 * factor, 61 * factor);
                graphics.DrawString("0333-2209910, 0332-3825634", new Font("Arial", 8 * factor, FontStyle.Regular), Brushes.Black, 330 * factor, 77 * factor);


                graphics.DrawString(DateTime.Now.ToLongDateString(), new Font("Arial", 8 * factor, FontStyle.Regular), Brushes.Black, 30 * factor, 45 * factor);
                graphics.DrawString(DateTime.Now.ToLongTimeString(), new Font("Arial", 8 * factor, FontStyle.Regular), Brushes.Black, 30 * factor, 62 * factor);
                graphics.DrawString("Page: " + currentpage.ToString(), new Font("Arial", 8 * factor, FontStyle.Regular), Brushes.Black, 750 * factor, 45 * factor);

                graphics.DrawString(nameBox.Text, new Font("Arial", 9.5f * factor, FontStyle.Bold), Brushes.Black, 30 * factor, 105 * factor);
                graphics.DrawString(dateTimeFrom.Text + " - " + dateTimeTo.Text, body, Brushes.Black, 343 * factor, 105 * factor);
                graphics.DrawString("Previous Balance:  " + prevBal.Text.ToString(), body, Brushes.Black, 635 * factor, 105 * factor);
                graphics.DrawLine(Pens.Black, 30 * factor, 125 * factor, 795 * factor, 125 * factor);

                for (int i = 0; i < listView1.Columns.Count; i++)
                {
                    graphics.DrawString(listView1.Columns[i].Text, fbold, Brushes.Black, weight[i]*factor + (i * 100 * factor), 145 * factor);
                }
                for (int i = 0; i < listView1.Items.Count; i++)
                {

                    for (int j = 0; j < listView1.Columns.Count; j++)
                    {
                        graphics.DrawString(listView1.Items[i].SubItems[j].Text, body, Brushes.Black, weight[j]*factor + (j * 100 * factor), heingh * factor) ;
                    }
                    totalitem++;
                    heingh += 18 ;
                }                  
                graphics.DrawString("Grand Total:", new Font("Arial", 9f * factor, FontStyle.Bold), Brushes.Black, 310 * factor, (heingh + 6) * factor);
                graphics.DrawString(total_crd.Text, new Font("Arial", 9f * factor, FontStyle.Bold), Brushes.Black, 500 * factor, (heingh + 6) * factor);
                graphics.DrawString(total_Rec.Text, new Font("Arial", 9f * factor, FontStyle.Bold), Brushes.Black, 591 * factor, (heingh + 6 )* factor);
                graphics.DrawString(listView1.Items[listView1.Items.Count - 1].SubItems[5].Text, new Font("Arial", 9f * factor, FontStyle.Bold), Brushes.Black, 700 * factor,( heingh + 6) * factor);
                Pen pen = new Pen(Color.Black, 1 * factor);
                pen.Alignment = PenAlignment.Inset; //<-- this
                graphics.DrawRectangle(pen, 695 * factor, (heingh + 2) * factor, 95 * factor, 20 * factor);

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = nameBox.Text;
                savefile.Filter = "Imgae file (*.png)|*.png|All files (*.*)|*.*";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    b.Save(savefile.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    MessageBox.Show("Image saved successfully!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               
            }
        }
    }
}

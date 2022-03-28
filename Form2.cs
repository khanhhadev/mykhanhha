using ShinkoComm_V205;
using System.Collections.Generic;
using System;
using System.Xml.Linq;
using System.IO.Ports;
using System.Windows.Forms;

namespace CommTest
{
    public partial class Form2 : Form
    {
        //STDComm STDCo = new STDComm();
        string[] XML = {
                    "CmslIndex",
                    "BaudrateIndex",
                    "ParityIndex",
                    "DatabitIndex" ,
                    "StopbitIndex",
                    "TimeoutIndex",
                    "StandbyIndex",
        "PortIndex",
        "ListCheck",
        "ListAddress",
        "ListName",
        "ListWrite"};
        
        public string getstringXML(string a, int i)
        {
            XElement xmlP = XElement.Load(a);
            return xmlP.Element(XML[i]).Value;
        }
        public string[] getstringarray(string a,int i)
        {
            XElement xmlP = XElement.Load(a);
            string[] array = System.Text.RegularExpressions.Regex.Split(xmlP.Element(XML[i]).Value, ", ", System.Text.RegularExpressions.RegexOptions.None);            
            return array;
        }
        public Form2()
        {
            InitializeComponent();
            cmb_ComPort.Items.AddRange(GetCOMList());
            cmb_BaudRate.SelectedIndex = cmb_ComPort.SelectedIndex = cmb_Databit.SelectedIndex = cmb_Parity.SelectedIndex
            = cmb_Stopbit.SelectedIndex = 0;
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
                this.Hide();
        }
        public void TextLimit(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public string[] GetCOMList()
        {
            List<string> Listt = new List<string>();
            Listt.Clear();
            for (int i = 1; i < 256; i++)
            {
                Listt.Add("COM" + i.ToString());
            }
            return Listt.ToArray();
        }
        public void GetXML(Form1 frm, string a)
        {
            cmb_BaudRate.Text = getstringXML(a, 1);
            cmb_Parity.Text = getstringXML(a, 2);
            cmb_Databit.Text = getstringXML(a, 3);
            cmb_Stopbit.Text = getstringXML(a, 4);
            cmb_TimeOut.Text = getstringXML(a, 5);
            cmb_ComPort.Text = getstringXML(a, 7);
            
        }

        public void SetXML(Form1 frm, string b)
        {
            XElement xmlP = XElement.Load(b);
            xmlP.Element("BaudrateIndex").Value = cmb_BaudRate.Text;
            xmlP.Element("ParityIndex").Value = cmb_Parity.Text;
            xmlP.Element("DatabitIndex").Value = cmb_Databit.Text;
            xmlP.Element("StopbitIndex").Value = cmb_Stopbit.Text;
            xmlP.Element("TimeoutIndex").Value = cmb_TimeOut.Text;
            xmlP.Element("PortIndex").Value = cmb_ComPort.Text;
            xmlP.Save(b);
        }

    }
}

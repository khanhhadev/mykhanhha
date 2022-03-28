using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Text;
using System.Xml.Linq;
using ShinkoComm_V205;
using System.Diagnostics;

namespace CommTest
{
    public partial class Form1 : Form
    {

        STDComm STDCo = new STDComm();
        Parity[] Paritybit = { Parity.None, Parity.Odd, Parity.Even };
        StopBits[] Stopbit = { StopBits.One, StopBits.Two };
        bool Connect = false, continous = false;
        bool first = false, firstwrite = false;
        int Itemrow, itemnum;
        int primaWidth, primaHeigth;
        float widthRatio, heightRatio;
        List<Control> control = new List<Control>();
        string time;
        int kaisuu;
        string pathfile,pathfolder;
        public string pathxml;
        public int Filenum = 1;
        int rowcount;
        string filelocation = @"\SHINKO TECHNOS\EEPROM_CHECK_V100\Logging";
        Form2 frm2 = new Form2();
        public enum RW
        {
            Read = 2,
            Write = 3,
        }
        public RW RWstate = RW.Read;

        public enum InputType
        {
            Int,
            Unint,
        }
        public InputType ValueType;

        public struct RWItem
        {
            public string Add, SubAdd, CmdType, Item, SetData, Datanum, Bytenum;
            public int Num, Retry;

        }
        RWItem SendDT = new RWItem();

        public struct RowItem
        {
            public bool SELECT, ERROR,DATAERROR;
            public string ITEM, ITEMNAME, READVALUE, WRITEVALUE;
            public int ERRORNUMBER;
            public int DATAERRORNUMBER;
            public bool JUSTREAD;
        }
        //public RowItem NewCreate()
        //{
        //    RowItem RWItm
        //}
        public enum CMSL
        {
            Shinko,
            Mewtocol,
            ModbusASCII,
            ModbusRTU
        }
        public Form1()
        {
            InitializeComponent();
            frm2.Hide();
            CopyXMLfile();
            frm2.GetXML(this, pathxml);
            frm2.btn_Open.Click += Btn_Open_Click;
            STDCo.ComEvent += SCom_ComEvent;
            cmb_Number.Items.AddRange(GetNumList());
            btn_COMSetting.Text = frm2.cmb_ComPort.Text;
            getXML();
            cmb_Command.SelectedIndex = 0;
            STDCo.SelectProtocol = (int)CMSL.Shinko;
            primaWidth = this.Width;
            primaHeigth = this.Height;
            GetResizeInfor();
        }

        private void Btn_Open_Click(object sender, EventArgs e)
        {
            this.Show();
            frm2.SetXML(this, pathxml);
            btn_COMSetting.Text = frm2.cmb_ComPort.Text;
        }

        private void btn_Read_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit(DataGridViewDataErrorContexts.Commit);
            Itemrow = 0;
            txt_OnOff.BackColor = Color.DarkGreen;
            if (btn_SaveData.Checked == true)
            {
                FileCreate();
            }
            btn_ReadContinous.Enabled = btn_OpenFile.Visible = false;
            timer1.Enabled = true;
            txt_Path.Text = "";
        }

        private void btn_ReadContinous_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit(DataGridViewDataErrorContexts.Commit);
            btn_Read.Enabled = !btn_Read.Enabled;
            continous = !continous;
            timer1.Enabled = !timer1.Enabled;
            btn_OpenFile.Visible = !timer1.Enabled;

            if (continous == false)
            {
                txt_Path.Text = pathfile;
                btn_ReadContinous.BackColor = Color.White;
                btn_ReadContinous.ForeColor = Color.SteelBlue;
                txt_OnOff.BackColor = SystemColors.Control;
            }
            else
            {
                for (int count = 0; count < itemnum; count++)
                {
                    DataGridViewRow Rw = dataGridView1.Rows[count];
                    Rw.Cells[ErrorCounter.Index].Value = "0"; Rw.Cells[DataerrorCounter.Index].Value = "0";
                }
                kaisuu = 0;
                txt_kaisuu.Text = kaisuu.ToString();
                if (btn_SaveData.Checked == true)
                {
                    FileCreate();
                }
                txt_Path.Text = "";
                btn_ReadContinous.BackColor = Color.SteelBlue;
                btn_ReadContinous.ForeColor = Color.White;
                txt_OnOff.BackColor = Color.DarkGreen;
            }
        }

        void ComProcess()
        {
            timer1.Enabled = false;
            if (Itemrow < itemnum)
            {
                RowItem RwItm = GetRowItem(Itemrow);
                //try
                //{
                if (firstwrite)
                {
                    GetComInfor(RW.Write, RwItm, Itemrow);
                    if (!Write())
                    {
                        MessageBox.Show("パスワードが設定できません。再確認ください。", "通信停止", MessageBoxButtons.OK);
                        continous = false;
                        Itemrow = itemnum + 1;
                        kaisuu--;
                        return;
                    }
                    else
                    {
                        firstwrite = false;
                        txt_Password.Visible = label3.Visible = false;
                    }
                };
                if (RwItm.SELECT)
                {
                    try
                    {
                        if (!RwItm.JUSTREAD)
                        {
                            GetComInfor(RW.Write, RwItm, Itemrow);
                            RwItm.ERROR = !Write();
                            
                        }
                        GetComInfor(RW.Read, RwItm, Itemrow);
                        RwItm.READVALUE = Read();
                        if (RwItm.READVALUE == "----") RwItm.ERROR = true;
                        if (RwItm.ERROR) RwItm.ERRORNUMBER++;
                        if (!RwItm.JUSTREAD)
                        {
                            if (RwItm.READVALUE != "----" && RwItm.READVALUE != RwItm.WRITEVALUE)
                            {
                                RwItm.DATAERROR = true;
                                RwItm.DATAERRORNUMBER++;
                                if (CB_ErrorStop.Checked == true && continous) { continous = false; return; }
                            }
                            else
                                RwItm.DATAERROR = false;
                        }
                    }
                    catch (Exception)
                    {
                        RwItm.ERROR = true;
                        RwItm.ERRORNUMBER++;
                        RwItm.READVALUE = "----";
                        OpenPort();
                        //timer1.Enabled = true;
                    }
                    PutRowItem(RwItm, Itemrow);
                    if (btn_SaveData.Checked == true) setFile(RwItm, pathfile);
                }
                //}
                //catch (Exception)
                //{
                //    RwItm.ERROR = true;
                //    RwItm.ERRORNUMBER++;
                //    RwItm.READVALUE = "----";
                //}

            }
            timer1.Enabled = true;
        }

        string Read()
        {
            string Readvalue;
            RWstate = RW.Read;
            string InputStr, RxDt;

            //STDCo.RecvBufClear();
            //STDCo.SendBufClear();

            InputStr = STDCo.RWcomm(SendDT.Add, SendDT.SubAdd, SendDT.CmdType, SendDT.Item, SendDT.SetData, SendDT.Retry, SendDT.Datanum);
            InputStr = STDCo.OutputBuffer;
                STDCo.RecvBufClear();
                STDCo.SendBufClear();
            //InputStr = STDCo.InputBuffer;
            if (InputStr != null)
            {
                if (STDCo.GetNData(InputStr, 1) != null)
                {
                    RxDt = STDCo.GetNData(InputStr, 1);
                    if (RxDt == null || RxDt == "")
                        Readvalue = "----";
                    else
                        Readvalue = String.Format("{0:d}", (int)AtoD(RxDt));
                }
                else
                {
                    Readvalue = "----";
                }
            }
            else
            {
                Readvalue = "----";
            }
            return Readvalue;
        }

        bool Write()
        {
            try
            {
                RWstate = RW.Write;
                string InputStr;
                if (firstwrite)
                {
                    SendDT.CmdType = ((char)0x50).ToString();
                    SendDT.SetData = String.Format("{0:X4}", int.Parse(txt_Password.Text));
                    SendDT.Item = "0000";
                }
                InputStr = STDCo.RWcomm(SendDT.Add, SendDT.SubAdd, SendDT.CmdType, SendDT.Item, SendDT.SetData, SendDT.Retry, SendDT.Datanum);
                //OutputStr = STDCo.InputBuffer;
                InputStr = STDCo.OutputBuffer;
                STDCo.RecvBufClear();
                STDCo.SendBufClear();
                if (InputStr == null || InputStr == "")
                {
                    //OpenPort(); 
                    return false;
                }
                else if (firstwrite && InputStr.Substring(0, 1) == ((char)0x15).ToString()) return false;
                else return true;
            }
            catch (Exception)
            {
                //OpenPort();
                return false; 
            }
        }
        public string[] GetNumList()
        {
            List<string> Listt = new List<string>();
            Listt.Clear();
            for (int i = 0; i < 96; i++)
            {
                Listt.Add(String.Format("{0:D2}", i));
            }
            return Listt.ToArray();
        }
        RowItem GetRowItem(int rowindex)
        {
            RowItem RwItm = new RowItem();
            DataGridViewRow dr = dataGridView1.Rows[rowindex];
            RwItm.SELECT = Convert.ToBoolean(dr.Cells[1].Value);
            RwItm.ITEM = Convert.ToString(dr.Cells[2].Value);
            RwItm.ITEMNAME = Convert.ToString(dr.Cells[3].Value);
            RwItm.WRITEVALUE = Convert.ToString(dr.Cells[4].Value);
            if (RwItm.WRITEVALUE == null || RwItm.WRITEVALUE == "") RwItm.JUSTREAD = true;
            else
            {
                RwItm.JUSTREAD = false;
            }
            RwItm.ERRORNUMBER = Convert.ToInt32(dr.Cells[7].Value);
            return RwItm;
        }

        void PutRowItem(RowItem RwItm, int rowindex)
        {　
            if ((rowindex >= 0) && (rowindex < itemnum))
            {
                DataGridViewRow dr = dataGridView1.Rows[rowindex];
                dr.Cells[5].Value = RwItm.READVALUE.ToString();
                dr.Cells[6].Value = (RwItm.ERROR == true || RwItm.DATAERROR == true) ? "NG" : "OK";
                dr.Cells[7].Value = RwItm.ERRORNUMBER.ToString();
                dr.Cells[8].Value = RwItm.DATAERRORNUMBER.ToString();
            }
        }

        void GetComInfor(RW RWstate, RowItem RwItm, int rowindex)
        {
            SendDT.SubAdd = ((char)0x20).ToString();
            SendDT.Add = ((char)(int.Parse(cmb_Number.Text) + 0x20)).ToString();
            SendDT.Num = 1;
            SendDT.Datanum = "0001";
            SendDT.Item = String.Format("{0:X4}", RwItm.ITEM);
            SendDT.Retry = 1;
            SendDT.SetData = "";
            if (RWstate == RW.Read)
            {
                SendDT.CmdType = ((char)0x62).ToString();
            }
            else if (RWstate == RW.Write)
            {

                ValueType = InputType.Int;
                SendDT.CmdType = ((char)0x72).ToString();
                if (RwItm.WRITEVALUE == null || RwItm.WRITEVALUE == "") RwItm.JUSTREAD = true;
                else
                {
                    if (int.Parse(RwItm.WRITEVALUE) > 0x7FFF)
                    {
                        ValueType = InputType.Unint;
                    }
                    if (int.Parse(RwItm.WRITEVALUE) < 0)
                    {
                        SendDT.SetData = String.Format("{0:X4}", int.Parse(RwItm.WRITEVALUE) + 0xFFFF + 1);
                    }
                    else
                        SendDT.SetData = String.Format("{0:X4}", int.Parse(RwItm.WRITEVALUE));
                    ;
                }
            }
        }

        public void TextLimit(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (c != '\b' && !((c <= 0x66 && c >= 0x61) || (c <= 0x46 && c >= 0x41) || (c >= 0x30 && c <= 0x39)))
            {
                e.Handled = true;
            }
        }
        public void TextLimit2(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (sender != txt_Password && sender != cmb_LogCycle && sender != cmb_Number && c == 0x2D)
            {
                e.Handled = false;
                return;
            };
            if ((c != '\b' && !(c >= 0x30 && c <= 0x39)))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == Address.Index)
            {
                DataGridViewRow currentRow = dataGridView1.Rows[e.RowIndex];
                currentRow.Cells[Address.Index].Value = Convert.ToString(currentRow.Cells[Address.Index].Value).PadLeft(4, '0');
            }
            if (e.ColumnIndex == WriteValue.Index)
            {
                DataGridViewRow currentRow = dataGridView1.Rows[e.RowIndex];
                try
                {
                    int trans = int.Parse(Convert.ToString(currentRow.Cells[WriteValue.Index].Value));
                    if (trans > 0xFFFF)
                    {
                        currentRow.Cells[WriteValue.Index].Value = 65535.ToString();
                    }
                    else if (trans < -32768)
                    {
                        currentRow.Cells[WriteValue.Index].Value = (-32768).ToString();
                    }
                }
                catch (Exception )
                {
                    currentRow.Cells[WriteValue.Index].Value = "";
                }
            }
            dataGridView1.Update();
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int a = dataGridView1.CurrentCell.RowIndex;
            int b = dataGridView1.CurrentCell.ColumnIndex;
            if (a < 0) return;
            if (b == Address.Index)
            {
                TextBox TBITM;
                TBITM = e.Control as TextBox;
                TBITM.KeyPress -= TextLimit2;
                TBITM.KeyPress += new KeyPressEventHandler(TextLimit);
                TBITM.CharacterCasing = CharacterCasing.Upper;
                TBITM.ReadOnly = false;
                TBITM.ImeMode = ImeMode.Disable;
            }
            else if (b == ReadValue.Index)
            {
                TextBox TBDATA;
                TBDATA = e.Control as TextBox;
                TBDATA.ReadOnly = true;
                TBDATA.ImeMode = ImeMode.NoControl;
            }
            else if (b == WriteValue.Index)
            {
                TextBox TBITM;
                TBITM = e.Control as TextBox;
                TBITM.KeyPress -= TextLimit2;
                TBITM.KeyPress += new KeyPressEventHandler(TextLimit2);
                //TBITM.TextChanged += new EventArgs(WriteValueChanged);
                TBITM.ReadOnly = false;
                TBITM.ImeMode = ImeMode.Disable;
            }
            else
            {
                TextBox TBOTHER;
                TBOTHER = e.Control as TextBox;
                TBOTHER.CharacterCasing = CharacterCasing.Normal;
                TBOTHER.KeyPress -= TextLimit;
                TBOTHER.KeyPress -= TextLimit2;
                TBOTHER.ReadOnly = false;
                TBOTHER.ImeMode = ImeMode.NoControl;
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            int c = dataGridView1.CurrentCell.ColumnIndex;
            if (c == Select.Index)
                dataGridView1.EndEdit(DataGridViewDataErrorContexts.Commit);
            else dataGridView1.BeginEdit(true);
        }

        private void cmb_Number_Leave(object sender, EventArgs e)
        {
            if (cmb_Number.Text == "") cmb_Number.SelectedIndex = 0;
        }

        private void cmb_Number_TextUpdate(object sender, EventArgs e)
        {
            if (cmb_Number.Text == "") return;
            if (int.Parse(cmb_Number.Text) > 95) cmb_Number.SelectedIndex = 0;
        }

        //
        //Convert Hex String to Number
        //
        private double AtoD(string d)
        {
            int len = d.Length;
            char[] l = d.ToCharArray();
            double S = 0;
            for (int i = (len - 1); i >= 0; i--)
            {
                double k = (double)l[i];
                if ((k >= (double)'0') && (k <= (double)'9'))
                    S += (k - (double)'0') * (System.Math.Pow(16, len - i - 1));
                else if ((k >= (double)'A') && (k <= (double)'F'))
                    S += (k - (double)'A' + 10) * (System.Math.Pow(16, len - i - 1));
            }
            if (ValueType == InputType.Int)
            {
                if (S > 0x7FFF) S = S - 0xFFFF - 1;
            }
            return S;
        }

        public void setFile(RowItem RwItm, string path)
        {
            string delimiter = ", ";
            string stringA = "";
            StringBuilder sb = new StringBuilder();
            if (first)
            {
                stringA ="Port Name"+delimiter+STDCo.Port + delimiter;
                sb.AppendLine(stringA);
                stringA = "Baudrate" + delimiter + STDCo.BaudRate + delimiter;
                sb.AppendLine(stringA);
                stringA = "Databit" + delimiter + STDCo.DataBit + delimiter;
                sb.AppendLine(stringA);
                stringA = "Parity" + delimiter + STDCo.Parity.ToString() + delimiter;
                sb.AppendLine(stringA);
                stringA = "Stopbit" + delimiter + STDCo.StopBit.ToString() + delimiter;
                sb.AppendLine(stringA);
                stringA = "Timeout" + delimiter + STDCo.TimeOut+ "ms";
                sb.AppendLine(stringA);
                stringA = "Device Number" + delimiter + cmb_Number.Text;
                sb.AppendLine(stringA);
                stringA = "Logging cycle" + delimiter + timer1.Interval.ToString() + "ms";
                sb.AppendLine(stringA);
                sb.AppendLine("");
                File.AppendAllText(path, sb.ToString());
                sb.Clear();
                stringA = "Turn" + delimiter + "Time" + delimiter + "Address" + delimiter + "Item Name" + delimiter + "WriteValue" + delimiter + "Read Value" + delimiter + "Status" +
                    delimiter + "Com Error Count" + delimiter + "Data Error Count" + delimiter;
                sb.AppendLine(stringA);
                File.AppendAllText(path, sb.ToString());
                first = !first;
                rowcount += 2;
            }
            sb.Clear();

            stringA = kaisuu.ToString() + delimiter + DateTime.Now.ToString("HH:mm:ss:fff") + delimiter
                + "'" + RwItm.ITEM + delimiter + RwItm.ITEMNAME + delimiter + RwItm.WRITEVALUE + delimiter +
                   RwItm.READVALUE + delimiter + ((RwItm.ERROR == true) ? "NG" : "OK") + delimiter
                   + ((RwItm.ERROR == true) ? RwItm.ERRORNUMBER.ToString() : "") + delimiter + ((RwItm.DATAERROR == true) ? RwItm.DATAERRORNUMBER.ToString() : "");
            sb.AppendLine(stringA);
            File.AppendAllText(path, sb.ToString());
            rowcount++;
            //if (rowcount >= 65000||time != DateTime.Now.ToString("yyMMdd"))
            if (rowcount >= 65000)
            FileCreate();

        }
        public void FileCreate()
        {
            //string a = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //string pathString = a + filelocation;
            string pathString;
            //if (time != DateTime.Now.ToString("yyMMdd"))
            //{
            //    time = DateTime.Now.ToString("yyMMdd");
            //    Filenum = -1;
            //}
            if (pathfolder == "")
            {
                string a = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                pathString = a + filelocation;
                //pathString = System.IO.Path.Combine(pathString, DateTime.Now.ToString("yyyy"));
                //pathString = System.IO.Path.Combine(pathString, DateTime.Now.ToString("MM"));
                //pathString = System.IO.Path.Combine(pathString, DateTime.Now.ToString("dd"));
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                    Filenum = -1;
                }
                //do
                //{
                //    Filenum++;
                //    if (Filenum == 1000)
                //    {
                //        Filenum = -1;
                //        return;
                //    }
                //    string fileName = String.Format("{0:D3}", Filenum) + ".csv";
                //    pathfile = System.IO.Path.Combine(pathString, fileName);
                //}
                //while (System.IO.File.Exists(pathfile));
            }
            else
            {
                pathString = pathfolder;
            }
            string fileName = DateTime.Now.ToString("yyMMdd_HHmmss") + ".csv"; 
            pathfile = System.IO.Path.Combine(pathString, fileName);
            //string pathString = filelocation;
            // Create a file name for the file you want to create.
            System.IO.FileStream fs = System.IO.File.Create(pathfile);
            fs.Close();
            first = true;
            rowcount = 0;
        }

        private void btn_OpenFile_Click(object sender, EventArgs e)
        {
            Process.Start(pathfile);
        }

        private void btn_OpenFolder_Click(object sender, EventArgs e)
        {
            string pathString;
            if (pathfolder == "")
            {
                string a = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                pathString = a + filelocation;
            }
            else pathString = pathfolder;
            Process.Start(pathString);
        }

        private void cmb_LogCycle_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer1.Interval = int.Parse(cmb_LogCycle.Text);
        }

        public bool Setcom(string PORT, string Baudrate, string Databit, Parity Parityy, StopBits stopBits, int Timeout)
        {
            STDCo.Port = PORT;
            STDCo.BaudRate = Baudrate;
            STDCo.DataBit = Databit;
            STDCo.Parity = Parityy;
            STDCo.StopBit = stopBits;
            STDCo.TimeOut = Timeout;
            STDCo.Sendwait = 0;
            STDCo.Flow = Handshake.None;
            STDCo.PortOpen();
            if (STDCo.PortOpen()) return true;
            else return false;
        }

        void CopyXMLfile()
        {
            string sourcepath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            sourcepath = Path.Combine(sourcepath, @"Appdata\Roaming\SHINKO TECHNOS\EEPROM_CHECK_V100");
            //string currentpath = System.IO.Directory.GetCurrentDirectory() + @"\Parameter.xml";
            string currentpath = Application.StartupPath + @"\Parameter.xml";
            pathxml = System.IO.Path.Combine(sourcepath, "Parameter.xml");
            if (!File.Exists(pathxml))
            {
                Directory.CreateDirectory(sourcepath);
                System.IO.File.Copy(currentpath, pathxml, false);
            }

            string a = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string pathString = a + filelocation;
            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
                Filenum = -1;
            }
        }
        void getXML()
        {
            try
            {
                XElement xmlP = XElement.Load(pathxml);
                cmb_Number.Text = xmlP.Element("NumberIndex").Value;
                cmb_LogCycle.Text = xmlP.Element("Logcycle").Value;
                itemnum = int.Parse(xmlP.Element("Itemnum").Value);
                pathfolder = xmlP.Element("PathFolder").Value;
                btn_SaveData.Checked = (xmlP.Element("DataLogCheck").Value == "1") ? true : false;
                CB_ErrorStop.Checked = (xmlP.Element("ErrorStopCheck").Value == "1") ? true : false;
                CB_KaisuuStop.Checked = (xmlP.Element("KaisuuStopCheck").Value == "1") ? true : false;
                for (int count = 1; count <= itemnum; count++)
                    dataGridView1.Rows.Add();
                List<string[]> Backup = new List<string[]>();
                for (int i = 8; i <= 11; i++)
                {
                    Backup.Add(frm2.getstringarray(pathxml, i));
                }
                for (int i = 0; i < itemnum; i++)
                {
                    DataGridViewRow dr = dataGridView1.Rows[i];
                    for (int j = 1; j <= 4; j++)
                    {
                        if (j == 1)
                        {
                            if (Backup[j - 1][i] == "1") dr.Cells[j].Value = true;
                            else dr.Cells[j].Value = false;
                        }
                        else
                            dr.Cells[j].Value = Backup[j - 1][i];
                    }
                }

            }
            catch
            {
                CopyXMLfile();
                getXML();
            }
        }

        void SetXML(string a)
        {
            try
            {
                XElement xmlP = XElement.Load(pathxml);
                xmlP.Element("NumberIndex").Value = cmb_Number.Text;
                xmlP.Element("Logcycle").Value = cmb_LogCycle.Text;
                xmlP.Element("Itemnum").Value = itemnum.ToString();
                string delemiter = ", ";
                List<string> Backup = new List<string> { "", "", "", "" };
                for (int i = 0; i < itemnum; i++)
                {
                    DataGridViewRow dr = dataGridView1.Rows[i];
                    for (int j = 1; j <= 4; j++)
                    {
                        if (j == 1)
                        {
                            if (Convert.ToBoolean(dr.Cells[j].Value) == true) Backup[j - 1] += "1";
                            else Backup[j - 1] += "0";
                        }
                        else
                            Backup[j - 1] += Convert.ToString(dr.Cells[j].Value);
                        Backup[j - 1] += delemiter;
                    }
                }
                xmlP.Element("ListCheck").Value = Backup[0];
                xmlP.Element("ListAddress").Value = Backup[1];
                xmlP.Element("ListName").Value = Backup[2];
                xmlP.Element("ListWrite").Value = Backup[3];
                xmlP.Element("PathFolder").Value = pathfolder;
                xmlP.Element("DataLogCheck").Value = (btn_SaveData.Checked == true) ? "1" : "0";
                xmlP.Element("ErrorStopCheck").Value = (CB_ErrorStop.Checked == true) ? "1" : "0";
                xmlP.Element("KaisuuStopCheck").Value = (CB_KaisuuStop.Checked == true) ? "1" : "0";
                xmlP.Save(pathxml);
            }
            catch
            {
                CopyXMLfile();
                SetXML(pathxml);
            }
        }
        private void OpenPort()
        {
            //try
            //{
            //    STDCo.PortClose();
            //}
            //catch (Exception) { };
            Connect = Setcom(frm2.cmb_ComPort.Text, frm2.cmb_BaudRate.Text, frm2.cmb_Databit.Text, Paritybit[frm2.cmb_Parity.SelectedIndex], Stopbit[frm2.cmb_Stopbit.SelectedIndex], int.Parse(frm2.cmb_TimeOut.Text));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //STDCo.PortClose();
            //Connect = false;
            //if (!Connect)
            //{
            //    AccessSTT.Text = "接続中";
            //    btn_Open.Text = "ClosePort";
            //}
            frm2.Show();
            this.Hide();
        }

        private void SCom_ComEvent(object sender, ComEventArgs e)
        {
            OpenPort();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ComProcess();
            Itemrow++;
            if (Itemrow > itemnum)
            {
                kaisuu++;
                if (CB_KaisuuStop.Checked == true && kaisuu >= (int.Parse(txt_FixedKaisuu.Text)) && continous) continous = false;
                txt_kaisuu.Text = kaisuu.ToString();
                Itemrow = 0;
                if (!continous)
                {
                    timer1.Enabled = false;
                    btn_OpenFile.Visible = true;
                    btn_COMSetting.Enabled = btn_ReadContinous.Enabled = btn_Read.Enabled = true;
                    txt_Path.Text = pathfile;
                    txt_OnOff.BackColor = SystemColors.Control;
                    btn_ReadContinous.BackColor = Color.White;
                    btn_ReadContinous.ForeColor = Color.SteelBlue;
                }
            }
        }

        private void btn_DataClear_Click(object sender, EventArgs e)
        {
            for (int count = 0; count < itemnum; count++)
            {
                DataGridViewRow Rw = dataGridView1.Rows[count];
                Rw.Cells[ErrorCounter.Index].Value = "0"; 
                Rw.Cells[DataerrorCounter.Index].Value = "0";
            }
            kaisuu = 0;
            txt_kaisuu.Text = kaisuu.ToString();
        }

        private void btn_Open_Click_1(object sender, EventArgs e)
        {
            if (btn_Open.Text == "OpenPort")
            {
                OpenPort();
                if (Connect)
                {
                    if (Connect) firstwrite = true;
                    AccessSTT.Text = "接続中"; 
                    btn_Read.Enabled = btn_ReadContinous.Enabled = true;
                    btn_Open.Text = "ClosePort";
                    txt_Password.Visible = label3.Visible = true;
                }
            }
            else
            {
                STDCo.PortClose();
                Connect = false;
                btn_Read.Enabled = btn_ReadContinous.Enabled = false;
                btn_Open.Text = "OpenPort"; AccessSTT.Text = "未接続";
            }
            txt_Password.Visible = label3.Visible = true;
        }

        private void txt_Password_Leave(object sender, EventArgs e)
        {
            if (sender == txt_Password)
            {
                if (txt_Password.Text == "") txt_Password.Text = 22883.ToString();
                if (int.Parse(txt_Password.Text) > 65535) txt_Password.Text = 22883.ToString();
            }
            if (sender == txt_FixedKaisuu)
            {
                if (txt_FixedKaisuu.Text == "") txt_FixedKaisuu.Text = 100.ToString();
                if (int.Parse(txt_FixedKaisuu.Text) == 0)
                {
                    txt_FixedKaisuu.Text = 100.ToString();
                    MessageBox.Show("固定回数値は1以上のように再入力ください", "範囲以外の入力値", MessageBoxButtons.OK);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("EEPROM_CHECK ソフト終了しますか", "EEPROM_CHECK 終了", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                SetXML(pathxml);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Connect) AccessSTT.Text = "接続中";
            else AccessSTT.Text = "未接続";
            if (txt_OnOff.BackColor != SystemColors.Control)
            {
                cmb_Number.Enabled = btn_Open.Enabled = btn_COMSetting.Enabled = false;
                dataGridView1.ReadOnly = true;

            }
            else
            {
                cmb_Number.Enabled = btn_Open.Enabled = btn_COMSetting.Enabled = true;
                dataGridView1.ReadOnly = false;
            }
        }

        #region Resize
        public struct ControlResize
        {
            public int controlWidth, controlHeight, controlX, controlY;
            //public float fontSize, defaultfontSize;
            public Font font, headerFont;
        }
        private ControlResize RzGet(Control c)
        {
            ControlResize Rz;
            Rz.controlWidth = c.Width;
            Rz.controlHeight = c.Height;
            Rz.controlX = c.Location.X;
            Rz.controlY = c.Location.Y;
            Rz.font = c.Font;
            if (c.GetType() == typeof(DataGridView))
                Rz.headerFont = ((DataGridView)c).ColumnHeadersDefaultCellStyle.Font;
            else Rz.headerFont = null;
            return Rz;
        }
        public List<ControlResize> GetControlResizes(List<Control> con)
        {
            //List<ControlResize> ListCt = new List<ControlResize>();
            //ListCt.Add(Rz);

            for (int i = 0; i < con.Count; i++)
            {

                ListCt.Add(RzGet(con[i]));
            }
            return ListCt;
        }
        public List<ControlResize> ListCt = new List<ControlResize>();

        private void CB_KaisuuStop_CheckedChanged(object sender, EventArgs e)
        {
            txt_FixedKaisuu.Visible = CB_KaisuuStop.Checked;
        }

        private void btn_Browser_Click(object sender, EventArgs e)
        {
            //OpenFileDialog dlg = new OpenFileDialog();
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.SelectedPath))
            {
                //string folderName;
                pathfolder = dlg.SelectedPath;
                //MessageBox.Show(folderName);
            }
        }

        void Resizetool(int i)
        {
            System.Drawing.Size _controlSize;
            System.Drawing.Point _controlposition;
            int controlwidth, controlheight, controlX, controlY;

            controlX = (int)(ListCt[i].controlX * widthRatio);
            controlY = (int)(ListCt[i].controlY * heightRatio);
            controlwidth = (int)(ListCt[i].controlWidth * widthRatio);
            controlheight = (int)(ListCt[i].controlHeight * heightRatio);

            _controlSize = new System.Drawing.Size(controlwidth, controlheight); //use for resizing
            _controlposition = new System.Drawing.Point(controlX, controlY); //use for relocation
            control[i].Bounds = new System.Drawing.Rectangle(_controlposition, _controlSize);
            float a = (widthRatio>heightRatio)?heightRatio:widthRatio;
            control[i].Font = new Font(ListCt[i].font.FontFamily, ListCt[i].font.SizeInPoints*a, ListCt[i].font.Style);
            //control[i].Font = new Font(ListCt[i].font.FontFamily, ListCt[i].font.SizeInPoints * widthRatio * heightRatio, ListCt[i].font.Style);
            if (control[i].GetType() == typeof(DataGridView))
            {
                // Font dataGrid = ((DataGridView)control[i]).DefaultCellStyle.Font;
                Font dataGrid = new Font(ListCt[i].headerFont.FontFamily, ListCt[i].headerFont.SizeInPoints * widthRatio * heightRatio, ListCt[i].headerFont.Style);
                ((DataGridView)control[i]).ColumnHeadersDefaultCellStyle.Font = dataGrid;
                control[i].ForeColor = SystemColors.WindowText;
            }
            //control[i].Font = new Font(control[i].Font.FontFamily, PR.ListCt[i].fontSize * widthRatio * heightRatio, control[i].Font.Style);
            control[i].Refresh();
        }
        private IEnumerable<Control> GetAllControls(Control container)
        {
            List<Control> controlList = new List<Control>();
            foreach (Control c in container.Controls)
            {

                controlList.Add(c);
                controlList.AddRange(GetAllControls(c));

            }
            return controlList;
        }
        void GetResizeInfor()
        {
            control.AddRange(GetAllControls(this));
            GetControlResizes(control);
        }
        void ResizeForm1(Form1 frm)
        {
            //confor.Add(PR.Rz);
            for (int i = 0; i < control.Count; i++)
                Resizetool(i);
            //frm.ResumeLayout(false);
            //frm.PerformLayout();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            widthRatio = (float)this.Width / (float)primaWidth;
            heightRatio = (float)this.Height / (float)primaHeigth;
            ResizeForm1(this);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SLMFuncSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /**********************************************************************
         * Display Test
         * mode 0
        **********************************************************************/
        private void btnTest_Click(object sender, EventArgs e)
        {
            UInt32 DisplayNumber = (UInt32)nupDisplayNumber.Value;
            UInt32 Flags = 0;
            ushort w = 0;
            ushort h = 0;
            SLMFunc.SLM_STATUS ret;

            if (chkRate120.Checked) {
                Flags |= (UInt32)SLMFunc.SLM_FLAGS.FLAGS_RATE120;
            }
            // Display Info
            StringBuilder name = new StringBuilder(256);
            ret = SLMFunc.SLM_Disp_Info2(DisplayNumber, ref w, ref h, name);

            txtStatus.Text = string.Format("{0},{1}:{2}", w, h, name);
            Application.DoEvents();


            // Display Open
            ret = SLMFunc.SLM_Disp_Open(DisplayNumber);

            for(ushort gs = 0; gs < 1024; gs+=50){                
                ret = SLMFunc.SLM_Disp_GrayScale(DisplayNumber, Flags, gs);
                System.Threading.Thread.Sleep(100);
            }

            string folder = "C:\\santec\\SLM-200\\Files\\";

            //ret = SLMFunc.SLM_Disp_ReadBMP(DisplayNumber, Flags, folder + "santec_logo2.bmp");
            //System.Threading.Thread.Sleep(1000);

            // CSV File
            ret = SLMFunc.SLM_Disp_ReadCSV(DisplayNumber, Flags, folder + "Laguerre-Gaussian(LG0,-1).csv");
            System.Threading.Thread.Sleep(2000);

            ushort[] data = new ushort[1920*1200];
            int i,j,pos;
            for (i = 0, pos = 0; i < 1200; i++)
            {
                for (j = 0; j < 1920; j++) data[pos++] = (ushort)(1023 * Math.Abs(Math.Sin(j*720/1920 * Math.PI /180 )));
            }
            // ArrayData
            ret = SLMFunc.SLM_Disp_Data(DisplayNumber,1920,1200, Flags, data);


            System.Threading.Thread.Sleep(2000);

            ret = SLMFunc.SLM_Disp_Close(DisplayNumber);

        }


        /**********************************************************************
         * Display Number Check
        **********************************************************************/
        private void btnCheck_Click(object sender, EventArgs e)
        {
            UInt32 DisplayNumber = (UInt32)nupDisplayNumber.Value;
            ushort w = 0;
            ushort h = 0;
            SLMFunc.SLM_STATUS ret;

            // Display Info
            StringBuilder name = new StringBuilder(256);
            ret = SLMFunc.SLM_Disp_Info2(DisplayNumber, ref w, ref h, name);
            txtStatus.Text = string.Format("{0},{1}:{2}", w, h, name);


        }

        /**********************************************************************
         * USB test
        **********************************************************************/
        private void btnUSBTest_Click(object sender, EventArgs e)
        {
            UInt32 SLMNumber = (UInt32)nupSLMNumber.Value;
            SLMFunc.SLM_STATUS ret;

            AddStatus("USB Open");
            ret = SLMFunc.SLM_Ctrl_Open(SLMNumber);
            if(ret != SLMFunc.SLM_STATUS.SLM_OK) AddStatus(SLMFunc.GetSLMError(ret));

            // OK or BUSY
            for (int retry = 0; retry < 60; retry++)
            {
                ret = SLMFunc.SLM_Ctrl_ReadSU(SLMNumber);
                if (ret == SLMFunc.SLM_STATUS.SLM_OK) break;
                AddStatus(SLMFunc.GetSLMError(ret));
                System.Threading.Thread.Sleep(1000);
            }

            AddStatus("Change memory mode");
            ret = SLMFunc.SLM_Ctrl_WriteVI(SLMNumber, 0);

            UInt32 wavelength = 0, phase = 0;
            ret = SLMFunc.SLM_Ctrl_ReadWL(SLMNumber, ref wavelength,ref phase);
            if (ret == SLMFunc.SLM_STATUS.SLM_OK){
                AddStatus(string.Format("wavelength:{0}, phase:{1}", wavelength, (double)phase / 100));
            }
            else
            {
                AddStatus(SLMFunc.GetSLMError(ret));
            }
            // change wavelenght
            if (wavelength != 635){
                AddStatus("Change wavelenght&phase");
                ret = SLMFunc.SLM_Ctrl_WriteWL(SLMNumber, 635, 200);
                ret = SLMFunc.SLM_Ctrl_WriteAW(SLMNumber);
            }

            string folder = "C:\\santec\\SLM-200\\Files\\";

            ret = SLMFunc.SLM_Ctrl_WriteGS(SLMNumber, 0);

            ret = SLMFunc.SLM_Ctrl_WriteMI_CSV(SLMNumber, 1, 0, folder + "diagonal-grating.csv");
            ret = SLMFunc.SLM_Ctrl_WriteMI_CSV(SLMNumber, 2, 0, folder + "horizontal-grating.csv");
            ret = SLMFunc.SLM_Ctrl_WriteMI_CSV(SLMNumber, 3, 0, folder + "Laguerre-Gaussian(LG0,-1).csv");
            ret = SLMFunc.SLM_Ctrl_WriteMI_CSV(SLMNumber, 4, 0, folder + "santec_logo.csv");
            ushort[] data = new ushort[1920 * 1200];
            int i, j, pos;
            for (i = 0, pos = 0; i < 1200; i++)
            {
                for (j = 0; j < 1920; j++) data[pos++] = (ushort)(1023 * Math.Abs(Math.Sin(j * 720 / 1920 * Math.PI / 180)));
            }
            ret = SLMFunc.SLM_Ctrl_WriteMI(SLMNumber,5,1920,1200,0,data);

            // change interval
            ret = SLMFunc.SLM_Ctrl_WriteMW(SLMNumber, 30);  // 0.5s@30Hz

            // interval start
            ret = SLMFunc.SLM_Ctrl_WriteDR(SLMNumber, 0);
            
            UInt32 dE = 0, oE = 0;
            Int32 dT = 0, oT = 0;
            StringBuilder dID = new StringBuilder(10);
            StringBuilder oID = new StringBuilder(10);

            for (int loop = 0; loop < 10; loop++)
            {
                ret = SLMFunc.SLM_Ctrl_ReadT(SLMNumber,ref dT,ref oT);
                ret = SLMFunc.SLM_Ctrl_ReadEDO(SLMNumber,ref dE,ref oE);
                ret = SLMFunc.SLM_Ctrl_ReadSDO(SLMNumber, dID, oID);
                AddStatus(string.Format("{0}:{1}:{2:F1} {3:F1} {4:X4} {5:X4} {6} {7}",loop, SLMFunc.GetSLMError(ret), 
                    (double)dT/ 10, (double)oT / 10, dE, oE, dID, oID));
                System.Threading.Thread.Sleep(1000);
            }

            // stop
            SLMFunc.SLM_Ctrl_WriteDB(SLMNumber);

            AddStatus("Change DVI mode(40s)");
            // DVI mode = 1
            ret = SLMFunc.SLM_Ctrl_WriteVI(SLMNumber, 1);

            ret = SLMFunc.SLM_Ctrl_Close(SLMNumber);
            AddStatus("End");
        }

        /**********************************************************************
         * Status
        **********************************************************************/
        public delegate void DelegateAddStatus(string msg);
        private void AddStatus(string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateAddStatus(this.AddStatus), new object[] { msg });
                return;
            }
            lstStatus.Items.Add(DateTime.Now.ToString("HH:mm:ss.fff ") + msg);
            lstStatus.TopIndex = lstStatus.Items.Count -1;
            if (lstStatus.Items.Count > 2000)  lstStatus.Items.Remove(0);
            Application.DoEvents();
        }
    }
}

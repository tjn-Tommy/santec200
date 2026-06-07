using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// DllImportに必要
using System.Runtime.InteropServices;


namespace SLMFuncSample
{

    class SLMFunc
    {

        /*****************************************************************************
        / SLM Status Codes
        *****************************************************************************/
        public enum SLM_STATUS : Int32
        {
            SLM_OK = 0, // OK
            SLM_NG = 1, // NG
            SLM_BS = 2, // Busy
            SLM_INVAID_MONITOR = -1, // Not find display no
            SLM_NOT_OPEN_MONITOR = -2, // Not open display
            SLM_OPEN_WINDOW_ERR = -3, // window open Error
            SLM_DATA_FORMAT_ERR = -4, // Data foramt Error


            SLM_FILE_READ_ERR = -101, // Not find  file

            SLM_NOT_OPEN_USB = -200, // Not open usb


            SLM_OTHER_ERROR = -1000,  // other Error

            FT_INVALID_HANDLE = -10001,           //	USB driver error.
            FT_DEVICE_NOT_FOUND = -10002,           //	Check connected device's power.
                                                    //  If connected, reset the power.
            FT_DEVICE_NOT_OPENED = -10003,          //	Already opened.
            FT_IO_ERROR = -10004,                   // USB driver error.
            FT_INSUFFICIENT_RESOURCES = -10005,     // USB driver error.
            FT_INVALID_PARAMETER = -10006,          // USB driver error.
            FT_INVALID_BAUD_RATE = -10007,          // USB driver error.
            FT_DEVICE_NOT_OPENED_FOR_ERASE = -10008, // USB driver error.
            FT_DEVICE_NOT_OPENED_FOR_WRITE = -10009, // USB driver error.
            FT_FAILED_TO_WRITE_DEVICE = -10010,     // USB driver error.
            FT_EEPROM_READ_FAILED = -10011,         // USB driver error.
            FT_EEPROM_WRITE_FAILED = -10012,        // USB driver error.
            FT_EEPROM_ERASE_FAILED = -10013,        // USB driver error.
            FT_EEPROM_NOT_PRESENT = -10014,         // USB driver error.
            FT_EEPROM_NOT_PROGRAMMED = -10015,      // USB driver error.
            FT_INVALID_ARGS = -10016,               // USB driver error.
            FT_NOT_SUPPORTED = -10017,              // USB driver error.
            FT_NO_MORE_ITEMS = -10018,              // USB driver error.
            FT_TIMEOUT = -10019,                    // USB driver error.
            FT_OPERATION_ABORTED = -10020,          // USB driver error.
            FT_RESERVED_PIPE = -10021,              // USB driver error.
            FT_INVALID_CONTROL_REQUEST_DIRECTION = -10022, // USB driver error.
            FT_INVALID_CONTROL_REQUEST_TYPE = -10023, // USB driver error.
            FT_IO_PENDING = -10024,                 // USB driver error.
            FT_IO_INCOMPLETE = -10025,              // USB driver error.
            FT_HANDLE_EOF = -10026,                 // USB driver error.
            FT_BUSY = -10027,                       // USB driver error.
            FT_NO_SYSTEM_RESOURCES = -10028,        // USB driver error.
            FT_DEVICE_LIST_NOT_READY = -10029,      // USB driver error.
            FT_DEVICE_NOT_CONNECTED = -10030,       // USB driver error.
            FT_INCORRECT_DEVICE_PATH = -10031,      // USB driver error.
            FT_OTHER_ERROR = -10032                 // USB driver error.


        }

        public enum SLM_FLAGS : UInt32
        {

            FLAGS_COLOR_R     = 0x00000001,
            FLAGS_COLOR_G     = 0x00000002,
            FLAGS_COLOR_B     = 0x00000004,
            FLAGS_COLOR_GRAY  = 0x00000008,
            FLAGS_COLOR_10BIT = 0x00000100,
            FLAGS_RATE120     = 0x20000000

        }



        /*****************************************************************************
        / SLM Status Codes
        *****************************************************************************/
        public static string GetSLMError(SLM_STATUS ret)
        {
            string msg;

            switch (ret)
            {


                case SLM_STATUS.SLM_OK:
                    msg = "OK";
                    break;
                case SLM_STATUS.SLM_NG:
                    msg = "NG";
                    break;
                case SLM_STATUS.SLM_BS:
                    msg = "Busy";
                    break;
                case SLM_STATUS.SLM_INVAID_MONITOR:
                    msg = "not find display no";
                    break;
                case SLM_STATUS.SLM_NOT_OPEN_MONITOR:
                    msg = "not open display";
                    break;
                case SLM_STATUS.SLM_OPEN_WINDOW_ERR:
                    msg = "open window error";
                    break;
                case SLM_STATUS.SLM_DATA_FORMAT_ERR:
                    msg = "data foramt error";
                    break;
                case SLM_STATUS.SLM_FILE_READ_ERR:
                    msg = "not find  file";
                    break;
                case SLM_STATUS.SLM_NOT_OPEN_USB:
                    msg = "not open USB";
                    break;
                case SLM_STATUS.SLM_OTHER_ERROR:
                    msg = "other error";
                    break;
                case SLM_STATUS.FT_DEVICE_NOT_FOUND:
                    msg = "Check connected device's power. If connected, reset the power.";
                    break;
                case SLM_STATUS.FT_DEVICE_NOT_OPENED:
                    msg = "Already opened.";
                    break;
                default:
                    msg = String.Format("undefined status({0})", ret);
                    break;
            }
            return msg;
        }

#if (true)
        const string DLLFileName = ".\\64\\SLMFunc.dll";
#else
        const string DLLFileName = ".\\32\\SLMFunc.dll";
#endif



        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Disp_Info(UInt32 DisplayNumber, ref UInt16 width, ref UInt16 height);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Disp_Info2(UInt32 DisplayNumber, ref UInt16 width, ref UInt16 height, StringBuilder name);


        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Disp_Open(UInt32 DisplayNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Disp_Close(UInt32 DisplayNumber);


        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Disp_GrayScale(UInt32 DisplayNumber, UInt32 Flags, UInt16 GrayScale);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Disp_BMP(UInt32 DisplayNumber, UInt32 Flags, IntPtr b);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Disp_Data(UInt32 DisplayNumber, UInt16 width, UInt16 height, UInt32 Flags, ushort[] data);


        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern SLM_STATUS SLM_Disp_ReadBMP(UInt32 DisplayNumber, UInt32 Flags, string FileName);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern SLM_STATUS SLM_Disp_ReadCSV(UInt32 DisplayNumber, UInt32 Flags, string FileName);


        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SLM_STATUS SLM_Disp_ReadBMP_A(UInt32 DisplayNumber, UInt32 Flags, string FileName);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SLM_STATUS SLM_Disp_ReadCSV_A(UInt32 DisplayNumber, UInt32 Flags, string FileName);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_Open(UInt32 SLMNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_Close(UInt32 SLMNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteVI(UInt32 SLMNumber, UInt32 mode);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadVI(UInt32 SLMNumber, ref UInt32 mode);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteWL(UInt32 SLMNumber, UInt32 wavelength, UInt32 phase);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadWL(UInt32 SLMNumber, ref UInt32 wavelength, ref UInt32 phase);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteAW(UInt32 SLMNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteTI(UInt32 SLMNumber, UInt32 onoff);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadTI(UInt32 SLMNumber, ref UInt32 onoff);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteTM(UInt32 SLMNumber, UInt32 onoff);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadTM(UInt32 SLMNumber, ref UInt32 onoff);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteTC(UInt32 SLMNumber, UInt32 order);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadTC(UInt32 SLMNumber, ref UInt32 order);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteTS(UInt32 SLMNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteMC(UInt32 SLMNumber, UInt32 MemoryNumber);



        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteMI(UInt32 SLMNumber, UInt32 MemoryNumber, UInt16 width, UInt16 height, UInt32 Flags, ushort[] data);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern SLM_STATUS SLM_Ctrl_WriteMI_BMP(UInt32 SLMNumber, UInt32 MemoryNumber, UInt32 Flags, String FileName);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern SLM_STATUS SLM_Ctrl_WriteMI_CSV(UInt32 SLMNumber, UInt32 MemoryNumber, UInt32 Flags, String FileName);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SLM_STATUS SLM_Ctrl_WriteMI_BMP_A(UInt32 SLMNumber, UInt32 MemoryNumber, UInt32 Flags, String FileName);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SLM_STATUS SLM_Ctrl_WriteMI_CSV_A(UInt32 SLMNumber, UInt32 MemoryNumber, UInt32 Flags, String FileName);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteME(UInt32 SLMNumber, UInt32 MemoryNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteMT(UInt32 SLMNumber, UInt32 TableNumber, UInt32 MemoryNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadMS(UInt32 SLMNumber, UInt32 TableNumber, ref UInt32 MemoryNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteMR(UInt32 SLMNumber, UInt32 TableNumber1, UInt32 TableNumber2);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadMR(UInt32 SLMNumber, ref UInt32 TableNumber1, ref UInt32 TableNumber2);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteMP(UInt32 SLMNumber, UInt32 TableNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteMZ(UInt32 SLMNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteMW(UInt32 SLMNumber, UInt32 frames);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadMW(UInt32 SLMNumber, ref UInt32 frames);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteDS(UInt32 SLMNumber, UInt32 MemoryNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadDS(UInt32 SLMNumber, ref UInt32 MemoryNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteDR(UInt32 SLMNumber, UInt32 order);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteDB(UInt32 SLMNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_WriteGS(UInt32 SLMNumber, UInt16 GrayScale);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadGS(UInt32 SLMNumber, ref UInt16 GrayScale);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadT(UInt32 SLMNumber, ref Int32 deviceTemp, ref Int32 optionTemp);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadEDO(UInt32 SLMNumber, ref UInt32 deviceError, ref UInt32 optionTemp);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadSU(UInt32 SLMNumber);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadSDO(UInt32 SLMNumber, StringBuilder deviceID, StringBuilder optionID);

        [DllImport(DLLFileName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SLM_STATUS SLM_Ctrl_ReadSN(UInt32 SLMNumber, StringBuilder SerialNo);


    }// Class SLMFunc
}

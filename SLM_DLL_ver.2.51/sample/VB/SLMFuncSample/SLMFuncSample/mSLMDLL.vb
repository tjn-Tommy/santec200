Option Strict On
Option Explicit On

Imports System.Runtime.InteropServices
Imports System.Text

Public Module mSLMFuncDLL


    '*****************************************************************************
    '/ SLM Status Codes
    '*****************************************************************************/
    Public Enum SLM_STATUS As Integer
        SLM_OK = 0 ' OK
        SLM_NG = 1 ' NG
        SLM_BS = 2 ' Busy
        SLM_INVAID_MONITOR = -1 ' Not find display no
        SLM_NOT_OPEN_MONITOR = -2 ' Not open display
        SLM_OPEN_WINDOW_ERR = -3 ' window open Error
        SLM_DATA_FORMAT_ERR = -4 ' Data foramt Error


        SLM_FILE_READ_ERR = -101 ' Not find  file

        SLM_NOT_OPEN_USB = -200 ' Not open usb


        SLM_OTHER_ERROR = -1000 ' other Error
    End Enum


    Public Enum SLM_FLAGS As UInteger
        FLAGS_COLOR_R = &H1UI
        FLAGS_COLOR_G = &H2UI
        FLAGS_COLOR_B = &H4UI
        FLAGS_COLOR_GRAY = &H8UI
        FLAGS_COLOR_10BIT = &H100UI
        FLAGS_RATE120 = &H20000000UI

    End Enum

    '*****************************************************************************
    '/ SLM Status Codes
    '*****************************************************************************/
    Function GetSLMError(ret As SLM_STATUS) As String
        Dim msg As String
        Select Case ret
            Case SLM_STATUS.SLM_OK
                msg = "OK"
            Case SLM_STATUS.SLM_NG
                msg = "NG"
            Case SLM_STATUS.SLM_BS
                msg = "Busy"
            Case SLM_STATUS.SLM_INVAID_MONITOR
                msg = "not find display no"
            Case SLM_STATUS.SLM_NOT_OPEN_MONITOR
                msg = "not open display"
            Case SLM_STATUS.SLM_OPEN_WINDOW_ERR
                msg = "open window error"
            Case SLM_STATUS.SLM_DATA_FORMAT_ERR
                msg = "data foramt error"
            Case SLM_STATUS.SLM_FILE_READ_ERR
                msg = "not find  file"
            Case SLM_STATUS.SLM_NOT_OPEN_USB
                msg = "not open USB"
            Case SLM_STATUS.SLM_OTHER_ERROR
                msg = "other error"
            Case Else
                msg = String.Format("undefined status({0})", ret)
        End Select
        Return msg

    End Function


    Private Const DLLFileName As String = "SLMFunc.dll"

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Disp_Info(ByVal DisplayNumber As UInt32, ByRef width As UShort, ByRef height As UShort) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Disp_Info2(ByVal DisplayNumber As UInt32, ByRef width As UShort, ByRef height As UShort, <MarshalAs(UnmanagedType.LPStr)> ByVal name As StringBuilder) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Disp_Open(ByVal DisplayNumber As UInt32) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Disp_Close(ByVal DisplayNumber As UInt32) As Int32
    End Function


    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Disp_GrayScale(ByVal DisplayNumber As UInt32, ByVal Flags As UInt32, ByVal GrayScale As UShort) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Disp_BMP(ByVal DisplayNumber As UInt32, ByVal Flags As UInt32, ByVal b As IntPtr) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Disp_Data(ByVal DisplayNumber As UInt32, ByVal width As UInt16, ByVal height As UInt16, ByVal Flags As UInt32, ByVal data() As UShort) As Int32
    End Function


    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Disp_ReadBMP(ByVal DisplayNumber As UInt32, ByVal Flags As UInt32, <MarshalAs(UnmanagedType.LPWStr)> ByVal Para1 As String) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Disp_ReadCSV(ByVal DisplayNumber As UInt32, ByVal Flags As UInt32, <MarshalAs(UnmanagedType.LPWStr)> ByVal Para1 As String) As Int32
    End Function


    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Disp_ReadBMP_A(ByVal DisplayNumber As UInt32, ByVal Flags As UInt32, <MarshalAs(UnmanagedType.LPStr)> ByVal Para1 As String) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Disp_ReadCSV_A(ByVal DisplayNumber As UInt32, ByVal Flags As UInt32, <MarshalAs(UnmanagedType.LPStr)> ByVal Para1 As String) As Int32
    End Function



    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_Open(ByVal SLMNumber As UInt32) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_Close(ByVal SLMNumber As UInt32) As Int32
    End Function



    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteVI(ByVal SLMNumber As UInt32, ByVal mode As UInt32) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadVI(ByVal SLMNumber As UInt32, ByRef mode As UInt32) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteWL(ByVal SLMNumber As UInt32, ByVal wavelength As UInt32, ByVal phase As UInt32) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadWL(ByVal SLMNumber As UInt32, ByRef wavelength As UInt32, ByRef phase As UInt32) As Int32
    End Function


    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteAW(ByVal SLMNumber As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteTI(ByVal SLMNumber As UInt32, ByVal onoff As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadTI(ByVal SLMNumber As UInt32, ByRef onoff As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteTM(ByVal SLMNumber As UInt32, ByVal onoff As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadTM(ByVal SLMNumber As UInt32, ByRef onoff As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteTC(ByVal SLMNumber As UInt32, ByVal order As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadTC(ByVal SLMNumber As UInt32, ByRef order As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteTS(ByVal SLMNumber As UInt32) As Int32

    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteMC(ByVal SLMNumber As UInt32, ByVal MemoryNumber As UInt32) As Int32

    End Function


    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteMI(ByVal SLMNumber As UInt32, ByVal MemoryNumber As UInt32, ByVal width As UShort, ByVal height As UShort, ByVal Flags As UInt32, ByVal data() As UShort) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteMI_BMP(ByVal SLMNumber As UInt32, ByVal MemoryNumber As UInt32, ByVal Flags As UInt32, <MarshalAs(UnmanagedType.LPWStr)> ByVal FileName As String) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteMI_CSV(ByVal SLMNumber As UInt32, ByVal MemoryNumber As UInt32, ByVal Flags As UInt32, <MarshalAs(UnmanagedType.LPWStr)> ByVal FileName As String) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteME(ByVal SLMNumber As UInt32, ByVal MemoryNumber As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteMT(ByVal SLMNumber As UInt32, ByVal TableNumber As UInt32, ByVal MemoryNumber As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadMS(ByVal SLMNumber As UInt32, ByVal TableNumber As UInt32, ByRef MemoryNumber As UInt32) As Int32

    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteMR(ByVal SLMNumber As UInt32, ByVal TableNumber1 As UInt32, ByVal TableNumber2 As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadMR(ByVal SLMNumber As UInt32, ByRef TableNumber1 As UInt32, ByRef TableNumber2 As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteMP(ByVal SLMNumber As UInt32, ByVal TableNumber As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteMZ(ByVal SLMNumber As UInt32) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteMW(ByVal SLMNumber As UInt32, ByVal frames As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadMW(ByVal SLMNumber As UInt32, ByRef frames As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteDS(ByVal SLMNumber As UInt32, ByVal MemoryNumber As UInt32) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadDS(ByVal SLMNumber As UInt32, ByRef MemoryNumber As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteDR(ByVal SLMNumber As UInt32, ByVal order As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteDB(ByVal SLMNumber As UInt32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_WriteGS(ByVal SLMNumber As UInt32, ByVal GrayScale As UShort) As Int32
    End Function


    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadGS(ByVal SLMNumber As UInt32, ByRef GrayScale As UShort) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadT(ByVal SLMNumber As UInt32, ByRef deviceTemp As Int32, ByRef optionTemp As Int32) As Int32
    End Function
    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadEDO(ByVal SLMNumber As UInt32, ByRef deviceError As UInt32, ByRef optionTemp As UInt32) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadSU(ByVal SLMNumber As UInt32) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadSDO(ByVal SLMNumber As UInt32, <MarshalAs(UnmanagedType.LPStr)> ByVal deviceID As StringBuilder, <MarshalAs(UnmanagedType.LPStr)> ByVal optionID As StringBuilder) As Int32
    End Function

    <System.Runtime.InteropServices.DllImport(DLLFileName, CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Function SLM_Ctrl_ReadSN(ByVal SLMNumber As UInt32, <MarshalAs(UnmanagedType.LPStr)> ByVal SerialNo As StringBuilder) As Int32
    End Function







End Module

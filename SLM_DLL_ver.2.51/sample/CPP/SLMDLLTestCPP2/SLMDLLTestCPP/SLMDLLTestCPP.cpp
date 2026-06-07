
#include "pch.h"
#include <iostream>


const wchar_t WBMP01[] = _T("C:\\santec\\SLM-200\\Files\\Laguerre-Gaussian(LG0,-1).bmp");
const wchar_t WCSV01[] = _T("C:\\santec\\SLM-200\\Files\\Laguerre-Gaussian(LG0,-1).csv");
const char    BMP01[] =    ("C:\\santec\\SLM-200\\Files\\Laguerre-Gaussian(LG0,-1).bmp");
const char    CSV01[] =    ("C:\\santec\\SLM-200\\Files\\Laguerre-Gaussian(LG0,-1).csv");

const wchar_t WBMP02[] = _T("C:\\santec\\SLM-200\\Files\\diagonal-grating.bmp");
const wchar_t WCSV02[] = _T("C:\\santec\\SLM-200\\Files\\diagonal-grating.csv");
const char    BMP02[] =    ("C:\\santec\\SLM-200\\Files\\diagonal-grating.bmp");
const char    CSV02[] =    ("C:\\santec\\SLM-200\\Files\\diagonal-grating.csv");

const wchar_t WBMP03[] = _T("C:\\santec\\SLM-200\\Files\\vertically-grating.bmp");
const wchar_t WCSV03[] = _T("C:\\santec\\SLM-200\\Files\\vertically-grating.csv");
const char    BMP03[] =    ("C:\\santec\\SLM-200\\Files\\vertically-grating.bmp");
const char    CSV03[] =    ("C:\\santec\\SLM-200\\Files\\vertically-grating.csv");

const wchar_t WBMP04[] = _T("C:\\santec\\SLM-200\\Files\\horizontal-grating.bmp");
const wchar_t WCSV04[] = _T("C:\\santec\\SLM-200\\Files\\horizontal-grating.csv");
const char    BMP04[] =    ("C:\\santec\\SLM-200\\Files\\horizontal-grating.bmp");
const char    CSV04[] =    ("C:\\santec\\SLM-200\\Files\\horizontal-grating.csv");


int main()
{

	/*********************************************
	 DVI Display
	********************************************/

	// Display2 Information
	unsigned short width, height;
	char DisplayName[128];
	DWORD DisplayNumber, SLMNumber;
	SLMNumber = 1;
	SLM_STATUS ret;

	/*********************************************
	 USB Open
	********************************************/
	// Open USB interface
	if (SLM_Ctrl_Open(SLMNumber) != SLM_OK) return false;
	// Reads status
	for (int i = 0; i < 100; i++) {
		Sleep(1000);
		ret = SLM_Ctrl_ReadSU(SLMNumber);
		if (ret == SLM_OK) break;
		else if (ret == SLM_BS) {
			printf("BUSY\n");
			Sleep(1000);
			continue;
		}
		else return false;		// error
	}
	if (ret != SLM_OK) return false;

	// Read display mode
	DWORD mode;
	if (ret = SLM_Ctrl_ReadVI(SLMNumber, &mode) == SLM_OK) {
		printf("mode %d\n", mode);
	}
	else return false;

	if (mode != 1) {
		printf("mode changing\n", mode);
		// Set video mode 1(DVI mode)
		if (SLM_Ctrl_WriteVI(SLMNumber, 1) != SLM_OK) return false;
		printf("mode change done\n", mode);
	}


	/*********************************************
	 DVI display
	********************************************/
	// Search LCOS
	for (DisplayNumber = 1; DisplayNumber <= 8; DisplayNumber++){
		if (SLM_Disp_Info2(DisplayNumber, &width, &height, DisplayName) == SLM_OK) {
			printf("%s\n", DisplayName);
			if (strcmp(DisplayName, "LCOS-SLM") > 0) break;
		}
	}

	//DisplayNumber = 1;

	// Open display
	if(ret = SLM_Disp_Open(DisplayNumber) != SLM_OK) return false;
	if(SLM_Disp_GrayScale(DisplayNumber, 0, 256) != SLM_OK) return false;
	Sleep(100);

	HBITMAP hbmp;
	hbmp = (HBITMAP)LoadImage(0, WBMP01,
		IMAGE_BITMAP, 0, 0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);

	// display bmp data
	if (SLM_Disp_BMP(DisplayNumber, 0, hbmp) != SLM_OK) return false;

	USHORT *dat, *pos;
	dat = pos = (USHORT*)malloc(sizeof(short) * 1920 * 1200);
	for (int y = 0; y < 1200; y++) {
		for (int x = 0; x < 1920; x++) {
			*pos = (short)(rand() * 1023);
			pos++;
		}
	}
	// display array data
	if (ret = SLM_Disp_Data(DisplayNumber, 1920, 1200, 0, dat) != SLM_OK) return false;

	Sleep(1000);

	// display csv data(unicode)
	if (ret = SLM_Disp_ReadCSV(DisplayNumber, 0, WCSV02) != SLM_OK) return false;
	Sleep(1000);

	// display csv data(ANSI)
	if (ret = SLM_Disp_ReadCSV_A(DisplayNumber, 0, CSV03) != SLM_OK) return false;
	Sleep(1000);

	// display bmp data(unicode)
	if (ret = SLM_Disp_ReadBMP(DisplayNumber, 0, WBMP02) != SLM_OK) return false;
	Sleep(1000);

	// display bmp data(ANSI)
	if (ret = SLM_Disp_ReadBMP_A(DisplayNumber, 0, BMP01) != SLM_OK) return false;
	Sleep(1000);

	// DVI input to internal memory 1.
	if (ret = SLM_Ctrl_WriteMC(SLMNumber, 1) != SLM_OK) return false;

	// close display
	if(ret = SLM_Disp_Close(DisplayNumber)) return false;


	/*********************************************
	 USB Control
	********************************************/

	// Set video mode 0
	if (SLM_Ctrl_WriteVI(SLMNumber, 0) != SLM_OK) return false;

	// Read display mode
	DWORD wavelength, phase;
	if (ret = SLM_Ctrl_ReadWL(SLMNumber, &wavelength, &phase) == SLM_OK) {
		// OK
		printf("wavelength %d nm, phase %0.2f pai\n", wavelength, ((float)phase) / 100);
	}
	else return false;
	if (wavelength != 1500) {
		// Set wavelength (1500nm) and phase (2pai)
		if (ret = SLM_Ctrl_WriteWL(SLMNumber, 1500, 200) != SLM_OK) return false;
		// Save wavelength asn phase
		if (ret = SLM_Ctrl_WriteAW(SLMNumber) != SLM_OK) return false;
	}

	// entire display 1023
	if (ret = SLM_Ctrl_WriteGS(SLMNumber, 1023) != SLM_OK) return false;

	// read grayscale
	USHORT grayscale;
	if (ret = SLM_Ctrl_ReadGS(SLMNumber, &grayscale) == SLM_OK) {
		printf("grayscale %d\n", grayscale);
	}
	else return false;

	// write array data to memory number 2
	dat = pos = (USHORT*)malloc(sizeof(short) * 1920 * 1200);
	for (int y = 0; y < 1200; y++) {
		for (int x = 0; x < 1920; x++) {
			*pos = (USHORT)(rand() * 1023);	// All data random
			pos++;
		}
	}
	if (ret = SLM_Ctrl_WriteMI(SLMNumber, 2, 1920, 1200, 0, dat) != SLM_OK) return false;

	// bmp file to memory number 3
	if (ret = SLM_Ctrl_WriteMI_BMP(SLMNumber, 3, 0, WBMP01) != SLM_OK) return false;
	
	// bmp file to memory number 4
	if (ret = SLM_Ctrl_WriteMI_BMP_A(SLMNumber, 4, 0, BMP02) != SLM_OK) return false;

	// csv file to memory number 5
	if (ret = SLM_Ctrl_WriteMI_CSV(SLMNumber, 5, 0, WCSV03) != SLM_OK) return false;

	// csv file to memory number 6
	if (ret = SLM_Ctrl_WriteMI_CSV_A(SLMNumber, 6, 0, CSV04) != SLM_OK) return false;

	// table default setting
	if (ret = SLM_Ctrl_WriteMZ(SLMNumber) != SLM_OK) return false;

	// Invalidates phase pattern stored in internal memory.
	if (ret = SLM_Ctrl_WriteME(SLMNumber, 6) != SLM_OK) return false;

	// replace memory number
	if (ret = SLM_Ctrl_WriteMT(SLMNumber, 6, 1) != SLM_OK) return false;

	// Read memory mode
	DWORD MemoryNumber;
	if (ret = SLM_Ctrl_ReadMS(SLMNumber, 6, &MemoryNumber) == SLM_OK) {
		printf("MemoryNumber %d\n", MemoryNumber);
	}
	else return false;

	// effective range
	if (ret = SLM_Ctrl_WriteMR(SLMNumber, 2, 6) != SLM_OK) return false;
	// effective range
	DWORD st, ed;
	if (ret = SLM_Ctrl_ReadMR(SLMNumber, &st, &ed) == SLM_OK) {
		printf("effective range %d - %d\n", st, ed);
	}
	else return false;

	// first display table
	if (ret = SLM_Ctrl_WriteMP(SLMNumber, 3) != SLM_OK) return false;

	// 1s interval
	if (ret = SLM_Ctrl_WriteMW(SLMNumber, 60) != SLM_OK) return false;

	// frames
	DWORD frames;
	if (ret = SLM_Ctrl_ReadMW(SLMNumber, &frames) == SLM_OK) {
		printf("frames %d\n", frames);
	}
	else return false;


	// display memory number1
	if (ret = SLM_Ctrl_WriteDS(SLMNumber, 1) != SLM_OK) return false;
	// Read display mode
	if (ret = SLM_Ctrl_ReadDS(SLMNumber, &MemoryNumber) == SLM_OK) {
		printf("display memory number %d\n", MemoryNumber);
	}
	else return false;

	// continuous display
	if (ret = SLM_Ctrl_WriteDR(SLMNumber, 1) != SLM_OK) return false;
	Sleep(1000);

	// stop
	if (ret = SLM_Ctrl_WriteDB(SLMNumber) != SLM_OK) return false;


	// trigger input on
	if (ret = SLM_Ctrl_WriteTI(SLMNumber, 0) != SLM_OK) return false;
	// read trigger input
	DWORD onoff;
	if (ret = SLM_Ctrl_ReadTI(SLMNumber, &onoff) == SLM_OK) {
		printf("input triger %d\n", onoff);
	}
	else return false;

	// trigger output on
	if (ret = SLM_Ctrl_WriteTM(SLMNumber, 0) != SLM_OK) return false;

	if (ret = SLM_Ctrl_ReadTM(SLMNumber, &onoff) == SLM_OK) {
		printf("ouput triger %d\n", onoff);
	}
	else return false;

	// trigger display order
	if (ret = SLM_Ctrl_WriteTC(SLMNumber, 0) != SLM_OK) return false;

	// Read trigger order
	DWORD order;
	if (ret = SLM_Ctrl_ReadTC(SLMNumber, &order) == SLM_OK) {
		printf("triger order %d\n", onoff);
	}
	else return false;

	// software trigger
	if (ret = SLM_Ctrl_WriteTS(SLMNumber) != SLM_OK) return false;

	// Read display mode
	int dTemp, oTemp;
	if (ret = SLM_Ctrl_ReadT(SLMNumber, &dTemp, &oTemp) == SLM_OK) {
		printf("Drive Board %0.1f degrees, Option Board %0.1f degrees\n", ((float)dTemp) / 10, ((float)oTemp) / 10);
	}
	else return false;

	// Read error
	DWORD driveerr, optionerr;
	if (ret = SLM_Ctrl_ReadEDO(SLMNumber, &driveerr, &optionerr) == SLM_OK) {
		printf("Drive Board Error %4X, Option Board Error %4X\n",driveerr, optionerr);
	}
	else return false;

	// Read display mode
	char driveboardID[16];
	char optionboardID[16];
	if (ret = SLM_Ctrl_ReadSDO(SLMNumber, driveboardID, optionboardID) == SLM_OK) {
		printf("Drive Board ID %s, Option Board ID %s\n", driveboardID, optionboardID);
	}
	else return false;
	
	// Open USB interface
	if (ret = SLM_Ctrl_Close(SLMNumber) != SLM_OK) return false;

	printf("Done\n", driveboardID, optionboardID);

	return true;
	
}

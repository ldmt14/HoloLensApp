// GCodeConverter.cpp : Definiert die exportierten Funktionen für die DLL-Anwendung.
//

#include "stdafx.h"
#include <string>
#include <fstream>
using namespace std;

#define DLLExport __declspec (dllexport)

extern "C" {

	DLLExport void Convert(string gCodePath, string outputPath) {
		ofstream gCodeFile, outputFile;
		gCodeFile.open(gCodePath, ios::in);
		gCodeFile.close();
	}
}
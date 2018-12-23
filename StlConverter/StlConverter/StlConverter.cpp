// StlConverter.cpp : Definiert die exportierten Funktionen für die DLL-Anwendung.
//
#include "stdafx.h"
#include "StlConverter.h"
#include <fstream>
#include <string>
#include <vector>

typedef struct {
	double x, y, z;
} Point;

typedef struct {
	Point normal, v1, v2, v3;
} Triangle;

std::vector<Triangle>* parseASCIIStl(FILE* file) {

}

void convert(char* inputPath, char* outputPath) {
	FILE* inputFile = fopen(inputPath, "r");
	char lineBuffer[501];
	char* currentWord;
	int startOfFile = ftell(inputFile);
	std::vector<Triangle>* triangles;
	
	// check if File starts with "solid"
	fgets(lineBuffer, 500, inputFile);
	currentWord = strtok(lineBuffer, " ");
	if (strcmp(currentWord, "solid") == 0) {
		fseek(inputFile, startOfFile, SEEK_SET);
		triangles = parseASCIIStl(inputFile);
	}
}
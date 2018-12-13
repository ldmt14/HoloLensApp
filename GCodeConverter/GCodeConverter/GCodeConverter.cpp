// GCodeConverter.cpp : Definiert die exportierten Funktionen für die DLL-Anwendung.
//

#include "stdafx.h"
#include <string>
#include <fstream>
#include <iostream>

using namespace std;

#define DLLExport __declspec (dllexport)
constexpr auto STATE_DEFAULT = 0;
constexpr auto STATE_MOVE = 1;
constexpr auto STATE_MOVE_X = 2;
constexpr auto STATE_MOVE_Y = 3;
constexpr auto STATE_MOVE_Z = 4;
constexpr auto STATE_MOVE_E = 5;
constexpr auto STATE_COMMAND_G = 6;
constexpr auto STATE_ENDOFLINE = 7;
constexpr auto STATE_ENDOFLAYER = 8;
constexpr auto STATE_ENDOFFILE = 9;
constexpr auto STATE_COMMENT = 10;
constexpr auto STATE_RESETPOSITION = 11;

constexpr auto BUFFER_SIZE = 500;
constexpr auto BUFFER_OFFSET = 10;


typedef struct {
	float x, y, z;
} Point;

int operator != (Point a, Point b) {
	return (a.x != b.x) || (a.y != b.x) || (a.z != b.z);
}

template <class t> class ArrayList {
	int capacity;
	int length = 0;
	t * list;

public:
	ArrayList(int c) {
		if (c <= 0) {
			throw invalid_argument("The capacity must be a positive number.");
		}
		capacity = c;
		list = (t*)malloc(sizeof(t) * capacity);
		if (list == NULL) {
			cout << "Memory Allocation Failed";
			exit(1);
		}
	}
public:
	void add(t* p) {
		if (length >= capacity) {
			capacity *= 2;
			t* newArray = (t*)malloc(sizeof(t) * capacity);
			if (newArray == NULL) {
				cout << "Memory Allocation Failed";
				exit(1);
			}
			memcpy(newArray, list, length * sizeof(t));
			free(list);
			list = newArray;
		}
		list[length++] = *p;
	}
public:
	Point operator [](int index) {
		if (index < 0) {
			throw invalid_argument("The index can't be negative.");
		}
		if (index >= length) {
			throw invalid_argument("Index out of bounds");
		}
		return list[index];
	}
};

extern "C" {
	ArrayList<Point>* parseLayer(FILE* file) {
		int parseState = STATE_DEFAULT;
		ArrayList<Point>* points = new ArrayList<Point>(10);
		Point currentPos;
		Point newPos;
		float currentExtruder = 0;
		float newExtruder;
		bool currentPosIsAdded = 0;
		int bufferIndex = BUFFER_SIZE;
		char** endptr = 0;
		char charbuffer[BUFFER_SIZE];
		currentPos.x = currentPos.y = currentPos.z = 0.f;
		while (1) {
			if (bufferIndex >= BUFFER_SIZE - BUFFER_OFFSET) {
				bufferIndex -= (BUFFER_SIZE - BUFFER_OFFSET);
				for (int i = bufferIndex; i < BUFFER_OFFSET; i++) {
					charbuffer[i] = charbuffer[BUFFER_SIZE - BUFFER_OFFSET + i];
				}
				int numberRead = fread(charbuffer + BUFFER_OFFSET, sizeof(char), BUFFER_SIZE - BUFFER_OFFSET, file);
				if (numberRead == 0) {
					parseState = STATE_ENDOFFILE;
				} else if (numberRead < BUFFER_SIZE - BUFFER_OFFSET) {
					for (int i = numberRead + BUFFER_OFFSET - 1; i >= 0; i--) {
						charbuffer[i + BUFFER_SIZE - BUFFER_OFFSET - numberRead] = charbuffer[i];
					}
					bufferIndex += BUFFER_SIZE - BUFFER_OFFSET - numberRead;
				}
			}
			switch (parseState) {
			case STATE_DEFAULT:
				switch (charbuffer[bufferIndex]) {
				case ' ': case '\n': break;
				case 'G': parseState = STATE_COMMAND_G;
					break;
				case ';':
				default: parseState = STATE_COMMENT;
					break;
				}
				break;
			case STATE_COMMAND_G:
				switch (strtol(charbuffer + bufferIndex, endptr, 10)) {
				case 0: case 1:
					parseState = STATE_MOVE;
					newPos = currentPos;
					newExtruder = currentExtruder;
					break;
				case 92:
					parseState = STATE_RESETPOSITION;
					break;
				default:
					parseState = STATE_COMMENT;
				}
				bufferIndex = *endptr - charbuffer - 1;
				break;
			case STATE_MOVE:
				switch (charbuffer[bufferIndex]) {
				case ' ': break;
				case '\n': parseState = STATE_ENDOFLINE; break;
				case 'X': parseState = STATE_MOVE_X; break;
				case 'Y': parseState = STATE_MOVE_Y; break;
				case 'Z': parseState = STATE_MOVE_Z; break;
				case 'E': parseState = STATE_MOVE_E; break;
				}
				break;
			case STATE_MOVE_X:
				newPos.x = strtof(charbuffer + bufferIndex, endptr);
				parseState = STATE_MOVE;
				bufferIndex = *endptr - charbuffer - 1;
				break;
			case STATE_MOVE_Y:
				newPos.y = strtof(charbuffer + bufferIndex, endptr);
				parseState = STATE_MOVE;
				bufferIndex = *endptr - charbuffer - 1;
				break;
			case STATE_MOVE_Z:
				newPos.z = strtof(charbuffer + bufferIndex, endptr);
				parseState = STATE_MOVE;
				bufferIndex = *endptr - charbuffer - 1;
				break;
			case STATE_MOVE_E:
				newExtruder = strtof(charbuffer + bufferIndex, endptr);
				parseState = STATE_MOVE;
				bufferIndex = *endptr - charbuffer - 1;
				break;
			case STATE_ENDOFLINE:
				if (newExtruder < currentExtruder) {
					if (!currentPosIsAdded) {
						points->add(&currentPos);
					}
					currentExtruder = newExtruder;
					points->add(&newPos);
					currentPos = newPos;
					currentPosIsAdded = 1;
				}
				else {
					if (currentPos != newPos) {
						currentPos = newPos;
						currentPosIsAdded = false;
					}
				}
				parseState = STATE_DEFAULT;
				break;
			case STATE_RESETPOSITION:
			case STATE_COMMENT:
				if (charbuffer[bufferIndex] == '\n') parseState = STATE_DEFAULT;
				break;
			case STATE_ENDOFFILE: case STATE_ENDOFLAYER:
				return points;
			default: parseState = STATE_COMMENT;
				break;
			}
			bufferIndex++;
		}
		return points;
	}

	DLLExport void Convert(string gCodePath, string outputPath) {
		ofstream  outputFile;
		FILE* gCodeFile;
		fopen_s(&gCodeFile, &gCodePath[0], "r");
		fclose(gCodeFile);
	}
}
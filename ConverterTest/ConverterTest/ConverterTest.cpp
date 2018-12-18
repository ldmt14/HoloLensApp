#include "pch.h"
#include "GCodeConverter.h"
#include <iostream>


extern "C" {

	int main(int argc, const char* argv[])
	{
		Point lastPos;
		ArrayList<Point>* points;
		if (argc != 2) {
			printf("usage: Test <input>\n");
			exit(1);
		}
		FILE* gCodeFile;
		fopen_s(&gCodeFile, argv[1], "r");
		if (gCodeFile == NULL) {
			printf("Could not open file %s\n", argv[1]);
			exit(1);
		}
		points = parseLayer(gCodeFile, &lastPos);
		printf("%d points found\n", points->getLength());
		for (int i = 0; i < points->getLength(); i++) {
			printf("x = %f, y = %f, z = %f\n", (*points)[i].x, (*points)[i].y, (*points)[i].z);
		}
		return 0;
	}
}
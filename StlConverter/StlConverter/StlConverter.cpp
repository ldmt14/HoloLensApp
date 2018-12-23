// StlConverter.cpp : Definiert die exportierten Funktionen für die DLL-Anwendung.
//
#include "stdafx.h"
#include "StlConverter.h"
#include <fstream>
#include <string>
#include <vector>

#define STATE_DEFAULT 0
#define STATE_OUTERLOOP 1
#define STATE_V1 2
#define STATE_V2 3
#define STATE_V3 4
#define STATE_ENDLOOP 5
#define STATE_ENDFACET 6

typedef struct {
	double x, y, z;
} Point;

typedef struct {
	Point normal, v1, v2, v3;
} Triangle;

typedef struct {
	int normal, v1, v2, v3;
} Indexed_Triangle;

std::vector<Triangle>* parseASCIIStl(FILE* file) {
	char lineBuffer[501];
	std::vector<Triangle>* result = new std::vector<Triangle>();
	int state = STATE_DEFAULT;
	char* currentWord;
	Triangle currentTriangle;

	//skip header line
	fgets(lineBuffer, 500, file);
	for (fgets(lineBuffer, 500, file); lineBuffer; fgets(lineBuffer, 500, file)) {
		//skip empty lines
		while (strlen(lineBuffer) == 0) {
			if (fgets(lineBuffer, 500, file) == NULL) {
				return result;
			}
		}
		switch (state) {
		case STATE_DEFAULT:
			currentWord = strtok(lineBuffer, " \t\n");
			if (strcmp(currentWord, "facet") != 0) {
				printf("Format Error");
				exit(1);
			}
			currentWord = strtok(NULL, " \t\n");
			if (strcmp(currentWord, "normal") != 0) {
				printf("Format Error");
				exit(1);
			}
			currentWord = strtok(NULL, " \t\n");
			currentTriangle.normal.x = strtod(currentWord, NULL);
			currentWord = strtok(NULL, " \t\n");
			currentTriangle.normal.y = strtod(currentWord, NULL);
			currentWord = strtok(NULL, " \t\n");
			currentTriangle.normal.z = strtod(currentWord, NULL);
			state = STATE_OUTERLOOP;
			break;
		case STATE_OUTERLOOP:
			currentWord = strtok(lineBuffer, " \t\n");
			if (strcmp(currentWord, "outer") != 0) {
				printf("Format Error");
				exit(1);
			}
			currentWord = strtok(NULL, " \t\n");
			if (strcmp(currentWord, "loop") != 0) {
				printf("Format Error");
				exit(1);
			}
			state = STATE_V1;
			break;
		case STATE_V1:
			currentWord = strtok(lineBuffer, " \t\n");
			if (strcmp(currentWord, "vertex") != 0) {
				printf("Format Error");
				exit(1);
			}
			currentWord = strtok(NULL, " \t\n");
			currentTriangle.v1.x = strtod(currentWord, NULL);
			currentWord = strtok(NULL, " \t\n");
			currentTriangle.v1.y = strtod(currentWord, NULL);
			currentWord = strtok(NULL, " \t\n");
			currentTriangle.v1.z = strtod(currentWord, NULL);
			state = STATE_V2;
			break;
		case STATE_V2:
			currentWord = strtok(lineBuffer, " \t\n");
			if (strcmp(currentWord, "vertex") != 0) {
				printf("Format Error");
				exit(1);
			}
			currentWord = strtok(NULL, " \t\n");
			currentTriangle.v2.x = strtod(currentWord, NULL);
			currentWord = strtok(NULL, " \t\n");
			currentTriangle.v2.y = strtod(currentWord, NULL);
			currentWord = strtok(NULL, " \t\n");
			currentTriangle.v2.z = strtod(currentWord, NULL);
			state = STATE_V3;
			break;
		case STATE_V3:
			currentWord = strtok(lineBuffer, " \t\n");
			if (strcmp(currentWord, "vertex") != 0) {
				printf("Format Error");
				exit(1);
			}
			currentWord = strtok(NULL, " \t\n");
			currentTriangle.v3.x = strtod(currentWord, NULL);
			currentWord = strtok(NULL, " \t\n");
			currentTriangle.v3.y = strtod(currentWord, NULL);
			currentWord = strtok(NULL, " \t\n");
			currentTriangle.v3.z = strtod(currentWord, NULL);
			state = STATE_ENDLOOP;
			break;
		case STATE_ENDLOOP:
			currentWord = strtok(lineBuffer, " \t\n");
			if (strcmp(currentWord, "endloop") != 0) {
				printf("Format Error");
				exit(1);
			}
			state = STATE_ENDFACET;
			break;
		case STATE_ENDFACET:
			currentWord = strtok(lineBuffer, " \t\n");
			if (strcmp(currentWord, "endfacet") != 0) {
				printf("Format Error");
				exit(1);
			}
			result->push_back(currentTriangle);
			state = STATE_DEFAULT;
			break;
		}
	}
	return result;
}

bool operator==(Point a, Point b) {
	return a.x == b.x && a.y == b.y && a.z == b.z;
}

bool contains(std::vector<Point>* points, Point* point, unsigned int* index) {
	for (*index = 0; *index < points->size(); (*index)++) {
		if ((*points)[*index] == *point) {
			return true;
		}
	}
	return false;
}

void writeObj(char* outputPath, char* name, std::vector<Triangle>* list) {
	std::vector<Point> vertices;
	std::vector<Point> normals;
	std::vector<Indexed_Triangle> triangles;
	Indexed_Triangle currentTriangle;
	unsigned int index;
	FILE* outputFile;

	for (unsigned int i = 0; i < list->size(); i++) {
		if (contains(&normals, &(*list)[i].normal, &index)) {
			currentTriangle.normal = index;
		}
		else {
			vertices.push_back((*list)[i].normal);
			currentTriangle.normal = index;
		}

		if (contains(&vertices, &(*list)[i].v1, &index)) {
			currentTriangle.v1 = index;
		}
		else {
			vertices.push_back((*list)[i].v1);
			currentTriangle.v1 = index;
		}

		if (contains(&vertices, &(*list)[i].v2, &index)) {
			currentTriangle.v2 = index;
		} else {
			vertices.push_back((*list)[i].v2);
			currentTriangle.v2 = index;
		}

		if (contains(&vertices, &(*list)[i].v3, &index)) {
			currentTriangle.v3 = index;
		} else {
			vertices.push_back((*list)[i].v3);
			currentTriangle.v3 = index;
		}
		triangles.push_back(currentTriangle);
	}
	outputFile = fopen(outputPath, "w");

	fprintf(outputFile, "o %s\n", name);

	for (unsigned int i = 0; i < vertices.size(); i++) {
		fprintf(outputFile, "v %f %f %f\n", vertices[i].x, vertices[i].y, vertices[i].z);
	}

	for (unsigned int i = 0; i < normals.size(); i++) {
		fprintf(outputFile, "vn %f %f %f\n", normals[i].x, normals[i].y, normals[i].z);
	}

	for (unsigned int i = 0; i < triangles.size(); i++) {
		fprintf(outputFile, "f %d//%d %d//%d %d//%d\n", triangles[i].v1, triangles[i].normal, triangles[i].v2, triangles[i].normal, triangles[i].v3, triangles[i].normal);
	}
	fclose(outputFile);
}

void convert(char* inputPath, char* outputPath) {
	FILE* inputFile = fopen(inputPath, "r");
	char lineBuffer[501];
	char *currentWord, *name = NULL;
	int startOfFile = ftell(inputFile);
	std::vector<Triangle>* triangles = NULL;
	
	// check if File starts with "solid"
	fgets(lineBuffer, 500, inputFile);
	currentWord = strtok(lineBuffer, " ");
	if (strcmp(currentWord, "solid") == 0) {
		name = strtok(NULL, " \t\n");
		fseek(inputFile, startOfFile, SEEK_SET);
		triangles = parseASCIIStl(inputFile);
	}
	fclose(inputFile);
	writeObj(outputPath, name, triangles);
}
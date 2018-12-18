#pragma once

#ifdef GCODECONVERTER_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif

#include <fstream>
#include <string>

using namespace std;

template<class t> class ArrayList {
	int capacity;
	int length;
	t* list;
public:
	ArrayList(int c);
public:
	void add(t* p);
public:
	int getLength();
public:
	t operator [](int index);
};

extern "C" DLL_API typedef struct {
		float x, y, z;
	} Point;

extern "C" DLL_API ArrayList<Point>* parseLayer(FILE* file, Point* lastPos);

extern "C" DLL_API void Convert(string gCodePath, string outputPath);

template class DLL_API ArrayList<Point>;

int operator != (Point a, Point b);
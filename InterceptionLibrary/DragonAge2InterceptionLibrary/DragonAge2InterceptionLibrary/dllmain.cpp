// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <Windows.h>

#include <metahost.h>

#pragma comment(lib, "mscoree.lib")

#import "mscorlib.tlb" raw_interfaces_only \
    high_property_prefixes("_get","_put","_putref") \
    rename("ReportEvent", "InteropServices_ReportEvent")

HANDLE hMenuOrDialogueEnteredEvent;
HANDLE hMenuOrDialogueExitedEvent;
HANDLE hLoadingScreenEnteredEvent;
HANDLE hLoadingScreenExitedEvent;
int* isLoadingScreenFlagPointer;

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
		case DLL_PROCESS_ATTACH:
			isLoadingScreenFlagPointer = (int*)(*((int*)0xD5869C) + 0x1D0);
			hMenuOrDialogueEnteredEvent = CreateEvent(NULL, FALSE, FALSE, TEXT("GlobalDA2MenuOrDialogueEntered"));
			hMenuOrDialogueExitedEvent = CreateEvent(NULL, FALSE, FALSE, TEXT("GlobalDA2MenuOrDialogueExited"));
			hLoadingScreenEnteredEvent = CreateEvent(NULL, FALSE, FALSE, TEXT("GlobalDA2LoadingScreenEntered"));
			hLoadingScreenExitedEvent = CreateEvent(NULL, FALSE, FALSE, TEXT("GlobalDA2LoadingScreenExited"));
			break;
		case DLL_THREAD_ATTACH:
			break;
		case DLL_THREAD_DETACH:
			break;
		case DLL_PROCESS_DETACH:
			CloseHandle(hMenuOrDialogueEnteredEvent);
			CloseHandle(hMenuOrDialogueExitedEvent);
			CloseHandle(hLoadingScreenEnteredEvent);
			CloseHandle(hLoadingScreenExitedEvent);
			break;
    }
    return TRUE;
}

bool processMenuOrDialogueEntered = true;
bool processMenuOrDialogueExited = true;
bool processLoadingScreenEntered = true;
bool processLoadingScreenExited = true;

extern "C"
{
	void __declspec(dllexport) __declspec(naked) MenuOrDialogueEnteredHook()
	{
		__asm 
		{
			pushad
			pushfd
		}

		if (processLoadingScreenEntered)
		{
			processMenuOrDialogueEntered = false;
			processMenuOrDialogueExited = true;

			SetEvent(hMenuOrDialogueEnteredEvent);
		}

		__asm
		{
			mov eax, 0xD54492
			mov byte ptr[eax], 0x00
			popfd
			popad
			ret
		}
	}

	void __declspec(dllexport) __declspec(naked) MenuOrDialogueExitedHook()
	{
		__asm 
		{
			pushad
			pushfd
		}

		if (processMenuOrDialogueExited)
		{
			processMenuOrDialogueEntered = true;
			processMenuOrDialogueExited = false;

			SetEvent(hMenuOrDialogueExitedEvent);
		}
		
		__asm 
		{
			mov eax, 0xD54492
			mov byte ptr[eax], 0x01
			popfd
			popad
			ret
		}
	}

	void __declspec(dllexport) __declspec(naked) LoadingScreenEnteredHook()
	{
		__asm
		{
			pushad
			pushfd
		}


		if (processLoadingScreenEntered)
		{
			processLoadingScreenEntered = false;
			processLoadingScreenExited = true;
			
			SetEvent(hLoadingScreenEnteredEvent);
		}

		__asm 
		{
			popfd
			popad
			mov byte ptr[esi + 0x110], 01
			ret
		}
	}

	void __declspec(dllexport) __declspec(naked) LoadingScreenExitedHook()
	{
		__asm 
		{
			pushad
			pushfd
		}

		if (processLoadingScreenExited)
		{
			processLoadingScreenEntered = true;
			processLoadingScreenExited = false;

			SetEvent(hLoadingScreenExitedEvent);
		}

		__asm {
			popfd
			popad
			mov byte ptr[esi + 00000110], 00
			ret
		}
	}
}


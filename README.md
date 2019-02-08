# HoloLensApp

## Bauen des Projekts:
1. Stellen Sie sicher, dass alle Tools (Visual Studio, Windows 10 SDK, Unity) wie [hier](https://docs.microsoft.com/en-us/windows/mixed-reality/install-the-tools) beschrieben installiert sind.
1. Das Projekt oder die Datei 'Assets/Scenes/Main.unity' in Unity öffnen.
1. Wählen Sie **File > Build Settings**
1. Wählen Sie **Universal Windows Platform** aus und klicken Sie **Switch Platform**
1. Setzen Sie **Target Device** zu **Any Device** oder **HoloLens**
1. Setzen Sie **Build Type** zu **D3D**
1. Setzen Sie **SDK** und **Visual Studio Version** zu **Latest Installed**
1. Setzen Sie ein Häkchen bei **Unity C\# Projects**
1. Stellen Sie sicher, dass unter **Scenes in Build** die Szene **Scene/Main** vorhanden ist und das Häkchen gesetzt ist. Falls nicht drücken sie **Add Open Scenes**
1. Bauen Sie das Projekt mit **Build**

Für mehr Details siehe [hier](https://docs.microsoft.com/en-us/windows/mixed-reality/exporting-and-building-a-unity-visual-studio-solution)

## Ausführen des Builds:
1. Öffnen Sie die von Unity generierte Visual Studio Solution 'HoloLensApp.sln' im Build-Ordner in Visual Studio
1. Wählen Sie in der Kopfzeile als Konfiguraion **Release** statt **Debug** und als Architektur **x86**
1. Führen Sie das Programm aus unter **Debuggen > Starten ohne Debuggen**

Für mehr Details siehe [hier](https://docs.microsoft.com/en-us/windows/mixed-reality/using-visual-studio)
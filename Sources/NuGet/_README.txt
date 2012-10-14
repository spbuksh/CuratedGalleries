
----------------- Description
NuGet solution folder contains projects for NuGet package creation.

----------------- Rules
All 3rd party components must be included into the project via NuGet (either from online storage or from local repository)!!!

----------------- Tools to install:
1. In order to create NuGet project go to the (File)>(New)>(Project>(Online Templates) and search "NuGet packager". Install this template. Based on this template all NuGet packages must be created (all must use the same approach).
If you know more convinient way to create NuGet package projects let us know!
2. NuGet package explorer http://nuget.codeplex.com/releases/view/59864

----------------- Steps
1. Add here your NuGet package creation project

2. Edit BuildPublishPackage.cmd file in the new project

3. Edit Package.nuspect: pay attension to dependencies!

4. Edit post build event in project properties. Add next 2 lines

call "$(ProjectDir)BuildPublishPackage.cmd"
xcopy /r /y "$(ProjectDir)*.nupkg" "..\..\..\..\..\NuGet.Repository\"

These commands copy NuGet package in to the $\CorbisPremium\NuGet.Repository\ folder. THIS FOLDER IS TARGETED TO STORE NUGET PACKAGES!!! Go to the (Tools)>(Options)>(Package Managers)>(Package Sources).
Add NuGet.Repository forlder as package sources! 

5. Packages must be added into the source control. In order to optimize solution build process unload all projects from NuGet solution folder




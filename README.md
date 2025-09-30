# C/AL-AL Code Analyzer

This code analyzer is meant to help anyone, who runs into some errors during the C/AL -> AL code conversion or OnPrem -> Cloud upgrade/conversion.

## What does it do?

Many feautures, which were supported back in NAV or in BC OnPrem version doesn't work in the Cloud. Microsoft analyzer usually tells you something like:

"Codeunit 'Something' is missing.

The application object or method 'something' has scope 'OnPrem' and cannot be used for 'Extension/Cloud' development.

Then you have to search the documentation or articles, how to rewrite/replace it. This analyzer should help you with some of these errors.

You can find many fixes and code samples, how to fix some of the errors/warnings here https://github.com/microsoft/ALAppExtensions/blob/main/BREAKINGCHANGES.md

## Installation

For now, this analyzer can be installed only as direct path in settings.json. Download your AL language specific version or just general file (at the very bottom) from <b>Releases</b>. 
![Releases](<Screenshot 2025-09-04 093524.png>)

Google Chrome and Edge will require to confirm you really want to download the file (because of type .dll). Confirm and save the .dll file wherever you like, just avoid using Users folders like Documents as it might cause some issues with loading the analyzer.
Then in your global settings.json add a path to "al.codeAnalyzers" like this (it’s very likely, you already have some code cops there):

"al.codeAnalyzers": [
        "${CodeCop}",
        "${UICop}",
        "${PerTenantExtensionCop}",
        <b>"C:/SomeFolder/Analyzer/CustomCodeCop.dll"</b>,
    ]

Be aware that if you use workspace file or local settings.json, the codeAnalyzers from the global settings.json won’t be loaded. If you want to use it in the workspace as well, you need to add it in the workspace file, same way as in global settings.json.

## Custom snippets file

Another big help is to create you own snippets file. You can find the provided <i>ALCustomSnippets</i> file in the <b>assets</b> folder. It contains some snippets, which can help you better understand, how to rewrite old code like Dotnet Excel or old Email functions. Download this file directly from the repository and paste it to <i>C:\Users\your_username\AppData\Roaming\Code\User\snippets</i>. 

You can then access it from VS Code like this: <i>File -> Preferences -> Configure Snippets</i>.

## Reimplementation Cheat sheet

This analyzer doesn't aim to cover every possible DotNet/OnPrem/Removed object - it targets the objects/functions which I encountered repeatedly during C/AL -> AL conversions. You can fiind this sheet in the <b>assets</b> folder.
This sheet was shown on BC Tech Days 2023. Side by side compared old DotNet/OnPrem/Removed objects with new, Cloud ready implementations. Since it's from 2023, some things may be little different today, but not by much.

Disclaimer: This document was created by BCILITY Business Software Solution and was shown during BC Tech Days 2023,  I'm not a creator of this document.

## Disclaimer

Inspired by LinterCop from Stefan Maron. Big thanks to him for creating a custom template for custom analyzer, which helped me to get started with this.
If you're not using LinterCop yet, you can check how it works at https://github.com/StefanMaron/BusinessCentral.LinterCop .

# C/AL-AL Code Analyzer

This code analyzer is meant to help anyone, who runs into some errors during the C/AL -> AL code conversion.

## What does it do?

Many feautures, which were supported back in NAV or in BC OnPrem version doesn't work in the Cloud. Microsoft analyzer usually tells you something like:

"Codeunit 'Something' is missing.

The application object or method 'something' has scope 'OnPrem' and cannot be used for 'Extension/Cloud' development.

Then you have to search the documentation or articles, how to rewrite/replace it. This analyzer should help you with some of these errors.

You can find many fixes and code samples, how to fix some of the errors/warnings here https://github.com/microsoft/ALAppExtensions/blob/main/BREAKINGCHANGES.md

## Custom snippets file

Another big help is to create you own snippets file. The provided ALCustomSnippets file contains some snippets, which can help you better understand, how to rewrite old code like Dotnet Excel or old Email functions. Paste this file to C:\Users\your_username\AppData\Roaming\Code\User\snippets. You can then access it from VS Code like this: File -> Preferences -> Configure Snippets.

## Disclaimer

Inspired by LinterCop from Stefan Maron. Big thanks to him for creating a custom template for custom analyzer, which helped me to get started with this.
If you're not using LinterCop yet, you can check how it works at https://github.com/StefanMaron/BusinessCentral.LinterCop .

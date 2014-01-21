source-code-watermark
=====================

Utility to add release number to source code files.

![Alt text](/img/AddWatermarkToSourceCode.png "Add watermark to source code")

Supported comment symbols and file extensions are contained inside `src/CommentSymbols.txt`.
The watermark text can be found and edited inside `src/Watermark.txt`. A place-holder exists for the provided release number (`RELEASE_NUMBER`) and for the date (`TIMESTAMP`). 

Files are processed using the [Parallel Loop pattern](http://msdn.microsoft.com/en-us/library/ff963552.aspx). The property `ThreadsUsedToProcessFiles` contains the number of threads involved in the watermarking process.

The watermark is appended to each file using a `MemoryStream`. The `Stream.CopyTo` method avoids to read the whole file into memory. The buffer size is calculated for each watermark.

The project includes a couple of unit tests that can be run on Windows using `unit-test/run-nunit-tests.bat` (assuming you have NUnit installed and available in your path).
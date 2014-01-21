source-code-watermark
=====================

Utility to add release number to source code files.

Supported comment symbols and file extensions are contained inside `src/CommentSymbols.txt`.
Watermark text can be found and edited inside `src/Watermark.txt`. A place-holder exists for the provided release number (`RELEASE_NUMBER`) and for the date (`TIMESTAMP`). 

Files are processed using the [Parallel Loop pattern link](http://msdn.microsoft.com/en-us/library/ff963552.aspx). The property `ThreadsUsedToProcessFiles` contains the number of threads involved in the watermarking process.

Watermark is appended to each file using a `MemoryStream`. In order to avoid reading the whole file into memory I used the `Stream.CopyTo` method. The buffer size is calculated for each watermark.

The project includes a couple of unit tests that can be run on Windows using `unit-test/run-nunit-tests.bat` (assuming you have NUnit installed and available in your path).
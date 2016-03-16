ECHO Run this batch file to clean the existing reference of all samples
ECHO This is necessary when you switch all samples to switch between referencing the Debug and Release build of SDK kernal libraries.

del /S /Q eBayFetch\bin
del /S /Q eBayFetch\obj
rmdir /S /Q eBayFetch\bin
rmdir /S /Q eBayFetch\obj


# opus_gui
A GUI to process music files into Opus.

![v1.6 Screenshot](https://moisescardona.me/files/2019-02-02/1.PNG)

I wrote this software to convert my FLAC music collection to the Opus format.

You need to copy `opusenc.exe` to the location where you have this software. You can also use ffmpeg if you have it in your system.

Written in Visual Basic .NET using Visual Studio 2017.

By default, non-compatible files are copied to the output folder. You can make an "ignore.txt" file to ignore certain file types from being copied. Just add the file types like ".accurip", ".cue", etc and they will not be copied. FLACs and WAVs will be encoded.

Enjoy!

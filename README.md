# Opus GUI
A GUI to encode music files into Opus.

![v1.13.2 Screenshot](https://moisescardona.me/wp-content/uploads/2019/08/Opus-GUI-v1.13.2.png)

It allows you to encode files to Opus using the following encoding methods:

* opusenc (libopus)
* ffmpeg - libopus
* ffmpeg - opus (native ffmpeg opus library, using CELT only)

The software comes bundled with the Opus encoder. You will need to download ffmpeg to use it if it is not in your system.

You can get updated Opus Tools builds at my site here: [https://moisescardona.me/opusenc-builds/](https://moisescardona.me/opusenc-builds/).

Written in Visual Basic .NET using Visual Studio 2017.

By default, non-compatible files are copied to the output folder. You can make an "ignore.txt" file to ignore certain file types from being copied. Just add the file types like ".accurip", ".cue", etc and they will not be copied. FLACs and WAVs will be encoded.

Enjoy!

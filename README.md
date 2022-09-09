# KeyChordTester

This fork adds KEYUP, SYSKEYDOWN, and SYSKEYUP printing.

Just a small tool to print received key strokes and their appropriate timestamps.
All timestamps are in milliseconds and relative to the Windows boot time.
If 2 or more timestamps are equal it means windows received them in at the same time with the same poll.

## Downloads

Downloads are available in [Releases](https://github.com/Kuuuube/KeyChordTester/releases).

## Dependencies

.Net Runtime 6.0 x64: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

## Building:

Run `build.ps1` or manually run:

```
$options= @('--configuration', 'Release', '-p:PublishSingleFile=true', '-p:DebugType=embedded', '--self-contained', 'false')
dotnet publish KeyChordTester $options --runtime win-x64 --framework net6.0-windows -o build/win-x64
```

## Credits
[This Stackoverflow](https://stackoverflow.com/questions/604410/global-keyboard-capture-in-c-sharp-application) for a majority of the code. (basically all of it)
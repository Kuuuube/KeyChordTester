$options= @('--configuration', 'Release', '-p:PublishSingleFile=true', '-p:DebugType=embedded', '--self-contained', 'false')
dotnet publish KeyChordTester $options --runtime win-x64 --framework net6.0-windows -o build/win-x64
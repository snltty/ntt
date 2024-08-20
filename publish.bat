@echo off

rd /s /q public\\publish
rd /s /q public\\publish-zip
mkdir public\\publish-zip


for %%r in (win-x64,win-arm64) do (
	dotnet publish ./ntt -c release -f net8.0 -o ./public/publish/%%r/ntt-%%r  -r %%r  -p:PublishSingleFile=false -p:PublishAot=true -p:PublishTrimmed=true  --self-contained true  -p:TrimMode=partial -p:TieredPGO=true  -p:DebugType=none -p:DebugSymbols=false -p:EnableCompressionInSingleFile=true -p:DebuggerSupport=false -p:EnableUnsafeBinaryFormatterSerialization=false -p:EnableUnsafeUTF7Encoding=false -p:HttpActivityPropagationSupport=false -p:InvariantGlobalization=true  -p:MetadataUpdaterSupport=false  -p:UseSystemResourceKeys=true
)
for %%r in (win-x86,linux-x64,linux-arm,linux-arm64,linux-musl-x64,linux-musl-arm,linux-musl-arm64,osx-x64,osx-arm64) do (
	dotnet publish ./ntt -c release -f net8.0 -o ./public/publish/%%r/ntt-%%r  -r %%r  -p:PublishSingleFile=true -p:PublishTrimmed=true  --self-contained true  -p:TrimMode=partial -p:TieredPGO=true  -p:DebugType=none -p:DebugSymbols=false -p:EnableCompressionInSingleFile=true -p:DebuggerSupport=false -p:EnableUnsafeBinaryFormatterSerialization=false -p:EnableUnsafeUTF7Encoding=false -p:HttpActivityPropagationSupport=false -p:InvariantGlobalization=true  -p:MetadataUpdaterSupport=false  -p:UseSystemResourceKeys=true
)

for %%r in (win-x86,win-x64,win-arm64) do (
	echo F|xcopy "ntt.win\\dist\\*" "public\\publish\\%%r\\ntt-%%r\\*"  /s /f /h /y
)

for %%r in (win-x86,win-x64,win-arm64,linux-x64,linux-arm,linux-arm64,linux-musl-x64,linux-musl-arm,linux-musl-arm64,osx-x64,osx-arm64) do (
	7z a -tzip ./public/publish-zip/ntt-%%r.zip ./public/publish/%%r/*
)
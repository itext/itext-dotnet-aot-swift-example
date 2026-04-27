# Example of Running iText (.NET AOT version) in Swift app

## Description
This is a sample Swift iOS app that runs iText. The app simply creates a PDF file with text passed from the Swift code.

## Structure
The repository contains two projects:
* `iTextNativeAOTLibrary` - a .NET project compiled as Native AOT, exposing iText PDF generation as a native shared 
library (via the `itext_create_pdf` function), callable from Swift, C, or any language with C FFI support.
* `iTextNativeAOTApp` - a Swift app demonstrating how to call the exported `itext_create_pdf` function to generate 
a PDF.

## How to run
### Build and prepare framework

You'll first need to prepare an Apple platform framework (a bundle directory that packages a library together 
with its metadata) containing the Ahead-Of-Time compiled .NET library that depends on iText for .NET. We will rely on 
the Xcode utilities `install_name_tool` and `lipo`. 
Execute the following commands:  

```bash
# cleanup
rm -R ./iTextNativeAOTLibrary/bin ./iTextNativeAOTLibrary/obj ./iTextNativeAOTLibrary.framework

# prepare framework directory
mkdir -p "iTextNativeAOTLibrary.framework"

# add property list for the prepared framework
cat > "iTextNativeAOTLibrary.framework/Info.plist" << EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleName</key>
    <string>iTextNativeAOTLibrary</string>
    <key>CFBundleIdentifier</key>
    <string>com.apryse.iTextNativeAOTLibrary</string>
    <key>CFBundleVersion</key>
    <string>1.0</string>
    <key>CFBundleExecutable</key>
    <string>iTextNativeAOTLibrary</string>
    <key>CFBundlePackageType</key>
    <string>FMWK</string>
</dict>
</plist>
EOF

# build dotnet code into AOT dylib to run on simulator
dotnet publish -r iossimulator-arm64 ./iTextNativeAOTLibrary/iTextNativeAOTLibrary.csproj -c Release
# update dylib install name to framework-style @rpath path
install_name_tool -id @rpath/iTextNativeAOTLibrary.framework/iTextNativeAOTLibrary \
    ./iTextNativeAOTLibrary/bin/Release/net10.0/iossimulator-arm64/publish/iTextNativeAOTLibrary.dylib
# and place the framework binary in the target directory
lipo -create ./iTextNativeAOTLibrary/bin/Release/net10.0/iossimulator-arm64/publish/iTextNativeAOTLibrary.dylib \
    -output iTextNativeAOTLibrary.framework/iTextNativeAOTLibrary

# if you want to run on device instead, use the commands below instead of the ones above

# dotnet publish -r ios-arm64 ./iTextNativeAOTLibrary/iTextNativeAOTLibrary.csproj -c Release
# install_name_tool -id @rpath/iTextNativeAOTLibrary.framework/iTextNativeAOTLibrary \
#    ./iTextNativeAOTLibrary/bin/Release/net10.0/ios-arm64/publish/iTextNativeAOTLibrary.dylib
# lipo -create ./iTextNativeAOTLibrary/bin/Release/net10.0/ios-arm64/publish/iTextNativeAOTLibrary.dylib \
#    -output iTextNativeAOTLibrary.framework/iTextNativeAOTLibrary
```

### Open and run the `iTextNativeAOTApp` project in Xcode 

Before running double check that the framework is added correctly to the project:
1. Check that a reference to the `iTextNativeAOTLibrary` framework is added. Go to Xcode project settings
`General` tab, check the `Frameworks, Libraries and Embedded Content` and see if the reference is added and has 
`Embed & Sign` option selected. If it's not added, click `+`, then choose `Add Other` -> `Add Files` and then browse to 
the location of `iTextNativeAOTLibrary.framework` and add it. Once added, set `Embed & Sign` option for it.

2. On the `Build Settings` tab, ensure that directory containing `iTextNativeAOTLibrary.framework` is listed in 
`Search Paths` -> `Framework Search Paths`. It can be a relative path like `../`.

3. On the `Build Settings` tab, ensure that `iTextNativeAOTApp/Bridging-Header.h` is listed in `Swift Compiler - General`
-> `Bridging Header`.


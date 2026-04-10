# iText .NET AOT for Swift example

## Description
Sample iOS app to run iText on iOS. The code simply creates PDF with hardcoded (in swift) text.


## How to run
### Build and prepare framework

```bash
# prepare framework directory
mkdir -p "iTextNativeAOTLibrary.framework"

# build dotnet code into aot dylib
dotnet publish -r iossimulator-arm64 ./iTextNativeAOTLibrary/iTextNativeAOTLibrary.csproj -c Release

# modify rpath in dylib
install_name_tool -id @rpath/iTextNativeAOTLibrary.framework/iTextNativeAOTLibrary ./iTextNativeAOTLibrary/bin/Release/net10.0/iossimulator-arm64/publish/iTextNativeAOTLibrary.dylib

# prepare framework file in target directory
lipo -create ./iTextNativeAOTLibrary/bin/Release/net10.0/iossimulator-arm64/publish/iTextNativeAOTLibrary.dylib -output iTextNativeAOTLibrary.framework/iTextNativeAOTLibrary

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

```

### Double check that the framework is added into XCode
1. Check that a reference to the MyNativeAOTLibrary framework exists in xcode project properties and has Embed and Sign option chosen. 
If it is not, then in the iTextNativeAOTApp targets General tab, under Frameworks, Libraries and Embedded Content, select + to add iTextNativeAOTLibrary as the referenced framework.
In the dialog, choose Add Other -> Add Files and then browse to the location of iTextNativeAOTLibrary.framework and select it. 
Once selected, set Embed and Sign option for iTextNativeAOTLibrary framework.

2. Ensure that iTextNativeAOTLibrary.framework location (base repo directory path) is present in the list of Framework Search Paths in the Build Settings tab.

3. Ensure that iTextNativeAOTApp/Briging-Header.h is added into the list of Swift Compiler - General, Bridging Header in the Build Settings tab.


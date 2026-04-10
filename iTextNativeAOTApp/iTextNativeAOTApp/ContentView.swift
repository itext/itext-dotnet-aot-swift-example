import SwiftUI

struct ContentView: View {
    var body: some View {
        VStack {
            Image(systemName: "globe")
                .imageScale(.large)
                .foregroundStyle(.tint)
            let resPtr = String(cString: itext_aotsample_print("test text 123", URL.documentsDirectory.path.description + "/res.pdf"))
            Text("Hello, world!" + resPtr)
        }
        .padding()
    }
}

#Preview {
    ContentView()
}

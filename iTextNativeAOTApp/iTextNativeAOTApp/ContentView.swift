import SwiftUI

struct ContentView: View {
    var body: some View {
        VStack {
            Image(systemName: "globe")
                .imageScale(.large)
                .foregroundStyle(.tint)
            let resPtr = String(cString: itext_create_pdf("test text 123", URL.documentsDirectory.path.description + "/res.pdf"))
            Text("Hello, world!" + resPtr)
        }
        .padding()
    }
}

#Preview {
    ContentView()
}

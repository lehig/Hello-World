import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;

public class HelloChatsClient {
	public static void main(String[] args) {
		try {
			// Connect to the server on localhost and port 1234
			Socket socket = new Socket("localhost", 1234);

			// Create input and output streams for communication
			BufferedReader in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
			PrintWriter out = new PrintWriter(socket.getOutputStream(), true);

			// Create a thread to read messages from the server
			Thread readThread = new Thread(() -> {
				try {
					String message;
					while ((message = in.readLine()) != null) {
						System.out.println("Server: " + message);
					}
				} catch (IOException e) {
					e.printStackTrace();
				}
			});
			readThread.start();

			// Read messages from the console and send them to the server
			BufferedReader consoleIn = new BufferedReader(new InputStreamReader(System.in));
			String consoleMessage;
			while ((consoleMessage = consoleIn.readLine()) != null) {
				out.println(consoleMessage);
			}

			// Close the socket
			socket.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
}

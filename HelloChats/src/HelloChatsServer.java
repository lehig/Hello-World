import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;

public class HelloChatsServer {
	public static void main(String[] args) {
		try {
			// Create a server socket on port 1234
			ServerSocket serverSocket = new ServerSocket(1234);
			System.out.println("Server started. Waiting for a client...");

			// Wait for a client to connect
			Socket clientSocket = serverSocket.accept();
			System.out.println("Client connected.");

			// Create input and output streams for communication
			BufferedReader in = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
			PrintWriter out = new PrintWriter(clientSocket.getOutputStream(), true);

			// Create a thread to read messages from the client
			Thread readThread = new Thread(() -> {
				try {
					String message;
					while ((message = in.readLine()) != null) {
						System.out.println("Client: " + message);
					}
				} catch (IOException e) {
					e.printStackTrace();
				}
			});
			readThread.start();

			// Read messages from the console and send them to the client
			BufferedReader consoleIn = new BufferedReader(new InputStreamReader(System.in));
			String consoleMessage;
			while ((consoleMessage = consoleIn.readLine()) != null) {
				out.println(consoleMessage);
			}

			// Close the socket and server socket
			clientSocket.close();
			serverSocket.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
}

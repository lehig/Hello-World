using System.Net;
using System.Net.Sockets;

namespace SendAFile; 

public class Receiver : Command{
    private string _ipAddress { get; set; }

    private string _saveLocation { get; set; }

    private byte[] _fileData { get; set; }

    private int _fileSize { get; set; }

    public Receiver() {
        _ipAddress = "IP Address";
        _saveLocation = "file_received";
        _fileData = null;
        _fileSize = 0;

    }

    public Receiver(string ipAddress, string saveLocation, byte[] fileData, int fileSize = 0) {
        _ipAddress = ipAddress;
        _saveLocation = saveLocation;
        _fileData = fileData;
        _fileSize = fileSize;
    }

    public void ReceiveFile() {
        IPAddress ipAddress = IPAddress.Any;
        int port = 8888;
        
        // create a tcp/ip socket
        TcpListener listener = new TcpListener(ipAddress, port);
        listener.Start();
        Console.WriteLine("Waiting for sender... ");

        try {
            // accept incoming connection from sender
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Sender connected!");

            // Get a network stream for receiving data
            NetworkStream stream = client.GetStream();

            // receive the file size from the sender
            byte[] fileSizeBytes = new byte[4];
            stream.Read(fileSizeBytes, 0, 4);
            _fileSize = BitConverter.ToInt32(fileSizeBytes, 0);

            // receive the file data from the sender
            byte[] fileData = new byte[_fileSize];
            int bytesRead = stream.Read(fileData, 0, _fileSize);

            // write the received file data to disk
            File.WriteAllBytes(_saveLocation, fileData);

            Console.WriteLine("File receive and saved successfully!");
        }
        catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally {
            listener.Stop();
        }

    }
    
    public override void Run() {
        Console.WriteLine("Running Program...");
        Console.WriteLine("What would you like to call the file once received: ");
        _saveLocation = Console.ReadLine();
        ReceiveFile();

    }
}
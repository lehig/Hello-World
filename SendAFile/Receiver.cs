using System.Net;
using System.Net.Sockets;

namespace SendAFile; 

public class Receiver : Command{
    private string _ipAddress;

    private string _saveLocation;

    private byte[] _fileData;

    public Receiver() {
        _ipAddress = "IP Address";
        _saveLocation = "file_received";
        _fileData = null;

    }

    public Receiver(string ipAddress, string saveLocation, byte[] fileData) {
        _ipAddress = ipAddress;
        _saveLocation = saveLocation;
        _fileData = fileData;
    }

    public string GetipAddress() {
        return _ipAddress;
    }

    public void SetipAddress(string ipAddress) {
        _ipAddress = ipAddress;
    }

    public string GetLocation() {
        return _saveLocation;
    }

    public void SetLocation(string location) {
        _saveLocation = location;
    }

    public byte[] GetFileData() {
        return _fileData;
    }

    public void SetFileData(byte[] fileData) {
        _fileData = fileData;
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
            int fileSize = BitConverter.ToInt32(fileSizeBytes, 0);

            // receive the file data from the sender
            byte[] fileData = new byte[fileSize];
            int bytesRead = stream.Read(fileData, 0, fileSize);

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
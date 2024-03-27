using System.Net;
using System.Net.Sockets;

namespace SendAFile; 

public class Sender : Command{
    
    private string _ipAddress;

    private string _location;

    private byte[] _fileData;

    public Sender() {
        _ipAddress = "";
        _location = "";
        _fileData = null;
    }

    public Sender(string ipAddress, string location, byte[] fileData) {
        _ipAddress = ipAddress;
        _location = location;
        _fileData = fileData;
    }

    public string GetipAddress() {
        return _ipAddress;
    }

    public void SetipAddress(string ipAddress) {
        _ipAddress = ipAddress;
    }

    public string GetLocation() {
        return _location;
    }

    public void SetLocation(string location) {
        _location = location;
    }

    public byte[] GetFileData() {
        return _fileData;
    }

    public void SetFileData(byte[] fileData) {
        _fileData = fileData;
    }

    private void ReadFile(string fileLocation) {
        byte[] fileData = File.ReadAllBytes(fileLocation);
        _fileData = fileData;
    }

    private void SendFile() {
        // defining the receiver's IP and port
        IPAddress receiverIp = IPAddress.Parse(_ipAddress);
        int receiverPort = 8888;

        // Create a TCP/IP socket
        TcpClient client = new TcpClient();

        try {
            // connect to the receiver
            client.Connect(receiverIp, receiverPort);

            // Get a network stream for sending data
            NetworkStream stream = client.GetStream();

            // read the file
            ReadFile(_location);

            // send the file SIZE to the receiver
            byte[] fileSize = BitConverter.GetBytes(_fileData.Length);
            stream.Write(fileSize, 0, fileSize.Length);

            // send the file DATA to the receiver
            stream.Write(_fileData, 0, _fileData.Length);

            Console.WriteLine("File sent successfully!");

        }
        catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally {
            client.Close();
        }
    }
    

    public override void Run() {
        Console.WriteLine("IP Address: ");
        _ipAddress = Console.ReadLine();
        
        Console.WriteLine("Location of file: ");
        _location = Console.ReadLine();

        Console.WriteLine("Running Program...");
        SendFile();

    }
    
    
    
    
    
    


}
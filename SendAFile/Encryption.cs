using System.Security.Cryptography.X509Certificates;
using SendAFile;

namespace DefaultNamespace; 
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Encryption : Command {
    private string _originalFilePath { get; set; }
    private string _encryptedFilePath { get; set; }
    private int _fileSize { get; set; }
    

    public Encryption() {
        _originalFilePath = "";
        _encryptedFilePath = "";
    }
    
    public Encryption(string originalFilePath, string encryptedFilePath) {
        _originalFilePath = originalFilePath;
        _encryptedFilePath = encryptedFilePath;
    }

    private void GenerateKeys(out RSAParameters publicKey, out RSAParameters privateKey) {
        var rsa = new RSACryptoServiceProvider();
        publicKey = rsa.ExportParameters(false);
        privateKey = rsa.ExportParameters(true);
    }

    private void EncryptFile( string publicKey) {
        var rsa = new RSACryptoServiceProvider();

        try {
            rsa.FromXmlString(publicKey);
            using var inputStream = new FileStream(_originalFilePath, FileMode.Open);
            using var outputStream = new FileStream(_encryptedFilePath, FileMode.Create);
            
            var fileData = File.ReadAllBytes(_originalFilePath);
            var buffer = new byte[fileData.Length];
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0) {
                var encryptedData = rsa.Encrypt(buffer, true);
                outputStream.Write(encryptedData, 0, encryptedData.Length);
            }
        }
        catch (Exception e) {
            Console.WriteLine($"Error: {e}");
        }
        finally {
            rsa.Dispose();
        }
    }

    private void KeySetUp() {
        
        GenerateKeys(out var publicKey, out var privateKey);

        Console.WriteLine("Generating public and private keys..." +
                          "\nPublic Key: \n");
        Console.WriteLine(publicKey.ToString());
        
        Console.WriteLine("\nPrivate Key:\n");
        Console.WriteLine(privateKey.ToString());
        
    }

    protected string FilePathSetUp(string checkPath, string objectPath, string type) {
        /*
         * This function takes two paths. checkPath is the path from the EncryptRun()
         * function. objectPath is from the object created if it is still instantiated.
         * The type param is for whether this is for where the file is (orgPath) or
         * where it's supposed to be saved (encPath). 
         */
        if (checkPath != "" || objectPath != "") return checkPath != "" ? checkPath : objectPath;
        Console.WriteLine($"Need a file path for {type} file: ");
        var userPath = Console.ReadLine();
        return userPath;

    }

    private void EncryptRun(string orgPath = "", string encPath = "") {
        Console.WriteLine("Running encryption program...\n");
        KeySetUp();
        _originalFilePath = FilePathSetUp(orgPath, _originalFilePath, "original");
        _encryptedFilePath = FilePathSetUp(encPath, _encryptedFilePath, "encrypted");
        Console.WriteLine("File paths have been set up.\n");

        Console.WriteLine("Need the public key to encrypt file: ");
        var publicKey = Console.ReadLine();
        EncryptFile(publicKey);
        Console.WriteLine("File encrypted and saved locally.");
        
        Console.WriteLine("Running sender program...");
        var sender = new Sender();
        sender.Run();
    }

    public override void Run() {
        EncryptRun();
    }

    

}
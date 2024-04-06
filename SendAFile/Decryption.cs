using System.Security.Cryptography;
using DefaultNamespace;

namespace SendAFile; 

public class Decryption : Encryption{
    private string _originalFilePath { get; set; }
    private string _decryptedFilePath { get; set; }

    public Decryption() {
        _decryptedFilePath = "";
    }

    public Decryption(string originalFilePath, string decryptedFilePath) {
        _originalFilePath = originalFilePath;
        _decryptedFilePath = decryptedFilePath;
    }
    
    private void DecryptFile(string privateKey) {
        var rsa = new RSACryptoServiceProvider();
        try {
            rsa.FromXmlString(privateKey);
            using var inputStream = new FileStream(_originalFilePath, FileMode.Open);
            using var outputStream = new FileStream(_decryptedFilePath, FileMode.Create);
            
            var fileData = File.ReadAllBytes(_originalFilePath);
            var buffer = new byte[fileData.Length * 2];
            var bytesRead = inputStream.Read(buffer, 0, buffer.Length);
            while (bytesRead > 0) {
                var decryptedData = rsa.Decrypt(buffer, true);
                outputStream.Write(decryptedData, 0, decryptedData.Length);
            }
        }
        catch (Exception e) {
            Console.WriteLine($"Error: {e}");
        }
        finally {
            rsa.Dispose();
        }
    }
    
    private void EncryptRun(string orgPath = "", string decPath = "") {
        Console.WriteLine("Running decryption program...\n");
        _originalFilePath = FilePathSetUp(orgPath, _originalFilePath, "original");
        _decryptedFilePath = FilePathSetUp(decPath, _decryptedFilePath, "decrypted");
        
        Console.WriteLine("Need the public and private key in XML format to decrypt file: ");
        var keys = Console.ReadLine();
        DecryptFile(keys);
        Console.WriteLine("File decrypted and saved.");
    }

    public override void Run() {
        EncryptRun();
    }
}
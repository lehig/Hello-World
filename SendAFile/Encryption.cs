namespace DefaultNamespace; 
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Encryption {
    private string _originalText { get; set; }
    private string _encryptedText { get; set; }
    private string _originalFilePath { get; set; }
    private string _encryptedFilePath { get; set; }
    private string _decryptedFilePath { get; set; }

    private void GenerateKeys(out RSAParameters publicKey, out RSAParameters privateKey) {
        var rsa = new RSACryptoServiceProvider();
        publicKey = rsa.ExportParameters(false);
        privateKey = rsa.ExportParameters(true);
    }

    private void EncryptFile(int fileSize, RSAParameters publicKey) {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        try {
            rsa.ImportParameters(publicKey);
            using (FileStream inputStream = new FileStream(_originalFilePath, FileMode.Open))
            using (FileStream outputStream = new FileStream(_encryptedFilePath, FileMode.Create)) {
                byte[] buffer = new byte[fileSize];
                int bytesRead;
                while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0) {
                    byte[] encryptedData = rsa.Encrypt(buffer, true);
                    outputStream.Write(encryptedData, 0, encryptedData.Length);
                }
            }
        }
        finally {
            rsa.Dispose();
        }
    }

    private void DecryptFile(int fileSize, RSAParameters privateKey) {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        try {
            rsa.ImportParameters(privateKey);
            using (FileStream inputStream = new FileStream(_originalFilePath, FileMode.Open))
            using (FileStream outputStream = new FileStream(_decryptedFilePath, FileMode.Create)) {
                byte[] buffer = new byte[fileSize * 2];
                int bytesRead;
                while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0) {
                    byte[] decryptedData = rsa.Decrypt(buffer, true);
                    outputStream.Write(decryptedData, 0, decryptedData.Length);
                }
            }
        }
        finally {
            rsa.Dispose();
        }
    }

    public void KeySetUp() {
        RSAParameters publicKey;
        RSAParameters privateKey;
        GenerateKeys(out publicKey, out privateKey);
        
        Console.WriteLine("");
        
    }

}
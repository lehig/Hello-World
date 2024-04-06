// See https://aka.ms/new-console-template for more information

using System.Security.AccessControl;
using DefaultNamespace;
using SendAFile;

class Program{

    static void Main(string[] args) {
        
        // creating the sender and receiver objects to place in the dictionary
        var sender = new Sender();
        var receiver = new Receiver();
        var encrypt = new Encryption();
        var decrypt = new Decryption();
        
        // building the dictionary
        var commands = new Dictionary<string, Command>();
        
        // adding the objects to the dictionary of commands
        commands.Add("1", sender);
        commands.Add("2", receiver);
        commands.Add("3", encrypt);
        commands.Add("4", decrypt);
        
        Console.WriteLine("This is a program to send files to other people that are waiting to receive them.\n" +
                          "\nList of actions:\n" +
                          "1 - send (type '1')\n" +
                          "2 - receive (type '2')\n" +
                          "3 - send encrypted (type '3') " +
                          "4 - decrypt (type '4')" + 
                          "\nWhat would you like to do: ");
        
        var action = Console.ReadLine().ToLower(); // Convert to lowercase for case-insensitive comparison
        
        // taking the action from the argument and putting it through the dictionary and running the object
        try {
            commands[action].Run();
        }
        catch (Exception e) {
            Console.WriteLine("Error: " + e.Message);
        }


    }

}
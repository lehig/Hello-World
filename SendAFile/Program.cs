// See https://aka.ms/new-console-template for more information

using System.Security.AccessControl;
using SendAFile;

class Program{

    static void Main(string[] args) {
        
        // creating the sender and receiver objects to place in the dictionary
        Sender sender = new Sender();
        Receiver receiver = new Receiver();
        
        // building the dictionary
        Dictionary<string, Command> commmands = new Dictionary<string, Command>();
        
        // adding the objects to the dictionary of commands
        commmands.Add("send", sender);
        commmands.Add("receive", receiver);
        
        Console.WriteLine("This is a program to send files to other people that are waiting to receive them.\n" +
                          "\nList of actions:\n" +
                          "- send (type in 'send')\n" +
                          "- receive (type in 'receive')\n" +
                          "\nWhat would you like to do: ");
        
        string action = Console.ReadLine().ToLower(); // Convert to lowercase for case-insensitive comparison
        
        // taking the action from the argument and putting it through the dictionary and running the object
        try {
            commmands[action].Run();
        }
        catch (Exception e) {
            Console.WriteLine("Error: " + e.Message);
        }


    }

}
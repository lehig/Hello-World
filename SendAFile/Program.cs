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
        
        // Check the first argument to determine what action to take
        string action = args[0].ToLower(); // Convert to lowercase for case-insensitive comparison
        
        // taking the action from the argument and putting it through the dictionary and running the object
        try {
            commmands[action].Run();
        }
        catch (Exception e) {
            Console.WriteLine("Error: " + e.Message);
        }


    }

}
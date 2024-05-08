using System;

class Program
{
    static void Main(string[] args)
    {
        DrawAsciiArt();
        Console.WriteLine("\nPK성공횟수: ( ) ( ) ( ) ( ) ( )  득점보상:     $:");
        Console.WriteLine("도핑횟수:       :");
    }

    static void DrawAsciiArt()
    {
         Console.WriteLine("                             +---------------------------+    ");
        Console.WriteLine("                             |\\                          |\\   ");
        Console.WriteLine("                             | \\           0             | \\  ");
        Console.WriteLine("                             |  \\         (X)            |  \\ ");
        Console.WriteLine("_____________________________|___|_________LL____________|-|\\|__________________");
        Console.WriteLine("         /                   /                             /                  / ");
        Console.WriteLine("        /                   /                             /                  /  ");
        Console.WriteLine("       /                   /                             /                  /  ");        
        Console.WriteLine("      /                   /_____________________________/                  /    ");
        Console.WriteLine("     /                                                                    /     ");
        Console.WriteLine("    /                            O/                                      /      ");
        Console.WriteLine("   /                           _/x                                      /       ");
        Console.WriteLine("  /                            _/ \\                                    /        ");
        Console.WriteLine(" /________________________________/___________________________________/         ");
    }
}

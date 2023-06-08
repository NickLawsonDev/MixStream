// See https://aka.ms/new-console-template for more information
using Core;

Do();

void Do()
{
    Console.WriteLine("Hello, World!");
    var input = Console.ReadLine();

    if (input.ToLower() == "start")
    {
        Sound.Play();
    }

    if (input.ToLower() == "stop")
    {
        Sound.Stop();
    }

    Do();
}
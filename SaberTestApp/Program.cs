using SaberTestApp;

ListRandom newlist = new ListRandom();

Console.ForegroundColor = ConsoleColor.Green;
Console.Write("Please, input the count of the listnodes:");
Console.ForegroundColor = ConsoleColor.White;
int i = Convert.ToInt32(Console.ReadLine());
while (i>0)
{
    Console.Write("Please, input the data of the node:");
    string nodedata = Console.ReadLine();
    newlist.Add(nodedata);
    i--;
}

//newlist.Add("Test Data 1");
//newlist.Add("Test Data 2");
//newlist.Add("Test Data 3");


using (Stream filestream = new FileStream("./lists.dat", FileMode.OpenOrCreate, FileAccess.Write))
{
    newlist.Serialize(filestream);
}

using (Stream filestream = new FileStream("./lists.dat", FileMode.Open, FileAccess.Read))
{
    newlist.Deserialize(filestream);
}


using (Stream memstream = new MemoryStream())
{
    newlist.Serialize(memstream);
    newlist.Deserialize(memstream);
}
    


Console.ReadLine();
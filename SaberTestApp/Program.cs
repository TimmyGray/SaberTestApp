using SaberTestApp;

ListRandom newlist = new ListRandom();

newlist.Add("Test Data 1");
newlist.Add("Test Data 2");
newlist.Add("Test Data 3");


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
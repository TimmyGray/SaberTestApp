using SaberTestApp;

ListRandom newlist = new ListRandom();


Console.Write("Please, input the count of the listnodes:");     //Для введения кастомных данных

int i = Convert.ToInt32(Console.ReadLine());    
while (i > 0)
{
    Console.Write("Please, input the data of the node:");
    string nodedata = Console.ReadLine();
    newlist.Add(nodedata);
    i--;
}
//конкретный набор данных
//newlist.Add("Test Data 1");          
//newlist.Add("Test Data 1");
//newlist.Add("Test Data 3");
//newlist.Add("Test Data 4");
//newlist.Add("Test Data 5");
//newlist.Add("Test Data 6");


//использование файлового потока для сериализации-десериализации 
//using (Stream filestream = new FileStream("./lists.dat", FileMode.OpenOrCreate, FileAccess.Write))
//{
//    newlist.Serialize(filestream);
//}

//using (Stream filestream = new FileStream("./lists.dat", FileMode.Open, FileAccess.Read))
//{
//    newlist.Deserialize(filestream);
//}

//использования потока памяти для сериализации-десериализации
using (Stream memstream = new MemoryStream())
{
    newlist.Serialize(memstream);
    newlist.Deserialize(memstream);
}

//проверка полученных значений после десериализации
newlist.ChekingDes();

Console.ReadLine();
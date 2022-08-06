using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaberTestApp
{
	class ListNode
	{
		public ListNode Previos;
		public ListNode Next;
		public ListNode ListNodeRandom;
		public string data;

	}

	class ListRandom
	{
		public ListNode Head;
		public ListNode Tail;
		public int Count;

		private ListNode GetNode(int index)		//аналог индексатора, для возможности получения конкретного нода по индексу
        {
			ListNode node = Head;
			for (int i = 0; i < index; i++)
			{
				node = node.Next;
			}
			return node;
		}
		
		private ListNode Randomaizer()		//простой рандомайзер для получения ссылки на случайный нод
		{
			var random = new Random();
			int index = random.Next(0,Count-1);
			ListNode rndnode = GetNode(index);

			return rndnode;
		}

		public void Add(string somedata)	//реализация добавления новых нодов
		{
			ListNode newnode = new ListNode() { data = somedata };
			if (Count == 0)
			{
				Head = newnode;

			}
			else
			{
				Tail.Next = newnode;
				newnode.Previos = Tail;
			}
			Tail = newnode;
			Count++;

			Console.WriteLine("ListNode succesfully add!");

			ShowInfo(newnode);
		}

		public void Serialize(Stream s)		//реализация сериализации без сторонних библиотек и стандартных средств сериализации
		{
			if(Count != 0)		//проверка на пустой список, чтобы не было ошибок
            {
				Console.WriteLine("Start Serialize");
                for (int i = 0; i < Count; i++)		// применение рандомайзера
                {
					GetNode(i).ListNodeRandom = Randomaizer();
                }

				Dictionary<ListNode,int> dic = new Dictionary<ListNode,int>();
				AddToDict(dic);		//добавляем в словарь все ноды
				
                using (BinaryWriter writer = new BinaryWriter(s,Encoding.UTF8,true))    //создаем binarywriter и передаем в него поток, который выбрали для записи.
																						//третий аргумент указывает, что после закрытия writer родительский поток не уничтожится
																						// и можно будет применить дессериализацию в этом же потоке. Это важно при мемори стрим.
				{

					WriteInSream(dic, writer);											//пишем данные каждого нода и айди случайного нода, который с ним связан

                }


                Console.WriteLine("ListRandom is Serialized\n");
			}
            else
            {
				Console.WriteLine("You must add at least one node\n");
            }
			
		}

		public void Deserialize(Stream s)
		{
			s.Position = 0;			//так как потом у нас не закрыт и остался в своей конечной точке, необходимо вернуть позицию в начало
			Console.WriteLine("Start Deserialize");
			Count = 0;			//так как лист у нас условно пустой, сброс количества нодов в 0, это позволит далее корректно применить метод добавления нодов
			
			Queue<int> ids = new Queue<int>(); //создаем очередь, куда будем помещать уникальные айдишки рандомных нодов
            
			using (BinaryReader reader = new BinaryReader(s)) //создаем bineryreader которые позволит считать данные из переданного потока
            {
                while (reader.PeekChar() != -1) //проверка на окончание данных в потоке
                {
					string data = reader.ReadString();
					int randomid = reader.ReadInt32();

					Console.WriteLine(data);
					Console.WriteLine(randomid);
					
					ids.Enqueue(randomid);

					Add(data); // создаем новые ноды, которые пока что еще без рандомных нодов

					Count++;	
				}

            }

			ListNode node = Head;

            while (node!=null) //проходимся по всем нодам, и возвращаем связь с рандомными нодами
            {
                while (ids.Count!=0)
                {
					SearchRandom(ids.Dequeue(), node); //с каждым разом, очередь будет уменьшаться. 
					break; //брейк необходим, чтобы происходил шаг по листу
                }

                node = node.Next;
				
			}


		}    //реализация десериализации без сторонних библиотек и стандартных средств сериализации

		private void SearchRandom(int id, ListNode node) //восстановление связей с рандомными нодами
        {

			ListNode cur = Head;		//уникальный id  соотвествует позиции в листе, при сериализации, поэтому, мы просто
										//шагаем количество раз соответствущее id, и после этого, в текущий нод, переданный аргументом в метод
										//записываем нод, который получился после шагания
			int count = 0;
			while (count != id) 
			{
				cur = cur.Next;
				count++;
			}

			node.ListNodeRandom = cur;
		}

		public void ChekingDes()
        {
			ListNode node = Head;
			while(node != null)
            {

				if (node.Next != null)
				{
					Console.WriteLine($"Node next data: {node.Next.data}");

				}
				else
                {
					Console.WriteLine("This is Tail");

				} 
				if (node.Previos != null)
				{
					Console.WriteLine($"Node previos data: {node.Previos.data}");
					
				}
                else
                {
					Console.WriteLine("This is Head");
                }
				Console.WriteLine($"Current node data: {node.data}");
				Console.WriteLine($"Random node data: {node.ListNodeRandom.data}");
				Console.WriteLine();

				node = node.Next;
			
            }
        } //информационный метод, чтобы проверить сериализацию-десериализацию

		private void ShowInfo(ListNode node) //простенькая информационная функция, позволяющая увидеть как выглядит нод при добавлении в лист
        {
			Console.WriteLine($"Current node data - {node.data}");

			if (node.Previos == null)
			{
				Console.WriteLine("Previos node - null");
			}
			else
			{
				Console.WriteLine($"Previos node data - {node.Previos.data}");
			}
			if (node.Next == null)
			{
				Console.WriteLine("Next node - null");
			}
			else
			{
				Console.WriteLine($"Next node data - {node.Next.data}");
			}
			Console.WriteLine();
		}

		private void AddToDict(Dictionary<ListNode,int> dict) //метод для создания уникального айди для каждого нода, вынесен отдельно, чтобы не загромождать код и повысить читаемость
        {
			int id= 0;
			ListNode cur = Head;
			while(cur != null)
            {
				dict.Add(cur, id++);
				cur = cur.Next;
            }
        }

		private void WriteInSream(Dictionary<ListNode,int> nodes, BinaryWriter writer)		//основная часть метода сериализации. проходимся по всем нодам с головы листа
																							//записываем данные каждого нода и жестко привязываем айди рандомного нода
																							//это позволит точно восстановить весь лист, так как айди уникальны в отличие от данных нода
        {
			ListNode cur = Head;
            while (cur!=null)
            {
				writer.Write(cur.data);
				writer.Write(nodes[cur.ListNodeRandom]);

				Console.WriteLine(cur.data);
				Console.WriteLine(cur.ListNodeRandom.data);
				Console.WriteLine(nodes[cur.ListNodeRandom]);
				Console.WriteLine();

				cur = cur.Next;

            }
           
        } 
        
	}
}

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

		private ListNode GetNode(int index)
        {
			ListNode node = Head;
			for (int i = 0; i < index; i++)
			{
				node = node.Next;
			}
			return node;
		}
		
		private ListNode GetNode(string data)
        {
			ListNode node = Head;
            while (node.data!=data)
            {
				node = node.Next;
            }
			return node;
        }

		private ListNode Randomaizer()
		{
			var random = new Random();
			int index = random.Next(0,Count-1);
			ListNode rndnode = GetNode(index);

			return rndnode;
		}

		public void Add(string somedata)
		{
			ListNode newnode = new ListNode() { data = somedata };
			if (Count == 0)
			{
				Head = newnode;
				//newnode.ListNodeRandom = Head;

			}
			else
			{
				Tail.Next = newnode;
				newnode.Previos = Tail;
			//	newnode.ListNodeRandom = Randomaizer(); //было здесь, но я так понимаю, что по заданию надо все - таки при сериализации создавать ссылки на рандомные ноды
			}
			Tail = newnode;
			Count++;

			Console.WriteLine("ListNode succesfully add!");

			ShowInfo(newnode);
		}

		public void Serialize(Stream s)
		{
			if(Count != 0)
            {
				Console.WriteLine("Start Serialize");
                for (int i = 0; i < Count; i++)
                {
					GetNode(i).ListNodeRandom = Randomaizer();
                }
				int index = 0;
				ListNode curnode = Head;
                using (BinaryWriter writer = new BinaryWriter(s,Encoding.UTF8,true))
                {
                    while (curnode != null)
					{
						if (curnode == Head)
						{
							WriteInSream(index, curnode, writer);
							
						}

						else if (curnode.Next == null)
						{
							index++;
							WriteInSream(index, curnode, writer);


						}
						else
						{
							index++;
							WriteInSream(index, curnode, writer);
						}

						Console.WriteLine(index);
						Console.WriteLine(curnode.data);
						Console.WriteLine(curnode.ListNodeRandom.data);
						Console.WriteLine();

						curnode = curnode.Next;
					}

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
			s.Position = 0; 
			Console.WriteLine("Start Deserialize");
			Count = 0;
            using (BinaryReader reader = new BinaryReader(s))
            {
				List<string> randomdata = new List<string>();
                while (reader.PeekChar() != -1)
                {
					int index = reader.ReadInt32();

					ListNode newnode = new ListNode();
                   
					if (index==0)
                    {
						Head = newnode;
						
                    }
                    else
                    {
						Tail.Next = newnode;
						newnode.Previos = Tail;

					}
					
					Tail = newnode;

					string Randomnodedata = reader.ReadString();
					randomdata.Add(Randomnodedata);

					newnode.data = reader.ReadString();
					
					Console.WriteLine(index);
					Console.WriteLine(newnode.data);
					Console.WriteLine(Randomnodedata);
					Console.WriteLine();
					
					Count++;	
				}
                for (int i = 0; i < Count; i++)												//Только для проверки
                {
					Console.WriteLine($"Deserialize reffs on rnd node {i+1} element:");
					GetNode(i).ListNodeRandom = GetNode(randomdata[i]);
					Console.WriteLine(GetNode(i).ListNodeRandom.data);
					Console.WriteLine();
                }
					

            }

			ChekingDes();//Только для проверки работоспособности


		}

		private void ChekingDes()
        {
			ListNode node = Head;
			while(node != null)
            {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Current node data - {node.data}");
                if (node.Next!=null)
                {
					Console.WriteLine($"node.Next data - {node.Next.data}");
					Console.WriteLine($"node.Next.ListNodeRandom.data - {node.Next.ListNodeRandom.data}");
				}
                else
                {
					Console.WriteLine("node.Next = null");
					
				}	
				Console.WriteLine($"Current node.ListNodeRandom.data - {node.ListNodeRandom.data}");
				
				Console.ForegroundColor = ConsoleColor.Yellow;
                if (node.Previos!=null)
                {
					Console.WriteLine($"node.Previos.data - {node.Previos.data}");
					Console.WriteLine($"node.Previos.ListNodeRandom.data - {node.Previos.ListNodeRandom.data}");
				}
				else
				{
					Console.WriteLine("node.Previos = null");

				}
				Console.WriteLine();
				node = node.Next;
			
            }
			Console.ForegroundColor = ConsoleColor.White;
        }//Только для проверки работоспособности

		private void ShowInfo(ListNode node)
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

		private void WriteInSream(int index, ListNode node, BinaryWriter writer)
        {
			writer.Write(index);
			writer.Write(node.ListNodeRandom.data);	
			writer.Write(node.data);
        }
        
	}
}

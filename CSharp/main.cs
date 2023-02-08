using System;
using static System.Console;
using static System.Math;
class main{
	public static void Main(){
		Write("Hello from main\n");
		static_hello.print();
		static_world.print();
		static_hello.greeting = "Goodbye";
		static_hello.print();
		static_world.print();
	}
}

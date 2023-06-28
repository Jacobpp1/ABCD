using System;
using static System.Console;
using static System.Math;
public static class main {
	public static string s = "class scope s";
	public static void print_s(){WriteLine(s);}
	public static void Main(){
		print_s();
		string s = "method scope s";
		print_s();
		WriteLine(s);
		{
			string ss = "block scope";
			WriteLine(ss);
		}
		Write("Hello from main\n");
		static_hello.print();
		static_world.print();
		static_hello.greeting = "Goodbye";
		static_hello.print();
		static_world.print();
		hello hello1 = new hello("Hello1");
		hello world1 = new hello("world1");
		hello1.print();
		world1.print();
		hello another_hello = hello1;
		another_hello.greeting = "another greeting";
		hello1.print();
		hello test = new hello();
		test.print();
		WriteLine($"The value of pi in Math is {PI}");
		WriteLine("The value of pi in Math is {0}, and e is {1}",PI, E);
		double sqrt2 = Sqrt(2);
		WriteLine($"sqrt2^2 = {sqrt2*sqrt2}");
		WriteLine($"1/2={1/2}");
		WriteLine($"1.0/2={1.0/2}");
	}
	
}

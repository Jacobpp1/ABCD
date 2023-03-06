using System;
using static System.Console;
using static System.Math;
class main{
	public static void Main(string[] args){
		/*
		genlist<double> listd = new genlist<double>();
		listd.add(1.0);
		listd.add(2.0);
		listd.add(3.0);
		listd.add(4.0);
		listd.add(5.0);
		listd.add(6.0);
		// Delegate
		Func<double,double> f;
		f = Sin;
		f = delegate(double x){return x*x;};
		f = (double x) => x*x;
		double y = f(2.0);
		for(int i=0; i < listd.size; i++){
			double x = listd[i];
			WriteLine($"{x} {f(x)}");
		}
		*/

		var list = new genlist<double[]>();
		char[] delimiters = {' ', '\t'};
		var options = StringSplitOptions.RemoveEmptyEntries;
		for(string line = ReadLine(); line!=null; line = ReadLine()){
			var words = line.Split(delimiters,options);
			int n = words.Length;
			var numbers = new double[n];
			for(int i=0; i<n; i++) numbers[i] = double.Parse(words[i]);
			list.add(numbers);
		}
		for(int i=0; i<list.size; i++){
			var numbers = list[i];
			foreach(var number in numbers)Write($"{number : 0.00e+00;-0.00e+00}");
		}
		Write("\n");
	}
	
}

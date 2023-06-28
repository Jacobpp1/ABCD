using System;
using static System.Console;
using static System.Math;
class main{
	public static void Main(){
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
	}
}

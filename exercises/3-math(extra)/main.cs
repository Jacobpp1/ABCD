using System;
using static System.Console;
using static System.Math;
public static class main{
	public static void Main(){
		double sqrt2 = System.Math.Sqrt(2);
		WriteLine($"Sqrt2 = {sqrt2}");
		WriteLine($"2^1/5 = {Pow(2.0,1.0/5)}");
		WriteLine($"e^pi = {Pow(E,PI)}");
		WriteLine($"pi^e = {Pow(PI,E)}");
		WriteLine($"Gamma(1) = {sfuns.gamma(1)}");
		WriteLine($"Gamma(2) = {sfuns.gamma(2)}");
		WriteLine($"Gamma(3) = {sfuns.gamma(3)}");
		WriteLine($"Gamma(0) = {sfuns.gamma(0)}");
		WriteLine($"Gamma(1) = {sfuns.lngamma(1)}");
		WriteLine($"Gamma(2) = {sfuns.lngamma(2)}");
		WriteLine($"Gamma(3) = {sfuns.lngamma(3)}");
		WriteLine($"Gamma(0) = {sfuns.lngamma(0)}");
	}
}

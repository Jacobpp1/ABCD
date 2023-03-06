using System;
using static System.Console;
using static System.Math;
public static class main{

	public static void Main(){
		WriteLine($"The value of pi in Math is {PI}");
		WriteLine("The value of pi in Math is {0}, and e is {1}",PI, E);
		double sqrt2 = Sqrt(2);
        WriteLine($"sqrt(2) = {sqrt2}");
        WriteLine($"2^1/5 = {Pow(2,1.0/5.0)}");
        WriteLine($"(2^1/5)^5 = {Pow(Pow(2,1.0/5.0),5.0)}");
        WriteLine($"e^pi = {Pow(E,PI)}");
        WriteLine($"e^pi*e^-pi = {Pow(E,PI)*Pow(E,-PI)}");
        WriteLine($"pi^e = {Pow(PI,E)}");
		WriteLine($"sqrt2^2 = {sqrt2*sqrt2}");

        //2 Gamma
        WriteLine($"gamma(1) = {sfuns.gamma(1)}, actual value is 1");
        WriteLine($"gamma(2) = {sfuns.gamma(2)}, actual value is 1");
        WriteLine($"gamma(3) = {sfuns.gamma(3)}, actual value is 2");
        WriteLine($"gamma(10) = {sfuns.gamma(10)}, actual value is 362880");

        //3 Lngamma
        WriteLine($"lngamma(1) = {sfuns.lngamma(1)}");
        WriteLine($"lngamma(2) = {sfuns.lngamma(2)}");
        WriteLine($"lngamma(3) = {sfuns.lngamma(3)}");
        WriteLine($"lngamma(10) = {sfuns.lngamma(10)}");
        WriteLine($"exp(lngamma(10)) = {Exp(sfuns.lngamma(10))}");
	}
	
}

using System;
using static System.Console;
using static System.Math;
class main{
	public static int Main(){

		//Complex
		complex neg_one = new complex(-1,0);
		complex i = new complex(0,1);
		complex ln_i = cmath.log(cmath.I);
		complex i_pow_i = cmath.pow(cmath.I,cmath.I);

		WriteLine($"Complex\nsqrt(-1)={cmath.sqrt(neg_one)}");
		WriteLine($"sqrt(-1) == i: {cmath.sqrt(neg_one).approx(i)}");
		WriteLine($"sqrt(-1) == -i: {cmath.sqrt(neg_one).approx(-i)}\n");

		WriteLine($"sqrt(i)={cmath.sqrt(cmath.I)}");
		WriteLine($"sqrt(i) == e^(1/2 ln(i)): {cmath.sqrt(cmath.I).approx(cmath.exp(0.5*ln_i))}");
		WriteLine($"sqrt(i) == i*pi/2: {cmath.sqrt(cmath.I).approx(cmath.exp(i*PI/4))}\n");

		WriteLine($"exp(i)={cmath.exp(cmath.I)}");
		WriteLine($"exp(i*pi)={cmath.exp(cmath.I*PI)}\n");

		WriteLine($"i^i={cmath.pow(cmath.I,cmath.I)}");
		WriteLine($"i^i == e^(i*ln(i)): {i_pow_i.approx(cmath.exp(i*ln_i))}");
		WriteLine($"i^i == e^(-pi/2): {i_pow_i.approx(Exp(-PI/2))}\n");

		WriteLine($"ln(i)={cmath.log(cmath.I)}");
		WriteLine($"ln(i) == ln(e^(i*pi/2)): {ln_i.approx(cmath.log(cmath.exp(i*PI/2)))}");
		WriteLine($"ln(i) == i*pi/2: {ln_i.approx(i*PI/2)}\n");
		
		WriteLine($"sin(i*pi)={cmath.sin(cmath.I*PI)}");
		return 0;
	}
}

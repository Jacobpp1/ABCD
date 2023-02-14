using System;
using static System.Console;
using static System.Math;
public static class main{
	public static void Main(){
		/*
		int n = 9;
		double[] a = new double[n];
		for(int i = 0; i < n; i++) WriteLine($"a[{i}] = {a[i]}");
		for(int i=0;i<n;i++) a[i]=i;
		for(int i=0;i<n;i++) WriteLine($"a[{i}] = {a[i]}");
		WriteLine($"array lenght = {a.Length}");
		double[] b = new double[n];
		for(int i = 0; i < n; i++){
			b[i] = i;
			WriteLine($"b[{i}]={b[i]}");
		}
		*/
		vec v = new vec(2,3);
		v.print("v = ");
		vec u = new vec(1,2,3);
		u.print("u = ");
		(u+v).print("u+v = ");
		(2*u).print("2*u = ");
		(2*(u+v)).print("2*(u+v) = ");
		(2*(u-v)).print("2*(u-v) = ");
		WriteLine($"Dot product of u and v = {u%v}");
		WriteLine($"Dot product of u and v = {u.dot(v)}");
		u.cross(v).print($"Cross product of u and v = ");
		
		WriteLine($"Norm of u = {u.norm()}");
		WriteLine($"Testing ToString override: u = ({u.ToString()}) and ({u})");
		
		WriteLine($"comparing u and v: {u.approx(v)}");
		WriteLine($"comparing u and u: {u.approx(u)}");
		
		// Epsilon
		WriteLine("Epsilon exercise");
		epsilon();
		
	}
	
	public static void epsilon(){
		//1
		int i=1; while(i+1>i) {i++;}
		Write("my max int = {0}\n",i);
		int j=1; while(j-1<j) {j++;}
		Write("my min int = {0}, while int.MinValue = {1}\n",j,int.MinValue);

		//2
		double x=1; while(1+x!=1){x/=2;} x*=2;
		float y=1F; while((float)(1F+y) != 1F){y/=2F;} y*=2F;
		WriteLine("The double machine epsilon is ({0}), while the 52-bit representation gives: ({1})", x, System.Math.Pow(2,-52));
		WriteLine("The float machine epsilon is ({0}), while the 23-bit representation gives: ({1})", x, System.Math.Pow(2,-23));

		//3
		int n=(int)1e6;
		double epsilon=Pow(2,-52);
		double tiny=epsilon/2;
		double sumA=0,sumB=0;
	
		sumA+=1; for(int k=0;k<n;k++){sumA+=tiny;}
		for(int l=0;l<n;l++){sumB+=tiny;} sumB+=1;
	
		WriteLine($"sumA-1 = {sumA-1:e} should be {n*tiny:e}");
		WriteLine($"sumB-1 = {sumB-1:e} should be {n*tiny:e}");

		//4
		double d1 = 0.1+0.1+0.1+0.1+0.1+0.1+0.1+0.1;
		double d2 = 8*0.1;

		WriteLine($"d1={d1:e15}");
		WriteLine($"d2={d2:e15}");
		WriteLine($"d1==d2 ? => {d1==d2}");

		WriteLine("Testing approx with d1 and d2: {0}", approx(d1, d2));
	}
	public static bool approx(double a, double b, double acc=1e-9, double eps=1e-9){
		if(Abs(b-a) < acc) return true;
		else if(Abs(b-a) < Max(Abs(a),Abs(b))*eps) return true;
		else return false;
	}
}

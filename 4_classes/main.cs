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
	}
}

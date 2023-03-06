using System;
using static System.Console;
using static System.Math;

class main{
	public static void Main(){
		//A1
		WriteLine("Problem A1");
		matrix A = new matrix("3,4,5;6,7,18;9,10,11;12,13,15");
		A.print();
		matrix R = new matrix(A.size2,A.size2);
		funcs.QRGSdecomp(A, R);
		R.print();
		matrix b = A.transpose()*A;
		b.print();
		matrix P = A*R;
		P.print();
		
		//A2
		WriteLine("Problem A2");
		A = new matrix("3,4,5;6,7,18;9,10,11;12,13,15");
		R = new matrix(A.size2,A.size2);
		A.print();
		funcs.QRGSdecomp(A,R); //A is now Q.
		vector v = new vector("1,2,3,4");
		vector x = funcs.QRGSsolve(A,R,v);
		vector check = A*R*x;
		WriteLine("A*x=Q*R*x");
		check.print();
		WriteLine("Starting vector");
		v.print();

		//B
		WriteLine("Problem B");
		A = new matrix("1,2,3;4,5,6;7,8,10");
		R = new matrix(A.size2, A.size2);
		WriteLine("A=");
		A.print();
		funcs.QRGSdecomp(A,R);
		matrix B = funcs.QRGSinverse(A,R);
		WriteLine("Inverse of A=");
		B.print();
		WriteLine("A*A^-1=");
		matrix C = A*R;
		matrix D = C*B;
		D.print();

		//C
		WriteLine("Problem C");
		
	}
}

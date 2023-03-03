using System;
using static System.Console;
using static System.Math;

class main{
	public static void Main(){
		//A1
		WriteLine("A1");
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
		WriteLine("A2");
		A = new matrix("3,4,5;6,7,18;9,10,11;12,13,15");
		R = new matrix(A.size2,A.size2);
		A.print();
		funcs.QRGSdecomp(A,R); //A is now Q.
		vector v = new vector("1,2,3,4");
		vector x = funcs.QRGSsolve(A,R,v);
		vector check = A*R*x;
		WriteLine("A*x=Q*R*x");
		check.print();
		v.print();

		//B
		A = new matrix("1,2,3;4,5,6;7,8,9");
		R = new matrix(A.size2, A.size2);
		funcs.QRGSdecomp(A,R);
		matrix B = funcs.QRGSinverse(A,R);
		B.print();
	}
}

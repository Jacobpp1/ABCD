using System;
using static System.Console;
using static System.Math;

class main{
	public static void Main(string[] args){
		//A1
		var rnd = new Random(1); // debugging !
		WriteLine("Problem A1");
		matrix A = rnd_matrix(4,2);
		for(int i=0;i<A.size1;i++)
		for(int j=0;j<A.size2;j++)
			A[i,j]=rnd.NextDouble();
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
		int N = 5;
		
		foreach(var arg in args){
			var words = arg.Split(':');
			if(words[0]=="-size"){
				var number=words[1];
				N = int.Parse(number);
			}
		}

		A = rnd_matrix(N,N);
		A.print();
		R = new matrix(N,N);
		funcs.QRGSdecomp(A,R);
		WriteLine("See figure Time_fit. Fits well to third degree polynomial.");
	}

	static matrix rnd_matrix(int n, int m){
		var rnd = new Random(1); // debugging !
		matrix A = new matrix(n,m);
		for(int i=0;i<A.size1;i++)
			for(int j=0;j<A.size2;j++)
				A[i,j]=rnd.NextDouble();
		return A;
	}
}

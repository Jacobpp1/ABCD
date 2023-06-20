using System;
using static System.Console;
using static System.Math;

class main{
	public static void Main(string[] args){
		//A12
		//var rnd = new Random(1); // debugging);
		var rnd = new Random(); // debugging);
		WriteLine("Problem A1 (checking decomp)");
		matrix A = rnd_matrix(4,2);
		//for(int i=0;i<A.size1;i++)
		//	for(int j=0;j<A.size2;j++)
		//		A[i,j]=rnd.NextDouble();
		WriteLine("A Matrix:");
		A.print();
		matrix R = new matrix(A.size2,A.size2);
		funcs.QRGSdecomp(A, R);
		WriteLine("R Matrix:");
		R.print();
		matrix b = A.transpose()*A;
		WriteLine("Q^T * Q");
		b.print();
		matrix P = A*R;
		WriteLine("Q*R: (should be same as A)");
		P.print();
		WriteLine("And it is :)\n");
		
		//A2
		WriteLine("Problem A2 (checking solve)");
		//A = new matrix("3,4,5;6,7,18;9,10,11;12,13,15");
		A = rnd_matrix(4,4);
		R = new matrix(A.size2,A.size2);
		WriteLine("A Matrix:");
		A.print();
		matrix Q = A.copy();
		funcs.QRGSdecomp(Q,R); //A is now Q.
		//vector v = new vector("1,2,3,4");
		vector v = new vector(Q.size1);
		for(int i = 0; i< v.size; i++) v[i] = rnd.NextDouble();
		vector x = funcs.QRGSsolve(Q,R,v);
		vector check = A*x;
		WriteLine("A*x=Q*R*x =");
		check.print();
		WriteLine("Starting vector, b (should be same as above)");
		v.print();
		WriteLine("Which it is\n");

		//A3?? Check determinant
		double det = funcs.QRGSdet(A);
		WriteLine($"Determinant of matrix A is: {det}");

		//B
		WriteLine("Problem B");
		//A = new matrix("1,2,3;4,5,6;7,8,10");
		A = rnd_matrix(3,3);
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
		WriteLine("As expected. Only numerical uncertainty thus e-16 or so numbers equal 0\n");

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
		R = new matrix(N,N);
		funcs.QRGSdecomp(A,R);
		WriteLine("See figure Time_fit. Fits well to third degree polynomial.");
	}

	static matrix rnd_matrix(int n, int m, int rand_id = -1){
		Random rnd = null;
		if(rand_id == -1){
			rnd = new Random();} // debugging !
		else{rnd = new Random(rand_id);};
		matrix A = new matrix(n,m);
		for(int i=0;i<A.size1;i++)
			for(int j=0;j<A.size2;j++)
				A[i,j]=rnd.NextDouble();
		return A;
	}
}

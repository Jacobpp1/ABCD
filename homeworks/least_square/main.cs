using System;
using static System.Console;
using static System.Math;
class main{
    public static void Main(){
        
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
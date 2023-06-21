using System;
using static System.Math;
using static System.Console;
using System.IO;

public class main{

    public static void Main(string[] args){
        //A
        matrix A = rnd_matrix(4,4);
        matrix B = rnd_matrix(3,3);
        matrix C = rnd_matrix(4,4);
        matrix V = C.copy();
        V.set_identity();
        matrix I = V.copy();
        
        //A1
        WriteLine("A1 - testing timesJ with matrix before and after");
        A.print();
        funcs.timesJ(A, 2, 3, 21.4);
        A.print();
        
        //A2
        WriteLine("\nA2 - testing Jtimes with matrix before and after");
        B.print();
        funcs.Jtimes(B,2,2,21.4);
        B.print();
        
        //A3
        WriteLine("\nA3 - testing cyclic with matrix before and after");
        matrix C_copy = C.copy();
        C.print();
        funcs.cyclic(C,V);
        C.print();
        WriteLine("\nNow showing V and then D=V^T*C*V matrices");
        V.print();
        matrix D = V.transpose()*C_copy*V;
        D.print();
        WriteLine("\nTesting if matrices are the same, using approx method");
        WriteLine($"VT*A*V == D: {C.approx(D)}");
        WriteLine($"A == V*D*VT: {C_copy.approx(V*D*V.transpose())}");
        WriteLine($"VT*V == I: {I.approx(V.transpose()*V)}");
        WriteLine($"V*VT == I: {I.approx(V*V.transpose())}\n");
        /*WriteLine("V = ");
        V.print();
        WriteLine("D = ");
        D.print();*/


        //B Numerical

        /* mono main.exe -rmax:10 -dr:0.3 */
        double rmax = 0;
        double dr = 0;

		foreach(var arg in args){
			var words = arg.Split(':');
			if(words[0]=="-rmax"){
				var number=words[1];
				rmax = double.Parse(number);
			}
            if(words[0]=="-dr")
                dr = double.Parse(words[1]);
		}
        
        WriteLine("B:");
        WriteLine($"rmax = {rmax}, dr = {dr}");
        
        matrix H = ham_creator(rmax, dr);
        matrix VB = H.copy();
        VB.set_identity();
        matrix DB = H.copy();

        funcs.cyclic(DB,VB);
        /*WriteLine("Energies:");
        for( int i=0; i<H.size2;i++) WriteLine(DB[i,i]);
        var rand = new Random(1);
        double[] drs = {0.3, 0.34, 0.38, 0.42, 0.46, 0.50, 0.54, 0.58, 0.62, 0.66, 0.7, 0.74, 0.78, 0.82, 0.86, 0.9, 0.94, 0.98};
        */

        //WriteLine("dr from 0.3 to 4 in intervals of 0.1:");

        // Fixed rmax
        string toWrite = $"";
        for(double i=0.1; i<=4; i+=0.1){
            dr = i;
            H = ham_creator(rmax, dr);
            V = H.copy();
            V.set_identity();
            funcs.cyclic(H,V);
            //WriteLine($"E0 = {H[0,0]}");
            toWrite += $"{dr}\t{H[0,0]}\n";
        }
        File.WriteAllText("fixed_rmax.data", toWrite);
        WriteLine($"Plot varying dr between 0.1 and 4 and with rmax fixed at {rmax} found in 'rmax_fixed.svg'");

        // Fixed dr
        dr = 0.3;
        //double[] rmaxs = {0.8, 1.1, 1.4, 1.7, 2.0, 2.3, 2.6, 2.9, 3.2, 3.5, 3.8, 4.1, 4.4, 4.7, 5};
        //WriteLine("rmax from 0.8 to 5 in intervals of 0.3:");
        toWrite = $"";
        int len_vec = (int)(5.0/0.8);
        //var eigfuns = new vector[len_vec];
        for(double i=0.8; i<=5; i+=0.3){
            rmax = i;
            H = ham_creator(rmax, dr);
            V = H.copy();
            V.set_identity();
            funcs.cyclic(H,V);
            //WriteLine($"E0 = {H[0,0]}");
            toWrite += $"{rmax}\t{H[0,0]}\n";
            //eigfuns.Add(V[0]);
        }
        File.WriteAllText("fixed_dr.data", toWrite);
        WriteLine($"Plot varying rmax between 0.8 and 5 and with dr fixed at {dr} found in 'dr_fixed.svg'");
        
        // Plot eigenfunctions
        Func<double, double>[] fs = {x => 2*Exp(-x), x => 2/Pow(2,3.0/2)*(1-x/2)*Exp(-x/2), x => 2/Pow(3,3.0/2)*(1-2*x/3+2*x*x/27)*Exp(-x/3)};
        dr = 0.1;
        rmax = 25;
        H = ham_creator(rmax,dr);
        V = H.copy();
        V.set_identity();
        funcs.cyclic(H,V);
        int N = V[0].size;
        double num_wf;
        double anal_wf;
        WriteLine("Seeing if summing V_ik^2 gives unity");
        for(int k = 0; k<3; k++){
            toWrite = $"";
            double sum_check = 0;
            for(int i=0; i<N; i++){
                double r = dr*(i+1);
                sum_check += V[k][i]*V[k][i];
                num_wf = Pow(V[k][i]/Sqrt(dr),2);
                anal_wf = Pow(fs[k](r)*r, 2);
                toWrite += $"{r}\t{num_wf}\t{anal_wf}\n";
            }
            File.WriteAllText($"eigenfuns_n{k+1}.data", toWrite);
            WriteLine(sum_check);
        }
        WriteLine("Radial wavefunctions (r*R)^2 plotted in figure 'eigenfuns.svg'. Analytical functions found at ");
        WriteLine("https://quantummechanics.ucsd.edu/ph130a/130_notes/node233.html and were multiplied by r and all squared.");
    }

    static matrix ham_creator(double rmax, double dr){
        int npoints = (int)(rmax/dr) - 1;
        vector r = new vector(npoints);
        for(int i=0;i<npoints;i++) r[i]=dr*(i+1);
        matrix H = new matrix(npoints,npoints);
        for(int i=0;i<npoints-1;i++){
            H[i,i]  =-2;
            H[i,i+1]= 1;
            H[i+1,i]= 1;
        }
        H[npoints-1,npoints-1]=-2;
        //H.scale(-0.5/dr/dr);
        H *= -0.5/dr/dr;
        for(int i=0;i<npoints;i++) H[i,i]+=-1/r[i];
        return H;
    }

	static matrix rnd_matrix(int n, int m){
		var rnd = new Random(1); // debugging !
		matrix A = new matrix(n,m);
		for(int i=0;i<A.size1;i++)
			for(int j=0;j<A.size2;j++){
				A[i,j]=rnd.NextDouble();
                A[j,i] = A[i,j];
            }
		return A;
	}
}
using System;
using static System.Console;
using static System.Math;
using System.IO;
class main{
    public static void Main(){
		Func<double, double> f0 = (double x) => 1;
		Func<double, double> f1 = (double x) => x;
		Func<double, double>[] fitfuncs = {f0, f1};

		double[] ts = {1, 2, 3, 4, 6, 9, 10, 13, 15};
		double[] As = {117, 100, 88, 72, 53, 29.5, 25.2, 15.2, 11.1};
		double[] dAs = {5, 5, 5, 4, 4, 3, 3, 2, 2};
		int n = As.Length;

		double[] lnAs = new double[n];
		for(int i=0; i<n; i++) lnAs[i] = Math.Log(As[i]);
		double[] lndAs = new double[n];
		for(int i=0; i<n; i++) lndAs[i] = dAs[i]/As[i];

		(vector fit_vec, matrix fit_mat) = funcs.lsfit(fitfuncs, ts, lnAs, lndAs);

		double lambda = -fit_vec[1];
        WriteLine($"A\nFrom the fit I got lambda to be {lambda:e2} days^-1, so the half life is T=ln(2)/lambda={(Math.Log(2)/lambda):e2}d");
		WriteLine($"Modern value is known to be 3.63d");
		double a = fit_vec[0];
		double b = fit_vec[1];
		WriteLine($"Fit parameters: ln(a)={fit_vec[0]:e2}, lambda={-fit_vec[1]:e2}");

		string toWrite = $"";
		double t = 0;
		for(int i=0; i<n; i++){
			t = ts[i];
			toWrite += $"{t}\t{a*f0(t)+b*f1(t)}\n";
		}
		File.WriteAllText("self_fit.data", toWrite);
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
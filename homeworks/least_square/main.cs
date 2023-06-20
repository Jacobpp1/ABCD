using System;
using static System.Console;
using static System.Math;
using System.IO;
class main{
    public static void Main(){
		// Want to fit to a constant and a linear term
		Func<double, double> f0 = (double x) => 1;
		Func<double, double> f1 = (double x) => x;
		Func<double, double>[] fitfuncs = {f0, f1};

		double[] ts = {1, 2, 3, 4, 6, 9, 10, 13, 15};
		double[] As = {117, 100, 88, 72, 53, 29.5, 25.2, 15.2, 11.1};
		double[] dAs = {5, 5, 5, 4, 4, 3, 3, 2, 2};
		string thx_write = $"";
		int n = As.Length;
		for(int i=0; i<n; i++) thx_write += $"{ts[i]}\t{As[i]}\t{dAs[i]}\n";
		File.WriteAllText("thx.data", thx_write);

		double[] lnAs = new double[n];
		for(int i=0; i<n; i++) lnAs[i] = Math.Log(As[i]);
		double[] lndAs = new double[n];
		for(int i=0; i<n; i++) lndAs[i] = dAs[i]/As[i];

		(vector fit_vec, matrix fit_mat) = funcs.lsfit(fitfuncs, ts, lnAs, lndAs);

		double lambda = -fit_vec[1];
		double T = (Math.Log(2)/lambda);
        WriteLine($"A\nFrom the fit I got lambda to be {lambda:e2} days^-1, so the half life is T=ln(2)/lambda={T:e2}d");
		WriteLine($"Modern value is known to be 3.63d");
		double a = fit_vec[0];
		double b = fit_vec[1];
		WriteLine($"Fit parameters: ln(a)={fit_vec[0]:e2}, lambda={-fit_vec[1]:e2}");
		WriteLine($"Fit can be seen in figure 'self_fit.svg'\n");

		string toWrite = $"";
		double t = 0;
		for(int i=0; i<n; i++){
			t = ts[i];
			toWrite += $"{t}\t{a*f0(t)+b*f1(t)}\n";
		}
		File.WriteAllText("self_fit.data", toWrite);

		//B
		WriteLine($"B)\nCovariance matrix is:");
		fit_mat.print();
		(double err_lna, double err_lambda) = (Sqrt(fit_mat[0][0]), Sqrt(fit_mat[1][1]));
		WriteLine($"Uncertainty of ln(a) is {err_lna} and uncertainty of lambda is {err_lambda}");
		double err_T = Log(2)*err_lambda/(lambda*lambda);
		WriteLine($"Thus the uncertainty of the half life is |dT/dlambda*deltalambda| = ln(2)/(lambda^2)*delta_lambda = {err_T:e2}d");
		WriteLine($"This does not get the modern value within the uncertainty, [{T-err_T}, {T+err_T}]");

		//C
		string toWrite_plus = $"";
		string toWrite_minus = $"";
		t = 0;
		for(int i=0; i<n; i++){
			t = ts[i];
			toWrite_plus += $"{t}\t{(a+err_lna)*f0(t)+(b+err_lambda)*f1(t)}\n";
			toWrite_minus += $"{t}\t{(a-err_lna)*f0(t)+(b-err_lambda)*f1(t)}\n";
		}
		File.WriteAllText("self_fit_C_p.data", toWrite_plus);
		File.WriteAllText("self_fit_C_m.data", toWrite_minus);
		WriteLine("\nC)\nFit with uncertainties in parameters (added or subtracted for both parameters) found in figure 'self_fit_C.svg'");
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
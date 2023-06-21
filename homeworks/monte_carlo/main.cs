using System;
using static System.Math;
using static System.Console;
using static System.Random;
using System.IO;
class main{
    public static void Main(){
        Func<vector,double> circ = x => {
            double r = Sqrt(x[0]*x[0] + x[1]*x[1]);
            if(r<=1) return 1;
            else return 0;
        };
        vector circ_low = new vector(-1,-1);
        vector circ_high = new vector(1,1);
        WriteLine($"Integrating a unit circle with 1000000 points. Should give PI. Integral is: {plainmc(circ, circ_low, circ_high, 1000000).Item1}");
        string toWrite = $"";
        Func<vector, double> f1 = x => 0.1*Exp(-x[0]*x[0]/(2*4) - x[1]*x[1]/(2*1)); // 2-d Gaussian
        vector gauss_low = new vector(-5, -5);
        vector gauss_high = new vector(5, 5);

        Func<vector, double> f2 = x => Cos(x[0]-1)*Sin(x[1]); // cos and sin - Oscillating
        vector osc_low = new vector(-1, 0);
        vector osc_high = new vector(0.5, 1);
        double circ_res = 0, circ_err = 0, gauss_res = 0, gauss_err = 0, osc_res = 0, osc_err = 0;
        WriteLine(gauss_res);
        for(int n=10; n<=1e8; n*=10){
            (circ_res, circ_err) = plainmc(circ, circ_low, circ_high, n);
            (gauss_res, gauss_err) = plainmc(f1, gauss_low, gauss_high, n);
            (osc_res, osc_err) = plainmc(f2, osc_low, osc_high, n);

            //WriteLine($"Using {n} points gives {ress}±{errr}");
            toWrite += $"{n}\t{circ_res}\t{Abs(circ_err)}\t{Abs(PI-circ_res)}\t"; // Circle
            toWrite += $"{gauss_res}\t{Abs(gauss_err)}\t{Abs(1.24103-gauss_res)}\t"; // 2d Gauss
            toWrite += $"{osc_res}\t{Abs(osc_err)}\t{Abs(0.197611-osc_res)}\n"; // Oscillating
        }
        WriteLine(gauss_res);
        WriteLine("A\nIntegrating three functions:");
        WriteLine("Circle which is just the unit circle integrated from -1 to 1 for both x and y.");
        WriteLine("A 2D-Gaussian which is 0.1*e^(-x^2/8 - y^2/2), integrated from -5 to 5 for both x and y");
        WriteLine("And 'Oscillating' which is Cos(x-1)*Sin(y), integrated from -1 to 0.5 for x and 0 to 1 for y");
        WriteLine("\nPlot indicating error reduction with number of points shown in 'A_err_plot.svg'\nSeems to generally follow 1/sqrt(n) as expected");
        File.WriteAllText("A_errs.data", toWrite);
        //Solving hard integral in A)
        Func<vector,double> f3 = x => Pow(1/PI,3) / (1-Cos(x[0])*Cos(x[1])*Cos(x[2]));
        vector a3 = new vector(0, 0, 0);
        vector b3 = new vector(PI, PI, PI);
        (double res3, double err3) = plainmc(f3, a3, b3, 10000000);
        WriteLine($"\nSolving integral given in problem A with 1e7 points gives: {res3}±{err3}\nShould be 1.3932039296856768591842462603255");

        // B (Compare error between plain and quasi)
        // Calculating errors on same functions as before but using quasi
        (double circ_qres, double circ_qerr) = quasimc(circ, circ_low, circ_high, (int)(1e8));
        (double gauss_qres, double gauss_qerr) = quasimc(f1, gauss_low, gauss_high, (int)(1e8));
        (double osc_qres, double osc_qerr) = quasimc(f2, osc_low, osc_high, (int)(1e8));
        WriteLine("B\nComparing errors between plain and quasi MC, using 1e8 points:");
        WriteLine($"For circle integral, plain error is {circ_err}, and quasi is {circ_qerr}");
        WriteLine($"For Gaussian integral, plain error is {gauss_err}, and quasi is {gauss_qerr}");
        WriteLine($"For oscillating integral, plain error is {osc_err}, and quasi is {osc_qerr}");
        WriteLine("So the error seems to generally be a couple of orders of magnitude better using the quasi-random method");
        WriteLine("\nTo investigate how the error scales with number of points, I now just integrate the circle.\n");
        WriteLine("Results are shown in figure 'B_err_plot.svg'. Now with log10 of errors also.");
        WriteLine("Seems to have the same slope for both methods, but more precise using quasi method.");
        toWrite = $"";
        for(int n=10; n<=1e8; n*=10){
            (circ_qres, circ_qerr) = quasimc(circ, circ_low, circ_high, n);
            toWrite += $"{n}\t{circ_qerr}\t{Abs(PI-circ_qres)}\n";
        }
        File.WriteAllText("B_errs.data", toWrite);
    }

    static (double,double) plainmc(Func<vector,double> f,vector a,vector b,int N){
        Random rand = new Random();
        double RANDOM = 0;
        int dim = a.size; double V = 1;
        for(int i=0;i<dim;i++) V*=b[i]-a[i];
        double sum = 0, sum2 = 0;
	    var x = new vector(dim);
        //string toWrite = $"";
        for(int i=0;i<N;i++){
                for(int k=0;k<dim;k++){RANDOM = rand.NextDouble(); x[k] = a[k] + RANDOM*(b[k]-a[k]);} //toWrite += $"{x[k]}\t";}
                //toWrite += $"\n";
                double fx = f(x);
                sum += fx; sum2 += fx*fx;
        }
        //File.WriteAllText("A.data", toWrite);
        double mean = sum/N;
        double sigma = Sqrt(sum2/N-mean*mean);
        var result = (mean*V,sigma*V/Sqrt(N));
        return result;
    }

    static (double,double) quasimc(Func<vector,double> f,vector a,vector b,int N){
        int dim = a.size; double V = 1;
        for(int i=0;i<dim;i++) V*=b[i]-a[i];
        double sum = 0, sum2 = 0;
	    var x = new vector(dim);
        var y = new vector(dim);
        //string toWrite = $"";
        for(int i=0;i<N;i++){
                halton(i, dim, x);
                halton(i, dim, y, true);
                for(int k=0;k<dim;k++){x[k] = a[k] + x[k]*(b[k]-a[k]);} //toWrite += $"{x[k]}\t";}
                for(int k=0;k<dim;k++){y[k] = a[k] + y[k]*(b[k]-a[k]);} //toWrite += $"{x[k]}\t";}
                //toWrite += $"\n";
                sum += f(x); sum2 += f(y);
        }
        //File.WriteAllText("A.data", toWrite);
        var result = (sum*V/N,Abs(sum*V/N-sum2*V/N));
        return result;
    }

    public static double corput (int n, int b){
        double q=0 , bk=(double) 1/ b ;
        while(n>0){
            q += (n%b) * bk;
            n /= b;
            bk /= b;
        }
        return q;
    }

    public static void halton(int n, int d, double[] x, bool reverse = false){
        int[] bases = {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67};
        int n_base = bases.Length;
        int maxd = n_base;
        if(!(d <= maxd)) throw new Exception("halton, d too large");
        for(int i=0; i<d; i++){
            if(reverse) x[i] = corput(n, bases[n_base-1-i]);
            else x[i] = corput(n, bases[i]);
        }

    }
}
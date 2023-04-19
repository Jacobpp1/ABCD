using System;
using static System.Math;
using static System.Console;
using static System.Random;
using System.IO;
class main{
    public static void Main(){
        vector A = new vector(2,3);  //Goes from x=2, y=3
        vector B = new vector(5,10); //  to      x=5, y=5
        int npoints = (int)1e7;
        Func<vector,double> f1 = (x) => Cos(x[0]) * Sin(x[1]);
        (double res1, double res2) = plainmc(f1, A, B,   npoints);
        //(double resA1, double resA2) = plainmc(f, A, B, 100000000);
        //(double resB1, double resB2) = plainmc(f, A, B, 100000000);
        //(double resC1, double resC2) = plainmc(f, A, B, 100000000);
        
        WriteLine($"Integrating cos(x)*sin(y) for x={A[0]}..{B[0]}, y={A[1]}..{B[1]}");
        WriteLine($"{res1} ± {res2}");
        WriteLine($"Integral should give 0.28195");
        //WriteLine($"{resA1}, {resA2}");
        //WriteLine($"{resB1}, {resB2}");
        //WriteLine($"{resC1}, {resC2}");

        Func<vector,double> f2 = x => Pow(1/PI,3) / (1-Cos(x[0])*Cos(x[1])*Cos(x[2]));
        A = new vector(0, 0, 0);
        B = new vector(PI, PI, PI);
        (double res, double err) = plainmc(f2, A, B, npoints);
        Write("Integral given in exercise gives: ");
        WriteLine($"{res} ± {err}, with {npoints} datapoints");
        WriteLine("Should be 1.3932039296856768591842462603255\n");

        //B
        A = new vector(2,3);  //Goes from x=2, y=3
        B = new vector(5,10); //  to      x=5, y=5
        (res1, res2) = quasimc(f1, A, B,   npoints);
        WriteLine("Problem B");
        WriteLine($"Integrating cos(x)*sin(y) for x={A[0]}..{B[0]}, y={A[1]}..{B[1]}");
        WriteLine($"{res1} ± {res2}");
        WriteLine($"Integral should give 0.28195");
        //WriteLine($"{resA1}, {resA2}");
        //WriteLine($"{resB1}, {resB2}");
        //WriteLine($"{resC1}, {resC2}");

        f2 = (x) => Exp(x[0])*Cos(x[1]*Sin(x[2]));
        A = new vector(0, 0, 0);
        B = new vector(PI, PI, PI);
        (res, err) = quasimc(f2, A, B, npoints);
        Write("Integrating e^(x)*Cos(y*sin(z)) from 0 to pi for x,y,z gives: ");
        WriteLine($"{res} ± {err}, with {npoints} datapoints");
        WriteLine("Should be 93.7299");

        // Data for scaling errors
        string plain_dat = $"";
        string quasi_dat = $"";
        double plain_err = 0;
        double quasi_err = 0;
        for(int n = 100; n<=1e7; n*=10){
            (_, plain_err) = plainmc(f2,A,B,n);
            (_, quasi_err) = quasimc(f2,A,B,n);
            plain_dat += $"{n}\t{plain_err}\n";
            quasi_dat += $"{n}\t{quasi_err}\n";
        }
        File.WriteAllText("plain.data", plain_dat);
        File.WriteAllText("quasi.data", quasi_dat);
        
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
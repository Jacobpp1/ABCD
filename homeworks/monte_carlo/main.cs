using System;
using static System.Math;
using static System.Console;
using static System.Random;
using System.IO;
class main{
    public static void Main(){
        vector A = new vector(2,3);  //Goes from x=2, y=3
        vector B = new vector(5,10); //  to      x=5, y=5
        int npoints = (int)1e8;
        Func<vector,double> f = (x) => Cos(x[0]) * Sin(x[1]);
        (double res1, double res2) = plainmc(f, A, B,   npoints);
        //(double resA1, double resA2) = plainmc(f, A, B, 100000000);
        //(double resB1, double resB2) = plainmc(f, A, B, 100000000);
        //(double resC1, double resC2) = plainmc(f, A, B, 100000000);
        
        WriteLine($"Integrating cos(x)*sin(y) for x={A[0]}..{B[0]}, y={A[1]}..{B[1]}");
        WriteLine($"{res1} ± {res2}");
        WriteLine($"Integral should give 0.28195");
        //WriteLine($"{resA1}, {resA2}");
        //WriteLine($"{resB1}, {resB2}");
        //WriteLine($"{resC1}, {resC2}");

        f = x => Pow(1/PI,3) / (1-Cos(x[0])*Cos(x[1])*Cos(x[2]));
        A = new vector(0, 0, 0);
        B = new vector(PI, PI, PI);
        npoints = (int)1e8;
        (double res, double err) = plainmc(f, A, B, npoints);
        Write("Integral given in exercise gives: ");
        WriteLine($"{res} ± {err}, with {npoints} datapoints");
        WriteLine("Should be 1.3932039296856768591842462603255");

        //B
        A = new vector(2,3);  //Goes from x=2, y=3
        B = new vector(5,10); //  to      x=5, y=5
        f = (x) => Cos(x[0]) * Sin(x[1]);
        (res1, res2) = quasimc(f, A, B,   npoints);
        WriteLine("Problem B");
        WriteLine($"Integrating cos(x)*sin(y) for x={A[0]}..{B[0]}, y={A[1]}..{B[1]}");
        WriteLine($"{res1} ± {res2}");
        WriteLine($"Integral should give 0.28195");
        //WriteLine($"{resA1}, {resA2}");
        //WriteLine($"{resB1}, {resB2}");
        //WriteLine($"{resC1}, {resC2}");

        f = x => Pow(1/PI,3) / (1-Cos(x[0])*Cos(x[1])*Cos(x[2]));
        A = new vector(0, 0, 0);
        B = new vector(PI, PI, PI);
        npoints = (int)1e8;
        (res, err) = quasimc(f, A, B, npoints);
        Write("Integral given in exercise gives: ");
        WriteLine($"{res} ± {err}, with {npoints} datapoints");
        WriteLine("Should be 1.3932039296856768591842462603255");
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
        Random rand = new Random();
        double RANDOM = 0;
        int dim = a.size; double V = 1;
        for(int i=0;i<dim;i++) V*=b[i]-a[i];
        double sum = 0, sum2 = 0;
	    var x = new vector(dim);
        //var x = new double[dim];
        //string toWrite = $"";
        //halton(N, dim, x);
        int[] bases = {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67};
        for(int i=0;i<N;i++){
                for(int k=0;k<dim;k++){halton(i,dim,x); x[k] = a[k] + x[k]*(b[k]-a[k]);} //toWrite += $"{x[k]}\t";}
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

    public static double corput (int n, int b){
        double q=0 , bk=(double) (1/ b) ;
        while(n>0){
            q += (n%b) * bk;
            n /= b;
            bk /= b;
        }
        return q;
    }

    public static void halton(int n, int d, double[] x){
        int[] bases = {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67};
        int maxd = bases.Length;
        if(!(d <= maxd)) throw new Exception("halton, d too large");
        for(int i=0; i<d; i++) x[i] = corput(n, bases[i]);
    }
}
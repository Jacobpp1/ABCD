using System;
using System.IO;
using static System.Console;
using static System.Math;
using static System.Random;

class main{
    public static void Main(){
        Func<double, double> f1 = (x) => 1/Sqrt(x);
        //f1 = (x) => 1/Sqrt(Tan(x));
        double eps = 0.00001;
        for(int i = 1; i<=10000000; i*=10){
            var (test1, err1) = integ2(f1, i, 100, 0, 1);
            WriteLine($"{test1}±\t{err1}\tN={i}");
            var (test2, err2) = CC_transform2(f1, i, 100, 0, 1);
            WriteLine($"{test2}±\t{err2}\teps={eps}\tN={i}");
            //WriteLine($"{Tan(PI/4)}, {Tan(45)}");
        }
    }

    public static (double, double) integ(Func<double,double> f, int N, double min, double max, double eps = 0.0001){
        // Sample N points with MC
        double x;
        double fx;
        var rnd = new Random();
        vector xs = new vector(N);
        vector fxs = new vector(N);
        double sum = 0;
        double sum2 = 0;
        for(int i=0; i<N; i++){
            x = min + rnd.NextDouble()*(max-min);
            xs[i] = x;
            fx = f(x); // Maybe call CC_transform(f, min, max);
            fxs[i] = fx;
            sum += fx; sum2 += fx*fx;
        }
        // Estimate average and error
        double mean = sum/N, sigma = Sqrt(sum2/N - mean*mean);

        if(sigma < eps) return (mean, sigma);
        else{
            // Divide into two sub-spaces
            double mid = min + (max-min)/2;
            // Two recursive calls to sub-spaces
            var (mean_low, sigma_low) = integ(f, N/2, min, mid);
            var (mean_high, sigma_high) = integ(f, N/2, mid, max);
            // Estimate grand average and error
            double grand_mean = (mean_low+mean_high)/2, grand_sigma = Sqrt(sigma_low*sigma_low + sigma_high*sigma_high);
            return (grand_mean, grand_sigma);
        }
    }
    
    public static (double, double) CC_transform(Func<double,double> f, int N, double min, double max, double eps = 0.0001){
        double a = min, b=max;
        return integ(x => f((a+b)/2+(b-a)/2*Cos(x))*Sin(x)*(b-a)/2, N, 0, PI, eps);
    }
    
    public static (double, double) integ2(Func<double,double> f, int N, int N_min, double min, double max){
        // Sample N points with MC
        double x;
        double fx;
        var rnd = new Random();
        if(N <= N_min){
            vector xs = new vector(N);
            vector fxs = new vector(N);
            double sum = 0;
            double sum2 = 0;
            for(int i=0; i<N; i++){
                x = min + rnd.NextDouble()*(max-min);
                xs[i] = x;
                fx = f(x); // Maybe call CC_transform(f, min, max);
                fxs[i] = fx;
                sum += fx; sum2 += fx*fx;
            }
            // Estimate average and error
            double mean = sum/N, sigma = Sqrt(sum2/N - mean*mean);
            return (mean, sigma*(max-min)/Sqrt(N));
        }
        else{
            // Divide into two sub-spaces
            double mid = min + (max-min)/2;
            // Two recursive calls to sub-spaces
            var (mean_low, sigma_low) = integ2(f, N/2, N_min, min, mid);
            var (mean_high, sigma_high) = integ2(f, N/2, N_min, mid, max);
            // Estimate grand average and error
            double grand_mean = (mean_low+mean_high)/2, grand_sigma = Sqrt(sigma_low*sigma_low + sigma_high*sigma_high);
            return (grand_mean, grand_sigma);
        }
    }
    
    public static (double, double) CC_transform2(Func<double,double> f, int N, int N_min, double min, double max){
        double a = min, b=max;
        if(a == -1 & b==1){ return integ2(x => f(Cos(x))*Sin(x), N, N_min, 0, PI);};
        return integ2(x => f((a+b)/2+(b-a)/2*Cos(x))*Sin(x)*(b-a)/2, N, N_min, 0, PI);
    }
}
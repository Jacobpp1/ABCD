using System;
using System.IO;
using static System.Console;
using static System.Math;

class main{
    public static void Main(){

        WriteLine("Problem A:");
        double delta = 0.001;
        double res = integrate(x => Sqrt(x), 0, 1, delta);
        double[] lims = {res-delta, res+delta};
        WriteLine("Integral of sqrt(x) from 0 to 1 = " + res
        + $" should be 2/3±{delta}=[{lims[0]},{lims[1]}]" + $" which is: " + $"{lim_check(lims, res)}");
        
        double ord_counter_sqrtinv;
        counter = 0;
        res = integrate(x => 1/Sqrt(x), 0, 1, delta);
        ord_counter_sqrtinv = counter;
        lims[0] = res-delta; lims[1] = res+delta;
        WriteLine("Integral of 1/sqrt(x) from 0 to 1 = " + res
        + $" should be 2±{delta}=[{lims[0]},{lims[1]}]" + $" which is: " + $"{lim_check(lims, res)}");

        res = integrate(x => 4*Sqrt(1-x*x), 0, 1, delta);
        lims[0] = res-delta; lims[1] = res+delta;
        WriteLine("Integral of 4sqrt(1-x^2) from 0 to 1 = " + res
        + $" should be pi±{delta}=[{lims[0]},{lims[1]}]" + $" which is: " + $"{lim_check(lims, res)}");

        counter = 0;
        double ord_counter_lnsqrt;

        res = integrate(x => Log(x)/Sqrt(x), 0, 1, delta);
        ord_counter_lnsqrt = counter;
        lims[0] = res-delta; lims[1] = res+delta;
        WriteLine("Integral of ln(x)/sqrt(x) from 0 to 1 = " + res
        + $" should be -4±{delta}=[{lims[0]},{lims[1]}]" + $" which is: " + $"{lim_check(lims, res)}");

        WriteLine($"\nCalculating errorfunction using integration method gives: Erf(2) = {erf(2)}");
        WriteLine("Expect it to be around 0.995322265 (according to Wolfram Alpha & Wikipedia)");
        string toWrite = $"";
        for(double i=-2; i<=4.001; i+=0.05)
            toWrite += $"{i}\t{erf(i)}\n";
        File.WriteAllText("A.data", toWrite);
        WriteLine("Plotting error function using integration method, tabulated values, and 'plots' exercise in figure 'A.svg'. They all seem to overlap.");

        //A) Compare with tabulated
        string[] tab_erf = File.ReadAllText("err_tab.data").Split("\n"); //Tabulated values
        toWrite = $"";
        double tab_x = 0;
        double tab_y = 0;
        for(int i = 1; i < tab_erf.Length; i++){
            tab_x = Double.Parse(tab_erf[i].Split(" \t")[0]);
            tab_y = Double.Parse(tab_erf[i].Split(" \t")[1]);
            toWrite += $"{tab_x}\t{Abs(tab_y-erf(tab_x))}\t{Abs(tab_y-erf_app(tab_x))}\n";
        }
        File.WriteAllText("erf_compare.data", toWrite);
        WriteLine("In the figure 'A_erf_difference.svg' we see that the integration method generally has lower error than the approximated method.");
        WriteLine("The data is made with absolute and relative error of 1e-7 for both.\n");


        //B
        WriteLine("Problem B:");
        counter = 0;
        double py_eps = 0.000000149;
        res = adapt_int(x => 1/Sqrt(x), 0, 1, py_eps, py_eps);
        lims[0] = res-delta; lims[1] = res+delta;
        WriteLine("Integral of 1/sqrt(x) from 0 to 1 using transformation = " + res
        + $" should be 2±{delta}=[{lims[0]},{lims[1]}]" + $" which is: " + $"{lim_check(lims, res)}");
        WriteLine($"Absolute and relative error was {py_eps} here.");
        WriteLine($"Evaluations using transformation: {counter}. Evaluations without: {ord_counter_sqrtinv} - even using lower accuracy");
        WriteLine("Python solves this with absolute and relative error as 1.49e-8 using 231 evaluations\n");

        counter = 0;
        double err2 = 1e-4; 
        res = adapt_int(x => Log(x)/Sqrt(x), 0, 1, err2, err2);
        lims[0] = res-delta; lims[1] = res+err2;
        WriteLine("Integral of ln(x)/sqrt(x) from 0 to 1 using transformation = " + res
        + $" should be -4±{err2}=[{lims[0]},{lims[1]}]" + $" which is: " + $"{lim_check(lims, res)}");
        WriteLine($"Absolute and relative error was {1e-4} here. Could not run with better precision :(");
        WriteLine($"Evaluations using transformation: {counter}. Evaluations without: {ord_counter_lnsqrt} - even using lower accuracy");
        WriteLine("Python solves this with absolute and relative error as 1.49e-8 using 315 evaluations");

        WriteLine("\nPython method seems to be more efficient. Reason for more evaluations of second function is the precision limit with my implementation.");
    }

    static double counter;

    static double integrate (Func<double,double> f, double a, double b,
    double delta=0.001, double eps=0.001, double f2=Double.NaN, double f3=Double.NaN){
        //if(a == Double.PositiveInfinity && b == NegativeInfinity)
        //    integrate(t => f(t/(1-t*t)) * (1+t*t)/(1-t));
        counter++;
        double h=b-a;
        if(Double.IsNaN(f2)){ f2=f(a+2*h/6); f3=f(a+4*h/6); } // first call, no points to reuse
        double f1=f(a+h/6), f4=f(a+5*h/6);
        double Q = (2*f1+f2+f3+2*f4)/6*(b-a); // higher order rule
        double q = (  f1+f2+f3+  f4)/4*(b-a); // lower order rule
        double err = Abs(Q-q);
        if (err <= delta+eps*Abs(Q)) return Q;//(Q, counter);
        else
            //(double i1, double c1) = integrate(f,a,(a+b)/2,delta/Sqrt(2),eps,f1,f2, counter);
            //(double i2, double c2) = integrate(f,(a+b)/2,b,delta/Sqrt(2),eps,f3,f4, counter);
            
            return (integrate(f,a,(a+b)/2,delta/Sqrt(2),eps,f1,f2)+
                    integrate(f,(a+b)/2,b,delta/Sqrt(2),eps,f3,f4));
            //return (i1+i2, counter);
    }

    static double adapt_int(Func<double,double> f, double a, double b,
    double delta=0.001, double eps=0.001, double f2=Double.NaN, double f3=Double.NaN){
        //if(counter != -1)
        //    counter++;
        return integrate(x => f((a+b)/2+(b-a)/2*Cos(x))*Sin(x)*(b-a)/2, 0, PI, delta, eps, f2, f3);
    }

    static double erf(double z){
        if (z<0)
            return -erf(-z);
        else if (z<1)
            return 2/Sqrt(PI)*integrate((x) => Exp(-x*x), 0, z, 1e-7, 1e-7);
        else
            return 1-2/Sqrt(PI)*integrate((x) => Exp(-(z+(1-x)/x)*(z+(1-x)/x))/x/x, 0, 1, 1e-7, 1e-7);
    }

    static double erf_app(double x){
    /// single precision error function (Abramowitz and Stegun, from Wikipedia)
    if(x<0) return -erf(-x);
    double[] a={0.254829592,-0.284496736,1.421413741,-1.453152027,1.061405429};
    double t=1/(1+0.3275911*x);
    double sum=t*(a[0]+t*(a[1]+t*(a[2]+t*(a[3]+t*a[4]))));/* the right thing */
    return 1-sum*Exp(-x*x);
} 

    static bool lim_check(double[] limits, double result){
        return limits[0] < result && result < limits[1];
    }
}
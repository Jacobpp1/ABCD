using System;
using System.IO;
using static System.Console;
using static System.Math;

class main{
    public static void Main(){

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

        WriteLine($"Erf(2) = {erf(2)}");
        string toWrite = $"";
        for(double i=-2; i<=2.001; i+=0.05)
            toWrite += $"{i}\t{erf(i)}\n";
        File.WriteAllText("A.data", toWrite);

        //B
        counter = 0;
        res = adapt_int(x => 1/Sqrt(x), 0, 1, delta);
        lims[0] = res-delta; lims[1] = res+delta;
        WriteLine("Integral of 1/sqrt(x) from 0 to 1 using adaptive = " + res
        + $" should be 2±{delta}=[{lims[0]},{lims[1]}]" + $" which is: " + $"{lim_check(lims, res)}");
        WriteLine($"Evaluations using adaptive: {counter}. Evaluations without: {ord_counter_sqrtinv}");

        counter = 0;
        res = adapt_int(x => Log(x)/Sqrt(x), 0, 1, delta);
        lims[0] = res-delta; lims[1] = res+delta;
        WriteLine("Integral of ln(x)/sqrt(x) from 0 to 1 using adaptive = " + res
        + $" should be -4±{delta}=[{lims[0]},{lims[1]}]" + $" which is: " + $"{lim_check(lims, res)}");
        WriteLine($"Evaluations using adaptive: {counter}. Evaluations without: {ord_counter_lnsqrt}");
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
            return 2/Sqrt(PI)*integrate((x) => Exp(-x*x), 0, z);
        else
            return 1-2/Sqrt(PI)*integrate((x) => Exp(-(z+(1-x)/x)*(z+(1-x)/x))/x/x, 0, 1);
    }

    static bool lim_check(double[] limits, double result){
        return limits[0] < result && result < limits[1];
    }
}
using System;
using static System.Console;
using static System.Math;
using System.IO;
class main{
    public static void Main(){

        //Func<double,double> myspline = qspline(x,y);
        //double fz=myspline(z);

        double[] xs = {0.0, 0.1, 0.2, 0.3, 0.4, 0.5};
        double[] ys = {1.1, 1.4, 1.7, 2.0, 2.3, 2.6};
        WriteLine(linterpInteg(xs, ys, 0.35));

        int l = 20;
        double[] plot_xs = new double[l];
        double[] plot_ys = new double[l];
        string toWrite = $"";
        double x = 0;
        double y = 0;
        for(int i=0; i<l; i++){
            double inc = 0.4/l;
            x = inc*i;
            WriteLine(x);
            y = linterp(xs,ys,x);
            plot_xs[i] = x;
            plot_ys[i] = y;
            toWrite += $"{x}\t{y}\n";
        }
        File.WriteAllText("A_test.data", toWrite);
    }




    //static Func<double,double> qspline(vector xs,vector ys){
	//    vector x, y, b, c; /* will be captured when qspline returns */
	//    /* copy xs,ys into x,y and calculate b and c */
	//    return delegate(double z){2/* evaluate and return spline(z) */};
    //}

    static double linterp(double[] x, double[] y, double z){
        int i=binsearch(x,z);
        double dx=x[i+1]-x[i]; if(!(dx>0)) throw new Exception("uups...");
        double dy=y[i+1]-y[i];
        return y[i]+dy/dx*(z-x[i]);
    }

    static double linterpInteg(double[] x, double[] y, double z){
        int i = binsearch(x,z);
        double sum = 0;
        for(int j=0; j<i; j++){
            double p_j = (y[j+1]-y[j]) / (x[j+1]-x[j]);
            sum += y[j]*(x[j+1]-x[j]) + p_j*(x[j+1]-x[j])*(x[j+1]-x[j])/2;
        }
        double p_i = (y[i+1]-y[i]) / (x[i+1]-x[i]);
        sum += y[i]*(z-x[i]) + p_i*(z-x[i])*(z-x[i])/2;
        return sum;
    }

    static int binsearch(double[] x, double z)
        {/* locates the interval for z by bisection */ 
        if(!(x[0]<=z && z<=x[x.Length-1])) throw new Exception("binsearch: bad z");
        int i=0, j=x.Length-1;
        while(j-i>1){
            int mid=(i+j)/2;
            if(z>x[mid]) i=mid; else j=mid;
        }
        return i;
    }

}
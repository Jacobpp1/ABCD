using System;
using static System.Console;
using static System.Math;
using System.IO;
class main{
    public static void Main(){

        //Func<double,double> myspline = qspline(x,y);
        //double fz=myspline(z);

        double[] xs = {0.0, 0.1, 0.2, 0.3, 0.4, 0.5};
        //double[] ys = {1.1, 1.4, 1.7, 2.0, 2.3, 2.6};
        double[] ys = new double[xs.Length];
        Random rand = new Random(1);
        string writing_dat = $"";
        double temp_y = 0;
        for(int i=0; i<ys.Length; i++){
            temp_y = rand.NextDouble();
            ys[i] = temp_y;
            writing_dat += $"{xs[i]}\t{temp_y}\n";
        }
        File.WriteAllText("A_orig.data", writing_dat);
        // Since it is linear spline, the integral should be the same as summing (x[i+1]-x[i])*(y[i+1]+y[i])/2
        double int_lim = 0.32;
        double int_sum = 0;
        for(int i=0; i<xs.Length-1; i++){
            if(xs[i+1] > int_lim){
                double a_coeff = (ys[i+1] - ys[i]) / (xs[i+1] - xs[i]);
                double y_max = (a_coeff * (int_lim-xs[i]) + ys[i]);
                int_sum += (int_lim - xs[i]) * (y_max + ys[i]) / 2;
                break;
            }
            int_sum += (xs[i+1] - xs[i]) * (ys[i+1] + ys[i]) / 2;
        }
        WriteLine("Integral " + linterpInteg(xs, ys, int_lim));
        WriteLine("Summing method integral: " + int_sum);

        int l = 20;
        double[] plot_xs = new double[l+1];
        double[] plot_ys = new double[l+1];
        string toWrite = $"";
        double x = 0;
        double y = 0;
        for(int i=0; i<=l; i++){
            double inc = 0.5/l;
            x = inc*i;
            //WriteLine(x);
            y = linterp(xs,ys,x);
            plot_xs[i] = x;
            plot_ys[i] = y;
            toWrite += $"{x}\t{y}\n";
        }
        File.WriteAllText("A_test.data", toWrite);

        // B
        toWrite = $"";
        for(int i=0; i<=l; i++){
            double inc = 0.5/l;
            x = inc*i;
            y = qspline(xs,ys)(x,0);
            plot_xs[i] = x;
            plot_ys[i] = y;
            toWrite += $"{x}\t{y}\n";
        }
        int_sum = 0;
        for(int i=0; i<xs.Length; i++){
            if(xs[i] > int_lim){
                int_sum += qspline(xs, ys)(int_lim, -1);
                break;
            }
            int_sum += qspline(xs, ys)(xs[i], -1);
        }
        File.WriteAllText("B_test.data", toWrite);
        WriteLine("Integral, quadratic: " + int_sum);

        //C
        
    }




    static Func<double, int, double> qspline(double[] xs, double[] ys){
        /* x=xs.copy(); y=ys.copy(); calculate b and c */
        int n = xs.Length;
        double[] cs = new double[n-1];
        double[] ps = new double[n-1];
        double[] bs = new double[n-1];
        //WriteLine("ps loop");
        for(int i=0; i<n-1; i++){
            //WriteLine($"{i}");
            ps[i] = (ys[i+1]-ys[i]) / (xs[i+1]-xs[i]);
        }
        //cs[-1] = 0;
        // Backwards recursion
        //WriteLine("cs loop");
        for (int i = n-2-1; i>=0; i--){
            cs[i] = 1/(xs[i+1]-xs[i]) * (ps[i+1] - ps[i] - cs[i+1] * (xs[i+2]-xs[i+1]) );
        }
        //WriteLine("bs loop");
        for(int i=0; i<n-1; i++) bs[i] = ps[i] - cs[i]*(xs[i+1]-xs[i]);
        return delegate(double z,int deriv){
            int i = binsearch(xs,z);
            if(deriv==1) /* return derivative */
                return bs[i] + 2*cs[i]*(z-xs[i]);
            if(deriv==-1) /* return integral */
                return ys[i]*(z-xs[i]) + bs[i]*(z-xs[i])*(z-xs[i])/2 + cs[i]*(z-xs[i])*(z-xs[i])*(z-xs[i])/3;
            else /* return spline */
                return ys[i] + bs[i]*(z-xs[i]) + cs[i]*(z-xs[i])*(z-xs[i]);
            }; /* x,y,b,c are captured here */
    }


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
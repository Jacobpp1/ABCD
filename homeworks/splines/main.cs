using System;
using static System.Console;
using static System.Math;
using System.IO;
class main{
    public static void Main(){
        int array_size = 20;
        double[] xs = new double[array_size];
        for(int i=0; i<array_size; i++) xs[i] = 1.0*i/(array_size-1) * 2*PI;
        double[] ys = new double[array_size];
        string writing_dat = $"";
        double temp_y = 0;
        for(int i=0; i<ys.Length; i++){
            temp_y = Sin(xs[i])*xs[i];
            ys[i] = temp_y;
            writing_dat += $"{xs[i]}\t{temp_y}\n";
        }
        File.WriteAllText("A_orig.data", writing_dat);
        double int_lim = 5;
        WriteLine("A)\nLinear splines\nSince it is linear spline, the integral should be the same as summing over the intervals");
        WriteLine($"Integrating sin(x)*x from 0 to {int_lim}");
        WriteLine("Increase number of datapoints in code (array_size variable) for better precision of integration");

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
        WriteLine("Integral of sin(x)*x, linear: " + linterpInteg(xs, ys, int_lim));
        WriteLine("Summing method integral: " + int_sum);
        WriteLine("Figure can be seen in 'A_fig.svg', including integral");

        string toWrite = $"";
        double y = 0;
        for(double x = 0; x < int_lim; x+=0.01){
            y = linterp(xs,ys,x);
            double z = linterpInteg(xs,ys, x);
            toWrite += $"{x}\t{y}\t{z}\n";
        }
        File.WriteAllText("A_test.data", toWrite);

        // B
        toWrite = $"";
        for(double x = 0; x < int_lim; x+=0.01){
            y = qspline(xs,ys).Item1(x,0);
            double z = qspline(xs,ys).Item1(x,-1);
            toWrite += $"{x}\t{y}\t{z}\n";
        }
        File.WriteAllText("B_test.data", toWrite);
        WriteLine("\nB)\nIntegral of sin(x)*x, quadratic: " + qspline(xs,ys).Item1(int_lim, -1));
        WriteLine("Figure can be seen in 'B_fig.svg', including evaluation, derivative, and integral");

        // B check against f(x) = 1, x, or x*x
        WriteLine("\nChecking qspline against 3 functions");
        double[] xs1 = {1,2,3,4,5};
        double[] ys1 = {1,1,1,1,1};
        double[] bs1 = qspline(xs1, ys1).Item2;
        double[] cs1 = qspline(xs1, ys1).Item3;
        WriteLine("For a constant function y=1, we expect no parameters, since y[i] is unchanging");
        for(int i = 0; i<bs1.Length; i++) WriteLine($"b{i}={bs1[i]}\tc{i}={cs1[i]}");

        double[] xs2 = {1,2,3,4,5};
        double[] ys2 = xs2;
        double[] bs2 = qspline(xs2, ys2).Item2;
        double[] cs2 = qspline(xs2, ys2).Item3;
        WriteLine("For a linear function y=x, we expect only linear terms added on, so c[i]=0 always");
        for(int i = 0; i<bs2.Length; i++) WriteLine($"b{i}={bs2[i]}\tc{i}={cs2[i]}");

        double[] xs3 = {1,2,3,4, 5};
        double[] ys3 = {1,4,9,16,25};
        double[] bs3 = qspline(xs3, ys3).Item2;
        double[] cs3 = qspline(xs3, ys3).Item3;
        WriteLine("For a constant function y=x^2, we expect both parameters to vary");
        for(int i = 0; i<bs3.Length; i++) WriteLine($"b{i}={bs3[i]}\tc{i}={cs3[i]}");

        //C
        
    }



    static (Func<double, int, double>, double[], double[]) qspline(double[] xs, double[] ys){
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

        // cs loop
        // Backwards recursion
        for (int i = n-2-1; i>=0; i--)
            cs[i] = 1/(xs[i+1]-xs[i]) * (ps[i+1] - ps[i] - cs[i+1] * (xs[i+2]-xs[i+1]) );
        // Forwards recursion
        //for (int i = 1; i<=n-2-1; i++)
        //    cs[i+1] = 1/(xs[i+2]-xs[i+1]) * (ps[i+1]-ps[i]-cs[i]*(xs[i+1]-xs[i])); 
        
        // bs loop
        for(int i=0; i<n-1; i++) bs[i] = ps[i] - cs[i]*(xs[i+1]-xs[i]);
        
        return (delegate(double z,int deriv){
            int i = binsearch(xs,z);
            if(deriv==1) /* return derivative */
                return bs[i] + 2*cs[i]*(z-xs[i]);
            if(deriv==-1){ /* return integral */
                double sum = 0;
                for(int j=0; j<i; j++) sum += ys[j]*(xs[j+1]-xs[j]) + bs[j]*Pow(xs[j+1]-xs[j],2)/2 + cs[j]*Pow(xs[j+1]-xs[j],3)/3;
                sum += ys[i]*(z-xs[i]) + bs[i]*Pow(z-xs[i],2)/2 + cs[i]*Pow(z-xs[i],3)/3; 
                // Maybe need to recalculate p[i] and thus c[i] and b[i], because it depends on y[i+1], which should be interpreted. Didn't seem to improve it
                /*double last_y = qspline(xs,ys).Item1(z, 0); //Interpreted
                double last_p = (last_y - ys[i]) / (z-xs[i]);
                double last_c = 1/(z-xs[i]) * (last_p - ps[i] - cs[i+1]*(z-xs[i]));
                double last_b = last_p - last_c*(z-xs[i]);
                sum += ys[i]*(z-xs[i]) + last_b*Pow(z-xs[i],2)/2 + last_c*Pow(z-xs[i],3)/3;*/
                return sum;
            }
            else /* return spline */
                return ys[i] + bs[i]*(z-xs[i]) + cs[i]*(z-xs[i])*(z-xs[i]);
            }, bs, cs); /* x,y,b,c are captured here */
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
        double p_i = (linterp(x, y, z) - y[i]) / (z - x[i]);
        sum += y[i]*(z-x[i]) + p_i*(z-x[i])*(z-x[i])/2; 
        return sum;
    }

    static int binsearch(double[] x, double z){
        /* locates the interval for z by bisection */ 
        if(!(x[0]<=z && z<=x[x.Length-1])) throw new Exception("binsearch: bad z");
        int i=0, j=x.Length-1;
        while(j-i>1){
            int mid=(i+j)/2;
            if(z>x[mid]) i=mid; else j=mid;
        }
        return i;
    }

}
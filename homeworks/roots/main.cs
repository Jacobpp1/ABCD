using System;
using static System.Math;
using static System.Console;
using System.IO;

class main{
    public static void Main(){
        Func<vector, vector> f = (x) => new vector(x[0]*x[0]+x[0]*2);
        vector y = new vector(2.0);
        vector sol = newton(f, y);
        WriteLine(sol[0]);

        // Rosenbrock 
        Func<vector,double> ros = (x) => (1-x[0])*(1*x[0]) + 100*(x[1]-x[0]*x[0])*(x[1]-x[0]*x[0]);
        Func<vector, vector> grads = (x) => new vector(2*(200*Pow(x[0],3)-200*x[0]*x[1]+x[0]-1),   200*(x[1]-x[0]*x[0]));
        y = new vector(0, 0);
        sol = newton(grads, y);
        WriteLine($"Starting from (x0,y0) = (-1,1) gives (x,y) = ({sol[0]}, {sol[1]}) and ros(x,y) = {ros(sol)}");
        WriteLine($"Analytically we expect it to be at x=y=1");

        //B
        double rmax = 8;
        double E = 2;
        Func<double, vector, vector> diff_f = (r, ys) => new vector(ys[1], 2*ys[0]/r-2*E*ys[0]);
        vector yas = new vector(2,4);
        var (xs, ps) = funcs.driverA(diff_f, 0.01, yas, rmax);
        WriteLine($"{xs[xs.size-1]}, {ps[ps.size-1][0]}, {ps[ps.size-1][1]}");

        Func<vector, vector> M_root = (x) => {
            double E = 2;
            Func<double, vector, vector> diff_f = (r, ys) => new vector(ys[1], 2*ys[0]/r-2*E*ys[0]);
            vector yas = new vector(2,4);
            (xs, ps) = funcs.driverA(diff_f, 0.01, yas, rmax);
            
    }

    static vector newton(Func<vector,vector> f, vector x, double eps=1e-2){
        int m = x.size;
        int n = f(x).size;
        double delta_x;
        vector x1;
        double lambda;
        matrix J = new matrix(n,m);
        while(f(x).norm() > eps){
            for(int i = 0; i<n; i++){
                if(Abs(x[i]) < Pow(2,-26)) x[i] = Pow(2,-24);
                delta_x = Abs(x[i])*Pow(2,-26);
                for(int k = 0; k<m; k++){
                    x1 = x.copy(); x1[k] += delta_x;
                    J[i,k] = (f(x1)[i] - f(x)[i])/delta_x;
                }
            }
            matrix R = new matrix(J.size1, J.size2);
            funcs.QRGSdecomp(J,R);
            vector d_x = funcs.QRGSsolve(J,R,-f(x));
            lambda = 1;
            while(f(x+d_x).norm() > (1-0.5*lambda)*f(x).norm() && lambda > 1.0/32.0) lambda /= 2;
            x += lambda*d_x;
        }
        return x;
    }
}
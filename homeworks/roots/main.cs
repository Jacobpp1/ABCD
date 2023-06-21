using System;
using static System.Math;
using static System.Console;
using System.IO;

class main{
    public static void Main(){
        Func<vector, vector> f = (x) => new vector(x[0]*x[0]+x[0]*2);
        vector y = new vector(2.0);
        vector sol = newton(f, y);
        WriteLine("A\nTesting Newton method on x^2 + 2x; starting at x=2");
        WriteLine($"x0 = {sol[0]}");
        f = (x) => new vector(x[0]*x[0] + x[1], 2*x[0]);
        WriteLine("Testing Newton method on x^2 + y; starting at x=y=2");
        y = new vector(2,2);
        sol = newton(f, y);
        WriteLine($"x0 = {sol[0]}");

        // Rosenbrock 
        Func<vector,double> ros = (x) => (1-x[0])*(1-x[0]) + 100*(x[1]-x[0]*x[0])*(x[1]-x[0]*x[0]);
        Func<vector, vector> grads = (x) => new vector(2*(200*Pow(x[0],3)-200*x[0]*x[1]+x[0]-1),   200*(x[1]-x[0]*x[0]));
        y = new vector(0, 0);
        sol = newton(grads, y);
        WriteLine("Testing Rosenbrock");
        WriteLine($"Starting from (x0,y0) = (-1,1) gives (x,y) = ({sol[0]}, {sol[1]}) and ros(x,y) = {ros(sol)}");
        WriteLine($"Analytically we expect it to be at x=y=1; this would give: {ros(new vector(1,1))}, as we expect.");

        //B
        WriteLine("\nProblem B");
        double rmin = 0.001;
        double rmax = 8;
        double E = 2;
        double E_th = -0.5;
        double acc = 0.01;
        double eps = 0.01;
        vector init_guess = new vector(-1.0);
        Func<double, vector, vector> diff_f = (r, ys) => new vector(ys[1], -2*(1/r+E)*ys[0]);
        vector yas = new vector(rmin-rmin*rmin, 1-2*rmin);
        var (xs, ps) = funcs.driverA(diff_f, rmin, yas, rmax);
        //WriteLine($"{xs[xs.size-1]}, {ps[ps.size-1][0]}, {ps[ps.size-1][1]}");

        Func<vector, vector> M_root = (x) => {
            E = x[0];
            (xs, ps) = funcs.driverA(diff_f, rmin, yas, rmax, acc = acc, eps = eps);
            return new vector(ps[ps.size-1][0]);
        };
        sol = newton(M_root, init_guess);
        WriteLine($"E min = {sol[0]}");
        E = sol[0];
        var (rs, yss) = funcs.driverA(diff_f, rmin, yas, rmax);
        string toWrite = $"";
        for (int i = 0; i<rs.size; i++)
            toWrite += $"{rs[i]}\t{yss[i][0]}\t{yss[i][1]}\n";
        
        File.WriteAllText("wavefunction.data", toWrite);
        WriteLine("Wavefunction plotted against theoretical. Only divergence towards the end.");

        // rmin convergence
        toWrite = $"";
        rmin = 0.0005;
        rmax = 8;
        for(int i=0; i<1000; i++){
            rmin += 0.0001;
            E = newton(M_root, init_guess)[0];
            toWrite += $"{rmin}\t{(E-E_th)/E_th}\n";
        }
        File.WriteAllText("rmin_conv.data", toWrite);
        // rmax convergence
        rmin = 0.001;
        rmax = 6;
        toWrite = $"";
        for(int i=0; i<1000; i++){
            rmax += 0.01;
            E = newton(M_root, init_guess)[0];
            toWrite += $"{rmax}\t{(E-E_th)/E_th}\n";
        }
        File.WriteAllText("rmax_conv.data", toWrite);
        // abs_acc convergence
        rmax = 8;
        toWrite = $"";
        double abs_acc = 0.00001;
        for(int i=0; i<1000; i++){
            abs_acc += 0.00001;
            acc = abs_acc;
            E = newton(M_root, init_guess)[0];
            toWrite += $"{acc}\t{(E-E_th)/E_th}\n";
        }
        File.WriteAllText("abs_acc_conv.data", toWrite);
        // eps_acc convergence
        rmax = 8;
        toWrite = $"";
        double eps_acc = 0.00001;
        for(int i=0; i<1000; i++){
            eps_acc += 0.00001;
            eps = eps_acc;
            E = newton(M_root, init_guess)[0];
            toWrite += $"{eps}\t{(E-E_th)/E_th}\n";
        }
        File.WriteAllText("eps_acc_conv.data", toWrite);
        WriteLine("\nConvergence plots can be found in corresponding .svg file.");

    }

    static vector newton(Func<vector,vector> f, vector x, double eps=1e-2){
        int m = x.size;
        int n = f(x).size;
        double delta_x;
        vector x1;
        double lambda;
        if(n!=m) WriteLine("Function vector and variable vector must be same size");
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
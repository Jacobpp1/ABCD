using System;
using static System.Console;
using static System.Math;
using System.IO;
using System.Collections.Generic;

class main{
    public static void Main(){
        WriteLine("Problem A:");
        Func<vector, double> rosen = (x) => (1-x[0])*(1-x[0]) + 100*Pow(x[1]-x[0]*x[0],2);
        Func<vector, double> himmel = (x) => Pow(x[0]*x[0]+x[1]-11, 2) + Pow(x[0]+x[1]*x[1]-7, 2);

        (vector r, int n_r) = qnewton(rosen, new vector(-3,-4), 1e-3);
        (vector h, int n_h) = qnewton(himmel, new vector(-3,-4), 1e-3);
        WriteLine($"For Rosenbrock: x0={r[0]}, y0={r[1]}; iterations = {n_r}");
        WriteLine($"For Himmelblau: x0={h[0]}, y0={h[1]}; iterations = {n_h}");

        WriteLine("Problem B:");
        Func<vector, double> bw = (x) => x[1]/(Pow(x[0]-x[2], 2) + x[3]*x[3]/4); // E = 0, A = 1, m = 2, gamma = 3 .
        var energy = new List<double>();
        var signal = new List<double>();
        var error  = new List<double>();
        string[] data = File.ReadAllLines("Higgs.data");
        string[] split_data;
        string[] delimiters = {"  ", " ", "\t"};
        
        for(int i = 0; i<data.Length; i++){
            split_data = data[i].Split(delimiters, StringSplitOptions.None);
            energy.Add(double.Parse(split_data[0]));
            signal.Add(double.Parse(split_data[1]));
            error.Add(double.Parse(split_data[2]));
        }

        vector guesses = new vector(2,2,2);
        vector bw_opt_params = fit(bw, guesses, energy, signal, error, 1e-4);
        WriteLine($"Found parameters to be: A = {bw_opt_params[0]}, m = {bw_opt_params[1]}, gamma = {bw_opt_params[2]}");

        double A, m, gam;
        double bw_dat;
        string toWrite = $"";
        for(double E = 100; E < 160; E+=0.1){
            A = bw_opt_params[0]; m = bw_opt_params[1]; gam = bw_opt_params[2];
            bw_dat = bw(new vector(E, A, m, gam));
            toWrite += $"{E}\t{bw_dat}\n";
        }
        File.WriteAllText("fit.data", toWrite);
    }

    public static vector fit(Func<vector, double> f, vector guess, List<double> xs, List<double> ys, List<double> errs, double acc){
        Func<vector, double> deviation = (x) => {
            double sum = 0; for(int i=0; i<xs.Count; i++) sum += Pow((f(new vector(xs[i], x[0], x[1], x[2])) - ys[i])/errs[i], 2); return sum;
        };
        return qnewton(deviation, guess, acc).Item1;
    }

    public static (vector, int) qnewton(
	Func<vector,double> f, /* objective function */
	vector start, /* starting point */
	double acc /* accuracy goal, on exit |gradient| should be < acc */
    ){
        int counter = 0;
        vector dx;
        vector s;
        vector u;
        vector y;
        vector grad_old;
        double lambda;
        double uy;
        matrix dB;
        int n = start.size;
        matrix B = new matrix(n,n);
        B.set_identity();
        vector grad = num_grad(f, start);
        // Maybe check that start is nonzero
        for(int i=0; i<n; i++){
            if(Abs(start[i]) < Pow(2,-26)) start[i] = Pow(2,-26);
        }
        while(grad.norm() > acc){
            counter++;
            lambda = 1.0;
            dx = -B*grad;
            while(true){
                s = lambda*dx;
                if (f(start+s) < f(start)){
                    start = start + s;
                    grad_old = grad;
                    grad = num_grad(f,start);
                    y = grad - grad_old;
                    u = s - B*y;
                    uy = u.dot(y); 
                    if(Abs(uy) > 1e-6){
                        // avoiding dividng by zero
                        dB = matrix.outer(u,u)/uy;
                        B = B + dB;
                    }
                    break;
                }
                lambda /= 2;
                if(lambda < Pow(2,-16)){
                    start = start + lambda*dx;
                    grad = num_grad(f,start);
                    B.set_identity();
                    break;
                }
            }
        }
        return (start, counter);
    }

    public static vector num_grad(Func<vector, double> f, vector x){
        int dim = x.size;
        vector grad = new vector(dim);
        for(int i = 0; i < dim; i++){
            vector newx = x.copy();
            double dx = Abs(x[i])*Pow(2,-26);
            newx[i] = x[i] + dx;
            grad[i] = (f(newx) - f(x))/dx;
        }
        return grad;
    }

}